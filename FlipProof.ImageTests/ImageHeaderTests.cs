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
}
