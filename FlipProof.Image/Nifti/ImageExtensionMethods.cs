using FlipProof.Base.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.Image.Nifti
{
    public static class ImageExtensionMethods
    {
      public static void SaveAsNifti<TVoxel,TSpace>(this Image<TVoxel,TSpace> im, FilePath saveTo, FileMode mode = FileMode.Create)
         where TSpace : struct, ISpace
         where TVoxel : struct, INumber<TVoxel>
      {
         NiftiWriter<TVoxel>.Write<TSpace>(im, saveTo, mode);
      }
    }
}
