using FlipProof.Image.Matrices;
using FlipProof.Image.Nifti;
using FlipProof.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlipProof.Image.IO;
using System.Numerics;
using FlipProof.Base;

namespace FlipProof.ImageTests.Nifti
{
   [TestClass]
   public class DecomposableNiftiTransformDTests
   {
      [TestMethod]
      public void FromNiftiQuaternions()
      {
         /*
          * Rotation matrix should be:
               [ -0.7081229,  0.4710896,  0.5259625;
                  0.5995155,  0.0076180,  0.8003269;
                  0.3730189,  0.8820525, -0.2878200 ]
          */
         var decomposable = DecomposableNiftiTransformD.FromNiftiQuaternions(0.3781795, 0.707736, 0.5942821, [5.4, 3.3, 7.1], [21.3, -87.1, 101.72], 1);

         var pixDim = decomposable.GetPixDim();
         Assert.AreEqual(5.4, pixDim[0], 1e-6);
         Assert.AreEqual(3.3, pixDim[1], 1e-6);
         Assert.AreEqual(7.1, pixDim[2], 1e-6);
         var trans = decomposable.GetTranslation();
         Assert.AreEqual(21.3, trans[0], 1e-5);
         Assert.AreEqual(-87.1, trans[1], 1e-5);
         Assert.AreEqual(101.72, trans[2], 1e-4);

         CollectionAssert.AreEqual(new double[] { -0.7081229, 0.4710896, 0.5259625 }, decomposable.GetRotation().GetRow(0),  new DoubleComparer(1e-4));
         CollectionAssert.AreEqual(new double[] { 0.5995155, 0.0076180, 0.8003269 }, decomposable.GetRotation().GetRow(1), new DoubleComparer(1e-4));
         CollectionAssert.AreEqual(new double[] { 0.3730189, 0.8820525, -0.2878200 }, decomposable.GetRotation().GetRow(2), new DoubleComparer(1e-4));

         // Check the orientation matrix is correct vs coordinates we've derived from
         // third party programs

         // 7,11,13 --> 60.18, 9.708, 121.3

         var result = decomposable.VoxelToWorldCoordinate(7, 11, 13);
         Assert.AreEqual(60.18, result.X, 0.01);
         Assert.AreEqual(9.708, result.Y, 0.01);
         Assert.AreEqual(121.3, result.Z, 0.1); // we only know to 4sf due to the third party programs only displaying this

      }

   }
}
