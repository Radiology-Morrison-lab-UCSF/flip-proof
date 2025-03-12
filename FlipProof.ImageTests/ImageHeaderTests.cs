using FlipProof.Base;
using FlipProof.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.ImageTests;

[TestClass]
public abstract class ReadOnlyOrientationTests
{
   protected abstract IReadOnlyOrientation GetSimple(XYZ<float> voxelSize, XYZ<float> translate);
   protected abstract IReadOnlyOrientation Get45DegRotInXYPlane(XYZ<float> voxelSize, XYZ<float> translate);
   protected abstract IReadOnlyOrientation Get31DegRotInXYPlane(XYZ<float> voxelSize, XYZ<float> translate);

   [TestMethod]
   public void GetVoxelSize()
   {
      const float voxelSizeX = 7;
      const float voxelSizeY = 13;
      const float voxelSizeZ = 11;

      IReadOnlyOrientation orientation = GetSimple(new(voxelSizeX, voxelSizeY, voxelSizeZ), new(10,11,31));

      Assert.AreEqual(new XYZ<double>(voxelSizeX, voxelSizeY, voxelSizeZ), orientation.VoxelSize);

      // Rotate 45 degrees in xy plane
      IReadOnlyOrientation rotated = Get45DegRotInXYPlane(new(voxelSizeX, voxelSizeY, voxelSizeZ), new(10, 11, 31));

      // The voxel size should be unaltered 
      Assert.AreEqual(0d, new XYZ<double>(voxelSizeX, voxelSizeY, voxelSizeZ).DistanceTo(rotated.VoxelSize), 0.001);

   }


   [TestMethod]
   public void VoxelToWorldCoordinate()
   {
      const float voxelSizeX = 7;
      const float voxelSizeY = 13;
      const float voxelSizeZ = 11;

      const float translateX = 10;
      const float translateY = 11;
      const float translateZ = 31;

      IReadOnlyOrientation orientation = GetSimple(new(voxelSizeX, voxelSizeY, voxelSizeZ), new(10,11,31));

      Assert.AreEqual(new XYZ<double>(translateX, translateY, translateZ), orientation.VoxelToWorldCoordinate(0, 0, 0));
      Assert.AreEqual(new XYZ<double>(translateX + voxelSizeX, translateY, translateZ), orientation.VoxelToWorldCoordinate(1, 0, 0));
      Assert.AreEqual(new XYZ<double>(translateX, translateY + voxelSizeY, translateZ), orientation.VoxelToWorldCoordinate(0, 1, 0));
      Assert.AreEqual(new XYZ<double>(translateX, translateY, translateZ + voxelSizeZ), orientation.VoxelToWorldCoordinate(0, 0, 1));
      Assert.AreEqual(new XYZ<double>(translateX + voxelSizeX, translateY + voxelSizeY, translateZ + voxelSizeZ), orientation.VoxelToWorldCoordinate(1, 1, 1));

      // Rotate 45 degrees in xy plane
      // No offset here as the math gets too messy to test readily
      orientation = Get45DegRotInXYPlane(new(voxelSizeX, voxelSizeY, voxelSizeZ), new());



      // A step +1 towards i or j should now be the same distance as it's a 45 degr rotation
      //
      // Orig:
      // step "down" (in this im), only y contributes
      //    0
      // |  ------
      // |  |    |
      // V  |    |
      //    |____|
      //
      // Rotated around 0,0,0:
      //  /\
      //0/  \
      // \   \ 
      //  \  /
      //   \/
      //
      // We can calculate the result from trig
      //      b
      // 1i  /|\
      //    / | \ 1j
      //  a/__|  \ 
      //    \ |   \ c
      //     \    /  
      //      \  / 
      //       \/
      //        c
      // a right angled triangle with 45 deg slope and hypotemuse
      // of 1 has two sides each of length sqrt(0.5) (because sqrt(sqrt(0.5)^2 + sqrt(0.5)^2)=1)  
      // so b.x  = sqrt(0.5) i
      // and b-->c.x = b.x + sqrt(0.5) j
      // so a -->c =  sqrt(0.5) *(i + j) (for x)
      //    a -->c =  sqrt(0.5) *(-i + j) (for y. See how we go up a-->b then down b-->c)
      // NB this will be different if rotated the other direction 


      double xExpected = Math.Sqrt(0.5) * (voxelSizeX + voxelSizeY);
      double yExpected = Math.Sqrt(0.5) * (voxelSizeY - voxelSizeX);
      const double zExpected = 0; // shouldn't change

      // First Sanity check moving along each axis. The step should still be the voxel size
      Assert.AreEqual(voxelSizeX, orientation.VoxelToWorldCoordinate(1, 0, 0).DistanceTo(orientation.VoxelToWorldCoordinate(0, 0, 0)), 1e-3);
      Assert.AreEqual(voxelSizeY, orientation.VoxelToWorldCoordinate(0, 1, 0).DistanceTo(orientation.VoxelToWorldCoordinate(0, 0, 0)), 1e-3);
      Assert.AreEqual(voxelSizeZ, orientation.VoxelToWorldCoordinate(0, 0, 1).DistanceTo(orientation.VoxelToWorldCoordinate(0, 0, 0)), 1e-3);

      // Now check moving diagonally from 0i,0j,0k ==> 1i,1j,0k
      Assert.AreEqual(0,new XYZ<double>(xExpected, yExpected, zExpected).DistanceTo(orientation.VoxelToWorldCoordinate(1, 1, 0)), 1e-3);


   }

