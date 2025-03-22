using FlipProof.Torch;
using System.Numerics;

namespace FlipProof.Image.Filters;

public static class SimpleMathFilters
{
   /// <summary>
   /// Converts the image to have a mean of 0 and std dev of 1
   /// </summary>
   /// <typeparam name="TVoxel"></typeparam>
   /// <typeparam name="TSpace"></typeparam>
   /// <typeparam name="TSelf"></typeparam>
   /// <typeparam name="TTensor"></typeparam>
   /// <returns></returns>
   public static TSelf IntensityNormalise<TVoxel, TSpace, TSelf, TTensor>(this Image_FloatingPoint<TVoxel, TSpace, TSelf, TTensor> im)
      where TVoxel : struct, INumber<TVoxel>, IFloatingPointIeee754<TVoxel>, IMinMaxValue<TVoxel>
      where TSpace : struct, ISpace
      where TSelf : Image_FloatingPoint<TVoxel, TSpace, TSelf, TTensor>
      where TTensor : FloatingPointTensor<TVoxel, TTensor>
   {
      TVoxel mean = im.Mean();
      TVoxel stdDev = im.StdDev();
      
      return (im - mean) / stdDev;
   }
}
