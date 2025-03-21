using FlipProof.Base;
using FlipProof.Torch;
using System.Numerics;
using TorchSharp;
using static TorchSharp.torch;

namespace FlipProof.Image.Filters;

/// <summary>
/// Applies kernels to an image
/// </summary>
internal static class KernelOperations
{
   /// <summary>
   /// Applies a kernel to an image via FFT
   /// </summary>
   /// <typeparam name="TVoxel">Voxel type</typeparam>
   /// <typeparam name="TTensor">Tensor type holding the image</typeparam>
   /// <param name="im">The 4D tensor to apply the kernel to. This is unmodified</param>
   /// <param name="kernel">The kernel to apply</param>
   /// <returns>A new tensor with the kernel applied</returns>
   /// <exception cref="NotSupportedException">Image or kernel are too large in combination (>int.Max when combined)</exception>
   internal static TTensor ApplyKernelViaFFT<TVoxel, TTensor>(this TTensor im, IKernel<TVoxel> kernel)
   where TVoxel : struct, INumber<TVoxel>
   where TTensor : SimpleNumericTensor<TVoxel, TTensor>
   {

      // Pad the image so that we don't wrap one side to the other using the kernel
      long padToX = im.Shape[0] + kernel.KernelExtent.X / 2;
      long padToY = im.Shape[1] + kernel.KernelExtent.Y / 2;
      long padToZ = im.Shape[2] + kernel.KernelExtent.Z / 2;

      if (padToX > int.MaxValue || padToY > int.MaxValue || padToZ > int.MaxValue)
      {
         throw new NotSupportedException("Image and/or kernel are too large in one or more dimensions");
      }
      using Tensor imFft = GetPaddedImagInFreqDomain<TVoxel, TTensor>(im, padToX, padToY, padToZ);
      using (Tensor kernelFft = BuildKernelInFreqDomain(kernel, padToX, padToY, padToZ))
      {
         imFft.mul_(kernelFft);
      }

      using var resultPadded = torch.fft.ifftn(imFft, dim: [0, 1, 2]);

      imFft.Dispose();

      using Tensor unpadded = resultPadded.GetSubTensor(im.Shape);

      resultPadded.Dispose();

      return im.CreateFromTensor(unpadded.real, true);

   }
   /// <summary>
   /// Pads image, performs FFT
   /// </summary>
   /// <typeparam name="TVoxel"></typeparam>
   /// <typeparam name="TTensor"></typeparam>
   /// <param name="im"></param>
   /// <param name="padToX"></param>
   /// <param name="padToY"></param>
   /// <param name="padToZ"></param>
   /// <returns></returns>
   static Tensor GetPaddedImagInFreqDomain<TVoxel, TTensor>(TTensor im, long padToX, long padToY, long padToZ)
      where TVoxel : struct, INumber<TVoxel>
      where TTensor : SimpleNumericTensor<TVoxel, TTensor>
   {
      using Tensor paddedIm = Pad4D(im.Storage, padToX, padToY, padToZ);
      return torch.fft.fftn(paddedIm, dim: [0, 1, 2]);
   }

   static Tensor BuildKernelInFreqDomain<TVoxel>(IKernel<TVoxel> kernelInstructions, long padToX, long padToY, long padToZ)
      where TVoxel : struct, INumber<TVoxel>
   {

      // We want to build a kernel where the center is actually at 0,0,0 as though fftshift was applied
      // So, i=0 ==> centre
      // i:
      //   0, 1, 2, 3, 4, 5, 6
      //            |
      //          middle
      // dist:
      //  -3, -2, -1, 0, 1, 2, 3
      //
      //
      // becomes:
      //
      // dist:
      //  0, 1, 2, 3, -3, -2, -1
      //  |
      // middle
      //
      //
      // -------------------------------------------------
      // -------------------------------------------------
      //
      // If the image is even
      // i:
      //   0, 1, 2, 3, 4, 5, 6, 7
      //
      // becomes:
      //
      // dist:
      //  0, 1, 2, 3, -4, -3, -2, -1
      //  |
      // middle

      List<int> distX = MapIndices((int)padToX);
      List<int> distY = MapIndices((int)padToY);
      List<int> distZ = MapIndices((int)padToZ);
      using Array3D<TVoxel> kern = Array3D<TVoxel>.FromValueGenerator((i, j, k) => kernelInstructions.GetIntensity(distX[i], distY[j], distZ[k]),
                                                                                           (int)padToX, (int)padToY, (int)padToZ);
      kernelInstructions.NormaliseKernel(kern);

      using var kernel4D = kern.ToTensor4D();
      kern.Dispose();//eager data cleanup
      return torch.fft.fftn(kernel4D, dim: [0, 1, 2]);

      static List<int> MapIndices(int length)
      {
         List<int> indices = new(length);
         for (int i = 0; i < length / 2; i++)
         {
            indices.Add(i);
         }

         int negLenMinus1 = -length - 1;
         for (int i = length / 2; i < length; i++)
         {
            indices.Add(negLenMinus1 + i + 1);
         }

         return indices;
      }
   }

   static Tensor Pad4D(this Tensor t, long padToX, long padToY, long padToZ)
   {
      return nn.functional.pad(t, [0, 0, 0, padToZ - t.shape[2], 0, padToY - t.shape[1], 0, padToX - t.shape[0]]);//args work backwards
   }



   internal delegate TTensor Create3DKernel<TVoxel, TTensor>(long imSizeX, long imSizeY, long imSizeZ)
   where TVoxel : struct, INumber<TVoxel>
   where TTensor : SimpleNumericTensor<TVoxel, TTensor>;


   [Obsolete("Opt for FFT method as memory use for this can be explosive")]
   internal static TTensor ApplyKernel<TVoxel, TTensor>(this TTensor im, Create3DKernel<TVoxel, TTensor> createKernel)
   where TVoxel : struct, INumber<TVoxel>
   where TTensor : SimpleNumericTensor<TVoxel, TTensor>
   {
      // Assuming the input image has the shape (batch_size, channels, x, y, z, volume)
      //var batch_size = 1;
      long x = im.Shape[0];
      long y = im.Shape[1];
      long z = im.Shape[2];// # Image dimensions (x, y, z)
      long volume = im.Shape[3];//  # Number of volumes

      // Kernel (ixjxk) for convolution final shape: (in_channels=1, out_channels=1, kx, ky, kz)
      using var kernelOrig = createKernel(x, y, z);
      if (kernelOrig.NDims != 3)
      {
         throw new ArgumentException("Expected 3D kernel");
      }
      var kernel = kernelOrig.Storage.view([1, 1, kernelOrig.Shape[0], kernelOrig.Shape[1], kernelOrig.Shape[2]]);
      kernel = kernel.repeat(volume, 1, 1, 1, 1);

      // Reshape the image tensor to be 5D for conv3d: (batch_size, channels, x, y, z * volume)
      var image_reshaped = im.Storage.view(1, x, y, z, volume).movedim([4], [1]);

      // Apply convolution to all volumes at once
      // we pad by half the kernel width. NB that padding takes in reversed order
      var output = nn.functional.conv3d(image_reshaped, kernel, padding: kernelOrig.Shape.Select(a => a / 2).Reverse().ToArray(), groups: volume);

      // Reshape the output back to the original volume dimension
      var output_image = output.view(volume, x, y, z).movedim([0], [3]);
      return im.CreateFromTensor(output_image.contiguous(), allowCast: true); //conv3D seems to produce a non contiguous tensor in memory which causes nifti write crashes
   }

}
