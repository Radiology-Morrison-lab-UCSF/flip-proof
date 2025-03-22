using TorchSharp;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public abstract class FloatingPointTensor<T, TSelf> : SimpleNumericTensor<T, TSelf>, IFloatingPointTensor
  where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
  where TSelf : NumericTensor<T, TSelf>
{
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public FloatingPointTensor(torch.Tensor t) : base(t)
   {
   }

   public override void FillWithRandom()
   {
      using torch.Tensor rand = torch.rand(Storage.shape, DType);
      Storage.copy_(rand);
   }

   /// <summary>
   /// Replaces NaN with the provided value, returning a new tensor
   /// </summary>
   public TSelf ReplaceNaN(T newVal) => CreateFromTensor(Storage.nan_to_num(nan: Convert.ToDouble(newVal)));

   #region Simple Elementwise Operations

   public TSelf ACos() => CreateFromTensor(Storage.acos());
   public TSelf ACosH() => CreateFromTensor(Storage.acosh());
   public TSelf Angle() => CreateFromTensor(Storage.angle());
   public TSelf ASin() => CreateFromTensor(Storage.asin());
   public TSelf ASinH() => CreateFromTensor(Storage.asinh());
   public TSelf ATan() => CreateFromTensor(Storage.atan());
   public TSelf ATanH() => CreateFromTensor(Storage.atanh());
   public TSelf Ceil() => CreateFromTensor(Storage.ceil());
   public TSelf Cos() => CreateFromTensor(Storage.cos());
   public TSelf CosH() => CreateFromTensor(Storage.cosh());
   public TSelf Deg2Rad() => CreateFromTensor(Storage.deg2rad());
   public TSelf DiGamma() => CreateFromTensor(Storage.digamma());
   public TSelf ErF() => CreateFromTensor(Storage.erf());
   public TSelf ErFC() => CreateFromTensor(Storage.erfc());
   public TSelf ErfFInv() => CreateFromTensor(Storage.erfinv());
   public TSelf Exp() => CreateFromTensor(Storage.exp());
   public TSelf Exp2() => CreateFromTensor(Storage.exp2());
   public TSelf ExpM1() => CreateFromTensor(Storage.expm1());
   public TSelf Floor() => CreateFromTensor(Storage.floor());
   public TSelf Frac() => CreateFromTensor(Storage.frac());
   public TSelf LGamma() => CreateFromTensor(Storage.lgamma());
   public TSelf Log() => CreateFromTensor(Storage.log());
   public TSelf Log10() => CreateFromTensor(Storage.log10());
   public TSelf Log1P() => CreateFromTensor(Storage.log1p());
   public TSelf Log2() => CreateFromTensor(Storage.log2());
   public TSelf Logit() => CreateFromTensor(Storage.logit());
   public TSelf I0() => CreateFromTensor(Storage.i0());
   public TSelf Negative() => CreateFromTensor(Storage.negative());
   public TSelf Rad2Deg() => CreateFromTensor(Storage.rad2deg());
   public TSelf Reciprocal() => CreateFromTensor(Storage.reciprocal());
   /// <summary>
   /// Rounds a copy of the input, returning the new tensor
   /// </summary>
   public TSelf Round() => CreateFromTensor(Storage.round());
   public TSelf RSqrt() => CreateFromTensor(Storage.rsqrt());
   public TSelf Sigmoid() => CreateFromTensor(Storage.sigmoid());
   public TSelf Sign() => CreateFromTensor(Storage.sign());
   public TSelf Sin() => CreateFromTensor(Storage.sin());
   public TSelf Sinc() => CreateFromTensor(Storage.sinc());
   public TSelf SinH() => CreateFromTensor(Storage.sinh());
   public TSelf Sqrt() => CreateFromTensor(Storage.sqrt());
   public TSelf Square() => CreateFromTensor(Storage.square());
   public TSelf Tan() => CreateFromTensor(Storage.tan());
   public TSelf TanH() => CreateFromTensor(Storage.tanh());
   public TSelf Trunc() => CreateFromTensor(Storage.trunc());

   #endregion


   /// <summary>
   /// This divided by the other, and floors the result
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public TSelf FloorDivide(TSelf other) => CreateFromTensor(Storage.floor_divide(other.Storage));
   /// <summary>
   /// Elementwise remainder of division
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public TSelf FMod(TSelf other) => CreateFromTensor(Storage.fmod(other.Storage));

   /// <summary>
   /// Rounds the input and returns it
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="TTensor"></typeparam>
   /// <param name="tensor"></param>
   /// <returns></returns>
   public TSelf RoundInPlace()
   {
      Storage.round_();
      return (this as TSelf)!;
   }

   #region Operators
   public static TSelf operator /(FloatingPointTensor<T, TSelf> left, T right) => CreateTensor(left.Storage / left.CreateScalar(right).Storage, false);
   #endregion
}
