using FlipProof.Image;
using FlipProof.Image.Matrices;
using FlipProof.ImageTests;
using System.Numerics;
namespace FlipProof.ITK.Tests;

public abstract class ITKTest(int seed) : ImageTestsBase(seed)
{
   protected ImageHeader GetRandomITKCompatibleImageHeader()
   {
      ImageHeader head = this.GetRandomHeader();

      Matrix4x4_Optimised<double> mat = new(-1.99879f, -0.0634398, -0.0286845, 121.278,
                                                      -0.0593802, 1.98365, -0.248181, -105.205,
                                                      -0.0363202, 0.247179, 1.98433, -89.5261,
                                                      0, 0, 0, 1);
      head = head with
      {
         // Orthonormal header, not something fully random ITK will reject
         Orientation = new OrientationMatrix(mat)
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
