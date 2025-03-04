using FlipProof.Image;
using FlipProof.ImageTests;
using System.Numerics;
namespace FlipProof.ITK.Tests;

public abstract class ITKTest(int seed) : ImageTestsBase(seed)
{
   protected ImageHeader GetRandomITKCompatibleImageHeader()
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
      return head;
   }
   protected ImageFloat<TestSpace3D> GetRandomITKCompatibleImage()
   {
      ImageHeader head = GetRandomITKCompatibleImageHeader();

      ImageFloat<TestSpace3D> img = this.GetRandom(head, out Torch.Tensor<float> voxels);
      return img;
   }
}
