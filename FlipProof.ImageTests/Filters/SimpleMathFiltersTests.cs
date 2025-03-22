using FlipProof.Base;
using FlipProof.Image;
using FlipProof.Image.Filters;
using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.ImageTests.Filters;

[TestClass]
public class SimpleMathFiltersTests
{
   struct TestSpace : ISpace
   {
   }
   [TestInitialize]
   public void Initialise()
   {
      ISpace.Debug_Clear<TestSpace>();
   }
      [TestMethod]
   public void NormaliseIntensity()
   {
      ImageDouble<TestSpace> input = GenerateRandomImage() * 10 + 131;

      double origMean = input.Mean();
      double origStdev = input.StdDev();

      Assert.IsTrue(origMean >= 135);//should be about 136
      Assert.IsTrue(origStdev >= 2.8);// should be about 2.87 if the image is big

      ImageDouble<TestSpace> result = input.IntensityNormalise();
      Assert.AreEqual(0, result.Mean(), 1e-3);
      Assert.AreEqual(1, result.StdDev(), 1e-3);

      // undo the scaling/offset and check images are the same
      var undone = (result * origStdev + origMean); 
      ImageDouble<TestSpace> diff = (input - undone).AbsInPlace();
      Assert.IsTrue(diff.GetMaxIntensity() < 1e-6);

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

   private static ImageDouble<TestSpace> GenerateRandomImage()
   {
      var head = new ImageHeader(new ImageSize(51, 71, 67, 4), new OrientationMatrix(new FlipProof.Image.Matrices.Matrix4x4_Optimised<double>(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)),
          CoordinateSystem.RAS, EncodingDirection.UnknownOrNA, EncodingDirection.UnknownOrNA, EncodingDirection.UnknownOrNA);

      double[] voxels = new double[head.Size.VoxelCount];
      voxels.FillArray(Random.Shared.NextDouble);

      var input = new ImageDouble<TestSpace>(head, voxels);
      return input;
   }

}
