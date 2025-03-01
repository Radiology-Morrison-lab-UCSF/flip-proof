using FlipProof.Image.Nifti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.Image.Nifti.Simple;

/// <summary>
/// Simple nifti reader for use in Python 
/// </summary>
/// <typeparam name="TSpace"></typeparam>
public static class NiftiReader
{
   public static ImageDouble<TSpace> ReadToDouble<TSpace>(string fileLoc, bool lookForZippedVariantIfNotFound)
         where TSpace : struct, ISpace
   {
      return FlipProof.Image.Nifti.NiftiReader.ReadToDouble<TSpace>(fileLoc, lookForZippedVariantIfNotFound, out _);
   }
   public static ImageFloat<TSpace> ReadToFloat<TSpace>(string fileLoc, bool lookForZippedVariantIfNotFound)
         where TSpace : struct, ISpace
   {
      return FlipProof.Image.Nifti.NiftiReader.ReadToFloat<TSpace>(fileLoc, lookForZippedVariantIfNotFound, out _);
   }
   public static ImageBool<TSpace> ReadToBool<TSpace>(string fileLoc, bool lookForZippedVariantIfNotFound)
         where TSpace : struct, ISpace
   {
      return FlipProof.Image.Nifti.NiftiReader.ReadToBool<TSpace>(fileLoc, lookForZippedVariantIfNotFound, out _);
   }

}
