using FlipProof.Base;
using System.Numerics;
using System.Runtime.InteropServices;
using TorchSharp;
using static TorchSharp.torch;

namespace FlipProof.Torch;

public static class Array2DExtensionMethods
{
   [CLSCompliant(false)]
   public static Array2D<T> ToArray2D<T>(this Tensor tensor) where T : struct => VoxelArrayExtensionMethods.ToArray<Array2D<T>, T>(tensor, arr => new Array2D<T>(arr[0], arr[1]), 2);
}
