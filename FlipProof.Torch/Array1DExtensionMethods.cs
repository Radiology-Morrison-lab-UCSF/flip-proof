using FlipProof.Base;
using static TorchSharp.torch;

namespace FlipProof.Torch;

public static class Array1DExtensionMethods
{
   [CLSCompliant(false)]
   public static Array1D<T> ToArray1D<T>(this Tensor tensor) where T : struct => VoxelArrayExtensionMethods.ToArray<Array1D<T>, T>(tensor, arr => new Array1D<T>(new T[arr[0]]), 1);
   [CLSCompliant(false)]
   public static T[] ToArray<T>(this Tensor tensor) where T : struct => ToArray1D<T>(tensor).Data;
}
