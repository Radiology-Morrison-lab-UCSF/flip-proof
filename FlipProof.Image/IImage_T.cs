using FlipProof.Torch;
using System.Numerics;
using TorchSharp;
using static TorchSharp.torch;

namespace FlipProof.Image;

public interface IImage<TVoxel,TSpace> : IImage
   where TVoxel:struct
   where TSpace : ISpace
{
   void OperatorInPlace(IImage<TVoxel, TSpace> other, Action<Tensor, Tensor> operation);
   public void AddInPlace(IImage<TVoxel, TSpace> other) => OperatorInPlace(other, (a, b) => torch.add_(a, b));
   public void SubtractInPlace(IImage<TVoxel, TSpace> other) => OperatorInPlace(other, (a, b) => torch.subtract_(a, b));
   public void MultiplyInPlace(IImage<TVoxel, TSpace> other) => OperatorInPlace(other, (a, b) => torch.multiply_(a, b));
   public void DivideInPlace(IImage<TVoxel, TSpace> other) => OperatorInPlace(other, (a, b) => torch.divide_(a, b));
}