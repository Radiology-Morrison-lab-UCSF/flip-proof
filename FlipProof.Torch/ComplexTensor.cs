using static TorchSharp.torch;
using TorchSharp;
using System.Numerics;

namespace FlipProof.Torch;

public class ComplexTensor : NumericTensor<Complex, ComplexTensor>
{

   public ComplexTensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.ComplexFloat64))
   {
   }

   [CLSCompliant(false)]
   public ComplexTensor(Tensor t) : base(t) { }

   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.ComplexFloat64;


   protected override void Set(Complex value, params long[] indices) => _storage[indices] = value;


   protected override ComplexTensor CreateSameSizeBlank() => new(_storage.shape);

   [CLSCompliant(false)]
   protected override ComplexTensor CreateFromTensorSub(Tensor t) => new(t);


   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(Complex arr) => torch.tensor(arr);
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(Complex[] arr) => torch.tensor(arr);

   [CLSCompliant(false)]
   protected override Complex ToScalar(Tensor t) => t.ToSingle();
}
