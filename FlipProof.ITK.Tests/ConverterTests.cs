using FlipProof.Image;
using FlipProof.ImageTests;
using FlipProof.ITK;
using FlipProof.Torch;
using System.Numerics;
namespace FlipProof.ITK.Tests;

[TestClass]
public sealed class ConverterTests : ITKTest
{
   public ConverterTests() : base(8748)
   {
   }

   [TestMethod]
   public void ToFromITK()
   {
      
      ImageFloat<TestSpace3D> img = GetRandomITKCompatibleImage();
      itk.simple.Image itkIm = img.ToITK();

      ImageFloat<TestSpace3D> result = itkIm.ToFlipProofFloat<TestSpace3D>();

      Assert.IsTrue(img.GetAllVoxelsAsTensor().ValuewiseEquals(result.GetAllVoxelsAsTensor()).All());

   }
}
