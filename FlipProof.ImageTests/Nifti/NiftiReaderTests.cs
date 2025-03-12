using FlipProof.Base;
using FlipProof.Image;
using FlipProof.Image.IO;
using FlipProof.Image.Matrices;
using FlipProof.Image.Nifti;
using System.Numerics;

namespace FlipProof.ImageTests.Nifti;
[TestClass]
public class NiftiReaderTests
{
   [TestInitialize]
   public void Initialise()
   {
#pragma warning disable CS0618 // Type or member is obsolete
      ISpace.Debug_Clear<TestSpace4D>();
#pragma warning restore CS0618 // Type or member is obsolete

   }

   [TestCleanup]
   public void Cleanup()
   {
#pragma warning disable CS0618 // Type or member is obsolete
      ISpace.Debug_Clear<TestSpace4D>();
#pragma warning restore CS0618 // Type or member is obsolete

   }

   [TestMethod]
   [DataRow(false)]
   [DataRow(true)]
   public void ReadWriteNifti_NonDiagonalOrientation(bool useSForm)
   {
      ImageFloat<TestSpace4D> orig = GetImageWithNonDiagonalOrientation(useSForm);
      CheckMatrix(orig);

      using TemporaryFilenameGenerator tempFiles = new();

      orig.SaveAsNifti(tempFiles.Next("nii"));

      var read = NiftiReader.ReadToFloat<TestSpace4D>(tempFiles.Last, false);

      Assert.IsTrue(orig.Header.Equals(read.Header));
      CheckMatrix(read);

      CollectionAssert.AreEqual(orig.GetAllVoxels(), read.GetAllVoxels());

      static void CheckMatrix(ImageFloat<TestSpace4D> read)
      {
         // Coords verified against ITKSnap
         var world = read.Header.VoxelToWorldCoordinate(13, 58, 19);
         Assert.AreEqual(132.7, world.X, 0.1);
         Assert.AreEqual(64.41, world.Y, 0.01);
         Assert.AreEqual(257.9, world.Z, 0.1);
      }
   }


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

      
      Assert.AreEqual(new Image.ImageSize(64, 60, 21, 3), im.Header.Size);
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

   }


   protected ImageFloat<TestSpace4D> GetImageWithNonDiagonalOrientation(bool useSForm)
   { 
      // Start with a real image
      using NiftiReader nr = new(Gen.GetUnzippedStream(new MemoryStream(Resource1.fmri_example_nii, false), true));
      Assert.IsTrue(nr.TryRead(out string msg, out NiftiFile_Base? nf));

      // Swap out the header for something more complex
      /*
       * Rotation matrix:
            [ -0.7081229,  0.4710896,  0.5259625;
               0.5995155,  0.0076180,  0.8003269;
               0.3730189,  0.8820525, -0.2878200 ]
       */
      var decomposable = DecomposableNiftiTransform<double>.FromNiftiQuaternions(0.3781795, 0.707736, 0.5942821, [5.4, 3.3, 7.1], [21.3, -87.1, 101.72], 1);

      nf.Head.SetVox2WorldMatrix_QForm(decomposable);
      nf.Head.SetVox2WorldMatrix_SForm(decomposable.FastCalcMat.Cast<float>());
     
      if (useSForm)
      {
         nf.Head.sFormCode = CoordinateMapping_Nifti.ScannerAnat;
         nf.Head.qFormCode = CoordinateMapping_Nifti.Unknown;
      }
      else
      {
         nf.Head.sFormCode = CoordinateMapping_Nifti.Unknown;
         nf.Head.qFormCode = CoordinateMapping_Nifti.ScannerAnat;
      }

      //NiftiWriter.Write(nf, @"C:\users\lreid\data\test.nii", FileMode.Create);

      var image = nf.ToImage<TestSpace4D>().ToFloat();

      return image;
   }

}
