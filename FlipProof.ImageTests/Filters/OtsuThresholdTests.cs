using FlipProof.Image.Filters;
using FlipProof.Torch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorchSharp;
using static TorchSharp.torch;

namespace FlipProof.ImageTests.Filters;
[TestClass]
public class OtsuThresholdTests
{
   [TestMethod]
   public void TestOtsu()
   {
      // Sample from two distributions 
      // mean of 4, variance of 2
      // mean of 10, std dev of 1
      // optimal separation is at ~6.5
      using var sampleA = torch.normal(3, Math.Sqrt(2), new long[] { 1000000 }, torch.ScalarType.Float64);
      using var sampleB = torch.normal(10, 1, new long[] { 1000000 }, torch.ScalarType.Float64);

      double threshold = OtsuThreshold.CalculateThreshold(new DoubleTensor(torch.cat([sampleA, sampleB])));

      Assert.AreEqual(6.5, threshold, 0.1);
   }
}
