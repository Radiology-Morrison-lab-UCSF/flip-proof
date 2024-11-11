using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.ImageTests;
[TestClass]
public  class ISpaceTests() : ImageTestsBase(55)
{
   [TestMethod]
   public void CreateSecondWithCorrectHeader()
   {
      Image.ImageUInt8<TestSpace3D> im1 = GetRandom(out Torch.Tensor<byte> _);
#pragma warning disable CS0618 // Type or member is obsolete
      Image.ImageInt8<TestSpace3D> im2 = new(im1.Header with { }, new sbyte[(int)im1.Header.Size.VoxelCount]); // create with shallow clone of header
#pragma warning restore CS0618 // Type or member is obsolete

      Assert.ReferenceEquals(im1.Header, im2.Header);
   }
}
