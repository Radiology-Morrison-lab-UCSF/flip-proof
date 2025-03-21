using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.ImageTests.Maths
{
   [TestClass]
   public class GenTests
   {

      [TestMethod]
      public void ToNextMultiple()
      {
         int[] inputs = [
            -7,-3,-1,1,3,5,7,11,13,19,23,48,100
            ];

         int[] expected = [
            -6,-3,0,3,3,6,9,12,15,21,24,48,102
            ];

         CollectionAssert.AreEqual(expected, inputs.Select(a=> FlipProof.Image.Maths.Gen.ToNextMultiple(a, 3)).ToArray());
      }
   }
}
