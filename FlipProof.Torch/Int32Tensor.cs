using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public sealed partial class Int32Tensor : IntegerTensor<Int32, Int32Tensor>
{

   [CLSCompliant(false)]
   protected override Int32 ToScalar(Tensor t) => t.ToInt32();

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
   public static Int32Tensor operator +(Tensor<Int16> left, Int32Tensor right) => new(left.Storage + right.Storage);
   public static Int32Tensor operator +(Int32Tensor left, Tensor<Int16> right) => new(left.Storage + right.Storage);
   public static Int32Tensor operator -(Tensor<Int16> left, Int32Tensor right) => new(left.Storage - right.Storage);
   public static Int32Tensor operator -(Int32Tensor left, Tensor<Int16> right) => new(left.Storage - right.Storage);
   public static Int32Tensor operator *(Tensor<Int16> left, Int32Tensor right) => new(left.Storage * right.Storage);
   public static Int32Tensor operator *(Int32Tensor left, Tensor<Int16> right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(Tensor<Int16> left, Int32Tensor right) => new(left.Storage / right.Storage);
   public static FloatTensor operator /(Int32Tensor left, Tensor<Int16> right) => new(left.Storage / right.Storage);

   [CLSCompliant(false)]
   public static Int32Tensor operator +(Tensor<sbyte> left, Int32Tensor right) => new(left.Storage + right.Storage);
   [CLSCompliant(false)]
   public static Int32Tensor operator +(Int32Tensor left, Tensor<sbyte> right) => new(left.Storage + right.Storage);
   [CLSCompliant(false)]
   public static Int32Tensor operator -(Tensor<sbyte> left, Int32Tensor right) => new(left.Storage - right.Storage);
   [CLSCompliant(false)]
   public static Int32Tensor operator -(Int32Tensor left, Tensor<sbyte> right) => new(left.Storage - right.Storage);
   [CLSCompliant(false)]
   public static Int32Tensor operator *(Tensor<sbyte> left, Int32Tensor right) => new(left.Storage * right.Storage);
   [CLSCompliant(false)]
   public static Int32Tensor operator *(Int32Tensor left, Tensor<sbyte> right) => new(left.Storage * right.Storage);
   [CLSCompliant(false)]
   public static FloatTensor operator /(Tensor<sbyte> left, Int32Tensor right) => new(left.Storage / right.Storage);
   [CLSCompliant(false)]
   public static FloatTensor operator /(Int32Tensor left, Tensor<sbyte> right) => new(left.Storage / right.Storage);

   public static Int32Tensor operator +(Tensor<byte> left, Int32Tensor right) => new(left.Storage + right.Storage);
   public static Int32Tensor operator +(Int32Tensor left, Tensor<byte> right) => new(left.Storage + right.Storage);
   public static Int32Tensor operator -(Tensor<byte> left, Int32Tensor right) => new(left.Storage - right.Storage);
   public static Int32Tensor operator -(Int32Tensor left, Tensor<byte> right) => new(left.Storage - right.Storage);
   public static Int32Tensor operator *(Tensor<byte> left, Int32Tensor right) => new(left.Storage * right.Storage);
   public static Int32Tensor operator *(Int32Tensor left, Tensor<byte> right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(Tensor<byte> left, Int32Tensor right) => new(left.Storage / right.Storage);
   public static FloatTensor operator /(Int32Tensor left, Tensor<byte> right) => new(left.Storage / right.Storage);

   #endregion
}