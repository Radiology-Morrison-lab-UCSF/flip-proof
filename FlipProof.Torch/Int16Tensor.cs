using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public sealed partial class Int16Tensor : IntegerTensor<Int16, Int16Tensor>
{

   [CLSCompliant(false)]
   protected override Int16 ToScalar(Tensor t) => t.ToInt16();

   

   #region Operators
   [CLSCompliant(false)]
   public static Int16Tensor operator +(Tensor<sbyte> left, Int16Tensor right) => new(left.Storage + right.Storage);
   [CLSCompliant(false)]
   public static Int16Tensor operator +(Int16Tensor left, Tensor<sbyte> right) => new(left.Storage + right.Storage);
   [CLSCompliant(false)]
   public static Int16Tensor operator -(Tensor<sbyte> left, Int16Tensor right) => new(left.Storage - right.Storage);
   [CLSCompliant(false)]
   public static Int16Tensor operator -(Int16Tensor left, Tensor<sbyte> right) => new(left.Storage - right.Storage);
   [CLSCompliant(false)]
   public static Int16Tensor operator *(Tensor<sbyte> left, Int16Tensor right) => new(left.Storage * right.Storage);
   [CLSCompliant(false)]
   public static Int16Tensor operator *(Int16Tensor left, Tensor<sbyte> right) => new(left.Storage * right.Storage);
   [CLSCompliant(false)]
   public static FloatTensor operator /(Tensor<sbyte> left, Int16Tensor right) => new(left.Storage / right.Storage);
   [CLSCompliant(false)]
   public static FloatTensor operator /(Int16Tensor left, Tensor<sbyte> right) => new(left.Storage / right.Storage);

   public static Int16Tensor operator +(Tensor<byte> left, Int16Tensor right) => new(left.Storage + right.Storage);
   public static Int16Tensor operator +(Int16Tensor left, Tensor<byte> right) => new(left.Storage + right.Storage);
   public static Int16Tensor operator -(Tensor<byte> left, Int16Tensor right) => new(left.Storage - right.Storage);
   public static Int16Tensor operator -(Int16Tensor left, Tensor<byte> right) => new(left.Storage - right.Storage);
   public static Int16Tensor operator *(Tensor<byte> left, Int16Tensor right) => new(left.Storage * right.Storage);
   public static Int16Tensor operator *(Int16Tensor left, Tensor<byte> right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(Tensor<byte> left, Int16Tensor right) => new(left.Storage / right.Storage);
   public static FloatTensor operator /(Int16Tensor left, Tensor<byte> right) => new(left.Storage / right.Storage);

   #endregion
}