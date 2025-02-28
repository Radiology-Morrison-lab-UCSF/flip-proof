using FlipProof.Torch;
using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed partial class ImageFloat<TSpace> : Image_FloatingPoint<float,TSpace, ImageFloat<TSpace>, FloatTensor>
      where TSpace : struct, ISpace
{

   /// <summary>
   /// Multiplies the first factor then adds the second, in place.
   /// </summary>
   /// <remarks>This is equivalent of nifti scaling</remarks>
   /// <param name="multiplyBy">factor</param>
   /// <param name="thenAdd">added after multiplication</param>
   public void MulAddInPlace(float multiplyBy, float thenAdd)
   {
      _data.Storage.mul_(multiplyBy).add_(thenAdd);
   }

   /// <summary>
   /// Replaces NaNs with the new value, returning a new object
   /// </summary>
   /// <param name="replaceWith">The new value</param>
   /// <returns>A new image without NaNs</returns>
   public ImageFloat<TSpace> ReplaceNaN(float replaceWith) => UnsafeCreateStatic(Data.ReplaceNaN(replaceWith));

   #region Pythonic Operators
   public ImageFloat<TSpace> Add_UInt8(ImageUInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data + right.Data);
   [CLSCompliant(false)]
   public ImageFloat<TSpace> Add_Int8(ImageInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageFloat<TSpace> Add_Int16(ImageInt16<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageFloat<TSpace> Add_Int32(ImageInt32<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageFloat<TSpace> Add_Int64(ImageInt64<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageFloat<TSpace> Add_Float(ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageDouble<TSpace> Add_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data + right.Data);

   public ImageFloat<TSpace> Subtract_UInt8(ImageUInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data - right.Data);
   [CLSCompliant(false)]
   public ImageFloat<TSpace> Subtract_Int8(ImageInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageFloat<TSpace> Subtract_Int16(ImageInt16<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageFloat<TSpace> Subtract_Int32(ImageInt32<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageFloat<TSpace> Subtract_Int64(ImageInt64<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageFloat<TSpace> Subtract_Float(ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageDouble<TSpace> Subtract_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data - right.Data);

   public ImageFloat<TSpace> Mul_UInt8(ImageUInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data * right.Data);
   [CLSCompliant(false)]
   public ImageFloat<TSpace> Mul_Int8(ImageInt8<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageFloat<TSpace> Mul_Int16(ImageInt16<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageFloat<TSpace> Mul_Int32(ImageInt32<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageFloat<TSpace> Mul_Int64(ImageInt64<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(Data * right.Data);
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

   // Operators remove ambiguity from operators in base class
   public static ImageDouble<TSpace> operator +(ImageFloat<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data + right.Data);
   public static ImageDouble<TSpace> operator +(ImageDouble<TSpace> left, ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data + right.Data);
   public static ImageDouble<TSpace> operator -(ImageFloat<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data - right.Data);
   public static ImageDouble<TSpace> operator -(ImageDouble<TSpace> left, ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data - right.Data);
   public static ImageDouble<TSpace> operator *(ImageFloat<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data * right.Data);
   public static ImageDouble<TSpace> operator *(ImageDouble<TSpace> left, ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data * right.Data);
   public static ImageFloat<TSpace> operator /(ImageFloat<TSpace> left, ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageDouble<TSpace> operator /(ImageFloat<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageDouble<TSpace> operator /(ImageDouble<TSpace> left, ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data / right.Data);

   #endregion
}
