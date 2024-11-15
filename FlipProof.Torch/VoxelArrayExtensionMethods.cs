using FlipProof.Base;
using System.Numerics;
using System.Runtime.InteropServices;
using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch;

namespace FlipProof.Torch;

public static class VoxelArrayExtensionMethods
{
   [CLSCompliant(false)]
   public static Tensor ToTensor(this IVoxelArray<Complex> array)
   {
      var as1D = array.GetAllVoxels_LastDimFastest();
      return torch.tensor(as1D, array.Shape.Select(Convert.ToInt64).ToArray(), null, null, false);
   }
   [CLSCompliant(false)]
   public static Tensor ToTensor<T>(this IVoxelArray<T> array) where T : struct => array.GetAllVoxels_LastDimFastest().ToTensor(array.Shape.Select(Convert.ToInt64).ToArray());

   [CLSCompliant(false)]
   public static TArr ToArray<TArr,T>(this Tensor tensor, Func<int[], TArr> createEmptyArray, int expectedDimensions)
      where TArr:IVoxelArray<T>
      where T : struct
   {
      if (tensor.shape.Any(a => a > int.MaxValue))
      {
         throw new ArgumentException("Input tensor is too large");
      }
      if (tensor.shape.Length != expectedDimensions)
      {
         throw new ArgumentException($"Input tensor is not {expectedDimensions}d");
      }

      tensor = tensor.to_type(GetTypeEnum<T>());

      TArr voxelArray = createEmptyArray(tensor.shape.Select(Convert.ToInt32).ToArray());
      T[] arr = MemoryMarshal.Cast<byte, T>(tensor.contiguous().bytes).ToArray();
      voxelArray.SetAllVoxels_LastDimFastest(arr);
      return voxelArray;
   }

   [CLSCompliant(false)]
   public static ScalarType GetTypeEnum<T>() => default(T) switch
   {
      default(double) => ScalarType.Float64,
      default(float) => ScalarType.Float32,
      default(byte) => ScalarType.Byte,
      default(sbyte) => ScalarType.Int8,
      default(short) => ScalarType.Int16,
      default(int) => ScalarType.Int32,
      default(long) => ScalarType.Int64,
      default(bool) => ScalarType.Bool,
      Complex32 => ScalarType.ComplexFloat32,
      Complex => ScalarType.ComplexFloat64,
      _ => throw new NotSupportedException()
   };

}
