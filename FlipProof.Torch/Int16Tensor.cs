using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Torch;

public class Int16Tensor : IntegerTensor<Int16, Int16Tensor>
{

   public Int16Tensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Int16))
   {
   }

   public Int16Tensor(Tensor t) : base(t) { }

   public override ScalarType DType => ScalarType.Int16;


   protected override void Set(Int16 value, params long[] indices) => _storage[indices] = value;


   protected override Int16Tensor CreateSameSizeBlank() => new(_storage.shape);

   protected override Int16Tensor CreateFromTensorSub(Tensor t) => new(t);


   public override Tensor ScalarToTensor(Int16 arr) => torch.tensor(arr);
   public override Tensor ArrayToTensor(Int16[] arr) => torch.tensor(arr);

   protected override Int16 ToScalar(Tensor t) => t.ToInt16();

   #region Operators
   public static Int16Tensor operator +(Tensor<sbyte> left, Int16Tensor right) => new(left.Storage + right.Storage);
   public static Int16Tensor operator +(Int16Tensor left, Tensor<sbyte> right) => new(left.Storage + right.Storage);
   public static Int16Tensor operator -(Tensor<sbyte> left, Int16Tensor right) => new(left.Storage - right.Storage);
   public static Int16Tensor operator -(Int16Tensor left, Tensor<sbyte> right) => new(left.Storage - right.Storage);
   public static Int16Tensor operator *(Tensor<sbyte> left, Int16Tensor right) => new(left.Storage * right.Storage);
   public static Int16Tensor operator *(Int16Tensor left, Tensor<sbyte> right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(Tensor<sbyte> left, Int16Tensor right) => new(left.Storage / right.Storage);
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