   [TestMethod]
   public void GetTranslated()
   {
      const float voxelSizeX = 7;
      const float voxelSizeY = 13;
      const float voxelSizeZ = 11;

      const float origTransX = 71;
      const float origTransY = 91;
      const float origTransZ = -171;

      const float translateX = 10;
      const float translateY = 11;
      const float translateZ = 31;

      IReadOnlyOrientation orig = Get31DegRotInXYPlane(new(voxelSizeX, voxelSizeY, voxelSizeZ), new(origTransX, origTransY, origTransZ));
      IReadOnlyOrientation translated = orig.GetTranslated(translateX, translateY, translateZ);

      Check(0, 0, 0);
      Check(0, 0, 1);
      Check(0, 1, 0);
      Check(1, 0, 0);
      Check(1, 1, 1);
      Check(17, 137, 15);

      Assert.AreEqual(orig.VoxelToWorldCoordinate(0,0,0), translated.VoxelToWorldCoordinate(0,0,0) - new XYZ<double>(translateX, translateY, translateZ));

      void Check(int i, int j, int k)
      {
         Assert.AreEqual(orig.VoxelToWorldCoordinate(i, j, k), translated.VoxelToWorldCoordinate(i, j, k) - new XYZ<double>(translateX, translateY, translateZ));
      }

   }
   [TestMethod]
   public void GetTranslatedPointOfReferenceVox()
   {
      const float voxelSizeX = 2;
      const float voxelSizeY = 1;
      const float voxelSizeZ = 1;

      const float origTransX = 0;
      const float origTransY = 0;
      const float origTransZ = 0;

      const long translateXVox = -10;
      const long translateYVox = -11;
      const long translateZVox = -31;

      IReadOnlyOrientation orig = Get31DegRotInXYPlane(new(voxelSizeX, voxelSizeY, voxelSizeZ), new(origTransX, origTransY, origTransZ));
      IReadOnlyOrientation translated = orig.GetForPaddedImage(translateXVox, translateYVox, translateZVox);

      // This method is used to pad images and offset the header so that
      // voxels retain their original position in space
      // So, the voxel originally at (0,0,0) would now correspond to the voxels at 10,11,31
      // and likewise, the voxel at (a,b,c) should now be at 10+a, 11+b, 31+c
      //
      // Orig
      //  --------
      // |a|b|c|d|
      // ---------
      // |e|f|g|h|
      // ---------
      //
      // Offset:
      // 11 cols
      // |
      // V
      // -----------
      // | | | | | | <-- 10 rows added
      // ------------
      // | |a|b|c|d|
      // ------------
      // | |e|f|g|h|
      // ------------
      // | | | | | |
      // ------------



      Check(0, 0, 0);
      Check(0, 0, 1);
      Check(0, 1, 0);
      Check(1, 0, 0);
      Check(1, 1, 1);
      Check(17, 137, 15);


      void Check(int i, int j, int k)
      {
         var expected = orig.VoxelToWorldCoordinate(i, j, k);
         var actual = translated.VoxelToWorldCoordinate(i + translateXVox, j + translateYVox, k + translateZVox);
         Assert.AreEqual(expected.X, actual.X, 1e-3);
         Assert.AreEqual(expected.Y, actual.Y, 1e-3);
         Assert.AreEqual(expected.Z, actual.Z, 1e-3);
      }

   }

}

[TestClass]
public class ImageHeaderTests
{
   [TestMethod]
   public void GetForPaddedImage()
   {
      const int padX0 = 21;
      const int padX1 = 17;
      const int padY0 = 19;
      const int padY1 = 41;
      const int padZ0 = 91;
      const int padZ1 = 3;
      var origMatrix = new OrientationMatrix(ImageTestsBase.GetRandomMatrix4x4(new Random(44)));
      ImageSize origSize = new(3, 5, 7, 11);
      ImageHeader origHead = new(origSize, origMatrix, CoordinateSystem.RAS, EncodingDirection.X, EncodingDirection.Y, EncodingDirection.Z);

      ImageHeader result = origHead.GetForPaddedImage(padX0, padX1, padY0, padY1, padZ0, padZ1, 5, 15);

      Assert.AreEqual(new ImageSize(3 + padX0 + padX1, 5 + padY0 + padY1, 7 + padZ0 + padZ1, 11 + 5 + 15), result.Size, "Image size wrong");

      for (int x = 0; x < 3; x++)
      {
         for (int y = 0; y < 3; y++)
         {
            for (int z = 0; z < 3; z++)
            {
               // coordinates of 0,0,0 in the origin should match coordinates
               // of padx0, pady0, padz0 in the grown image
               var origin = origHead.VoxelToWorldCoordinate(x, y, z);
               var newOrigin = result.VoxelToWorldCoordinate(x+padX0, y+padY0, z+padZ0);
               Assert.AreEqual(origin.X, newOrigin.X, 0.001, "Voxel location unexpected");
               Assert.AreEqual(origin.Y, newOrigin.Y, 0.001, "Voxel location unexpected");
               Assert.AreEqual(origin.Z, newOrigin.Z, 0.001, "Voxel location unexpected");
            }
         }
      }
   }
}
