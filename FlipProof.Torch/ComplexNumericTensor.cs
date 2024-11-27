using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public abstract class ComplexNumericTensor<T, TSelf> : NumericTensor<T, TSelf>
  where T : struct
  where TSelf : NumericTensor<T, TSelf>
{
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public ComplexNumericTensor(Tensor t) : base(t) { }


   /// <summary>
   /// The <see cref="ScalarType"/> for the real and imaginary components
   /// </summary>
   [CLSCompliant(false)]
   public virtual ScalarType RealImagDType => DType switch
   {
      ScalarType.ComplexFloat32 => ScalarType.Float32,
      ScalarType.ComplexFloat64 => ScalarType.Float64,
      _ => throw new NotSupportedException(DType.ToString())
   };

   public override void FillWithRandom()
   {
#warning needs unit test
      ScalarType realType = RealImagDType;
      Storage.real.copy_(torch.rand(Storage.shape, realType));
      Storage.imag.copy_(torch.rand(Storage.shape, realType));
   }

}
