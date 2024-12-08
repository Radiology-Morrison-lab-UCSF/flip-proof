using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed partial class ImageUInt8<TSpace> : Image_Integer<byte, TSpace, ImageUInt8<TSpace>, UInt8Tensor>
   where TSpace : struct, ISpace
{

}
