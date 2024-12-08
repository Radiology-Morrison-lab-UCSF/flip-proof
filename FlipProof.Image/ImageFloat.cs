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
