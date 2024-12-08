using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed partial class ImageInt64<TSpace> : Image_Integer<Int64, TSpace, ImageInt64<TSpace>, Int64Tensor>
   where TSpace : struct, ISpace
{

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

   [CLSCompliant(false)]
   public static ImageInt64<TSpace> operator +(ImageInt8<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   [CLSCompliant(false)]
   public static ImageInt64<TSpace> operator +(ImageInt64<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   [CLSCompliant(false)]
   public static ImageInt64<TSpace> operator -(ImageInt8<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   [CLSCompliant(false)]
   public static ImageInt64<TSpace> operator -(ImageInt64<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data - right.Data);
   [CLSCompliant(false)]
   public static ImageInt64<TSpace> operator *(ImageInt8<TSpace> left, ImageInt64<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   [CLSCompliant(false)]
   public static ImageInt64<TSpace> operator *(ImageInt64<TSpace> left, ImageInt8<TSpace> right) => UnsafeCreateStatic(left.Data * right.Data);
   [CLSCompliant(false)]
   public static ImageFloat<TSpace> operator /(ImageInt8<TSpace> left, ImageInt64<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   [CLSCompliant(false)]
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
