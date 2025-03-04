using FlipProof.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.ImageTests
{
   [TestClass]
   public class GenMethodsTests
    {
      [TestMethod]
      public void PadToLength_Longer()
      {
         int[] input = new int[] { 1, 2, 3, 4, 5 };

         var result = GenMethods.PadToLength(input, 8, -1);

         Assert.AreNotSame(input, result);

         CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, -1, -1, -1  }, result.ToArray());

      }

      [TestMethod]
      [DataRow(-1)]
      [DataRow(2)]
      [DataRow(5)]
      public void PadToLength_ShorterOrEqual(int length)
      {
         int[] input = new int[] { 1, 2, 3, 4, 5 };

         var result = GenMethods.PadToLength(input, 8, -1);

         Assert.AreNotSame(input, result);

         CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5 }, input);

      }

   }
}
