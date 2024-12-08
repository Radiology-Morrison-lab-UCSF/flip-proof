using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed partial class ImageInt32<TSpace> : Image_Integer<Int32, TSpace, ImageInt32<TSpace>, Int32Tensor>
   where TSpace : struct, ISpace
{


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
