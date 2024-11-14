using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Torch;

public class Int64Tensor : IntegerTensor<Int64, Int64Tensor>
{

   public Int64Tensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Int64))
   {
   }
   [CLSCompliant(false)]
   public Int64Tensor(Tensor t) : base(t) { }

   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Int64;


   protected override void Set(Int64 value, params long[] indices) => _storage[indices] = value;


   protected override Int64Tensor CreateSameSizeBlank() => new(_storage.shape);

   [CLSCompliant(false)]
   protected override Int64Tensor CreateFromTensorSub(Tensor t) => new(t);

   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(Int64 arr) => torch.tensor(arr);
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(Int64[] arr) => torch.tensor(arr);

   [CLSCompliant(false)]
   protected override Int64 ToScalar(Tensor t) => t.ToInt64();

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