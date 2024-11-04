using static TorchSharp.torch;
using TorchSharp;
using System.Numerics;

namespace FlipProof.Torch;

public class ComplexTensor : NumericTensor<Complex, ComplexTensor>
{

   public ComplexTensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.ComplexFloat64))
   {
   }

   public ComplexTensor(Tensor t) : base(t) { }

   public override ScalarType DType => ScalarType.ComplexFloat64;


   protected override void Set(Complex value, params long[] indices) => _storage[indices] = value;


   protected override ComplexTensor CreateSameSizeBlank() => new(_storage.shape);

   protected override ComplexTensor CreateFromTensorSub(Tensor t) => new(t);


   public override Tensor ScalarToTensor(Complex arr) => torch.tensor(arr);
   public override Tensor ArrayToTensor(Complex[] arr) => torch.tensor(arr);

   protected override Complex ToScalar(Tensor t) => t.ToSingle();
}
