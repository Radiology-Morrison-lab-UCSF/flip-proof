using FlipProof.Image;

namespace FlipProof.ITK;

public static class ImageExtensionMethods
{
   static ImageFloat<TSpace> ApplyFilter<TSpace>(ImageFloat<TSpace> input, Func<itk.simple.Image, itk.simple.Image> filter)
      where TSpace : struct,ISpace
   {
      using itk.simple.Image result =  filter.Invoke(FlipProof.ITK.Converter.ToITK(input));

      return ITK.Converter.ToFlipProofFloat<TSpace>(result);
   }

   public static ImageFloat<TSpace> N4BiasFieldCorrection<TSpace>(this ImageFloat<TSpace> input)
      where TSpace : struct, ISpace
   {
      return ApplyFilter<TSpace>(input, a => a.N4BiasFieldCorrection());

   }
   public static ImageFloat<TSpace> N4BiasFieldCorrection<TSpace>(this ImageFloat<TSpace> input, ImageBool<TSpace> mask)
      where TSpace : struct, ISpace
   {
      using var imF = mask.ToUInt8();
      using var maskITK = imF.ToITK();
      return ApplyFilter<TSpace>(input, a => a.N4BiasFieldCorrection(maskITK));

   }
}
