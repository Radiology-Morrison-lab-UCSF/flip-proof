using FlipProof.Base;
using FlipProof.Image;
using FlipProof.Image.Matrices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.ImageTests.Matrices;
[TestClass]
public class DecomposableTransformTests : ReadOnlyOrientationTests
{
   [TestMethod]
   public void ToQuaternions()
   {
      DenseMatrix<double> rotationMatrix = new DenseMatrix<double>(3, 3);
      rotationMatrix[0, 0] = -0.5793104;
      rotationMatrix[0, 1] = 0.4344828;
      rotationMatrix[0, 2] = 0.6896552;

      rotationMatrix[1, 0] = 0.5517241;
      rotationMatrix[1, 1] = -0.4137931;
      rotationMatrix[1, 2] = 0.7241379;

      rotationMatrix[2, 0] = 0.6;
      rotationMatrix[2, 1] = 0.8;
      rotationMatrix[2, 2] = 0;

      DecomposableNiftiTransform<double> transform = new DecomposableNiftiTransform<double>(rotationMatrix, [41, 43, 47], [19, 23, 27], 1);

      transform.TryGetNiftiQuaternions(out double b, out double c, out double d, out double[] pixdim, out double[] translation, out double qFac);

      Assert.AreEqual(0.4567501, b, 1e-6);
      Assert.AreEqual(0.5397956, c, 1e-6);
      Assert.AreEqual(0.7058866, d, 1e-6);

      CollectionAssert.AreEqual(new double[] { 41, 43, 47 }, pixdim);
      CollectionAssert.AreEqual(new double[] { 19, 23, 27 }, translation);
      Assert.AreEqual(1, qFac);

   }

   [TestMethod]
   public void FromQuaternions()
   {
      // Expected result from various third party sources online that were not consulted during implementation
      var transform = DecomposableNiftiTransform<double>.FromNiftiQuaternions(0.4567501, 0.5397956, 0.7058866, [41, 43, 47], [19, 23, 27], 1);

      var result_rot = transform.GetRotation();
      DenseMatrix<double> expected_rot = new DenseMatrix<double>(3, 3);
      expected_rot[0, 0] = -0.5793104;
      expected_rot[0, 1] = 0.4344828;
      expected_rot[0,2]=  0.6896552;

      expected_rot[1, 0] = 0.5517241;
      expected_rot[1, 1] = -0.4137931;
      expected_rot[1, 2] = 0.7241379;

      expected_rot[2, 0] = 0.6;
      expected_rot[2, 1] = 0.8;
      expected_rot[2, 2] = 0;

      Assert.IsTrue(expected_rot.Equals(result_rot, 1e-6));

      CollectionAssert.AreEqual(new double[] { 41, 43, 47 }, transform.GetPixDim());
      
      CollectionAssert.AreEqual(new double[] { 19, 23, 27 }, transform.GetTranslation());

   }
   [TestMethod]
   [DataRow(1)]
   [DataRow(-1)]
   public void ToFastCalcMatrix(double qFac)
   {
      DenseMatrix<double> rotationMatrix = new DenseMatrix<double>(3, 3);
      rotationMatrix[0, 0] = -0.5793104;
      rotationMatrix[0, 1] = 0.4344828;
      rotationMatrix[0, 2] = 0.6896552;

      rotationMatrix[1, 0] = 0.5517241;
      rotationMatrix[1, 1] = -0.4137931;
      rotationMatrix[1, 2] = 0.7241379;

      rotationMatrix[2, 0] = 0.6;
      rotationMatrix[2, 1] = 0.8;
      rotationMatrix[2, 2] = 0;

      DenseMatrix<double> transform = DecomposableNiftiTransform<double>.NiftiMatToFastMat(rotationMatrix, [3, 5, 7], [19, 23, 27d], qFac);
      Assert.AreEqual(19, transform[0, 3]);
      Assert.AreEqual(23, transform[1, 3]);
      Assert.AreEqual(27, transform[2, 3]);

      // Nifti matrices are weird. They are applied:
      // R * coords *. voxelSize + translation
      // where *. is a element wise multiplication
      // This is equivalent to:
      // R * (coords *. voxelSize) + translation
      // or scaling R by the voxel size where x,y,z are applied to columns 0,1,2 respectively
      // Q fac reverses the direction of the z axis
      Assert.AreEqual(3 * rotationMatrix[0,0], transform[0, 0]);
      Assert.AreEqual(3 * rotationMatrix[0,1], transform[0, 1]);
      Assert.AreEqual(3 * rotationMatrix[0,2], transform[0, 2]);
      Assert.AreEqual(5 * rotationMatrix[1,0], transform[1, 0]);
      Assert.AreEqual(5 * rotationMatrix[1,1], transform[1, 1]);
      Assert.AreEqual(5 * rotationMatrix[1,2], transform[1, 2]);
      Assert.AreEqual(7 * rotationMatrix[2,0] * qFac, transform[2, 0]);
      Assert.AreEqual(7 * rotationMatrix[2,1] * qFac, transform[2, 1]);
      Assert.AreEqual(7 * rotationMatrix[2,2] * qFac, transform[2, 2]);

   }

   protected override IReadOnlyOrientation GetSimple(XYZ<float> voxelSize, XYZ<float> translate)
   {
      return DecomposableNiftiTransformD.FromNiftiQuaternions(0, 0, 0, ((XYZ<double>)voxelSize).ToArray(), ((XYZ<double>)translate).ToArray(), 1);
   }

   protected override IReadOnlyOrientation Get45DegRotInXYPlane(XYZ<float> voxelSize, XYZ<float> translate)
   {
      // 45 degrees in xy plane is a rotation around z axis
      // 45 degrees is 2Pi/8
      // q = (thing for x)*i + (thing for y)*j + sin(angle/2)*k + (the w thing)
      // sin(2pi/8/2) =sin(pi/8) = 0.382683
      // negative here to match test's expected direction
      return DecomposableNiftiTransformD.FromNiftiQuaternions(0, 0, -0.38268343236509, ((XYZ<double>)voxelSize).ToArray(), ((XYZ<double>)translate).ToArray(), 1);
   }
}
