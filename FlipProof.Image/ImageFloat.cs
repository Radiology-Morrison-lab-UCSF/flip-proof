using FlipProof.Torch;
using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed class ImageFloat<TSpace> : Image_SimpleNumeric<float,TSpace, ImageFloat<TSpace>, FloatTensor>
      where TSpace : ISpace
{
#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageFloat<TSpace> UnsafeCreateStatic(FloatTensor voxels) => new(voxels, false);
   /// <summary>
   /// Creates a new Image from an array of voxels. The header is explicitly checked against <typeparamref name="TSpace"/>: 
   /// use an operation with an existing image instead to use compile-time-checks where possible
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Order data strides volume(fastest), z, y, x (slowest)</param>
#pragma warning restore CS0618 // Type or member is obsolete
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageFloat(ImageHeader header, float[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.Volumes))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageFloat(FloatTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }

   protected override ImageFloat<TSpace> UnsafeCreate(FloatTensor voxels) => UnsafeCreateStatic(voxels);

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
   /// Returns a new image with voxels converted to their absolute values
   /// </summary>
   /// <returns></returns>
   public ImageFloat<TSpace> Abs() => ImageFloat<TSpace>.UnsafeCreateStatic(Data.Abs());

   #region Operators

   // Operators remove ambiguity from operators in base class
   public static ImageDouble<TSpace> operator +(ImageFloat<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data + right.Data);
   public static ImageDouble<TSpace> operator +(ImageDouble<TSpace> left, ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data + right.Data);
   public static ImageDouble<TSpace> operator -(ImageFloat<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data - right.Data);
   public static ImageDouble<TSpace> operator -(ImageDouble<TSpace> left, ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data - right.Data);
   public static ImageDouble<TSpace> operator *(ImageFloat<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data * right.Data);
   public static ImageDouble<TSpace> operator *(ImageDouble<TSpace> left, ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data * right.Data);
   public static ImageDouble<TSpace> operator /(ImageFloat<TSpace> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageDouble<TSpace> operator /(ImageDouble<TSpace> left, ImageFloat<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data / right.Data);

   #endregion
}
