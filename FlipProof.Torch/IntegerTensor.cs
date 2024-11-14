using System.Numerics;
using System.Security.AccessControl;
using TorchSharp;

namespace FlipProof.Torch;

public abstract class IntegerTensor<T, TSelf> : SimpleNumericTensor<T, TSelf>
  where T : struct, IBinaryInteger<T>
  where TSelf : NumericTensor<T, TSelf>
{
   [CLSCompliant(false)]
   public IntegerTensor(torch.Tensor t) : base(t)
   {
   }

   public static FloatTensor operator +(IntegerTensor<T, TSelf> left, FloatTensor right) => new(left.Storage + right.Storage);
   public static FloatTensor operator +(FloatTensor left, IntegerTensor<T, TSelf> right) => new(left.Storage + right.Storage);
   public static FloatTensor operator -(IntegerTensor<T, TSelf> left, FloatTensor right) => new(left.Storage - right.Storage);
   public static FloatTensor operator -(FloatTensor left, IntegerTensor<T, TSelf> right) => new(left.Storage - right.Storage);
   public static FloatTensor operator *(IntegerTensor<T, TSelf> left, FloatTensor right) => new(left.Storage * right.Storage);
   public static FloatTensor operator *(FloatTensor left, IntegerTensor<T, TSelf> right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(IntegerTensor<T, TSelf> left, FloatTensor right) => new(left.Storage / right.Storage);
   public static FloatTensor operator /(FloatTensor left, IntegerTensor<T, TSelf> right) => new(left.Storage / right.Storage);
   /// <summary>
   /// Divides two integers and returns a float (default torch behaviour)
   /// </summary>
   /// <remarks>Does not apply to DoubleTensor as it has an explicit division operator</remarks>
   /// <param name="left"></param>
   /// <param name="right"></param>
   /// <returns></returns>
   public static FloatTensor operator /(IntegerTensor<T, TSelf> left, IntegerTensor<T, TSelf> right) => new(left.Storage / right.Storage);

   /// <summary>
   /// Divides two integer tensors, returning a float tensor
   /// </summary>
   /// <typeparam name="S"></typeparam>
   /// <typeparam name="SSelf"></typeparam>
   /// <param name="left"></param>
   /// <param name="right"></param>
   /// <returns></returns>
   internal FloatTensor DivideToF<S, SSelf>(IntegerTensor<S, SSelf> right)
      where S : struct, IBinaryInteger<S>
      where SSelf : NumericTensor<S, SSelf>
   {
      return new(this.Storage / right.Storage);
   }
}