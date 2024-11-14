﻿using TorchSharp;

namespace FlipProof.Torch;

/// <summary>
/// Tensor for simple numeric types, like double and int, but not Complex
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TSelf"></typeparam>
public abstract class SimpleNumericTensor<T,TSelf> : NumericTensor<T,TSelf>
  where T : struct
  where TSelf : NumericTensor<T, TSelf>
{
   [CLSCompliant(false)]
   public SimpleNumericTensor(torch.Tensor t) : base(t)
   {
   }

   /// <summary>
   /// Returns a new <see cref="TSelf"/> with absolute values
   /// </summary>
   /// <returns></returns>
   public virtual TSelf Abs() => CreateFromTensor(Storage.abs(), doNotCast:true);
   /// <summary>
   /// Converts to absolute values in place
   /// </summary>
   /// <returns></returns>
   public virtual TSelf AbsInPlace() => CreateFromTensor(Storage.abs_(), doNotCast:true);

   public static DoubleTensor operator +(SimpleNumericTensor<T, TSelf> left, DoubleTensor right) => new(left.Storage + right.Storage);
   public static DoubleTensor operator +(DoubleTensor left, SimpleNumericTensor<T, TSelf> right) => new(left.Storage + right.Storage);
   public static DoubleTensor operator -(SimpleNumericTensor<T, TSelf> left, DoubleTensor right) => new(left.Storage - right.Storage);
   public static DoubleTensor operator -(DoubleTensor left, SimpleNumericTensor<T, TSelf> right) => new(left.Storage - right.Storage);
   public static DoubleTensor operator *(SimpleNumericTensor<T, TSelf> left, DoubleTensor right) => new(left.Storage * right.Storage);
   public static DoubleTensor operator *(DoubleTensor left, SimpleNumericTensor<T, TSelf> right) => new(left.Storage * right.Storage);
   public static DoubleTensor operator /(SimpleNumericTensor<T, TSelf> left, DoubleTensor right) => new(left.Storage / right.Storage);
   public static DoubleTensor operator /(DoubleTensor left, SimpleNumericTensor<T, TSelf> right) => new(left.Storage / right.Storage);


}
