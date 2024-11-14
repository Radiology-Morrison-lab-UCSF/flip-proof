using System.Numerics;
using TorchSharp;

namespace FlipProof.Torch;
public static class ArrayAndValueExtensionMethods
{
   [CLSCompliant(false)]
   public static torch.Tensor ToTensor1D<T>(this T[] values) => values.ToTensor([values.LongLength]);
   /// <summary>
   /// Creates a row-wise tensor (row moves fastest), so that input[j][k] would give a tensor[j,k]
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="values">All elements must be the same length</param>
   /// <returns></returns>
   [CLSCompliant(false)]
   public static torch.Tensor ToTensor<T>(this T[][] values) where T : struct
   {
      ToTensorPrep(values, out int lenDim0, out T[] as1D);

      return as1D.ToTensor([values.LongLength, lenDim0]);
   }

   private static void ToTensorPrep<T>(T[][] values, out int lenDim0, out T[] as1D) where T : struct
   {
      try
      {
         lenDim0 = values.Select(a => a.Length).Distinct().Single();
      }
      catch (InvalidOperationException)
      {
         throw new ArgumentException("array must not be jagged in lengths", nameof(values));
      }


      as1D = new T[lenDim0 * values.Length];
      int offset = 0;
      for (int i = 0; i < values.Length; i++)
      {
         values[i].CopyTo(as1D.AsSpan(offset));
         offset += lenDim0;
      }
   }

   /// <summary>
   /// Creates a row-wise tensor (row moves fastest), so that input[j][k] would give a tensor[j,k]
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="values">All elements must be the same length</param>
   /// <returns></returns>
   [CLSCompliant(false)]
   public static torch.Tensor ToTensor(this Complex[][] values)
   {
      ToTensorPrep(values, out int lenDim0, out Complex[] as1D);
      return torch.tensor(as1D, [values.LongLength, lenDim0], null, null, false);
   }
   //public static torch.Tensor ToTensor(this byte value, bool requires_grad = false) => torch.tensor(value, requires_grad: requires_grad);
   //public static torch.Tensor ToTensor(this sbyte value, bool requires_grad = false) => torch.tensor(value, requires_grad: requires_grad);
   //public static torch.Tensor ToTensor(this UInt16 value, bool requires_grad = false) => torch.tensor(value, requires_grad: requires_grad);
   //public static torch.Tensor ToTensor(this Int16 value, bool requires_grad = false) => torch.tensor(value, requires_grad: requires_grad);
   //public static torch.Tensor ToTensor(this UInt32 value, bool requires_grad = false) => torch.tensor(value, requires_grad: requires_grad);
   //public static torch.Tensor ToTensor(this Int32 value, bool requires_grad = false) => torch.tensor(value, requires_grad: requires_grad);
   //public static torch.Tensor ToTensor(this UInt64 value, bool requires_grad = false) => torch.tensor(value, requires_grad: requires_grad);
   //public static torch.Tensor ToTensor(this Int64 value, bool requires_grad = false) => torch.tensor(value, requires_grad: requires_grad);
   //public static torch.Tensor ToTensor(this float value, bool requires_grad = false) => torch.tensor(value, requires_grad: requires_grad);
   //public static torch.Tensor ToTensor(this double value, bool requires_grad = false) => torch.tensor(value, requires_grad: requires_grad);
   //public static torch.Tensor ToTensor(this Complex value, bool requires_grad = false) => torch.tensor(value, requires_grad: requires_grad);
   ///// <summary>
   ///// Creates a tensor type (float, float) as this is the internal logic in torchsharp
   ///// </summary>
   ///// <param name="value"></param>
   ///// <param name="requires_grad"></param>
   ///// <returns></returns>
   //public static torch.Tensor ToTensor(this Complex32 value, bool requires_grad = false) => torch.tensor((value.Real, value.Imaginary), requires_grad: requires_grad);

}
