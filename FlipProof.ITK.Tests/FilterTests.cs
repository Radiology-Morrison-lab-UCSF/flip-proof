using FlipProof.Image;
using FlipProof.ImageTests;
namespace FlipProof.ITK.Tests;

[TestClass]
public sealed class FilterTests : ITKTest
{
   public FilterTests() : base(344)
   {
   }

   [TestMethod]
   public void BinaryThreshold()
   {
      ImageFloat<TestSpace3D> img = GetRandomITKCompatibleImage();

      ImageFloat<TestSpace3D> result = img.ToITK().BinaryThreshold().ToFlipProof<TestSpace3D>().ToFloat();

      Assert.AreNotSame(img, result);

      ImageFloat<TestSpace3D> expected = (img > 0).ToFloat();

      Assert.IsTrue(expected.GetAllVoxelsAsTensor().ValuewiseEquals(result.GetAllVoxelsAsTensor()).All());
   }
}
