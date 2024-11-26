using System.Numerics;
using TorchSharp;

namespace FlipProof.Torch;

public static class FloatingPointTensorExtensionMethods
{
   /// <summary>
   /// Replaces NaN with the provided value, returning a new tensor
   /// </summary>
   public static TTensor ReplaceNaN<T, TTensor>(this TTensor tensor, T newVal)
      where T : struct, INumber<T>
      where TTensor : SimpleNumericTensor<T, TTensor>, IFloatingPointTensor
   {
      return tensor.CreateFromTensor(torch.nan_to_num(tensor.Storage, nan: Convert.ToDouble(newVal)));
   }

   /// <summary>
   /// Rounds a copy of the input, returning the new tensor
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="TTensor"></typeparam>
   /// <param name="tensor"></param>
   /// <returns></returns>
   public static TTensor Round<T, TTensor>(this TTensor tensor)
      where T : struct, INumber<T>
      where TTensor : SimpleNumericTensor<T, TTensor>, IFloatingPointTensor
   {
      return tensor.CreateFromTensor(torch.round(tensor.Storage), true);
   }
   
   /// <summary>
   /// Rounds the input and returns it
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="TTensor"></typeparam>
   /// <param name="tensor"></param>
   /// <returns></returns>
   public static TTensor RoundInPlace<T, TTensor>(this TTensor tensor)
      where T : struct, INumber<T>
      where TTensor : SimpleNumericTensor<T, TTensor>, IFloatingPointTensor
   {
      torch.round_(tensor.Storage);
      return tensor;
   }
}