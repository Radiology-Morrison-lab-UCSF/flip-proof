using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed class ImageInt32<TSpace> : Image_Integer<Int32, TSpace, ImageInt32<TSpace>, Int32Tensor>
   where TSpace : ISpace
{
#pragma warning disable CS0618 // Type or member is obsolete
   protected override ImageInt32<TSpace> UnsafeCreate(Int32Tensor voxels) => new(voxels, false);

   internal static ImageInt32<TSpace> UnsafeCreateStatic(Int32Tensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt32(ImageHeader header, int[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.Volumes))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt32(Int32Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {

   }


   #region Operators

   public static ImageInt32<TSpace> operator +(ImageInt16<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt32<TSpace> operator +(ImageInt32<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt32<TSpace> operator -(ImageInt16<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt32<TSpace> operator -(ImageInt32<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt32<TSpace> operator *(ImageInt16<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageInt32<TSpace> operator *(ImageInt32<TSpace> left, ImageInt16<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt16<TSpace> left, ImageInt32<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt32<TSpace> left, ImageInt16<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);

   [CLSCompliant(false)]
   public static ImageInt32<TSpace> operator +(ImageInt8<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   [CLSCompliant(false)]
   public static ImageInt32<TSpace> operator +(ImageInt32<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   [CLSCompliant(false)]
   public static ImageInt32<TSpace> operator -(ImageInt8<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   [CLSCompliant(false)]
   public static ImageInt32<TSpace> operator -(ImageInt32<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   [CLSCompliant(false)]
   public static ImageInt32<TSpace> operator *(ImageInt8<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   [CLSCompliant(false)]
   public static ImageInt32<TSpace> operator *(ImageInt32<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   [CLSCompliant(false)]
   public static ImageFloat<TSpace> operator /(ImageInt8<TSpace> left, ImageInt32<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   [CLSCompliant(false)]
   public static ImageFloat<TSpace> operator /(ImageInt32<TSpace> left, ImageInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);

   public static ImageInt32<TSpace> operator +(ImageUInt8<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt32<TSpace> operator +(ImageInt32<TSpace> left, ImageUInt8<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   public static ImageInt32<TSpace> operator -(ImageUInt8<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt32<TSpace> operator -(ImageInt32<TSpace> left, ImageUInt8<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   public static ImageInt32<TSpace> operator *(ImageUInt8<TSpace> left, ImageInt32<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageInt32<TSpace> operator *(ImageInt32<TSpace> left, ImageUInt8<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   public static ImageFloat<TSpace> operator /(ImageUInt8<TSpace> left, ImageInt32<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageFloat<TSpace> operator /(ImageInt32<TSpace> left, ImageUInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);


   #endregion
}
