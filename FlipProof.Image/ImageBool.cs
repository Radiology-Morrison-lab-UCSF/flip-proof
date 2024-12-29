using FlipProof.Base;
using FlipProof.Torch;
using System.Runtime.CompilerServices;
using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch;

namespace FlipProof.Image;

public partial class ImageBool<TSpace> : Image<bool, TSpace, ImageBool<TSpace>, BoolTensor>
   where TSpace : struct, ISpace
{

   private new ImageBool<TSpace> ToBool() => this;


   /// <summary>
   /// Logical Not
   /// </summary>
   public ImageBool<TSpace> Not() => this.TrustedOperatorToNew(Torch.TensorExtensionMethods.Not);
      
   /// <summary>
   /// Logical Not. Returns this object
   /// </summary>
   public ImageBool<TSpace> NotInPlace()
   {
      _data.NotInPlace();
      return this;
   }

   /// <summary>
   /// Applies a tensor operator to create a new object from the resulting Tensor
   /// </summary>
   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
   /// <typeparam name="TOut">The expected output datatype from the operation</typeparam>
   /// <param name="other">The second image</param>
   /// <param name="operation">The operation to apply to the two images</param>
   /// <returns></returns>
   internal ImageBool<TSpace> TrustedOperatorToNew(Func<BoolTensor, BoolTensor> operation)
   {
      return UnsafeCreateStatic(operation(this.Data));
   }


   /// <summary>
   /// Applies a Tensor operator to create a new object from the resulting Tensor
   /// </summary>
   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
   /// <typeparam name="TOut">The expected output datatype from the operation</typeparam>
   /// <param name="other">The second image</param>
   /// <param name="operation">The operation to apply to the two images</param>
   /// <returns></returns>
   internal ImageBool<TSpace> TrustedOperatorToNew(ImageBool<TSpace> other, Func<BoolTensor, BoolTensor, BoolTensor> operation)
   {
      return UnsafeCreateStatic(operation(this.Data, other.Data));
   }

   /// <summary>
   /// Returns a box that tightly includes all true values in voxel space
   /// </summary>
   /// <param name="volume">The volume to inspect</param>
   /// <returns>Bounds of true values</returns>
   public Box<long> GetMaskBoundingBox(long volume)
   {
      using BoolTensor vol = new(ExtractVolumeAsTensor(volume));
      return (Box<long>)vol.GetMaskBounds4D();
   }

   /// <summary>
   /// Returns a box that tightly includes all true values in voxel space
   /// </summary>
   /// <returns>Bounds of true values</returns>
   public Box4D<long> GetMaskBoundingBox() => Data.GetMaskBounds4D();

   #region Operators

   /// <summary>
   /// Bitwise and
   /// </summary>
   public static ImageBool<TSpace> operator &(ImageBool<TSpace> left, ImageBool<TSpace> right) => left.TrustedOperatorToNew(right, Torch.TensorExtensionMethods.And);
   /// <summary>
   /// Bitwise or
   /// </summary>
   public static ImageBool<TSpace> operator |(ImageBool<TSpace> left, ImageBool<TSpace> right) => left.TrustedOperatorToNew(right, Torch.TensorExtensionMethods.Or);
   /// <summary>
   /// Bitwise exclusive or
   /// </summary>
   public static ImageBool<TSpace> operator ^(ImageBool<TSpace> left, ImageBool<TSpace> right) => left.TrustedOperatorToNew(right, Torch.TensorExtensionMethods.XOr);


   #endregion
}
