using FlipProof.Torch;
using System.Numerics;
using TorchSharp;
using static TorchSharp.torch;

namespace FlipProof.Image;

/// <summary>
/// Number types like double, but not Complex
/// </summary>
/// <typeparam name="TVoxel"></typeparam>
/// <typeparam name="TSpace"></typeparam>
[CLSCompliant(true)]
public abstract class Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> : Image<TVoxel, TSpace>
   where TVoxel : struct, INumber<TVoxel>
   where TSpace : struct, ISpace
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
   internal abstract TSelf UnsafeCreate(TTensor voxels);


   #endregion

   #region Wrapped
   public TSelf Abs() => UnsafeCreate(Data.Abs());
   public TSelf AbsInPlace()
   {
      Data.AbsInPlace();
      return (TSelf)this;
   }

   public ImageComplex32<TSpace> FFT() => ImageComplex32<TSpace>.UnsafeCreateStatic(Data.FFTN());

   /// <summary>
   /// Replaces all instances of a value with another. NaN is not supported
   /// </summary>
   /// <param name="replace">To replace. Must not be NaN</param>
   /// <param name="with">The new value</param>
   /// <returns>This object</returns>
   public TSelf ReplaceInPlace(TVoxel replace, TVoxel with)
   {
      _data.ReplaceInPlace(replace, with);
      return (TSelf)this;
   }


   /// <summary>
   /// Applies a <see cref="TTensor"/> operator to create a new object from the resulting <see cref="TTensor"/>
   /// </summary>
   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
   /// <typeparam name="TOut">The expected output datatype from the operation</typeparam>
   /// <param name="other">The second image</param>
   /// <param name="operation">The operation to apply to the two images</param>
   /// <returns></returns>
   internal TSelf TrustedOperatorToNew(TSelf other, Func<TTensor, TTensor, TTensor> operation)
   {
      return UnsafeCreate(operation(this.Data, other.Data));
   }


   /// <summary>
   /// Applies a tensor operator to create a new object from the resulting Tensor
   /// </summary>
   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
   /// <typeparam name="TOut">The expected output datatype from the operation</typeparam>
   /// <param name="other">The second image</param>
   /// <param name="operation">The operation to apply to the two images</param>
   /// <returns></returns>
   internal TSelf TrustedOperatorToNew(Func<TTensor, TTensor> operation)
   {
      return UnsafeCreate(operation(Data));
   }

   #endregion


   #region Operators

   private ImageBool<TSpace> Compare(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right, Func<Tensor, Tensor, Tensor> comparison)
   {
      return ImageBool<TSpace>.UnsafeCreateStatic(new BoolTensor(comparison(this._data.Storage, right._data.Storage)));
   }
   private ImageBool<TSpace> Compare(TVoxel right, Func<Tensor, Tensor, Tensor> comparison)
   {
      return ImageBool<TSpace>.UnsafeCreateStatic(new BoolTensor(comparison(this._data.Storage, Data.ScalarToTensor(right))));
   }
   public static ImageBool<TSpace> operator <(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => left.Compare(right, torch.less);
   public static ImageBool<TSpace> operator <=(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => left.Compare(right, torch.less_equal);
   public static ImageBool<TSpace> operator >(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => left.Compare(right, torch.greater);
   public static ImageBool<TSpace> operator >=(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> right) => left.Compare(right, torch.greater_equal);
   public static ImageBool<TSpace> operator >(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, TVoxel right) => left.Compare(right, torch.greater);
   public static ImageBool<TSpace> operator <(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, TVoxel right) => left.Compare(right, torch.less);
   public static ImageBool<TSpace> operator >=(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, TVoxel right) => left.Compare(right, torch.greater_equal);
   public static ImageBool<TSpace> operator <=(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, TVoxel right) => left.Compare(right, torch.less_equal);
   public static TSelf operator +(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> left, TVoxel right) => left.UnsafeCreate(left.Data + right);
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