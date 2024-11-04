using FlipProof.Torch;

namespace FlipProof.TorchTests;

[TestClass]
public class ArrayAndValueExtensionMethodsTests
{
   [TestMethod]
   public void ToTensor()
   {
      int seed = 9;
      Random r = new(seed);
      int size0 = 13;
      int size1 = 17;
      float[][] data = Enumerable.Repeat(0, size0).Select(a => Enumerable.Repeat(0, size1).Select(a => r.NextSingle()).ToArray()).ToArray();

      using TorchSharp.torch.Tensor tensor = ArrayAndValueExtensionMethods.ToTensor(data);

      CollectionAssert.AreEqual(new long[] { size0, size1 }, tensor.shape);

      for (int i = 0; i < size0; i++)
      {
         var cur = data[i];
         for (int j = 0; j < size1; j++)
         {
            Assert.AreEqual(cur[j], tensor[i, j].ReadCpuValue<float>(0));
         }
      }
   }

}
