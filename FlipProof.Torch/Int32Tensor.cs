using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Torch;

public class Int32Tensor : IntegerTensor<Int32, Int32Tensor>
{

   public Int32Tensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Int32))
   {
   }

   public Int32Tensor(Tensor t) : base(t) { }

   public override ScalarType DType => ScalarType.Int32;


   protected override void Set(Int32 value, params long[] indices) => _storage[indices] = value;


   protected override Int32Tensor CreateSameSizeBlank() => new(_storage.shape);

   protected override Int32Tensor CreateFromTensorSub(Tensor t) => new(t);


   public override Tensor ScalarToTensor(Int32 arr) => torch.tensor(arr);
   public override Tensor ArrayToTensor(Int32[] arr) => torch.tensor(arr);

   protected override Int32 ToScalar(Tensor t) => t.ToInt32();

   #region Operators
   public static Int32Tensor operator +(Tensor<Int16> left, Int32Tensor right) => new(left.Storage + right.Storage);
   public static Int32Tensor operator +(Int32Tensor left, Tensor<Int16> right) => new(left.Storage + right.Storage);
   public static Int32Tensor operator -(Tensor<Int16> left, Int32Tensor right) => new(left.Storage - right.Storage);
   public static Int32Tensor operator -(Int32Tensor left, Tensor<Int16> right) => new(left.Storage - right.Storage);
   public static Int32Tensor operator *(Tensor<Int16> left, Int32Tensor right) => new(left.Storage * right.Storage);
   public static Int32Tensor operator *(Int32Tensor left, Tensor<Int16> right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(Tensor<Int16> left, Int32Tensor right) => new(left.Storage / right.Storage);
   public static FloatTensor operator /(Int32Tensor left, Tensor<Int16> right) => new(left.Storage / right.Storage);

   public static Int32Tensor operator +(Tensor<sbyte> left, Int32Tensor right) => new(left.Storage + right.Storage);
   public static Int32Tensor operator +(Int32Tensor left, Tensor<sbyte> right) => new(left.Storage + right.Storage);
   public static Int32Tensor operator -(Tensor<sbyte> left, Int32Tensor right) => new(left.Storage - right.Storage);
   public static Int32Tensor operator -(Int32Tensor left, Tensor<sbyte> right) => new(left.Storage - right.Storage);
   public static Int32Tensor operator *(Tensor<sbyte> left, Int32Tensor right) => new(left.Storage * right.Storage);
   public static Int32Tensor operator *(Int32Tensor left, Tensor<sbyte> right) => new(left.Storage * right.Storage);
   public static FloatTensor operator /(Tensor<sbyte> left, Int32Tensor right) => new(left.Storage / right.Storage);
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