using FlipProof.Image.Filters;

namespace FlipProof.ImageTests.Filters;

[TestClass]
public class GaussianKernelTests
{
   [TestMethod]
   public void GetIntensity()
   {
      const double sigma = 7;
      const int sigmaI = (int)sigma;

      GaussianKernel kern = new(sigma);

      AssertEqual(1, 0,0,0);

      double expectedAt1Sigma = Math.Exp(-(sigma * sigma) / (2 * sigma * sigma));
      AssertEqual(expectedAt1Sigma, sigmaI, 0, 0);
      AssertEqual(expectedAt1Sigma, 0, sigmaI, 0);
      AssertEqual(expectedAt1Sigma, 0, 0, sigmaI);


      double expectedAt2Sigma = Math.Exp(-(2 * sigma * 2* sigma) / (2 * sigma * sigma));
      AssertEqual(expectedAt2Sigma, sigmaI*2, 0, 0);
      AssertEqual(expectedAt2Sigma, 0, sigmaI*2, 0);
      AssertEqual(expectedAt2Sigma, 0, 0, sigmaI * 2);


      // diagonal 1 sigma in two directons
      // distance is sqrt(2) * sigma
      //        |\ sqrt(sigma^2+sigma^2)
      // sigma  | \
      //        ----
      //         sigma 
      // eqn
      // exp((-sqrt(sigma^2+sigma^2)^2)) / (2*sigma^2)
      // becomes
      // exp(-(2* sigma^2) / (2*sigma^2))
      // then
      // exp(-1)
      double expectedDiag = Math.Exp(-1);
      AssertEqual(expectedDiag, sigmaI, sigmaI, 0);
      AssertEqual(expectedDiag, 0, sigmaI, sigmaI);
      AssertEqual(expectedDiag, sigmaI, 0, sigmaI);


      void AssertEqual(double expected, int x, int y, int z)
      {
         // Kernel implements both IKernel<double> and <float>
         Assert.AreEqual(expected, kern.GetIntensity(x, y, z), 1e-3);
         Assert.AreEqual((float)expected, ((IKernel<float>)kern).GetIntensity(x, y, z), 1e-3f);
      }
   }
}
