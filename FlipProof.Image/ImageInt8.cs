using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

[CLSCompliant(false)]
public sealed class ImageInt8<TSpace> : Image_Integer<sbyte,TSpace, ImageInt8<TSpace>, Int8Tensor>
   where TSpace : struct, ISpace
{
   #region Constructors
#pragma warning disable CS0618 // Type or member is obsolete
   internal override ImageInt8<TSpace> UnsafeCreate(Int8Tensor voxels) => new(voxels, false);

   internal static ImageInt8<TSpace> UnsafeCreateStatic(Int8Tensor voxels)
   {
      return new(voxels, false);
   }
#pragma warning restore CS0618 // Type or member is obsolete

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt8(ImageHeader header, sbyte[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt8(Int8Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {

   }
   #endregion

   #region Operators

   public static ImageInt16<TSpace> operator +(ImageUInt8<TSpace> left, ImageInt8<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt16<TSpace> operator +(ImageInt8<TSpace> left, ImageUInt8<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt16<TSpace> operator -(ImageUInt8<TSpace> left, ImageInt8<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt16<TSpace> operator -(ImageInt8<TSpace> left, ImageUInt8<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt16<TSpace> operator *(ImageUInt8<TSpace> left, ImageInt8<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(left.Data * right.Data);
   public static ImageInt16<TSpace> operator *(ImageInt8<TSpace> left, ImageUInt8<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(left.Data * right.Data);
   public static ImageFloat<TSpace> operator /(ImageUInt8<TSpace> left, ImageInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt8<TSpace> left, ImageUInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);


   #endregion
}
