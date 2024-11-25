using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public abstract class NumericTensor<T,TSelf> : Tensor<T>
   where T : struct
   where TSelf:NumericTensor<T,TSelf>
{
   [CLSCompliant(false)]
   [SetsRequiredMembers]

   public NumericTensor(torch.Tensor t) : base(t)
   {
   }

   [CLSCompliant(false)]
   protected sealed override Tensor<T> CreateFromTensor_Sub(Tensor t) => CreateFromTensor(t);

   /// <summary>
   /// Creates a new <see cref="TSelf"/> by applying a torch operation
   /// </summary>
   /// <param name="func">Must return a new object</param>
   /// <param name="doNotCast">when true, if the function returns the wrong type, an exception is thrown. When false, if the function returns the wrong type, the result is cast to <see cref="T"/> </param>
   /// <returns></returns>
   internal TSelf CreateFromTrustedOperation(Func<Tensor,Tensor> func, bool doNotCast = false)
   {
      Tensor t = func(Storage);
      System.Diagnostics.Debug.Assert(!object.ReferenceEquals(t, Storage), "Functions must return a new tensor to avoid sharing");
      return CreateFromTensor(func(Storage), doNotCast);
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

   /// <summary>
   /// Concatenates all along the provided dimension
   /// </summary>
   /// <exception cref="ArgumentException">Empty collection provided</exception>
   public static TSelf Concat(IReadOnlyList<NumericTensor<T, TSelf>> other, int dimension)
   {
      if(other.Count == 0)
      {
         throw new ArgumentException("No tensors provided");
      }
      return other[0].CreateFromTensor(torch.stack(other.Select(a => a.Storage), dimension), doNotCast:true);
   }
}
