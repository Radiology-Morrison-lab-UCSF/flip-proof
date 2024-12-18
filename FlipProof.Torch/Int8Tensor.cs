using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

/// <summary>
/// <see cref="sbyte"/> tensor
/// </summary>
/// <remarks>Nomenclature used for consistency between int types</remarks>
[CLSCompliant(false)]
public sealed partial class Int8Tensor : IntegerTensor<sbyte, Int8Tensor>
{

   protected override sbyte ToScalar(Tensor t) => t.ToSByte();


   #region Operators
   public static Int16Tensor operator +(Tensor<byte> left, Int8Tensor right) => new(left.Storage + right.Storage);
   public static Int16Tensor operator +(Int8Tensor left, Tensor<byte> right) => new(left.Storage + right.Storage);
   public static Int16Tensor operator -(Tensor<byte> left, Int8Tensor right) => new(left.Storage - right.Storage);
   public static Int16Tensor operator -(Int8Tensor left, Tensor<byte> right) => new(left.Storage - right.Storage);
   public static Int16Tensor operator *(Tensor<byte> left, Int8Tensor right) => new(left.Storage * right.Storage);
   public static Int16Tensor operator *(Int8Tensor left, Tensor<byte> right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(Tensor<byte> left, Int8Tensor right) => new(left.Storage / right.Storage);
   public static FloatTensor operator /(Int8Tensor left, Tensor<byte> right) => new(left.Storage / right.Storage);

   #endregion
}