using FlipProof.Base;
using FlipProof.Image;
using FlipProof.Torch;

namespace FlipProof.ImageTests;

[TestClass]
public class ImageBoolTests() :OperatorsTests(988145)
{
   [TestMethod]
   public void GetAllVoxelIndicesWhereTrue()
   {
      ImageHeader imageHeader = GetRandomHeader() with { Size = new ImageSize(3, 11, 13, 5) };

      ImageDouble<TestSpace3D> orig = GetRandom(imageHeader, out Tensor<double> voxels);

      var arr4D = orig.GetVoxelsAsArray4D();

      var gtp5 = (orig > 0.5).GetAllVoxelIndicesWhereTrue().ToHashSet();

      for (int i = 0; i < imageHeader.Size.X; i++)
      {
         for (int j = 0; j < imageHeader.Size.Y; j++)
         {
            for (int k = 0; k < imageHeader.Size.Z; k++)
            {
               for (int vol = 0; vol < imageHeader.Size.VolumeCount; vol++)
               {
                  var coord = new XYZA<long>(i, j, k, vol);
                  bool found = gtp5.Contains(coord);
                  Assert.AreEqual(orig[coord] > 0.5, found);
               }
            }
         }
      }
   }
}
