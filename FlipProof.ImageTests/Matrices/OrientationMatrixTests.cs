using FlipProof.Base;
using FlipProof.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.ImageTests.Matrices
{
   [TestClass]
   public class OrientationMatrixTests : ReadOnlyOrientationTests
   {
      protected override IReadOnlyOrientation GetSimple(XYZ<float> voxelSize, XYZ<float> translate)
      {
         Matrix4x4 imageMatrix = new(
            voxelSize.X, 0, 0, translate.X,
            0, voxelSize.Y, 0, translate.Y,
            0, 0, voxelSize.Z, translate.Z,
            0, 0, 0, 1
            );

         return new OrientationMatrix(imageMatrix);
      }
      protected override IReadOnlyOrientation Get45DegRotInXYPlane(XYZ<float> voxelSize, XYZ<float> translate)
      {
         // Rotate 45 degrees in xy plane
         // The x and y axes sizes are now even 
         float cos45Deg = MathF.Cos(MathF.PI / 4);
         float sin45Deg = MathF.Sin(MathF.PI / 4);
         Matrix4x4 rotationMatrix = new(cos45Deg, -sin45Deg, 0, 0,
                                       sin45Deg, cos45Deg, 0, 0,
                                       0, 0, 1, 0,
                                       0, 0, 0, 1);

         Matrix4x4 rotated = GetSimple(voxelSize, translate).GetMatrix() * rotationMatrix;

         return new OrientationMatrix(rotated);
      }
   }
}
