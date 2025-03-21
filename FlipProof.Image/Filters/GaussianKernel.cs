namespace FlipProof.Image.Filters;

/// <summary>
/// For applying aaussian smoothing kernel to the image
/// </summary>
class GaussianKernel : IKernel<double>, IKernel<float>
{
   readonly double _2SigmaSq;
   readonly float _2SigmaSqF;
   public Base.XYZ<int> KernelExtent { get; init; }

   public GaussianKernel(double sigma)
   {
      _2SigmaSq = 2 * sigma * sigma;
      _2SigmaSqF = (float)_2SigmaSq;
      int extent = (int)Math.Ceiling(sigma * 8); // 4 sigma either side
      KernelExtent = new Base.XYZ<int>(extent, extent, extent);
   }

   /// <summary>
   /// Returns the intensity for an non-normalised kernel at this offset from the centre
   /// </summary>
   /// <param name="offsetX"></param>
   /// <param name="offsetY"></param>
   /// <param name="offsetZ"></param>
   /// <returns></returns>
   public double GetIntensity(int offsetX, int offsetY, int offsetZ)
   {
      return Math.Exp(-(offsetX * offsetX + offsetY * offsetY + offsetZ * offsetZ) / _2SigmaSq);
   }

   float IKernel<float>.GetIntensity(int offsetX, int offsetY, int offsetZ)
   {
      return MathF.Exp(-(offsetX * offsetX + offsetY * offsetY + offsetZ * offsetZ) / _2SigmaSqF);
   }
}
