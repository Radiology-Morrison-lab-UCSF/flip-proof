using FlipProof.Image;
using FlipProof.Torch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TorchSharp;

namespace FlipProof.ImageTests;
[TestClass]
public partial class Image_ExpandedMethodsTests() : ImageTestsBase(1)
{
   struct TestSpacePadded : ISpace
   {

   }

   [TestMethod]
   public void AddScalarToIndex()
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageDouble<TestSpace3D> orig = GetRandom(out Tensor<double> vals);

      ImageBool<TestSpace3D> mask = GetRandom(orig.Header, out bool[] maskVals);

      ImageDouble<TestSpace3D> result = orig.DeepClone();

      ImageDouble<TestSpace3D> resultAgain = result;

      result[mask] += 11;

      // Change should be in place
      Assert.AreSame(result, resultAgain);

      // Voxels outside the mask should be unchanged
      CollectionAssert.AreEqual(orig[mask.Not()].GetAllVoxels(), result[mask.Not()].GetAllVoxels());
      
      // Voxels outside the mask should be changed
      CollectionAssert.AreEqual(orig[mask].GetAllVoxels().Zip(mask.GetAllVoxels()).Select(pair=>pair.First + (pair.Second ? 11 : 0)).ToArray(), result[mask].GetAllVoxels());

   }

   [TestMethod]
   public void PadSurround()
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageDouble<TestSpace3D> orig = GetRandom(out Tensor<double> vals);

      ImageDouble<TestSpacePadded> padded = orig.Pad<TestSpacePadded>(3, 5, 7, 11, 13, 17, 2, 1);

      // Check header
      // The coordinate system should be offset by the amount padded
      var origCoord = orig.Header.VoxelToWorldCoordinate(new(-3f, -7f, -13f));
      var newCoord = padded.Header.VoxelToWorldCoordinate(new(0f, 0f, 0f));
      Assert.AreEqual(origCoord, newCoord);

      // Check voxels
      for (int vol = 0; vol < orig.Header.Size.VolumeCount; vol++)
      {
         bool volPadded = vol < 2 || vol > 3;
         for (int x = 0; x < padded.Header.Size.X; x++)
         {
            bool xPadded = x < 3 || (x + 3) > orig.Header.Size.X;
            for (int y = 0; y < padded.Header.Size.Y; y++)
            {
               bool yPadded = y < 7 || (y + 7) > orig.Header.Size.Y;
               for (int z = 0; z < padded.Header.Size.Z; z++)
               {
                  bool zPadded = z < 13 || (z + 13) > orig.Header.Size.Z;
       
                  var actual = padded[x, y, z, vol];

                  double expected;
                  if(volPadded || xPadded || yPadded || zPadded)
                  {
                     expected = 0;
                  }
                  else
                  {
                     expected = orig[x - 3, y - 7, z - 13, vol];
                  }

                  Assert.AreEqual(expected, actual);
               }
            }
         }
      }
   }

}
