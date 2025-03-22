using FlipProof.Torch;

namespace FlipProof.Image.Filters;
public static class KernelBasedFilters
{
   /// <summary>
   /// Gaussian smoothing
   /// </summary>
   /// <param name="sigma">In voxels</param>
   public static ImageFloat<TSpace> Smooth<TSpace>(this ImageFloat<TSpace> im, float sigma = 1.0f)
         where TSpace : struct, ISpace
   {
      var result = im.Data.ApplyKernelViaFFT<float, FloatTensor>(new GaussianKernel(sigma));

      // Wrap as an image
      return im.UnsafeCreate(result);
   }

   /// <summary>
   /// Gaussian smoothing
   /// </summary>
   /// <param name="sigma">In voxels</param>
   public static ImageDouble<TSpace> Smooth<TSpace>(this ImageDouble<TSpace> im, double sigma = 1.0f)
         where TSpace : struct, ISpace
   {
      var result = im.Data.ApplyKernelViaFFT<double, DoubleTensor>(new GaussianKernel(sigma));
      
      // Wrap as an image
      return im.UnsafeCreate(result);

   }
   
   /// <summary>
   /// Unsharp mask sharpening
   /// </summary>
   /// <param name="sigma">In voxels</param>
   public static ImageFloat<TSpace> UnsharpMask<TSpace>(this ImageFloat<TSpace> im, float sigma = 1.0f)
         where TSpace : struct, ISpace
   {
      var result = im.Data.ApplyKernelViaFFT<float, FloatTensor>(new UnsharpKernel(sigma));

      // Wrap as an image
      return im.UnsafeCreate(result);
   }

   /// <summary>
   /// Unsharp mask sharpening
   /// </summary>
   /// <param name="sigma">In voxels</param>
   public static ImageDouble<TSpace> UnsharpMask<TSpace>(this ImageDouble<TSpace> im, double sigma = 1.0f)
         where TSpace : struct, ISpace
   {
      var result = im.Data.ApplyKernelViaFFT<double, DoubleTensor>(new UnsharpKernel(sigma));

      // Wrap as an image
      return im.UnsafeCreate(result);

   }
}
