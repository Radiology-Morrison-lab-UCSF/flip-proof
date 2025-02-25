using FlipProof.Image.Nifti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.Image.Nifti.Simple;
public static class NiftiReader<TSpace>
   where TSpace : struct, ISpace
{
   public static ImageDouble<TSpace> ReadToDouble(string fileLoc, bool lookForZippedVariantIfNotFound)
   {
      return NiftiReader.ReadToDouble<TSpace>(fileLoc, lookForZippedVariantIfNotFound, out _);
   }
   public static ImageFloat<TSpace> ReadToFloat(string fileLoc, bool lookForZippedVariantIfNotFound)
   {
      return NiftiReader.ReadToFloat<TSpace>(fileLoc, lookForZippedVariantIfNotFound, out _);
   }
   public static ImageBool<TSpace> ReadToBool(string fileLoc, bool lookForZippedVariantIfNotFound)
   {
      return NiftiReader.ReadToBool<TSpace>(fileLoc, lookForZippedVariantIfNotFound, out _);
   }

}
