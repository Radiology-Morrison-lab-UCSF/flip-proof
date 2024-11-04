using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Torch;

public abstract class NumericTensor<T,TSelf>(torch.Tensor t) : Tensor<T>(t)
   where T : struct
   where TSelf:NumericTensor<T,TSelf>
{
   protected sealed override Tensor<T> CreateFromTensor_Sub(Tensor t)
   {
      return CreateFromTensor(t);
   }
   public new TSelf CreateFromTensor(Tensor t, bool doNotCast=false)
   {
      if (t.dtype != DType)
      {
         if (doNotCast)
         {
            throw new ArgumentException("Input tensor is wrong data type");
         }
         t = t.to_type(DType);
      }
      return CreateFromTensorSub(t);
   }

   /// <summary>
   /// Create a new <typeparamref name="TSelf"/> from this input tensor
   /// </summary>
   /// <param name="t">Guaranteed not null and of the correct type</param>
   /// <returns></returns>
   protected abstract TSelf CreateFromTensorSub(Tensor t);

   private TSelf Create(Tensor other, Func<Tensor, Tensor, Tensor> operation) => CreateFromTensor(operation(this.Storage, other), true);

   public TSelf Add(NumericTensor<T, TSelf> other) => Create(other.Storage, torch.add);
   public TSelf Subtract(NumericTensor<T, TSelf> other) => Create(other.Storage, torch.subtract);
   public TSelf Multiply(NumericTensor<T, TSelf> other) => Create(other.Storage, torch.multiply);

   // Divide is complicated in that it returns float for integer types
}
