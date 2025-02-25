from pythonnet import load
load("coreclr")

import clr

clr.AddReference("FlipProof.Image") # Must be in your search path
from FlipProof.Image import *
from FlipProof.Image.Nifti.Simple import NiftiReader
from FlipProof.Image.Spaces import NativeT1

path="<put path here>"
im = NiftiReader[NativeT1].ReadToFloat(path, True)
    
im2 = NiftiReader[NativeT1].ReadToFloat(path, True)

im3 = im.Add(im2)

print("ready")