﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Torch;
public abstract class Tensor<T, TSelf> : Tensor<T>
   where T : struct
   where TSelf : Tensor<T, TSelf>
{
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public Tensor(torch.Tensor t) : base(t)
   {
   }
   /// <summary>
   /// Creates a new <see cref="TSelf"/> by applying a torch operation
   /// </summary>
   /// <param name="func">Must return a new object</param>
   /// <param name="doNotCast">when true, if the function returns the wrong type, an exception is thrown. When false, if the function returns the wrong type, the result is cast to <see cref="T"/> </param>
   /// <returns></returns>
   internal TSelf CreateFromTrustedOperation(Func<Tensor, Tensor> func)
   {
      Tensor t = func(Storage);
      System.Diagnostics.Debug.Assert(!object.ReferenceEquals(t, Storage), "Functions must return a new tensor to avoid sharing");
      return CreateFromTensor(func(Storage));
   }
   
   /// <summary>
   /// Creates a new Tensor
   /// </summary>
   /// <param name="input">The tensor to wrap</param>
   /// <param name="allowCast">If <paramref name="input"/> is the wrong type, this allows casting rather than throwing an <see cref="ArgumentException"/></param>
   /// <exception cref="ArgumentException">Bad input type</exception>
   [CLSCompliant(false)]
   public new TSelf CreateFromTensor(Tensor input, bool allowCast = false)
   {
      if (input.dtype != DType)
      {
         if (!allowCast)
         {
            throw new ArgumentException("Input tensor is wrong data type");
         }
         input = input.to_type(DType);
      }
      return CreateFromTensorSub(input);
   }

   [CLSCompliant(false)]
   protected sealed override Tensor<T> CreateFromTensor_Sub(Tensor t) => CreateFromTensor(t);



   /// <summary>
   /// Create a new <typeparamref name="TSelf"/> from this input tensor
   /// </summary>
   /// <param name="t">Guaranteed not null and of the correct type</param>
   /// <returns></returns>
   [CLSCompliant(false)]
   protected virtual TSelf CreateFromTensorSub(Tensor t) => throw new NotImplementedException("Must be implemented by deriving class");

   [CLSCompliant(false)]
   protected TSelf Create(Tensor other, Func<Tensor, Tensor, Tensor> operation) => CreateFromTensor(operation(this.Storage, other));


   /// <summary>
   /// Concatenates all along the provided dimension
   /// </summary>
   /// <exception cref="ArgumentException">Empty collection provided</exception>
   public static TSelf Concat(IReadOnlyList<TSelf> other, int dimension)
   {
      if (other.Count == 0)
      {
         throw new ArgumentException("No tensors provided");
      }
      return other[0].CreateFromTensor(torch.stack(other.Select(a => a.Storage), dimension));
   }

   /// <summary>
   /// Fills this tensor with random values
   /// </summary>
   public abstract void FillWithRandom();

}