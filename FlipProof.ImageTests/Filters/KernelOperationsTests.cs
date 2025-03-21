using FlipProof.Base;
using FlipProof.Image;
using FlipProof.Image.Filters;
using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.ImageTests.Filters;

[TestClass]
public class KernelOperationsTests
{
   struct TestSpace : ISpace
   {
   }


   class RandomKernel : IKernel<double>
   {
      public required XYZ<int> KernelExtent { get; init; }

      double IKernel<double>.GetIntensity(int offsetX, int offsetY, int offsetZ)
      {
         return Random.Shared.NextDouble();
      }
   }

   [TestMethod]
   [DataRow(3, 3, 3)]
   [DataRow(7, 11, 15)]
   [DataRow(15, 27, 25)]
   public void ApplyFFTKernel_Random(int kernSizeX, int kernSizeY, int kernSizeZ)
   {
      DoubleTensor inputTensor = new([11, 17, 23, 4]);
      inputTensor.FillWithRandom();

      DoubleTensor inputCopy = inputTensor.DeepClone();

      DoubleTensor result = KernelOperations.ApplyKernelViaFFT(inputTensor, new RandomKernel() { KernelExtent = new(kernSizeX, kernSizeY, kernSizeZ) });

      Assert.AreNotSame(inputTensor, result);
      Assert.AreNotSame(inputTensor.Storage, result.Storage);
      Assert.IsTrue(inputTensor.Storage.eq(inputCopy.Storage).all().ToBoolean(), "Input modified in place");
      CollectionAssert.AreEqual(inputTensor.Storage.shape, result.Storage.shape);
      Assert.IsFalse(inputTensor.Storage.eq(result.Storage).all().ToBoolean());
   }

   class DoNothingKernel : IKernel<float>
   {
      XYZ<int> IKernel<float>.KernelExtent => new(13,11,17);

      float IKernel<float>.GetIntensity(int offsetX, int offsetY, int offsetZ)
      {
         if (offsetX == 0 && 
            offsetY == 0 && 
            offsetZ == 0)
         {
            return 1;
         }
         return 0;
      }
   }

   [TestMethod]   
   public void ApplyFFTKernel_DoNothing()
   {


      FloatTensor inputTensor = new FloatTensor([11, 17, 23, 4]);
      inputTensor.FillWithRandom();

      FloatTensor result = KernelOperations.ApplyKernelViaFFT(inputTensor, new DoNothingKernel());

      Assert.AreNotSame(inputTensor, result);
      Assert.AreNotSame(inputTensor.Storage, result.Storage);
      CollectionAssert.AreEqual(inputTensor.Storage.shape, result.Storage.shape);
      Assert.IsTrue(inputTensor.Storage.subtract(result.Storage).abs().less_equal(1e-5).all().ToBoolean());
      Assert.IsTrue(inputTensor.Storage.max().ToDouble() > 0.5, "Comparison delta above assumes values are between 0 and 1");

   }

}
