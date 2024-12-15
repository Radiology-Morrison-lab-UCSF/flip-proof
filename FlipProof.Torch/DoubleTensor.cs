using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public class DoubleTensor : FloatingPointTensor<double, DoubleTensor>, IFloatingPointTensor
{
   [SetsRequiredMembers]
   public DoubleTensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Float64))
   {
   }

   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public DoubleTensor(Tensor t) : base(t) { }

   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Float64;


   protected override void Set(double value, params long[] indices) => Storage[indices] = value;


   [CLSCompliant(false)]
   protected override DoubleTensor CreateFromTensorSub(Tensor t) => new(t);

   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(double arr) => torch.tensor(arr);

   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(double[] arr) => torch.tensor(arr);

   [CLSCompliant(false)]
   protected override double ToScalar(Tensor t) => t.ToDouble();

   /// <summary>
   /// Forward fourier transform
   /// </summary>
   public new ComplexTensor FFTN() => ComplexTensor.CreateTensor(torch.fft.fftn(Storage), false);


   #region Operators
   // First three are to remove ambiguity the compiler trips over
   public static DoubleTensor operator +(DoubleTensor left, DoubleTensor right) => new(left.Storage + right.Storage);
   public static DoubleTensor operator -(DoubleTensor left, DoubleTensor right) => new(left.Storage - right.Storage);
   public static DoubleTensor operator *(DoubleTensor left, DoubleTensor right) => new(left.Storage * right.Storage);
   public static DoubleTensor operator /(DoubleTensor left, DoubleTensor right) => new(left.Storage / right.Storage);

   public static DoubleTensor operator *(DoubleTensor left, double right) => new(left.Storage.multiply(right));
   public static DoubleTensor operator *(double left, DoubleTensor right) => new(right.Storage.multiply(left));
   public static DoubleTensor operator /(DoubleTensor left, SimpleNumericTensor<double, DoubleTensor> right) => new(left.Storage / right.Storage);
   public static DoubleTensor operator /(SimpleNumericTensor<double, DoubleTensor> left, DoubleTensor right) => new(left.Storage / right.Storage);
   #endregion
}
