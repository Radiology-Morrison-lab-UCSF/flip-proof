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
public  partial class Image_ExpandedMethodsTests() : ImageTestsBase(984)
{
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
}
