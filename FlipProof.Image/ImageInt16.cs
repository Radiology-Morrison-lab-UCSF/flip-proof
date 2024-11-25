using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed class ImageInt16<TSpace> : Image_Integer<Int16, TSpace, ImageInt16<TSpace>, Int16Tensor>
   where TSpace : ISpace
{
   #region Constructors

#pragma warning disable CS0618 // Type or member is obsolete
   internal override ImageInt16<TSpace> UnsafeCreate(Int16Tensor voxels) => new(voxels, false);
   internal static ImageInt16<TSpace> UnsafeCreateStatic(Int16Tensor voxels) => new(voxels, false);

#pragma warning restore CS0618 // Type or member is obsolete

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt16(ImageHeader header, Int16[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.Volumes))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt16(Int16Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {

   }
   #endregion




   #region Operators
   [CLSCompliant(false)]
   public static ImageInt16<TSpace> operator +(ImageInt8<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   [CLSCompliant(false)]
   public static ImageInt16<TSpace> operator +(ImageInt16<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   [CLSCompliant(false)]
   public static ImageInt16<TSpace> operator -(ImageInt8<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   [CLSCompliant(false)]
   public static ImageInt16<TSpace> operator -(ImageInt16<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   [CLSCompliant(false)]
   public static ImageInt16<TSpace> operator *(ImageInt8<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   [CLSCompliant(false)]
   public static ImageInt16<TSpace> operator *(ImageInt16<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   [CLSCompliant(false)]
   public static ImageFloat<TSpace> operator /(ImageInt8<TSpace> left, ImageInt16<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   [CLSCompliant(false)]
   public static ImageFloat<TSpace> operator /(ImageInt16<TSpace> left, ImageInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);

   public static ImageInt16<TSpace> operator +(ImageUInt8<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt16<TSpace> operator +(ImageInt16<TSpace> left, ImageUInt8<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt16<TSpace> operator -(ImageUInt8<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt16<TSpace> operator -(ImageInt16<TSpace> left, ImageUInt8<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt16<TSpace> operator *(ImageUInt8<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageInt16<TSpace> operator *(ImageInt16<TSpace> left, ImageUInt8<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageFloat<TSpace> operator /(ImageUInt8<TSpace> left, ImageInt16<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt16<TSpace> left, ImageUInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);


   #endregion
}
