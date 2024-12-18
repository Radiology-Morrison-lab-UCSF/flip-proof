using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public sealed partial class DoubleTensor : FloatingPointTensor<double, DoubleTensor>, IFloatingPointTensor
{

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
