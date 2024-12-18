using static TorchSharp.torch;
using TorchSharp;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public sealed partial class ComplexTensor : ComplexNumericTensor<Complex, ComplexTensor>
{

   [CLSCompliant(false)]
   public new static ComplexTensor CreateTensor(Tensor t, bool wrapCopy) => (ComplexTensor)NumericTensor<Complex, ComplexTensor>.CreateTensor(t, wrapCopy);

   [CLSCompliant(false)]
   protected override Complex ToScalar(Tensor t) => t.ToSingle();


   /// <summary>
   /// Inverse fourier transform
   /// </summary>
   /// <param name="dimensions">Dimensions to transform</param>
   public DoubleTensor IFFTN(long[]? dimensions = null)
   {
      using var realImag = torch.fft.ifftn(Storage, dim: dimensions);
      return DoubleTensor.CreateTensor(realImag.real, true);
   }

   /// <summary>
   /// Angle in radians
   /// </summary>
   /// <returns></returns>
   public DoubleTensor Angle() => DoubleTensor.CreateTensor(torch.angle(Storage), false);
}
