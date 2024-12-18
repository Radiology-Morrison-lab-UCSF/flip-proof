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
public class ImageHeaderTests
{
   [TestMethod]
   public void GetVoxelSize()
   {
      float voxelSizeX = 7;
      float voxelSizeY = 13;
      float voxelSizeZ = 11;

      Matrix4x4 imageMatrix = new(voxelSizeX, 0, 0, 101,
         0, voxelSizeY, 0, 11,
         0, 0, voxelSizeZ, 31,
         0, 0, 0, 1
         );

      Assert.AreEqual(new XYZ<float>(voxelSizeX, voxelSizeY, voxelSizeZ), ImageHeader.GetVoxelSizeFromMatrix( imageMatrix ));
      
      // Rotate 45 degrees in xy plane
      // The x and y axes sizes are now even 
      float cos45Deg = MathF.Cos(MathF.PI / 4);
      float sin45Deg = MathF.Sin(MathF.PI / 4);
      Matrix4x4 rotationMatrix = new(cos45Deg, -sin45Deg, 0, 0,
                                    sin45Deg, cos45Deg, 0, 0,
                                    0, 0, 1, 0,
                                    0, 0, 0, 1);

      Matrix4x4 rotatedImage =  imageMatrix * rotationMatrix;

      // x and y sizes should now be equal as it's a 45 degr rotation
      // The size is thus the number that would have resulted in the same distance from (0,0,0) to (1,1,0) if used in both axes
      // dist to 111 =sqrt( vox_x^2 + vox_y^2)
      float xYSizes = MathF.Sqrt((voxelSizeX * voxelSizeX + voxelSizeY * voxelSizeY) / 2);
      var actual = ImageHeader.GetVoxelSizeFromMatrix(rotatedImage);
      Assert.IsTrue(new XYZ<float>(xYSizes, xYSizes, voxelSizeZ).DistanceTo(actual) < 0.01f);
   }


   [TestMethod]
   public void GetForPaddedImage()
   {
      const int padX0 = 21;
      const int padY0 = 19;
      const int padZ0 = 91;
      var origMatrix = ImageTestsBase.GetRandomMatrix4x4(new Random(44));
      ImageSize origSize = new(11, 3, 5, 7);
      ImageHeader origHead = new(origSize, origMatrix, CoordinateSystem.RAS, EncodingDirection.X, EncodingDirection.Y, EncodingDirection.Z);

      ImageHeader result = origHead.GetForPaddedImage(padX0, 17, padY0, 41, padZ0, 3, 5, 15);

      Assert.AreEqual(new ImageSize(11 + 5 + 15, 3 + padX0 + 17, 5 + padY0 + 41, 7 + padZ0 + 3), result.Size, "Image size wrong");

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
