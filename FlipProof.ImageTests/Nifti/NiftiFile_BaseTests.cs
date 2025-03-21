using FlipProof.Image;
using FlipProof.Image.IO;
using FlipProof.Image.Nifti;

namespace FlipProof.ImageTests.Nifti
{
   [TestClass]
   public class NiftiFile_BaseTests
   {
      struct MyTestSpace : ISpace { }

      /// <summary>
      /// Checks that an image created from a nifti is aligned to that nifti
      /// </summary>
      [TestMethod]
      public void ToImage_CoordinatesMatch()
      {
         ReadFloatImage(out NiftiFile_Base nii, out ImageFloat<MyTestSpace> image);

         IReadOnlyOrientation orient1 = nii.Head.GetVox2WorldDecomposableMatrix_ScannerSpace();
         IReadOnlyOrientation orient2 = image.Header.Orientation;

         Assert.IsTrue(orient1.TolerantEquals(orient2, new ImageSize(nii.Head.DataArrayDims[1], nii.Head.DataArrayDims[2], nii.Head.DataArrayDims[3], nii.Head.DataArrayDims[4])));

      }
      /// <summary>
      /// Checks that a nifti created from an image is aligned to that image
      /// </summary>
      [TestMethod]
      public void FromImage_CoordinatesMatch()
      {
         ReadFloatImage(out NiftiFile_Base orig, out ImageFloat<MyTestSpace> image);
         orig.Dispose();

         using NiftiFile<float> result = NiftiFile_Base.FromImage(image);

         IReadOnlyOrientation orient1 = result.Head.GetVox2WorldDecomposableMatrix_ScannerSpace();
         IReadOnlyOrientation orient2 = image.Header.Orientation;

         Assert.IsTrue(orient1.TolerantEquals(orient2, new ImageSize(orig.Head.DataArrayDims[1], orig.Head.DataArrayDims[2], orig.Head.DataArrayDims[3], orig.Head.DataArrayDims[4])));

      }

      /// <summary>
      /// Round trip of to-from image
      /// </summary>
      [TestMethod]
      public void ToImageFromImage()
      {
         ReadFloatImage(out NiftiFile_Base read, out ImageFloat<MyTestSpace> im);

         using var result = NiftiFile<float>.FromImage(im);


         // nii --> im --> nii --> im
         // will automatically check headers match
         using var resultIm = result.ToImage<MyTestSpace>();
         Assert.IsTrue((im == resultIm).GetAllVoxels().All(a => a));

         // nii --> im --> nii
         // just a voxelwise comparison
         var origVox = read.GetDataStream();
         var resultVox = result.GetDataStream();
         origVox.Seek(0, SeekOrigin.Begin);
         resultVox.Seek(0, SeekOrigin.Begin);

         CollectionAssert.AreEqual(
            origVox.ReadBytes((int)origVox.Length),
            resultVox.ReadBytes((int)resultVox.Length));
      }

      private static void ReadFloatImage(out NiftiFile_Base read, out ImageFloat<MyTestSpace> im)
      {
         using NiftiReader nr = new(Gen.GetUnzippedStream(new MemoryStream(Resource1.fmri_example_float_nii, false), true));
         Assert.IsTrue(nr.TryRead(out string msg, out read));
         im = (ImageFloat<MyTestSpace>)read.ToImage<MyTestSpace>();
      }
   }

   //[TestClass]
   //public class NiftiHeaderTests
   //{
   //   [TestMethod]
   //   public void SetVox2WorldMatrix_QForm()
   //   {
   //      // Start with a real image
   //      using NiftiReader nr = new(Gen.GetUnzippedStream(new MemoryStream(Resource1.fmri_example_nii, false), true));
   //      Assert.IsTrue(nr.TryRead(out string msg, out NiftiFile_Base? nf));

   //      // Swap out the header for something more complex
   //      /*
   //       * Rotation matrix:
   //            [ -0.7081229,  0.4710896,  0.5259625;
   //               0.5995155,  0.0076180,  0.8003269;
   //               0.3730189,  0.8820525, -0.2878200 ]
   //       */
   //      var decomposable = DecomposableNiftiTransformD.FromNiftiQuaternions(0.3781795, 0.707736, 0.5942821, [5.4, 3.3, 7.1], [21.3, -87.1, 101.72], 1);


   //      Assert.IsTrue(decomposable.SetVox2WorldMatrix_QForm(decomposable));

