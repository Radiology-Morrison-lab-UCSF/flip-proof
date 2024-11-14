using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Torch;

/// <summary>
/// Unsigned 8bit int
/// </summary>
public class UInt8Tensor : IntegerTensor<byte, UInt8Tensor>
{

   public UInt8Tensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Byte))
   {
   }

   [CLSCompliant(false)]
   public UInt8Tensor(Tensor t) : base(t) { }

   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Byte;


   protected override void Set(byte value, params long[] indices)
   {
      _storage[indices] = value;
   }

   protected override Tensor<byte> CreateSameSizeBlank() => new UInt8Tensor(_storage.shape);

   [CLSCompliant(false)]
   protected override UInt8Tensor CreateFromTensorSub(Tensor t) => new UInt8Tensor(t);


   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(byte arr) => torch.tensor(arr);
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(byte[] arr) => torch.tensor(arr);


   [CLSCompliant(false)]
   protected override byte ToScalar(Tensor t) => t.ToByte();

}
