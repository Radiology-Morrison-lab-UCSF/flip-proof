using FlipProof.Torch;
using System.Numerics;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.utils;

namespace FlipProof.Image;
/// <summary>
/// Number types like double, but not Complex
/// </summary>
/// <typeparam name="TVoxel"></typeparam>
/// <typeparam name="TSpace"></typeparam>
[CLSCompliant(true)]
public abstract class Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> : Image<TVoxel, TSpace>
   where TVoxel : struct, INumber<TVoxel>
   where TSpace : ISpace
   where TTensor: SimpleNumericTensor<TVoxel, TTensor>
   where TSelf : Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor>
{
   internal TTensor Data { get; }

   #region Construction

   /// <summary>
   /// Creates a new image, explicitly stating the orientation. Appropriate 
   /// for use in I/O operations or when first stating the orientation of 
   /// this type. Provided voxels are cloned, not used directly.
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Voxel data - a copy of the provided object is made</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal Image_SimpleNumeric(ImageHeader header, Tensor voxels) : base(header, voxels)
   {
      Data = (TTensor)_data;
   }

   /// <summary>
   /// Creates a new image, explicitly stating the orientation. Appropriate 
   /// for use in I/O operations or when first stating the orientation of 
   /// this type. Provided voxels are cloned, not used directly.
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Voxel data - a copy of the provided object is made</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal Image_SimpleNumeric(ImageHeader header, TTensor voxels) : base(header, voxels)
   {
      Data = voxels;
   }


   /// <summary>
   /// Creates a new Image from an operation known to not affect the shape or data-order of the tensor
   /// </summary>
   /// <param name="voxels">Voxel data. Data are used directly. Do not feed in a tensor accessible outside this object</param>
   /// <param name="verifyShape">Verify that the voxel array shape should be verified against the header. True if the operation is not trusted</param>
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal Image_SimpleNumeric(TTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
      Data = (TTensor)_data;
   }

   /// <summary>
   /// Creates an object like this with the provided voxels
   /// </summary>
   /// <param name="voxels"></param>
   /// <returns></returns>
   protected abstract TSelf UnsafeCreate(TTensor voxels);


   #endregion

   public TSelf Abs() => UnsafeCreate(Data.Abs());
   public TSelf AbsInPlace()
   {
      Data.AbsInPlace();
      return (TSelf)this;
   }

   #region Operators

   private ImageBool<TSpace> Compare(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right, Func<Tensor, Tensor, Tensor> comparison)
   {
     return TrustedOperatorToNew(right, (a, b) => new BoolTensor(comparison(a.Storage, b.Storage)), ImageBool<TSpace>.UnsafeCreate);
   }
   public static ImageBool<TSpace> operator <(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => left.Compare(right, torch.less);
   public static ImageBool<TSpace> operator <=(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => left.Compare(right, torch.less_equal);
   public static ImageBool<TSpace> operator >(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => left.Compare(right, torch.greater);
   public static ImageBool<TSpace> operator >=(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => left.Compare(right, torch.greater_equal);
   public static TSelf operator +(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => left.UnsafeCreate(left.Data.Add(right.Data));
   public static TSelf operator -(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => left.UnsafeCreate(left.Data.Subtract(right.Data));
   public static TSelf operator *(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => left.UnsafeCreate(left.Data.Multiply(right.Data));
   public static ImageDouble<TSpace> operator +(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data + right.Data);
   public static ImageDouble<TSpace> operator +(ImageDouble<TSpace> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data + right.Data);
   public static ImageDouble<TSpace> operator -(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data - right.Data);
   public static ImageDouble<TSpace> operator -(ImageDouble<TSpace> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data - right.Data);
   public static ImageDouble<TSpace> operator *(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data * right.Data);
   public static ImageDouble<TSpace> operator *(ImageDouble<TSpace> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data * right.Data);
   public static ImageDouble<TSpace> operator /(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, ImageDouble<TSpace> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageDouble<TSpace> operator /(ImageDouble<TSpace> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => ImageDouble<TSpace>.UnsafeCreateStatic(left.Data / right.Data);


   #endregion



}