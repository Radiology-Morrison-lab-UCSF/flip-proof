using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed partial class ImageUInt8<TSpace> : Image_Integer<byte, TSpace, ImageUInt8<TSpace>, UInt8Tensor>
   where TSpace : struct, ISpace
{
   #region Python Operators

   public ImageUInt8<TSpace> Add_UInt8(ImageUInt8<TSpace> right) => ImageUInt8<TSpace>.UnsafeCreateStatic(Data + right.Data);
   [CLSCompliant(false)]
   public ImageInt16<TSpace> Add_Int8(ImageInt8<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageInt16<TSpace> Add_Int16(ImageInt16<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageInt32<TSpace> Add_Int32(ImageInt32<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageInt64<TSpace> Add_Int64(ImageInt64<TSpace> right) => ImageInt64<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageFloat<TSpace> Add_Float(ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageDouble<TSpace> Add_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data + right.Data);

   public ImageUInt8<TSpace> Subtract_UInt8(ImageUInt8<TSpace> right) => ImageUInt8<TSpace>.UnsafeCreateStatic(Data - right.Data);
   [CLSCompliant(false)]
   public ImageInt16<TSpace> Subtract_Int8(ImageInt8<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageInt16<TSpace> Subtract_Int16(ImageInt16<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageInt32<TSpace> Subtract_Int32(ImageInt32<TSpace> right) => ImageInt32<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageInt64<TSpace> Subtract_Int64(ImageInt64<TSpace> right) => ImageInt64<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageFloat<TSpace> Subtract_Float(ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageDouble<TSpace> Subtract_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data - right.Data);

   public ImageUInt8<TSpace> Mul_UInt8(ImageUInt8<TSpace> right) => ImageUInt8<TSpace>.UnsafeCreateStatic(Data * right.Data);
   [CLSCompliant(false)]
   public ImageInt16<TSpace> Mul_Int8(ImageInt8<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageInt16<TSpace> Mul_Int16(ImageInt16<TSpace> right) => ImageInt16<TSpace>.UnsafeCreateStatic(Data * right.Data);
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
}
