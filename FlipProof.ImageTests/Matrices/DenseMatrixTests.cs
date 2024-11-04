using FlipProof.Image.Matrices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.ImageTests.Matrices;
[TestClass]
public class DenseMatrixTests
{
   [TestMethod]
   public void SetValue()
   {
      DenseMatrix<float> m = new(13, 35);

      m[12, 27] = 600;

      for (int i = 0; i < 13; i++)
      {
         for (int j = 0; j < 35; j++)
         {
            float expected = (i == 12 && j == 27) ? 600 : 0;
            Assert.AreEqual(expected, m[i, j]);
         }
      }
   }

   [TestMethod]
   public void AddToValue()
   {
      DenseMatrix<float> m = new(13, 35);

      m[1, 7] = 40;
      m[12, 27] = 600;
      m[12, 27] += m[1,7];

      for (int i = 0; i < 13; i++)
      {
         for (int j = 0; j < 35; j++)
         {
            float expected = (i == 12 && j == 27) ? 640 : (i == 1 && j == 7) ? 40 : 0;
            Assert.AreEqual(expected, m[i, j]);
         }
      }
   }
   [TestMethod]
   public void SetDiagonal_Scalar()
   {
      DenseMatrix<float> m = new(13, 35);

      m.SetDiagonal(55);

      for (int i = 0; i < 13; i++)
      {
         for (int j = 0; j < 35; j++)
         {
            float expected = (i == j) ? 55 : 0 ;
            Assert.AreEqual(expected, m[i, j]);
         }
      }
   }

   [TestMethod]
   public void SetDiagonal_Array()
   {
      DenseMatrix<float> m = new(8, 35);

      float[] diag = [1, 3, 5, 7, 11, 13, 17, 23];
      m.SetDiagonal(diag.ToArray());

      for (int i = 0; i < 8; i++)
      {
         for (int j = 0; j < 35; j++)
         {
            float expected = (i == j) ? diag[i] : 0;
            Assert.AreEqual(expected, m[i, j]);
         }
      }
   }
}
