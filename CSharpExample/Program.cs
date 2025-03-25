
using FlipProof.Image;
using FlipProof.Image.Filters;
using FlipProof.Image.Nifti;
using FlipProof.ITK;
using static FlipProof.Image.Nifti.NiftiReader;

var mask = ReadToBool<T1>("brainmask.nii");

ReadToFloat<T1>("t1-raw.nii").
   N4BiasFieldCorrection(mask).
   IntensityNormalise().
   Smooth().
   Mask(mask).
   SaveAsNifti("processed-t1.nii");

struct T1 : ISpace { }



