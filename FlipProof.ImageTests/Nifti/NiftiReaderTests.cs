using FlipProof.Image.IO;
using FlipProof.Image.Nifti;

namespace FlipProof.ImageTests.Nifti;
[TestClass]
public class NiftiReaderTests
{
   [TestMethod]
   public void ReadNifti()
   {
      /*
       mrtrix 3 printout:
        Dimensions:        64 x 60 x 21 x 3
        Voxel size:        1.2 x 3.4 x 5.4 x 3
        Data strides:      [ -1 2 3 4 ]
        Format:            NIfTI-1.1 (GZip compressed)
        Data type:         unsigned 16 bit integer (little endian)
        Intensity scaling: offset = 0, multiplier = 1
        Transform:                    1           0          -0        -252 <---- See notes on mrtrix coords below
                                     -0           1          -0           4
                                     -0           0           1           0
        comments:          FSL3.2beta
        mrtrix_version:    3.0.4-69-g0c84d455
       */
      using NiftiReader nr = new(Gen.GetUnzippedStream(new MemoryStream(Resource1.fmri_example_nii, false), true));
      bool result = nr.TryRead(out string msg, out NiftiFile_Base? nf);
      Assert.IsTrue(result);
      Assert.IsNotNull(nf);

      Assert.IsTrue(nf.Has4thDimension);
      CollectionAssert.AreEqual(nf.Head.DataArrayDims, new short[] { 4, 64, 60, 21, 3, 1, 1, 1 });
      CollectionAssert.AreEqual(nf.Head.PixDim.Skip(1).Take(4).ToArray(), new float[] { 1.2f, 3.4f, 5.4f, 3f });
      Assert.IsInstanceOfType<NiftiFile<UInt16>>(nf);
      NiftiFile<UInt16> nf16 = (NiftiFile<UInt16>)nf;

      Image.Image<TestSpace4D> im = nf16.ToImage<TestSpace4D>();

      
      Assert.AreEqual(new Image.ImageSize(3, 64, 60, 21), im.Header.Size);
      Assert.AreEqual(4, im.Header.NDims);
      Assert.AreEqual( Image.CoordinateSystem.RAS, im.Header.CoordinateSystem);

      // Coords verified against ITK snap
      // Note mrtrix uses a reversed x coordinate as the 
      // data order for x is -1 so the x coordinate does not match
      // there as to convert between voxel units you need to subtract
      // them from the image width
      var world = im.Header.VoxelToWorldCoordinate(13, 58, 19);
      Assert.AreEqual(-192, world.X, 0.001);
      Assert.AreEqual(201.2, world.Y, 0.001);
      Assert.AreEqual(102.6, world.Z, 0.001);

      Assert.AreEqual(new System.Numerics.Matrix4x4(-1.2f,0,0,-176.4f,
                                                      0,3.4f,0,4,
                                                      0,0,5.4f,0,
                                                      0,0,0,1), im.Header.Orientation);
   }

}
