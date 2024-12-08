using static TorchSharp.torch;
using TorchSharp;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public class ComplexTensor : ComplexNumericTensor<Complex, ComplexTensor>
{
   [SetsRequiredMembers]
   public ComplexTensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.ComplexFloat64))
   {
   }

   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public ComplexTensor(Tensor t) : base(t) { }

   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.ComplexFloat64;


   protected override void Set(Complex value, params long[] indices) => Storage[indices] = value;

   [CLSCompliant(false)]
   public new static ComplexTensor CreateTensor(Tensor t, bool wrapCopy) => (ComplexTensor)NumericTensor<Complex, ComplexTensor>.CreateTensor(t, wrapCopy);

   [CLSCompliant(false)]
   protected override ComplexTensor CreateFromTensorSub(Tensor t) => new(t);


   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(Complex arr) => torch.tensor(arr);
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(Complex[] arr) => torch.tensor(arr);

   [CLSCompliant(false)]
   protected override Complex ToScalar(Tensor t) => t.ToSingle();


   /// <summary>
   /// Inverse fourier transform
   /// </summary>
   public DoubleTensor IFFTN()
   {
      using var realImag = torch.fft.ifftn(Storage);
      return DoubleTensor.CreateTensor(realImag.real, true);
   }

   /// <summary>
   /// Angle in radians
   /// </summary>
   /// <returns></returns>
   public DoubleTensor Angle() => DoubleTensor.CreateTensor(torch.angle(Storage), false);
}