   //      Assert.AreEqual(0.3781795, decomposable.quartern_b, 1e-6);
   //      Assert.AreEqual(0.707736, decomposable.quartern_c, 1e-6);
   //      Assert.AreEqual(0.5942821, decomposable.quartern_d, 1e-6);
   //      Assert.AreEqual(5.4, decomposable.PixDim[1], 1e-6);
   //      Assert.AreEqual(3.3, decomposable.PixDim[2], 1e-6);
   //      Assert.AreEqual(7.1, decomposable.PixDim[3], 1e-6);
   //      Assert.AreEqual(21.3, decomposable.quartern_x, 1e-5);
   //      Assert.AreEqual(-87.1, decomposable.quartern_y, 1e-5);
   //      Assert.AreEqual(101.72, decomposable.quartern_z, 1e-4);

   //      var orientationMatrix = ((IImageHeader)decomposable).Orientation;

   //      // Check the orientation matrix is correct vs coordinates we've derived from
   //      // third party programs

   //      decomposable.qFormCode = CoordinateMapping_Nifti.ScannerAnat;
   //      decomposable.sFormCode = CoordinateMapping_Nifti.Unknown;
   //      NiftiWriter.Write(nf, @"C:\users\lreid\data\test.nii", FileMode.Create);

   //      // 7,11,13 --> 60.18, 9.708, 121.3

   //      // "Nifti way"
   //      {
   //         var rotMatrix = decomposable.GetRotation();
   //         Assert.AreEqual(60.18, rotMatrix[0, 0] * 21 * 5.4 + rotMatrix[0, 1] * 17 * 3.3 + rotMatrix[0, 2] * 13 * 7.1) + 21.3, 0.001);
   //      }

   //      var result = VoxelToWorldCoordinate(7, 11, 13, orientationMatrix);
   //      Assert.AreEqual(60.18, result.X, 0.01);
   //      Assert.AreEqual(9.708, result.Y, 0.01);
   //      Assert.AreEqual(121.3, result.Z, 0.01);

   //      static XYZ<float> VoxelToWorldCoordinate(float x, float y, float z, Matrix4x4 orientation)
   //      {
   //         return new XYZ<float>(
   //                orientation.M11 * x + orientation.M12 * y + orientation.M13 * z + orientation.M14,
   //                orientation.M21 * x + orientation.M22 * y + orientation.M23 * z + orientation.M24,
   //                orientation.M31 * x + orientation.M32 * y + orientation.M33 * z + orientation.M34
   //            );
   //      }
   //   }

   //   internal static ImageFloat<TestSpace4D> GetImageWithNonDiagonalOrientation(bool useSForm)
   //   {
   //      // Start with a real image
   //      using NiftiReader nr = new(Gen.GetUnzippedStream(new MemoryStream(Resource1.fmri_example_nii, false), true));
   //      Assert.IsTrue(nr.TryRead(out string msg, out NiftiFile_Base? nf));

   //      // Swap out the header for something more complex
   //      /*
   //       * Rotation matrix:
   //            [ -0.7081229,  0.4710896,  0.5259625;
   //               0.5995155,  0.0076180,  0.8003269;
   //               0.3730189,  0.8820525, -0.2878200 ]
   //       */
   //      var decomposable = DecomposableNiftiTransform<double>.FromNiftiQuaternions(0.3781795, 0.707736, 0.5942821, [5.4, 3.3, 7.1], [21.3, -87.1, 101.72], 1);

   //      Assert.IsTrue(decomposable.SetVox2WorldMatrix_QForm(decomposable));
   //      decomposable.SetVox2WorldMatrix_SForm(decomposable.FastCalcMat.Cast<float>());

   //      if (useSForm)
   //      {
   //         decomposable.sFormCode = CoordinateMapping_Nifti.ScannerAnat;
   //         decomposable.qFormCode = CoordinateMapping_Nifti.Unknown;
   //      }
   //      else
   //      {
   //         decomposable.sFormCode = CoordinateMapping_Nifti.Unknown;
   //         decomposable.qFormCode = CoordinateMapping_Nifti.ScannerAnat;
   //      }

   //      //NiftiWriter.Write(nf, @"C:\users\lreid\data\test.nii", FileMode.Create);

   //      var image = nf.ToImage<TestSpace4D>().ToFloat();

   //      return image;
   //   }
   //}
}
