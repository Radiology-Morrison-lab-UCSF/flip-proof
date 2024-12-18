using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Torch;

/// <summary>
/// Unsigned 8bit int
/// </summary>
public sealed partial class UInt8Tensor : IntegerTensor<byte, UInt8Tensor>
{
   [CLSCompliant(false)]
   protected override byte ToScalar(Tensor t) => t.ToByte();

}
