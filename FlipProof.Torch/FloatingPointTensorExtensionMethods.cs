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
}