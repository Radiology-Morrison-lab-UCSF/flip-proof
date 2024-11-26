using FlipProof.Torch;
using System.Numerics;
using static TorchSharp.torch;

namespace FlipProof.Image;

public abstract class Image_FloatingPoint<TVoxel, TSpace, TSelf, TTensor> : Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor>
   where TVoxel : struct, IFloatingPointIeee754<TVoxel>
   where TSpace : ISpace
   where TTensor : SimpleNumericTensor<TVoxel, TTensor>, IFloatingPointTensor
   where TSelf : Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor>
{
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal Image_FloatingPoint(ImageHeader header, Tensor voxels):base(header, voxels)
   {

   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal Image_FloatingPoint(TTensor voxels, bool verifyShape):base(voxels, verifyShape)
   {

   }

   public TSelf Round() => TrustedOperatorToNew(FloatingPointTensorExtensionMethods.Round<TVoxel, TTensor>);
   public TSelf RoundInPlace()
   {
      Data.RoundInPlace<TVoxel,TTensor>();
      return (this as TSelf)!;
   }
}
