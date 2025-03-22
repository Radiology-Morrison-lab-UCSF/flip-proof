using FlipProof.Base;
using FlipProof.Image.Filters;

namespace FlipProof.ImageTests.Filters;

[TestClass]
public class UnsharpMaskKernelTests
{
   [TestMethod]
   public void GetIntensityMatchesGaussian()
   {
      GaussianKernel gauseKern = new(3);
      UnsharpKernel kernel = new(3);

      for (int i = -10; i < 10; i++)
      {
         for (int j = -10; j < 10; j++)
         {
            for (int k = -10; k < 10; k++)
            {
               if(i == 0 && j == 0 && k == 0)
               {
                  // undefined - it is set during normalisation
                  continue;
               }
               MatchedNegGaussian(i, j, k);
            }
         }
      }

      void MatchedNegGaussian(int x, int y, int z)
      {
         if (x == 0 && y == 0 && z == 0)
         {
            throw new ArgumentException();
         }
         // Kernel implements both IKernel<double> and <float>
         Assert.AreEqual(-gauseKern.GetIntensity(x, y, z), kernel.GetIntensity(x, y, z));
         Assert.AreEqual(-((IKernel<float>)gauseKern).GetIntensity(x, y, z), ((IKernel<float>)kernel).GetIntensity(x, y, z), 1e-3f);
      }
   }

   [TestMethod]
   public void Normalise()
   {
      GaussianKernel gauseKern = new(3);
      UnsharpKernel kernel = new(3);

      Array3D<double> arr = Array3D<double>.FromValueGenerator(kernel.GetIntensity, 3, 7, 5);

      kernel.NormaliseKernel(arr);

      double[] allVox = arr.GetAllVoxels_XFastest();
      Assert.AreEqual(0d, allVox.Sum(), 1e-10);
      Assert.IsTrue(arr[0, 0, 0] > 0); //0,0,0 is the kernel centre because it's offset for fft
      Assert.AreEqual(1, allVox.Count(a => a >= 0));

   }
}

