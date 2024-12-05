using TorchSharp;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public abstract class FloatingPointTensor<T, TSelf> : SimpleNumericTensor<T, TSelf>, IFloatingPointTensor
  where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
  where TSelf : NumericTensor<T, TSelf>
{
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public FloatingPointTensor(torch.Tensor t) : base(t)
   {
   }

   public override void FillWithRandom()
   {
      using torch.Tensor rand = torch.rand(Storage.shape, DType);
      Storage.copy_(rand);
   }

   /// <summary>
   /// Replaces NaN with the provided value, returning a new tensor
   /// </summary>
   public TSelf ReplaceNaN(T newVal) => CreateFromTensor(Storage.nan_to_num(nan: Convert.ToDouble(newVal)));

   /// <summary>
   /// Rounds a copy of the input, returning the new tensor
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="TTensor"></typeparam>
   /// <param name="tensor"></param>
   /// <returns></returns>
   public TSelf Round() => CreateFromTensor(Storage.round(), true);

   /// <summary>
   /// Rounds the input and returns it
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="TTensor"></typeparam>
   /// <param name="tensor"></param>
   /// <returns></returns>
   public TSelf RoundInPlace()
   {
      Storage.round_();
      return (this as TSelf)!;
   }
}
