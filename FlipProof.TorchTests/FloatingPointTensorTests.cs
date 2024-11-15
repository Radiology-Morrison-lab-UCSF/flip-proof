using FlipProof.Base;
using FlipProof.Torch;
using System.Numerics;
using TorchSharp;

namespace FlipProof.TorchTests;

public abstract class FloatingPointTensorTests<T,TTensor>
   where T : struct, IFloatingPoint<T>, IFloatingPointIeee754<T>
   where TTensor : SimpleNumericTensor<T, TTensor>, IFloatingPointTensor
{
   protected abstract TTensor ToTensor(T[] inputs);
   static T Cast(int t) => (T)Convert.ChangeType(t, typeof(T));
   static T[] Cast(int[] t) => t.Select(Cast).ToArray();
   

   [TestMethod]
   public void ReplaceNaN()
   {
      T[] vals = [T.NaN, T.NaN, Cast(5), Cast(7), Cast(11), T.NaN];
      T[] expected = Cast([13, 13, 5, 7, 11, 13]);

      using TTensor tensor = ToTensor(vals);

      TTensor result = tensor.ReplaceNaN(Cast(13));

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