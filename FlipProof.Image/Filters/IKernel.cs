using FlipProof.Base;
using System.Numerics;

namespace FlipProof.Image.Filters;
internal interface IKernel<TVoxel> where TVoxel:struct, INumber<TVoxel>
{
   /// <summary>
   /// The full width of where the kernel is not effectively zero; the suggested kernel full width
   /// </summary>
   XYZ<int> KernelExtent { get; }

   /// <summary>
   /// Returns the value at this part of the kernel
   /// </summary>
   /// <typeparam name="TVoxel"></typeparam>
   /// <param name="offsetX">Dist from kernel center, x, in voxels</param>
   /// <param name="offsetY">Dist from kernel center, y, in voxels</param>
   /// <param name="offsetZ">Dist from kernel center, z, in voxels</param>
   TVoxel GetIntensity(int offsetX, int offsetY, int offsetZ);

   /// <summary>
   /// Performs normalisation on the kernel, by default by ensuring it adds to 1
   /// </summary>
   /// <param name="kern"></param>
   void NormaliseKernel(Array3D<TVoxel> kern) => NormaliseKernelTo(kern, TVoxel.One);
   
   /// <summary>
   /// Performs normalisation on the kernel, by ensuring it adds to the provided value
   /// </summary>
   /// <param name="kern"></param>
   internal static void NormaliseKernelTo(Array3D<TVoxel> kern, TVoxel targetSum)
   {
      TVoxel sum = TVoxel.Zero;
      TVoxel[] allValues = kern.GetAllVoxels_XFastest();
      foreach (var item in allValues)
      {
         sum += item;
      }

      TVoxel mulBy = targetSum / sum;

      for (int i = 0; i < allValues.Length; i++)
      {
         allValues[i] *= mulBy;
      }
      kern.SetAllVoxels_XFastest(allValues);
   }
}
