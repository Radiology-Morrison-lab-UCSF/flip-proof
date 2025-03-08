using FlipProof.Image.Matrices;
using FlipProof.Image.Nifti;
using FlipProof.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlipProof.Image.IO;
using System.Numerics;
using FlipProof.Base;

namespace FlipProof.ImageTests.Nifti
{
   [TestClass]
   public class DecomposableNiftiTransformDTests
   {
      [TestMethod]
      public void FromNiftiQuaternions()
      {
         /*
          * Rotation matrix should be:
               [ -0.7081229,  0.4710896,  0.5259625;
                  0.5995155,  0.0076180,  0.8003269;
                  0.3730189,  0.8820525, -0.2878200 ]
          */
         var decomposable = DecomposableNiftiTransformD.FromNiftiQuaternions(0.3781795, 0.707736, 0.5942821, [5.4, 3.3, 7.1], [21.3, -87.1, 101.72], 1);

         var pixDim = decomposable.GetPixDim();
         Assert.AreEqual(5.4, pixDim[0], 1e-6);
         Assert.AreEqual(3.3, pixDim[1], 1e-6);
         Assert.AreEqual(7.1, pixDim[2], 1e-6);
         var trans = decomposable.GetTranslation();
         Assert.AreEqual(21.3, trans[0], 1e-5);
         Assert.AreEqual(-87.1, trans[1], 1e-5);
         Assert.AreEqual(101.72, trans[2], 1e-4);

         CollectionAssert.AreEqual(new double[] { -0.7081229, 0.4710896, 0.5259625 }, decomposable.GetRotation().GetRow(0),  new DoubleComparer(1e-4));
         CollectionAssert.AreEqual(new double[] { 0.5995155, 0.0076180, 0.8003269 }, decomposable.GetRotation().GetRow(1), new DoubleComparer(1e-4));
         CollectionAssert.AreEqual(new double[] { 0.3730189, 0.8820525, -0.2878200 }, decomposable.GetRotation().GetRow(2), new DoubleComparer(1e-4));

         // Check the orientation matrix is correct vs coordinates we've derived from
         // third party programs

         // 7,11,13 --> 60.18, 9.708, 121.3

         var result = decomposable.VoxelToWorldCoordinate(7, 11, 13);
         Assert.AreEqual(60.18, result.X, 0.01);
         Assert.AreEqual(9.708, result.Y, 0.01);
         Assert.AreEqual(121.3, result.Z, 0.1); // we only know to 4sf due to the third party programs only displaying this

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
