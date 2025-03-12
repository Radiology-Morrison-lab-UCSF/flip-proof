using FlipProof.Base;
using FlipProof.Image;
using FlipProof.Image.Matrices;

namespace FlipProof.ImageTests.Matrices;

[TestClass]
public class OrientationMatrixTests : ReadOnlyOrientationTests
{
   protected override IReadOnlyOrientation GetSimple(XYZ<float> voxelSize, XYZ<float> translate)
   {
      Matrix4x4_Optimised<double> imageMatrix = new(
         voxelSize.X, 0, 0, translate.X,
         0, voxelSize.Y, 0, translate.Y,
         0, 0, voxelSize.Z, translate.Z,
         0, 0, 0, 1
         );

      return new OrientationMatrix(imageMatrix);
   }
   protected override IReadOnlyOrientation Get31DegRotInXYPlane(XYZ<float> voxelSize, XYZ<float> translate)
   {
      return GetRotatedInXYPlane(voxelSize, translate, -31 / 360f * 2 * MathF.PI);
   }
   
   protected override IReadOnlyOrientation Get45DegRotInXYPlane(XYZ<float> voxelSize, XYZ<float> translate)
   {
      return GetRotatedInXYPlane(voxelSize, translate, -45/360f*2*MathF.PI);
   }
   protected IReadOnlyOrientation GetRotatedInXYPlane(XYZ<float> voxelSize, XYZ<float> translate, float radians)
   {
      float cosRad = MathF.Cos(radians);
      float sinRad = MathF.Sin(radians);

      Matrix4x4_Optimised<double> rotated = new(cosRad * voxelSize.X, -sinRad * voxelSize.Y, 0,          translate.X,
                              sinRad * voxelSize.X, cosRad * voxelSize.Y, 0,           translate.Y,
                              0,                      0,                      voxelSize.Z, translate.Z,
                              0, 0, 0, 1);


      return new OrientationMatrix(rotated);
   }


}
