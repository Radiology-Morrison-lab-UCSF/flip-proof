using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

[CLSCompliant(false)]
public sealed partial class ImageInt8<TSpace> : Image_Integer<sbyte,TSpace, ImageInt8<TSpace>, Int8Tensor>
   where TSpace : struct, ISpace
{

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
