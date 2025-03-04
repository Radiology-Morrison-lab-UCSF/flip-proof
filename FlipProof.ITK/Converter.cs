using FlipProof.Base;
using FlipProof.Image;
using FlipProof.Image.Nifti;
using itk.simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.ITK
{
    public static class Converter
    {
      public static itk.simple.Image ToITK<TVoxel, TSpace>(this Image<TVoxel,TSpace> flipProofIm)
         where TSpace: struct, ISpace
         where TVoxel : struct, INumber<TVoxel>
      {
         // Converting via the disk is regretful but safer than assumptions
         // about how ITK tensors / dataorder works in the long term
         using TemporaryFilenameGenerator tempFiles = new();
         var saveTo = tempFiles.Next("nii");

         flipProofIm.SaveAsNifti(saveTo);
         
         itk.simple.ImageFileReader reader = new itk.simple.ImageFileReader();
         reader.SetFileName(saveTo);

         return reader.Execute();
      }

      [Obsolete("Header is checked at run time. Consider using a non-ITK method where possible to achieve compile-time-checks")]
      public static Image<TSpace> ToFlipProof<TSpace>(this itk.simple.Image itkImage)
         where TSpace : struct, ISpace
      {
         // Converting via the disk is regretful but safer than assumptions
         // about how ITK tensors / dataorder works in the long term

         using TemporaryFilenameGenerator tempFiles = new();
         var saveTo = tempFiles.Next("nii");
         
         itkImage.WriteImage(saveTo);

         return NiftiReader.ReadToVolume<TSpace>(saveTo, false);
      }
      public static ImageFloat<TSpace> ToFlipProofFloat<TSpace>(this itk.simple.Image itkImage)
         where TSpace : struct, ISpace
      {
         return ToFlipProof<TSpace>(itkImage).ToFloat();
      }
      public static Image<TSpace> ToFlipProofDouble<TSpace>(this itk.simple.Image itkImage)
         where TSpace : struct, ISpace
      {
         return ToFlipProof<TSpace>(itkImage).ToDouble();
      }

   }
}
