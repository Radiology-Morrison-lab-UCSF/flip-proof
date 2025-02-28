using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed partial class ImageInt32<TSpace> : Image_Integer<Int32, TSpace, ImageInt32<TSpace>, Int32Tensor>
   where TSpace : struct, ISpace
{
   #region Pythonic Operators
   public ImageInt32<TSpace> Add_UInt8(ImageUInt8<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data + right.Data);
   [CLSCompliant(false)]
   public ImageInt32<TSpace> Add_Int8(ImageInt8<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageInt32<TSpace> Add_Int16(ImageInt16<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageInt32<TSpace> Add_Int32(ImageInt32<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageInt64<TSpace> Add_Int64(ImageInt64<TSpace> right) => ImageInt64<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageFloat<TSpace> Add_Float(ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageDouble<TSpace> Add_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data + right.Data);

   public ImageInt32<TSpace> Subtract_UInt8(ImageUInt8<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data - right.Data);
   [CLSCompliant(false)]
   public ImageInt32<TSpace> Subtract_Int8(ImageInt8<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageInt32<TSpace> Subtract_Int16(ImageInt16<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageInt32<TSpace> Subtract_Int32(ImageInt32<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageInt64<TSpace> Subtract_Int64(ImageInt64<TSpace> right) => ImageInt64<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageFloat<TSpace> Subtract_Float(ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageDouble<TSpace> Subtract_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data - right.Data);

   public ImageInt32<TSpace> Mul_UInt8(ImageUInt8<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data * right.Data);
   [CLSCompliant(false)]
   public ImageInt32<TSpace> Mul_Int8(ImageInt8<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageInt32<TSpace> Mul_Int16(ImageInt16<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageInt32<TSpace> Mul_Int32(ImageInt32<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageInt64<TSpace> Mul_Int64(ImageInt64<TSpace> right) => ImageInt64<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageFloat<TSpace> Mul_Float(ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageDouble<TSpace> Mul_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data * right.Data);

   public ImageFloat<TSpace> Div_UInt8(ImageUInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data / right.Data);
   [CLSCompliant(false)]
   public ImageFloat<TSpace> Div_Int8(ImageInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data / right.Data);
   public ImageFloat<TSpace> Div_Int16(ImageInt16<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data / right.Data);
   public ImageFloat<TSpace> Div_Int32(ImageInt32<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data / right.Data);
   public ImageFloat<TSpace> Div_Int64(ImageInt64<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data / right.Data);
   public ImageFloat<TSpace> Div_Float(ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data / right.Data);
   public ImageDouble<TSpace> Div_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data / right.Data);
   #endregion

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
