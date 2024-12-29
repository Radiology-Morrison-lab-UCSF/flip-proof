using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public sealed partial class Int64Tensor : IntegerTensor<Int64, Int64Tensor>
{

   [CLSCompliant(false)]
   protected override Int64 ToScalar(Tensor t) => t.ToInt64();

   /// <summary>
   /// Forward fourier transform
   /// </summary>
   /// <param name="dimensions"></param>
   /// <returns></returns>
   public new ComplexTensor FFTN(long[]? dimensions = null)
   {
      using var asD = ToDouble(); // torch processed as float, which results in lost precision
      return asD.FFTN(dimensions);
   }

   #region Operators
   public static Int64Tensor operator +(Tensor<Int32> left, Int64Tensor right) => new(left.Storage + right.Storage);
   public static Int64Tensor operator +(Int64Tensor left, Tensor<Int32> right) => new(left.Storage + right.Storage);
   public static Int64Tensor operator -(Tensor<Int32> left, Int64Tensor right) => new(left.Storage - right.Storage);
   public static Int64Tensor operator -(Int64Tensor left, Tensor<Int32> right) => new(left.Storage - right.Storage);
   public static Int64Tensor operator *(Tensor<Int32> left, Int64Tensor right) => new(left.Storage * right.Storage);
   public static Int64Tensor operator *(Int64Tensor left, Tensor<Int32> right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(Tensor<Int32> left, Int64Tensor right) => new(left.Storage / right.Storage);
   public static FloatTensor operator /(Int64Tensor left, Tensor<Int32> right) => new(left.Storage / right.Storage);
   
   public static Int64Tensor operator +(Tensor<Int16> left, Int64Tensor right) => new(left.Storage + right.Storage);
   public static Int64Tensor operator +(Int64Tensor left, Tensor<Int16> right) => new(left.Storage + right.Storage);
   public static Int64Tensor operator -(Tensor<Int16> left, Int64Tensor right) => new(left.Storage - right.Storage);
   public static Int64Tensor operator -(Int64Tensor left, Tensor<Int16> right) => new(left.Storage - right.Storage);
   public static Int64Tensor operator *(Tensor<Int16> left, Int64Tensor right) => new(left.Storage * right.Storage);
   public static Int64Tensor operator *(Int64Tensor left, Tensor<Int16> right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(Tensor<Int16> left, Int64Tensor right) => new(left.Storage / right.Storage);
   public static FloatTensor operator /(Int64Tensor left, Tensor<Int16> right) => new(left.Storage / right.Storage);

   [CLSCompliant(false)]
   public static Int64Tensor operator +(Tensor<sbyte> left, Int64Tensor right) => new(left.Storage + right.Storage);
   [CLSCompliant(false)]
   public static Int64Tensor operator +(Int64Tensor left, Tensor<sbyte> right) => new(left.Storage + right.Storage);
   [CLSCompliant(false)]
   public static Int64Tensor operator -(Tensor<sbyte> left, Int64Tensor right) => new(left.Storage - right.Storage);
   [CLSCompliant(false)]
   public static Int64Tensor operator -(Int64Tensor left, Tensor<sbyte> right) => new(left.Storage - right.Storage);
   [CLSCompliant(false)]
   public static Int64Tensor operator *(Tensor<sbyte> left, Int64Tensor right) => new(left.Storage * right.Storage);
   [CLSCompliant(false)]
   public static Int64Tensor operator *(Int64Tensor left, Tensor<sbyte> right) => new(left.Storage * right.Storage);
   [CLSCompliant(false)]
   public static FloatTensor operator /(Tensor<sbyte> left, Int64Tensor right) => new(left.Storage / right.Storage);
   [CLSCompliant(false)]
   public static FloatTensor operator /(Int64Tensor left, Tensor<sbyte> right) => new(left.Storage / right.Storage);

   public static Int64Tensor operator +(Tensor<byte> left, Int64Tensor right) => new(left.Storage + right.Storage);
   public static Int64Tensor operator +(Int64Tensor left, Tensor<byte> right) => new(left.Storage + right.Storage);
   public static Int64Tensor operator -(Tensor<byte> left, Int64Tensor right) => new(left.Storage - right.Storage);
   public static Int64Tensor operator -(Int64Tensor left, Tensor<byte> right) => new(left.Storage - right.Storage);
   public static Int64Tensor operator *(Tensor<byte> left, Int64Tensor right) => new(left.Storage * right.Storage);
   public static Int64Tensor operator *(Int64Tensor left, Tensor<byte> right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(Tensor<byte> left, Int64Tensor right) => new(left.Storage / right.Storage);
   public static FloatTensor operator /(Int64Tensor left, Tensor<byte> right) => new(left.Storage / right.Storage);

   #endregion
}