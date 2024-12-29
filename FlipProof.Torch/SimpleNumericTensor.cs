using System.Diagnostics.CodeAnalysis;
using TorchSharp;
using static TorchSharp.torch;

namespace FlipProof.Torch;

/// <summary>
/// Tensor for simple numeric types, like double and int, but not Complex
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TSelf"></typeparam>
public abstract class SimpleNumericTensor<T,TSelf> : NumericTensor<T,TSelf>
  where T : struct
  where TSelf : NumericTensor<T, TSelf>
{
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public SimpleNumericTensor(torch.Tensor t) : base(t)
   {
   }

   /// <summary>
   /// Returns a new <see cref="TSelf"/> with absolute values
   /// </summary>
   /// <returns></returns>
   public virtual TSelf Abs() => CreateFromTensor(Storage.abs());
   /// <summary>
   /// Converts to absolute values in place
   /// </summary>
   /// <returns></returns>
   public virtual TSelf AbsInPlace() => CreateFromTensor(Storage.abs_());

   /// <summary>
   /// Forward fourier transform, returning single precision. Avoid usage on types that cannot be encoded by floats
   /// </summary>
   /// <param name="dimensions">Which dimensions to transform</param>
   protected Complex32Tensor FFTN(long[]? dimensions = null)
   {
      torch.Tensor result = torch.fft.fftn(Storage, dim:dimensions);
      if(result.dtype == torch.ScalarType.ComplexFloat64)
      {
         // when T is double
         result = result.to_type(torch.ScalarType.ComplexFloat32, disposeAfter:true);
      }
      return (Complex32Tensor)Complex32Tensor.CreateTensor(result, false);
   }
   /// <summary>
   /// Forward fourier transform, returning double precision
   /// </summary>
   /// <param name="dimensions">Which dimensions to transform</param>
   protected ComplexTensor FFTN_Double(long[]? dimensions = null)
   {
      torch.Tensor result = torch.fft.fftn(Storage, dim:dimensions);
      if(result.dtype == torch.ScalarType.ComplexFloat32)
      {
         // when T is float
         result = result.to_type(torch.ScalarType.ComplexFloat64, disposeAfter:true);
      }
      return (ComplexTensor)ComplexTensor.CreateTensor(result, false);
   }

   /// Creates <see cref="TSelf"/> that wraps the input <see cref="Tensor"/>. Casting is not allowed
   /// </summary>
   /// <param name="t">Internal data. Type must must match T.</param>
   /// <param name="wrapCopy">If true, a copy of the tensor is wrapped</param>
   /// <exception cref="NotSupportedException">The type of T is not supported</exception>
   [CLSCompliant(false)]
   public new static TSelf CreateTensor(Tensor t, bool wrapCopy) => (TSelf)Tensor<T>.CreateTensor(t, wrapCopy);

   public static TSelf operator +(SimpleNumericTensor<T, TSelf> left, T right) => left + left.CreateScalar(right);
   public static TSelf operator +(SimpleNumericTensor<T, TSelf> left, Tensor<T> right) => CreateTensor(left.Storage + right.Storage, false);
   public static TSelf operator -(SimpleNumericTensor<T, TSelf> left, T right) => left - left.CreateScalar(right);
   public static TSelf operator -(SimpleNumericTensor<T, TSelf> left, Tensor<T> right) => CreateTensor(left.Storage - right.Storage, false);
   public static TSelf operator *(SimpleNumericTensor<T, TSelf> left, T right) => left * left.CreateScalar(right);
   public static TSelf operator *(SimpleNumericTensor<T, TSelf> left, Tensor<T> right) => CreateTensor(left.Storage * right.Storage, false);
   public static DoubleTensor operator +(SimpleNumericTensor<T, TSelf> left, DoubleTensor right) => new(left.Storage + right.Storage);
   public static DoubleTensor operator +(DoubleTensor left, SimpleNumericTensor<T, TSelf> right) => new(left.Storage + right.Storage);
   public static DoubleTensor operator -(SimpleNumericTensor<T, TSelf> left, DoubleTensor right) => new(left.Storage - right.Storage);
   public static DoubleTensor operator -(DoubleTensor left, SimpleNumericTensor<T, TSelf> right) => new(left.Storage - right.Storage);
   public static DoubleTensor operator *(SimpleNumericTensor<T, TSelf> left, DoubleTensor right) => new(left.Storage * right.Storage);
   public static DoubleTensor operator *(DoubleTensor left, SimpleNumericTensor<T, TSelf> right) => new(left.Storage * right.Storage);
   public static DoubleTensor operator /(SimpleNumericTensor<T, TSelf> left, DoubleTensor right) => new(left.Storage / right.Storage);
   public static DoubleTensor operator /(DoubleTensor left, SimpleNumericTensor<T, TSelf> right) => new(left.Storage / right.Storage);


}
