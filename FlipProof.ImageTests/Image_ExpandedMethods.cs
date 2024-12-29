using FlipProof.Base;
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

   [TestInitialize]
   public override void Initialise()
   {
      base.Initialise();
#pragma warning disable CS0618 // Type or member is obsolete
      ISpace.Debug_Clear<TestSpacePadded>();
#pragma warning restore CS0618 // Type or member is obsolete
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
   public void PadWithVols()
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageDouble<TestSpace3D> orig = GetRandom(out Tensor<double> vals);

      Box4D<long> newBounds = new Box4D<long>(new(-3,-7,-13,-2), new(orig.Header.Size.X+3+5, orig.Header.Size.Y +7+11, orig.Header.Size.Z + 13+13, orig.Header.Size.VolumeCount+2+1));

      ImageDouble<TestSpacePadded> padded = orig.Pad<TestSpacePadded>(newBounds);

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
   
   [TestMethod]
   [DataRow(-3,-5,-7,-11,-13,-17)]// pad left, crop right
   [DataRow(3,5,7,-11,-13,-17)]// crop left,crop right
   [DataRow(-3,-5,-7,11,13,17)]// pad left, pad right
   [DataRow(3,5,7,11,13,17)] // crop left, pad right
   [DataRow(3,-5,-7,-11,13,17)] // mixture
   public void Pad(long x0, long x1, long y0, long y1, long z0, long z1)
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageDouble<TestSpace3D> orig = GetRandom(35, out Tensor<double> vals);

      var boxOrigin = new XYZA<long>(x0, y0, z0, 0);

      Box4D<long> region = new(boxOrigin, new XYZA<long>(orig.Header.Size.X + x1, orig.Header.Size.X + y1, orig.Header.Size.X + z1, 1));

      ImageDouble<TestSpacePadded> padded = orig.Pad<TestSpacePadded>(region);

      // Check header
      // The coordinate system should be offset by the amount padded
      var origCoord = orig.Header.VoxelToWorldCoordinate(new XYZ<float>(boxOrigin.X, boxOrigin.Y, boxOrigin.Z));
      var newCoord = padded.Header.VoxelToWorldCoordinate(new(0f, 0f, 0f));
      Assert.AreEqual(origCoord, newCoord);

      // Check voxels

      Assert.AreEqual(region.Size.X, padded.Header.Size.X, "Image size");
      Assert.AreEqual(region.Size.Y, padded.Header.Size.Y, "Image size");
      Assert.AreEqual(region.Size.Z, padded.Header.Size.Z, "Image size");

      for (int vol = 0; vol < orig.Header.Size.VolumeCount; vol++)
      {
         for (int paddedX = 0; paddedX < padded.Header.Size.X; paddedX++)
         {
            long origX = paddedX + boxOrigin.X;
            bool xPadded = origX < 0 || origX >= orig.Header.Size.X;
            for (int paddedY = 0; paddedY < padded.Header.Size.Y; paddedY++)
            {
               long origY = paddedY + boxOrigin.Y;

               bool yPadded = origY < 0 || origY >= orig.Header.Size.Y;
               for (int paddedZ = 0; paddedZ < padded.Header.Size.Z; paddedZ++)
               {
                  long origZ = paddedZ + boxOrigin.Z;

                  bool zPadded = origZ < 0 || origZ >= orig.Header.Size.Z;
       
                  var actual = padded[paddedX, paddedY, paddedZ, vol];

                  double expected;
                  if(xPadded || yPadded || zPadded)
                  {
                     expected = 0;
                  }
                  else
                  {
                     expected = orig[origX, origY, origZ, vol];
                  }

                  Assert.AreEqual(expected, actual);
               }
            }
         }
      }
   }

}
