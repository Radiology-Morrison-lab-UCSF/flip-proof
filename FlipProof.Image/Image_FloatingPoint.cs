using FlipProof.Torch;
using System.Numerics;
using static TorchSharp.torch;

namespace FlipProof.Image;

public abstract class Image_FloatingPoint<TVoxel, TSpace, TSelf, TTensor> : Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor>
   where TVoxel : struct, IFloatingPointIeee754<TVoxel>, IMinMaxValue<TVoxel>
   where TSpace : struct, ISpace
   where TTensor : FloatingPointTensor<TVoxel, TTensor>, IFloatingPointTensor
   where TSelf : Image_FloatingPoint<TVoxel, TSpace, TSelf, TTensor>
{
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal Image_FloatingPoint(ImageHeader header, Tensor voxels):base(header, voxels)
   {

   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal Image_FloatingPoint(TTensor voxels, bool verifyShape):base(voxels, verifyShape)
   {

   }
   /// <summary>
   /// Creates a new <see cref="TSelf"/> with rounded voxel values
   /// </summary>
   /// <returns></returns>
   public TSelf Round() => UnsafeCreate(Data.Round());
   /// <summary>
   /// Rounds voxel values in place
   /// </summary>
   /// <returns></returns>
   public TSelf RoundInPlace()
   {
      Data.RoundInPlace();
      return (this as TSelf)!;
   }

   public static TSelf operator -(Image_FloatingPoint<TVoxel, TSpace, TSelf, TTensor> left, TVoxel right) => left.UnsafeCreate(left.Data - right);
   public static TSelf operator +(Image_FloatingPoint<TVoxel, TSpace, TSelf, TTensor> left, TVoxel right) => left.UnsafeCreate(left.Data + right);

}
