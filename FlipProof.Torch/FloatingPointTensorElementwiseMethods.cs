using TorchSharp;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public abstract partial class FloatingPointTensor<T, TSelf>
{
   public TSelf ACos() => CreateFromTensor(Storage.acos(), true);
   public TSelf ACosH() => CreateFromTensor(Storage.acosh(), true);
   public TSelf Angle() => CreateFromTensor(Storage.angle(), true);
   public TSelf ASin() => CreateFromTensor(Storage.asin(), true);
   public TSelf ASinH() => CreateFromTensor(Storage.asinh(), true);
   public TSelf ATan() => CreateFromTensor(Storage.atan(), true);
   public TSelf ATanH() => CreateFromTensor(Storage.atanh(), true);
   public TSelf Ceil() => CreateFromTensor(Storage.ceil(), true);
   public TSelf Cos() => CreateFromTensor(Storage.cos(), true);
   public TSelf CosH() => CreateFromTensor(Storage.cosh(), true);
   public TSelf Deg2Rad() => CreateFromTensor(Storage.deg2rad(), true);
   public TSelf DiGamma() => CreateFromTensor(Storage.digamma(), true);
   public TSelf ErF() => CreateFromTensor(Storage.erf(), true);
   public TSelf ErFC() => CreateFromTensor(Storage.erfc(), true);
   public TSelf ErfFInv() => CreateFromTensor(Storage.erfinv(), true);
   public TSelf Exp() => CreateFromTensor(Storage.exp(), true);
   public TSelf Exp2() => CreateFromTensor(Storage.exp2(), true);
   public TSelf ExpM1() => CreateFromTensor(Storage.expm1(), true);
   public TSelf Floor() => CreateFromTensor(Storage.floor(), true);
   public TSelf Frac() => CreateFromTensor(Storage.frac(), true);
   public TSelf LGamma() => CreateFromTensor(Storage.lgamma(), true);
   public TSelf Log() => CreateFromTensor(Storage.log(), true);
   public TSelf Log10() => CreateFromTensor(Storage.log10(), true);
   public TSelf Log1P() => CreateFromTensor(Storage.log1p(), true);
   public TSelf Log2() => CreateFromTensor(Storage.log2(), true);
   public TSelf Logit() => CreateFromTensor(Storage.logit(), true);
   public TSelf I0() => CreateFromTensor(Storage.i0(), true);
   public TSelf Negative() => CreateFromTensor(Storage.negative(), true);
   public TSelf Rad2Deg() => CreateFromTensor(Storage.rad2deg(), true);
   public TSelf Reciprocal() => CreateFromTensor(Storage.reciprocal(), true);
   /// <summary>
   /// Rounds a copy of the input, returning the new tensor
   /// </summary>
   public TSelf Round() => CreateFromTensor(Storage.round(), true);
   public TSelf RSqrt() => CreateFromTensor(Storage.rsqrt(), true);
   public TSelf Sigmoid() => CreateFromTensor(Storage.sigmoid(), true);
   public TSelf Sign() => CreateFromTensor(Storage.sign(), true);
   public TSelf Sin() => CreateFromTensor(Storage.sin(), true);
   public TSelf Sinc() => CreateFromTensor(Storage.sinc(), true);
   public TSelf SinH() => CreateFromTensor(Storage.sinh(), true);
   public TSelf Sqrt() => CreateFromTensor(Storage.sqrt(), true);
   public TSelf Square() => CreateFromTensor(Storage.square(), true);
   public TSelf Tan() => CreateFromTensor(Storage.tan(), true);
   public TSelf TanH() => CreateFromTensor(Storage.tanh(), true);
   public TSelf Trunc() => CreateFromTensor(Storage.trunc(), true);

   /// <summary>
   /// This divided by the other, and floors the result
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public TSelf FloorDivide(TSelf other) => CreateFromTensor(Storage.floor_divide(other.Storage), true);
   /// <summary>
   /// Elementwise remainder of division
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public TSelf FMod(TSelf other) => CreateFromTensor(Storage.fmod(other.Storage), true);


}
