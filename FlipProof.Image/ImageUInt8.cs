using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed class ImageUInt8<TSpace> : Image_Integer<byte, TSpace, ImageUInt8<TSpace>, UInt8Tensor>
   where TSpace : ISpace
{
   #region Constructors
#pragma warning disable CS0618 // Type or member is obsolete
   internal override ImageUInt8<TSpace> UnsafeCreate(UInt8Tensor voxels) => new(voxels, false);

   internal static ImageUInt8<TSpace> UnsafeCreateStatic(UInt8Tensor voxels)
   {
      return new(voxels, false);
   }
#pragma warning restore CS0618 // Type or member is obsolete

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageUInt8(ImageHeader header, byte[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.Volumes))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageUInt8(UInt8Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {

   }
   #endregion
}
