using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public sealed class Int16Tensor : IntegerTensor<Int16, Int16Tensor>
{

   [SetsRequiredMembers]
   public Int16Tensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Int16))
   {
   }


   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public Int16Tensor(Tensor t) : base(t) { }

   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Int16;


   protected override void Set(Int16 value, params long[] indices) => Storage[indices] = value;



   [CLSCompliant(false)]
   protected override Int16Tensor CreateFromTensorSub(Tensor t) => new(t);

   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(Int16 arr) => torch.tensor(arr);
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(Int16[] arr) => torch.tensor(arr);

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