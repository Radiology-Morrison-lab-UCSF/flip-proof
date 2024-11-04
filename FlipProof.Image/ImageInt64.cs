using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed class ImageInt64<TSpace> : Image_Integer<Int64, TSpace, ImageInt64<TSpace>, Int64Tensor>
   where TSpace : ISpace
{
#pragma warning disable CS0618 // Type or member is obsolete
   protected override ImageInt64<TSpace> UnsafeCreate(Int64Tensor voxels) => new(voxels, false);
   static ImageInt64<TSpace> UnsafeCreateStatic(Int64Tensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt64(ImageHeader header, Int64[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.Volumes))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt64(Int64Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {

   }


   #region Operators

   public static ImageInt64<TSpace> operator +(ImageInt32<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt64<TSpace> operator +(ImageInt64<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt64<TSpace> operator -(ImageInt32<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt64<TSpace> operator -(ImageInt64<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt64<TSpace> operator *(ImageInt32<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageInt64<TSpace> operator *(ImageInt64<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt32<TSpace> left, ImageInt64<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt64<TSpace> left, ImageInt32<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);

   public static ImageInt64<TSpace> operator +(ImageInt16<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt64<TSpace> operator +(ImageInt64<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt64<TSpace> operator -(ImageInt16<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt64<TSpace> operator -(ImageInt64<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt64<TSpace> operator *(ImageInt16<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageInt64<TSpace> operator *(ImageInt64<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt16<TSpace> left, ImageInt64<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt64<TSpace> left, ImageInt16<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);

   public static ImageInt64<TSpace> operator +(ImageInt8<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt64<TSpace> operator +(ImageInt64<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt64<TSpace> operator -(ImageInt8<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt64<TSpace> operator -(ImageInt64<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt64<TSpace> operator *(ImageInt8<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageInt64<TSpace> operator *(ImageInt64<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt8<TSpace> left, ImageInt64<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt64<TSpace> left, ImageInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);

   public static ImageInt64<TSpace> operator +(ImageUInt8<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt64<TSpace> operator +(ImageInt64<TSpace> left, ImageUInt8<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt64<TSpace> operator -(ImageUInt8<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt64<TSpace> operator -(ImageInt64<TSpace> left, ImageUInt8<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt64<TSpace> operator *(ImageUInt8<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageInt64<TSpace> operator *(ImageInt64<TSpace> left, ImageUInt8<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageFloat<TSpace> operator /(ImageUInt8<TSpace> left, ImageInt64<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt64<TSpace> left, ImageUInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);


   #endregion
}
