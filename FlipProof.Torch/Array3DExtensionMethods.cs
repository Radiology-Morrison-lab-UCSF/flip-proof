using FlipProof.Base;
using static TorchSharp.torch;

namespace FlipProof.Torch;

public static class Array3DExtensionMethods
{
   [CLSCompliant(false)]
   public static Array3D<T> ToArray3D<T>(this Tensor tensor) where T : struct => VoxelArrayExtensionMethods.ToArray<Array3D<T>, T>(tensor, arr => new Array3D<T>(arr[0], arr[1], arr[2]), 3);
}
