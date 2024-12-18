using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;
public sealed partial class FloatTensor : FloatingPointTensor<float, FloatTensor>, IFloatingPointTensor
{

   [CLSCompliant(false)]
   protected override float ToScalar(Tensor t) => t.ToSingle();



   #region Operators
   // Operators remove ambiguity between float and double operations specified in the base class
   public static DoubleTensor operator +(FloatTensor left, DoubleTensor right) => new(left.Storage + right.Storage);
   public static DoubleTensor operator +(DoubleTensor left, FloatTensor right) => new(left.Storage + right.Storage);
   public static DoubleTensor operator -(FloatTensor left, DoubleTensor right) => new(left.Storage - right.Storage);
   public static DoubleTensor operator -(DoubleTensor left, FloatTensor right) => new(left.Storage - right.Storage);
   public static DoubleTensor operator *(FloatTensor left, DoubleTensor right) => new(left.Storage * right.Storage);
   public static DoubleTensor operator *(DoubleTensor left, FloatTensor right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(FloatTensor left, FloatTensor right) => new(left.Storage / right.Storage);
   public static DoubleTensor operator /(FloatTensor left, DoubleTensor right) => new(left.Storage / right.Storage);
   public static DoubleTensor operator /(DoubleTensor left, FloatTensor right) => new(left.Storage / right.Storage);
   #endregion
}
