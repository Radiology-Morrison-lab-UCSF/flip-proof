using FlipProof.Image;
using FlipProof.ImageTests;
using FlipProof.ITK;
using FlipProof.Torch;
using System.Numerics;
namespace FlipProof.ITK.Tests;

[TestClass]
public sealed class ConverterTests : ImageTestsBase
{
   public ConverterTests() : base(8748)
   {
   }

   [TestMethod]
   public void ToFromITK()
   {
      ImageHeader head = this.GetRandomHeader();

      Matrix4x4 mat = new System.Numerics.Matrix4x4(-1.99879f, -0.0634398f, -0.0286845f, 121.278f,
                                                      -0.0593802f, 1.98365f, -0.248181f, -105.205f,
                                                      -0.0363202f, 0.247179f, 1.98433f, -89.5261f,
                                                      0, 0, 0, 1f);
      //mat.Translation = new System.Numerics.Vector3(121.278f, -105.205f, -89.5261f);
      head = head with
      {
         // Orthonormal header, not something fully random ITK will reject
         Orientation = mat
      };
       
      ImageFloat<TestSpace3D> img =  this.GetRandom(head, out Torch.Tensor<float> voxels);

      itk.simple.Image itkIm = img.ToITK();

      ImageFloat<TestSpace3D> result = itkIm.ToFlipProofFloat<TestSpace3D>();

      Assert.IsTrue(img.GetAllVoxelsAsTensor().ValuewiseEquals(result.GetAllVoxelsAsTensor()).All());

   }
}
