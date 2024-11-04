using FlipProof.Base;
using FlipProof.Torch;

namespace FlipProof.TorchTests;

[TestClass]
public class VoxelArrayExtensionMethodsTests
{ 
   [TestMethod]
   public void ToTensor3D()
   {
      int size0 = 13;
      int size1 = 17;
      int size2 = 2;

      Random r = new Random(9);
      //      Array3D<float> arr3D = Array3D<float>.FromRandom(r.NextSingle, size0, size1, size2);
      int last = 0;
      Array3D<float> arr3D = Array3D<float>.FromRandom(()=>last++, size0, size1, size2);

      using TorchSharp.torch.Tensor tensor = VoxelArrayExtensionMethods.ToTensor(arr3D);

         for (int z = 1; z < size2; z++)
            for (int y = 0; y < size1; y++)
               for (int x = 0; x < size0; x++)
               {
                  Assert.AreEqual(arr3D[x, y, z], tensor[x, y, z].ReadCpuSingle(0), $"Mismatch at {x}, {y}, {z}");
               }

   }
   
   [TestMethod]
   public void ToTensor4D()
   {
      int seed = 9;
      int size0 = 13;
      int size1 = 17;
      int size2 = 3;
      int size3 = 7;

      Array4D<float> arr4D = CreateRandomArray4D(seed, size0, size1, size2, size3);

      using TorchSharp.torch.Tensor tensor = VoxelArrayExtensionMethods.ToTensor(arr4D);

      for (int i = 0; i < size3; i++)
         for (int z = 0; z < size2; z++)
            for (int y = 0; y < size1; y++)
               for (int x = 0; x < size0; x++)
               {
                  Assert.AreEqual(arr4D[x, y, z, i], tensor[x, y, z, i].ReadCpuSingle(0), $"Mismatch at {x}, {y}, {z}, {i}");
               }

   }

   private static Array4D<float> CreateRandomArray4D(int seed, int size0, int size1, int size2, int size3)
   {
      Random r = new(seed);
      Array3D<float>[] arrs = new Array3D<float>[size3];
      for (int i = 0; i < arrs.Length; i++)
      {
         arrs[i] = Array3D<float>.FromRandom(r.NextSingle, size0, size1, size2);
      }

      return new Array4D<float>(arrs, true);
   }
}