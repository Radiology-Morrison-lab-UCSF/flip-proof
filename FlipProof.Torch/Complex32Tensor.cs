using static TorchSharp.torch;
using TorchSharp;
using System.Numerics;

namespace FlipProof.Torch;

public class Complex32Tensor : NumericTensor<Complex32, Complex32Tensor>
{

   public Complex32Tensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.ComplexFloat32))
   {
   }

   [CLSCompliant(false)]
   public Complex32Tensor(Tensor t) : base(t) { }

   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.ComplexFloat32;


   protected override void Set(Complex32 value, params long[] indices) => _storage[indices] = (value.Real, value.Imaginary);


   protected override Complex32Tensor CreateSameSizeBlank() => new(_storage.shape);

   [CLSCompliant(false)]
   protected override Complex32Tensor CreateFromTensorSub(Tensor t) => new(t);

   [CLSCompliant(false)]
   public new static Complex32Tensor CreateTensor(Tensor t, bool wrapCopy) => (Complex32Tensor)NumericTensor<Complex32, Complex32Tensor>.CreateTensor(t, wrapCopy);

   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(Complex32 arr) => torch.tensor(arr.Real, arr.Imaginary, ScalarType.ComplexFloat32);
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(Complex32[] arr) => torch.tensor(arr.Select(a=>(a.Real, a.Imaginary)).ToArray());

   [CLSCompliant(false)]
   protected override Complex32 ToScalar(Tensor t) => new Complex32(t.ToComplex32());

   /// <summary>
   /// Inverse fourier transform
   /// </summary>
   public FloatTensor IFFTN()
   {
      using var realImag = torch.fft.ifftn(Storage);
      return FloatTensor.CreateTensor(realImag.real, true);
   }

}
