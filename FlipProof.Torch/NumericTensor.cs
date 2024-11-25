using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;



public abstract class NumericTensor<T,TSelf> : Tensor<T,TSelf>
   where T : struct
   where TSelf:NumericTensor<T,TSelf>
{
   [CLSCompliant(false)]
   [SetsRequiredMembers]

   public NumericTensor(torch.Tensor t) : base(t)
   {
   }


   public TSelf Add(NumericTensor<T, TSelf> other) => Create(other.Storage, torch.add);
   public TSelf Subtract(NumericTensor<T, TSelf> other) => Create(other.Storage, torch.subtract);
   public TSelf Multiply(NumericTensor<T, TSelf> other) => Create(other.Storage, torch.multiply);

   // Divide is complicated in that it returns float for integer types

}
