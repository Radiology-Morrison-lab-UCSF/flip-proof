from pythonnet import load
load("coreclr")

import clr
import sys
import os

sys.path.append(os.path.abspath("../FlipProof.Image/bin/Win-CPU-Debug/net8.0/win-x64/")) # path to the FlipProof.Image library

clr.AddReference("FlipProof.Image")

from FlipProof.Image import *
from FlipProof.Image.Nifti.Simple import NiftiReader
from FlipProof.Image.Spaces import NativeT1, NativeT2

path_im1="chris_t1.nii.gz"
path_im2="chris_t2.nii.gz"

# Mix images in the same space
im = NiftiReader.ReadToFloat[NativeT1](path_im1, True)
im2 = NiftiReader.ReadToFloat[NativeT1](path_im2, True)

im3 = im.Add_Float(im2)

# Can't mix T1 and T2 Spaces
im4 = NiftiReader.ReadToFloat[NativeT2](path_im2, True)
im5 = im.Add_Float(im4) # Error
