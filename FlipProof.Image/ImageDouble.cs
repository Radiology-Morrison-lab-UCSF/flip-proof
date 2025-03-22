using FlipProof.Torch;
using TorchSharp;
using TorchSharp.Modules;
using TensorExtensionMethods = FlipProof.Torch.TensorExtensionMethods;

namespace FlipProof.Image;

public sealed partial class ImageDouble<TSpace> : Image_FloatingPoint<double, TSpace, ImageDouble<TSpace>, DoubleTensor>
      where TSpace : struct, ISpace
{



   #region Parameters

   #endregion

   #region Pythonic Operators
   public ImageDouble<TSpace> Add_UInt8(ImageUInt8<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data + right.Data);
   [CLSCompliant(false)]
   public ImageDouble<TSpace> Add_Int8(ImageInt8<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageDouble<TSpace> Add_Int16(ImageInt16<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageDouble<TSpace> Add_Int32(ImageInt32<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageDouble<TSpace> Add_Int64(ImageInt64<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageDouble<TSpace> Add_Float(ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data + right.Data);
   public ImageDouble<TSpace> Add_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data + right.Data);

   public ImageDouble<TSpace> Subtract_UInt8(ImageUInt8<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data - right.Data);
   [CLSCompliant(false)]
   public ImageDouble<TSpace> Subtract_Int8(ImageInt8<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageDouble<TSpace> Subtract_Int16(ImageInt16<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageDouble<TSpace> Subtract_Int32(ImageInt32<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageDouble<TSpace> Subtract_Int64(ImageInt64<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageDouble<TSpace> Subtract_Float(ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data - right.Data);
   public ImageDouble<TSpace> Subtract_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data - right.Data);

   public ImageDouble<TSpace> Mul_UInt8(ImageUInt8<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data * right.Data);
   [CLSCompliant(false)]
   public ImageDouble<TSpace> Mul_Int8(ImageInt8<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageDouble<TSpace> Mul_Int16(ImageInt16<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageDouble<TSpace> Mul_Int32(ImageInt32<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageDouble<TSpace> Mul_Int64(ImageInt64<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageDouble<TSpace> Mul_Float(ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data * right.Data);
   public ImageDouble<TSpace> Mul_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data * right.Data);

   public ImageDouble<TSpace> Div_UInt8(ImageUInt8<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data / right.Data);
   [CLSCompliant(false)]
   public ImageDouble<TSpace> Div_Int8(ImageInt8<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data / right.Data);
   public ImageDouble<TSpace> Div_Int16(ImageInt16<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data / right.Data);
   public ImageDouble<TSpace> Div_Int32(ImageInt32<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data / right.Data);
   public ImageDouble<TSpace> Div_Int64(ImageInt64<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data / right.Data);
   public ImageDouble<TSpace> Div_Float(ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data / right.Data);
   public ImageDouble<TSpace> Div_Double(ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(Data / right.Data);
   #endregion


   protected override double DoubleToTVoxel(double val) => val;

   /// <summary>
   /// Multiplies the first factor then adds the second, in place.
   /// </summary>
   /// <remarks>This is equivalent of nifti scaling</remarks>
   /// <param name="multiplyBy">factor</param>
   /// <param name="thenAdd">added after multiplication</param>
   public void MulAddInPlace(double multiplyBy, double thenAdd)
   {
      _data.Storage.mul_(multiplyBy).add_(thenAdd);
   }


   /// <summary>
   /// Replaces NaNs with the new value, returning a new object
   /// </summary>
   /// <param name="replaceWith">The new value</param>
   /// <returns>A new image without NaNs</returns>
   public ImageDouble<TSpace> ReplaceNaN(double replaceWith) => UnsafeCreateStatic(Data.ReplaceNaN(replaceWith));



   #region Operators

   // Many of these are to remove ambiguousity the compiler trips over
   public static ImageDouble<TSpace> operator +(ImageDouble<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data + right.Data);
   public static ImageDouble<TSpace> operator -(ImageDouble<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data - right.Data);
   public static ImageDouble<TSpace> operator *(ImageDouble<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data * right.Data);
   public static ImageDouble<TSpace> operator /(ImageDouble<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data / right.Data);



   public static ImageDouble<TSpace> operator *(ImageDouble<TSpace> left, double right) => UnsafeCreateStatic(left.Data * right);
   public static ImageDouble<TSpace> operator *(double left, ImageDouble<TSpace> right) => UnsafeCreateStatic(right.Data * left);
   public static ImageDouble<TSpace> operator /(ImageDouble<TSpace> left, double right) => UnsafeCreateStatic(left.Data * (1.0/right));
   public static ImageDouble<TSpace> operator /(double left, ImageDouble<TSpace> right) => UnsafeCreateStatic(right.Data * (1.0/left));


   #endregion
}
