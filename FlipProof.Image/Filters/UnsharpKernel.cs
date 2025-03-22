using FlipProof.Base;

namespace FlipProof.Image.Filters;

class UnsharpKernel(double sigma) : IKernel<double>, IKernel<float>
{
   readonly GaussianKernel _gaussianKernel = new(sigma);

   public XYZ<int> KernelExtent => ((IKernel<double>)_gaussianKernel).KernelExtent;

   public double GetIntensity(int offsetX, int offsetY, int offsetZ)
   {
      if(offsetX == 0 && offsetY == 0 && offsetZ == 0)
      {
         // set during normalise
         return 0;
      }
      return -((IKernel<double>)_gaussianKernel).GetIntensity(offsetX, offsetY, offsetZ);
   }
   
   float IKernel<float>.GetIntensity(int offsetX, int offsetY, int offsetZ)
   {
      if(offsetX == 0 && offsetY == 0 && offsetZ == 0)
      {
         // set during normalise
         return 0;
      }
      return -((IKernel<float>)_gaussianKernel).GetIntensity(offsetX, offsetY, offsetZ);
   }

   public void NormaliseKernel(Array3D<double> kern)
   {
      // central value should equal the sum of all other voxels, * -1
      IKernel<double>.NormaliseKernelTo(kern, -1d);
      kern[0, 0, 0] = 1;
   }
   public void NormaliseKernel(Array3D<float> kern)
   {
      // central value should equal the sum of all other voxels, * -1
      IKernel<float>.NormaliseKernelTo(kern, -1f);
      kern[0, 0, 0] = 1;
   }
}
