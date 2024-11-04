using FlipProof.Torch;
using System.Runtime.CompilerServices;
using TorchSharp;
using static TorchSharp.torch;

namespace FlipProof.Image;

public class ImageBool<TSpace> : Image<bool, TSpace>
   where TSpace : ISpace
{
#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageBool<TSpace> UnsafeCreate(Tensor<bool> voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageBool(ImageHeader header, bool[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.Volumes))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageBool(Tensor<bool> voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }

   private new ImageBool<TSpace> ToBool() => this;

   /// <summary>
   /// Logical Not
   /// </summary>
   public ImageBool<TSpace> Not() => TrustedOperatorToNew(Torch.TensorExtensionMethods.Not, UnsafeCreate);
   /// <summary>
   /// Logical Not. Returns this object
   /// </summary>
   public ImageBool<TSpace> NotInPlace()
   {
      _data.NotInPlace();
      return this;
   }

   #region Operators

   /// <summary>
   /// Bitwise and
   /// </summary>
   public static ImageBool<TSpace> operator &(ImageBool<TSpace> left, ImageBool<TSpace> right) => left.TrustedOperatorToNew(right, Torch.TensorExtensionMethods.And, UnsafeCreate);
   /// <summary>
   /// Bitwise or
   /// </summary>
   public static ImageBool<TSpace> operator |(ImageBool<TSpace> left, ImageBool<TSpace> right) => left.TrustedOperatorToNew(right, Torch.TensorExtensionMethods.Or, UnsafeCreate);
   /// <summary>
   /// Bitwise exclusive or
   /// </summary>
   public static ImageBool<TSpace> operator ^(ImageBool<TSpace> left, ImageBool<TSpace> right) => left.TrustedOperatorToNew(right, Torch.TensorExtensionMethods.XOr, UnsafeCreate);


   #endregion
}
