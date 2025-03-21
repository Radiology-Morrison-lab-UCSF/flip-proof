﻿using FlipProof.Base;
using FlipProof.Torch;
using System.Numerics;
using TorchSharp;
using static TorchSharp.torch.utils;
using TorchSharp.Modules;

namespace FlipProof.TorchTests;

public abstract class FloatingPointTensorTests<T,TTensor>
   where T : struct, IFloatingPoint<T>, IFloatingPointIeee754<T>, IMinMaxValue<T>
   where TTensor : FloatingPointTensor<T, TTensor>, IFloatingPointTensor
{
   protected abstract TTensor ToTensor(T[] inputs);
   static T Cast(int t) => (T)Convert.ChangeType(t, typeof(T));
   static T Cast(float t) => (T)Convert.ChangeType(t, typeof(T));
   static T Cast(double t) => (T)Convert.ChangeType(t, typeof(T));
   static T[] Cast(int[] t) => t.Select(Cast).ToArray();
   static T[] Cast(double[] t) => t.Select(Cast).ToArray();
   

   [TestMethod]
   public void ReplaceNaN()
   {
      T[] vals = [T.NaN, T.NaN, Cast(5), Cast(7), Cast(11), T.NaN];
      T[] expected = Cast(new int[] { 13, 13, 5, 7, 11, 13 });

      using TTensor tensor = ToTensor(vals);
      using TTensor result = tensor.ReplaceNaN(Cast(13));

      Assert.AreNotSame(tensor, result);
      CollectionAssert.AreEqual(expected, result.ToArray());
   }

   #region Simple Elementwise Operations

   void SimpleElementwiseStabilityTest(Func<TTensor,TTensor> act, bool isInPlace=false)
   {
      T[] vals = [Cast(1), Cast(-1f), Cast(5), Cast(7.51f), Cast(11), Cast(55.5f)];

      TTensor input = ToTensor(vals);
      TTensor orig = (TTensor)(object)input.DeepClone();
      TTensor result = act(input);

      if (isInPlace)
      {
         Assert.AreSame(input, result, "Does not return input");
      }
      else
      {
         Assert.AreNotSame(input, result, "Returns input");
      }
      
      Assert.AreEqual(!isInPlace, input.ValuewiseEquals(orig).All(), isInPlace ? "Input not altered" : "Input altered");

      Assert.IsTrue(input.ShapesEqual(result));
   }

   [TestMethod]
   public void Abs() => SimpleElementwiseStabilityTest(a => a.Abs());
   [TestMethod]
   public void AbsInPlace() => SimpleElementwiseStabilityTest(a => a.AbsInPlace(), true);

   [TestMethod]
   public void ACos() => SimpleElementwiseStabilityTest(a=>a.ACos());
   [TestMethod]
   public void ACosH() => SimpleElementwiseStabilityTest(a=>a.ACosH());
   [TestMethod]
   public void Angle() => SimpleElementwiseStabilityTest(a=>a.Angle());
   [TestMethod]
   public void ASin() => SimpleElementwiseStabilityTest(a=>a.ASin());
   [TestMethod]
   public void ASinH() => SimpleElementwiseStabilityTest(a=>a.ASinH());
  [TestMethod] 
   public void ATan() => SimpleElementwiseStabilityTest(a=>a.ATan());
  [TestMethod] 
   public void ATanH() => SimpleElementwiseStabilityTest(a=>a.ATanH());
  [TestMethod] 
   public void Ceil() => SimpleElementwiseStabilityTest(a=>a.Ceil());
  [TestMethod] 
   public void Cos() => SimpleElementwiseStabilityTest(a=>a.Cos());
  [TestMethod] 
   public void CosH() => SimpleElementwiseStabilityTest(a=>a.CosH());
  [TestMethod] 
   public void Deg2Rad() => SimpleElementwiseStabilityTest(a=>a.Deg2Rad());
  [TestMethod] 
   public void DiGamma() => SimpleElementwiseStabilityTest(a=>a.DiGamma());
  [TestMethod] 
   public void ErF() => SimpleElementwiseStabilityTest(a=>a.ErF());
  [TestMethod]
   public void ErFC() => SimpleElementwiseStabilityTest(a=>a.ErFC());
  [TestMethod] 
   public void ErfFInv() => SimpleElementwiseStabilityTest(a=>a.ErfFInv());
  [TestMethod] 
   public void Exp() => SimpleElementwiseStabilityTest(a=>a.Exp());
  [TestMethod] 
   public void Exp2() => SimpleElementwiseStabilityTest(a=>a.Exp2());
  [TestMethod] 
   public void ExpM1() => SimpleElementwiseStabilityTest(a=>a.ExpM1());
  [TestMethod] 
   public void Floor() => SimpleElementwiseStabilityTest(a=>a.Floor());
  [TestMethod] 
   public void Frac() => SimpleElementwiseStabilityTest(a=>a.Frac());
  [TestMethod] 
   public void LGamma() => SimpleElementwiseStabilityTest(a=>a.LGamma());
  [TestMethod]
   public void Log() => SimpleElementwiseStabilityTest(a=>a.Log());
  [TestMethod] 
   public void Log10() => SimpleElementwiseStabilityTest(a=>a.Log10());
  [TestMethod] 
   public void Log1P() => SimpleElementwiseStabilityTest(a=>a.Log1P());
  [TestMethod] 
   public void Log2() => SimpleElementwiseStabilityTest(a=>a.Log2());
  [TestMethod] 
   public void Logit() => SimpleElementwiseStabilityTest(a=>a.Logit());
  [TestMethod] 
   public void I0() => SimpleElementwiseStabilityTest(a=>a.I0());
  [TestMethod] 
   public void Negative() => SimpleElementwiseStabilityTest(a=>a.Negative());
  [TestMethod] 
   public void Rad2Deg() => SimpleElementwiseStabilityTest(a=>a.Rad2Deg());
  [TestMethod] 
   public void Reciprocal() => SimpleElementwiseStabilityTest(a=>a.Reciprocal());
  [TestMethod] 
   public void Round() => SimpleElementwiseStabilityTest(a=>a.Round());
  [TestMethod] 
   public void RSqrt() => SimpleElementwiseStabilityTest(a=>a.RSqrt());
  [TestMethod] 
   public void Sigmoid() => SimpleElementwiseStabilityTest(a=>a.Sigmoid());
  [TestMethod] 
   public void Sign() => SimpleElementwiseStabilityTest(a=>a.Sign());
  [TestMethod] 
   public void Sin() => SimpleElementwiseStabilityTest(a=>a.Sin());
  [TestMethod] 
   public void Sinc() => SimpleElementwiseStabilityTest(a=>a.Sinc());
  [TestMethod] 
   public void SinH() => SimpleElementwiseStabilityTest(a=>a.SinH());
  [TestMethod] 
   public void Sqrt() => SimpleElementwiseStabilityTest(a=>a.Sqrt());
  [TestMethod] 
   public void Square() => SimpleElementwiseStabilityTest(a=>a.Square());
  [TestMethod]
   public void Tan() => SimpleElementwiseStabilityTest(a=>a.Tan());
  [TestMethod]
   public void TanH() => SimpleElementwiseStabilityTest(a=>a.TanH());
  [TestMethod] 
   public void Trunc() => SimpleElementwiseStabilityTest(a=>a.Trunc());


   #endregion

   [TestMethod]
   public void DivideOperator()
   {
      T[] input = Cast([13, 17, 5, 19, 191, float.MaxValue]);
      double denominator = 7d;
      T[] expected = Cast([13/ denominator, 17 / denominator, 5 / denominator, 19 / denominator, 191 / denominator, float.MaxValue / denominator]);

      using TTensor tensor = ToTensor(input);
      using TTensor result = tensor / Cast(denominator);

      Assert.AreNotSame(tensor, result);
      CollectionAssert.AreEqual(expected, result.ToArray());
   }
}


[TestClass]
public class DoubleTensorFloatingPointTests : FloatingPointTensorTests<double, DoubleTensor>
{
   protected override DoubleTensor ToTensor(double[] inputs) => new(torch.tensor(inputs));

}
[TestClass]
public class FloatTensorFloatingPointTests : FloatingPointTensorTests<float, FloatTensor>
{
   protected override FloatTensor ToTensor(float[] inputs) => new(torch.tensor(inputs));

}