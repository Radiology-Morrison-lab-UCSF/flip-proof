using FlipProof.Image;
using FlipProof.Image.Filters;
using TorchSharp;

namespace FlipProof.ImageTests.Filters;

[TestClass]
public class KernelBasedFiltersTests
{
   struct SmoothTestSpace : ISpace<SmoothTestSpace3D>
   {
   }
   struct SmoothTestSpace3D : ISpace3D
   {
   }

   [TestMethod]
   public void Smooth()
   {
      var head = new ImageHeader(new ImageSize(51, 71, 67, 4), new OrientationMatrix(new FlipProof.Image.Matrices.Matrix4x4_Optimised<double>(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)),
          CoordinateSystem.RAS, EncodingDirection.UnknownOrNA, EncodingDirection.UnknownOrNA, EncodingDirection.UnknownOrNA);

      using var input = new ImageDouble<SmoothTestSpace>(head,  new double[head.Size.VoxelCount]);

      // The marks must be a decent distance from the edge or the checks for bias won't work as
      // some signal is bled off the edge of the image
      const int centreX = 29;
      const int centreY = 17;
      const int centreZ = 23;
      const double sigma = 3;

      input[centreX, centreY, centreZ, 0] = 1;
      input[centreX, centreY, centreZ, 1] = 2;
      input[centreX, centreY, centreZ, 2] = 3;
      input[centreX, centreY, centreZ, 3] = 4;

      using ImageDouble<SmoothTestSpace> result = KernelBasedFilters.Smooth(input, sigma);

      Assert.AreNotSame(input, result);
      Assert.AreNotSame(input.Data.Storage, result.Data.Storage);

      // Check for introduced bias
      for (int vol = 0; vol < 4; vol++)
      {
         Assert.AreEqual(vol + 1, result.ExtractVolumeAsTensor(vol).sum().ToDouble(), 0.01);
      }

      // Check smoothing worked 
      for (int vol = 0; vol < 4; vol++)
      {
         var oldPeak = vol + 1;

         double expected = oldPeak / Math.Pow(2*Math.PI* sigma * sigma, 3d/2);
         
         // The actual gaussian kernel does not have an infinite span
         // so allow for some error in the peak
         Assert.AreEqual(expected, result[centreX, centreY, centreZ, vol], expected / 500);
         Assert.AreEqual(expected, result.ExtractVolume<SmoothTestSpace, SmoothTestSpace3D>(vol).GetMaxIntensity(), expected / 500, "Peak moved");

         // Now when it's 4 pixels away
         expected = expected * Math.Exp(-(4*4)/(2* sigma * sigma));

         Assert.AreEqual(expected, result[centreX - 4, centreY, centreZ, vol], expected / 500);
         Assert.AreEqual(expected, result[centreX + 4, centreY, centreZ, vol], expected / 500);
         Assert.AreEqual(expected, result[centreX, centreY - 4, centreZ, vol], expected / 500);
         Assert.AreEqual(expected, result[centreX, centreY + 4, centreZ, vol], expected / 500);
         Assert.AreEqual(expected, result[centreX, centreY, centreZ - 4, vol], expected / 500);
         Assert.AreEqual(expected, result[centreX, centreY, centreZ + 4, vol], expected / 500);
      }
   }
}
