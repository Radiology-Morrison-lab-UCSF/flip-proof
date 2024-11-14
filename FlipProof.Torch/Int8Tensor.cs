using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Torch;

/// <summary>
/// <see cref="sbyte"/> tensor
/// </summary>
/// <remarks>Nomenclature used for consistency between int types</remarks>
[CLSCompliant(false)]
public class Int8Tensor : IntegerTensor<sbyte, Int8Tensor>
{

   public Int8Tensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Int8))
   {
   }

   public Int8Tensor(Tensor t) : base(t) { }

   public override ScalarType DType => ScalarType.Int8;


   protected override void Set(sbyte value, params long[] indices) => _storage[indices] = value;


   protected override Int8Tensor CreateSameSizeBlank() => new(_storage.shape);

   protected override Int8Tensor CreateFromTensorSub(Tensor t) => new(t);


   public override Tensor ScalarToTensor(sbyte arr) => torch.tensor(arr);
   public override Tensor ArrayToTensor(sbyte[] arr) => torch.tensor(arr);

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