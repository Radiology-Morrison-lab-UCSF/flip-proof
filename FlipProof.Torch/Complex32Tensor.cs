using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace FlipProof.Torch;

public sealed partial class Complex32Tensor : ComplexNumericTensor<Complex32, Complex32Tensor>
{

   protected override void Set(Complex32 value, params long[] indices) => Storage[indices] = (value.Real, value.Imaginary);

   [CLSCompliant(false)]
   public new static Complex32Tensor CreateTensor(Tensor t, bool wrapCopy) => (Complex32Tensor)NumericTensor<Complex32, Complex32Tensor>.CreateTensor(t, wrapCopy);

   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(Complex32 arr) => torch.tensor(arr.Real, arr.Imaginary, ScalarType.ComplexFloat32);
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(Complex32[] arr) => torch.tensor(arr.Select(a=>(a.Real, a.Imaginary)).ToArray());

#warning needs unit test
   public FloatTensor GetReal() => new(Storage.real.clone());
#warning needs unit test
   public FloatTensor GetImaginary() => new(torch.imag(Storage).clone());

   /// <summary>
   /// Calculates the magnitude of the complex number: sqrt(real^2 + imag^2)
   /// </summary>
   /// <returns></returns>
#warning needs unit test
   public FloatTensor GetMagnitude() => new(Storage.abs());

   [CLSCompliant(false)]
   protected override Complex32 ToScalar(Tensor t) => new(t.ToComplex32());

   public override Complex32[] ToArray()
   {
      var real = Storage.real.ToArray<float>();
      var imag = Storage.imag.ToArray<float>();

      Complex32[] complexes = new Complex32[real.Length];
      for (int i = 0; i < real.Length; i++) 
      {
         complexes[i] = new(real[i], imag[i]);
      }
      return complexes;

   }

   /// <summary>
   /// Performs FFT shift to have negative frequencies first, a periodic shift so that the origin is in the center of the image
   /// </summary>
   /// <param name="dimensions">Dimensions to transform</param>
   public Complex32Tensor FFTShift(long[]? dimensions = null)
   {
#warning needs unit test
      var realImag = torch.fft.fftshift(Storage, dim:dimensions);
      return Complex32Tensor.CreateTensor(realImag, false);
   }

   /// <summary>
   /// Inverse fourier transform
   /// </summary>
   /// <param name="dimensions">Dimensions to transform</param>
   public FloatTensor IFFTN(long[]? dimensions = null)
   {
      using var realImag = torch.fft.ifftn(Storage, dim:dimensions);
      return FloatTensor.CreateTensor(realImag.real, true);
   }

   /// <summary>
   /// Multiplies real by the provided tensor, elementwise, in place
   /// </summary>
   /// <param name="right"></param>
   /// <returns></returns>
   public Complex32Tensor MultiplyInPlace(FloatTensor right)
   {
#warning needs unit test
      Storage.mul_(right.Storage);
      return this;
   }

   //#region Operators
   // TO DO
   //public static Complex32Tensor operator *(Complex32Tensor left, FloatTensor right) => CreateTensor(left.Storage * right.Storage, false);
   //#endregion
}
