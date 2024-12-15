using FlipProof.Torch;
using System.Numerics;
using static TorchSharp.torch;

namespace FlipProof.Image;

public abstract class Image_FloatingPoint<TVoxel, TSpace, TSelf, TTensor> : Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor>
   where TVoxel : struct, IFloatingPointIeee754<TVoxel>, IMinMaxValue<TVoxel>
   where TSpace : struct, ISpace
   where TTensor : FloatingPointTensor<TVoxel, TTensor>, IFloatingPointTensor
   where TSelf : Image_FloatingPoint<TVoxel, TSpace, TSelf, TTensor>
{
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal Image_FloatingPoint(ImageHeader header, Tensor voxels):base(header, voxels)
   {

   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal Image_FloatingPoint(TTensor voxels, bool verifyShape):base(voxels, verifyShape)
   {

   }

   #region Simple Elementwise Operations

   public TSelf ACos() => UnsafeCreate(Data.ACos());
   public TSelf ACosH() => UnsafeCreate(Data.ACosH());
   public TSelf Angle() => UnsafeCreate(Data.Angle());
   public TSelf ASin() => UnsafeCreate(Data.ASin());
   public TSelf ASinH() => UnsafeCreate(Data.ASinH());
   public TSelf ATan() => UnsafeCreate(Data.ATan());
   public TSelf ATanH() => UnsafeCreate(Data.ATanH());
   public TSelf Ceil() => UnsafeCreate(Data.Ceil());
   public TSelf Cos() => UnsafeCreate(Data.Cos());
   public TSelf CosH() => UnsafeCreate(Data.CosH());
   public TSelf Deg2Rad() => UnsafeCreate(Data.Deg2Rad());
   public TSelf DiGamma() => UnsafeCreate(Data.DiGamma());
   public TSelf ErF() => UnsafeCreate(Data.ErF());
   public TSelf ErFC() => UnsafeCreate(Data.ErFC());
   public TSelf ErfFInv() => UnsafeCreate(Data.ErfFInv());
   public TSelf Exp() => UnsafeCreate(Data.Exp());
   public TSelf Exp2() => UnsafeCreate(Data.Exp2());
   public TSelf ExpM1() => UnsafeCreate(Data.ExpM1());
   public TSelf Floor() => UnsafeCreate(Data.Floor());
   public TSelf Frac() => UnsafeCreate(Data.Frac());
   public TSelf LGamma() => UnsafeCreate(Data.LGamma());
   public TSelf Log() => UnsafeCreate(Data.Log());
   public TSelf Log10() => UnsafeCreate(Data.Log10());
   public TSelf Log1P() => UnsafeCreate(Data.Log1P());
   public TSelf Log2() => UnsafeCreate(Data.Log2());
   public TSelf Logit() => UnsafeCreate(Data.Logit());
   public TSelf I0() => UnsafeCreate(Data.I0());
   public TSelf Negative() => UnsafeCreate(Data.Negative());
   public TSelf Rad2Deg() => UnsafeCreate(Data.Rad2Deg());
   public TSelf Reciprocal() => UnsafeCreate(Data.Reciprocal());
   /// <summary>
   /// Creates a new <see cref="TSelf"/> with rounded voxel values
   /// </summary>
   /// <returns></returns>
   public TSelf Round() => UnsafeCreate(Data.Round());
   public TSelf RSqrt() => UnsafeCreate(Data.RSqrt());
   public TSelf Sigmoid() => UnsafeCreate(Data.Sigmoid());
   public TSelf Sign() => UnsafeCreate(Data.Sign());
   public TSelf Sin() => UnsafeCreate(Data.Sin());
   public TSelf Sinc() => UnsafeCreate(Data.Sinc());
   public TSelf SinH() => UnsafeCreate(Data.SinH());
   public TSelf Sqrt() => UnsafeCreate(Data.Sqrt());
   public TSelf Square() => UnsafeCreate(Data.Square());
   public TSelf Tan() => UnsafeCreate(Data.Tan());
   public TSelf TanH() => UnsafeCreate(Data.TanH());
   public TSelf Trunc() => UnsafeCreate(Data.Trunc());


   #endregion

   /// <summary>
   /// Rounds voxel values in place
   /// </summary>
   /// <returns></returns>
   public TSelf RoundInPlace()
   {
      Data.RoundInPlace();
      return (this as TSelf)!;
   }

   public static TSelf operator -(Image_FloatingPoint<TVoxel, TSpace, TSelf, TTensor> left, TVoxel right) => left.UnsafeCreate(left.Data - right);
   public static TSelf operator +(Image_FloatingPoint<TVoxel, TSpace, TSelf, TTensor> left, TVoxel right) => left.UnsafeCreate(left.Data + right);

}
