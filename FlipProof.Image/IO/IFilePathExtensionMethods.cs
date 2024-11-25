using FlipProof.Base;
using FlipProof.Image.Nifti;

namespace FlipProof.Image.IO;
public static class IImageOnFileProviderExtensionMethods
{
   
   /// <summary>
   /// Gets the file as an image. If this object is or holds an image, that may be returned without an IO operation
   /// </summary>
   public static Image<TSpace> GetImage<TSpace>(this IImageOnFileProvider provider, bool lookForGzNiftiIfNotFound=false)
      where TSpace : ISpace
   {
      return NiftiReader.ReadToVolume<TSpace>(provider.GetImagePath(null), lookForGzNiftiIfNotFound);
   }
}
