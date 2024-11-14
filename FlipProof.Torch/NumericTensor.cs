using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Torch;

public abstract class NumericTensor<T,TSelf> : Tensor<T>
   where T : struct
   where TSelf:NumericTensor<T,TSelf>
{
   [CLSCompliant(false)]
   public NumericTensor(torch.Tensor t) : base(t)
   {
   }

   [CLSCompliant(false)]
   protected sealed override Tensor<T> CreateFromTensor_Sub(Tensor t)
   {
      return CreateFromTensor(t);
   }
   [CLSCompliant(false)]
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
   [CLSCompliant(false)]
   protected virtual TSelf CreateFromTensorSub(Tensor t) => throw new NotImplementedException("Must be implemented by deriving class");

   private TSelf Create(Tensor other, Func<Tensor, Tensor, Tensor> operation) => CreateFromTensor(operation(this.Storage, other), true);

   public TSelf Add(NumericTensor<T, TSelf> other) => Create(other.Storage, torch.add);
   public TSelf Subtract(NumericTensor<T, TSelf> other) => Create(other.Storage, torch.subtract);
   public TSelf Multiply(NumericTensor<T, TSelf> other) => Create(other.Storage, torch.multiply);

   // Divide is complicated in that it returns float for integer types
}
