using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using TorchSharp;
using TorchSharp.Modules;
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
   public virtual TSelf AbsInPlace()
   {
      Storage.abs_(); 
      return (this as TSelf)!;
   }

   #region Statistics
   /// <summary>
   /// Mean over all elements
   /// </summary>
   /// <returns></returns>
   public double Mean()
   {
      using Tensor t = Storage.mean(ListDimensions(), type: ScalarType.Float64);
      return t.ToDouble();
   }
   /// <summary>
   /// Std Dev over all elements
   /// </summary>
   /// <param name="populationStdDev">Divides by n-1, not n to obtain the unbiased population std dev</param>
   /// <returns></returns>
   public double StdDev(bool populationStdDev=true)
   {
      Tensor asD = Storage.@double(); // std is only supported for double, float, complex, and does not calculate at double precision for float
      try
      {
         using Tensor t = asD.std(ListDimensions(), unbiased: populationStdDev);
         return t.ToDouble();
      }
      finally
      {
         if (!object.ReferenceEquals(asD, Storage))
         {
            asD.Dispose();
         }
      }
   }

   /// <summary>
   /// Returns an array of dimensions this contains
   /// </summary>
   private long[] ListDimensions()
   {
      long[] longs = new long[this.Shape.Length];
      for (long i = 0; i < longs.Length; i++)
      {
         longs[i] = i;
      }
      return longs;
   }

   #endregion

   /// <summary>
   /// Applies a 3D kernel returning a new object cast to the same type as this
   /// </summary>
   /// <remarks>To force the type returned, cast this to the desired output type first</remarks>
   /// <param name="kernel"></param>
   /// <returns>A new <see cref="TSelf"/></returns>
   /// <exception cref="ArgumentException">Kernel is the wrong shape</exception>
   [CLSCompliant(false)]
   public TSelf Apply3DKernel<S, TKernel>(SimpleNumericTensor<S, TKernel> kernel)
      where S : struct
      where TKernel : SimpleNumericTensor<S, TKernel>
   {
      if (kernel.NDims != 3)
      {
         throw new ArgumentException($"Expected 3D kernel but got {kernel.NDims}D");
      }
      return CreateFromTensor(nn.functional.conv3d(Storage, kernel.Storage), allowCast: true);
   }

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
