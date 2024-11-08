﻿using FlipProof.Torch;
using System.Numerics;
using TorchSharp;
using static TorchSharp.torch;
using TensorExtensionMethods = FlipProof.Torch.TensorExtensionMethods;

namespace FlipProof.Image;
public sealed class ImageDouble<TSpace> : Image_SimpleNumeric<double, TSpace, ImageDouble<TSpace>, DoubleTensor>
      where TSpace : ISpace
{

#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageDouble<TSpace> UnsafeCreateStatic(DoubleTensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageDouble(ImageHeader header, double[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.Volumes))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageDouble(DoubleTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }

   protected override ImageDouble<TSpace> UnsafeCreate(DoubleTensor voxels) => ImageDouble<TSpace>.UnsafeCreateStatic(voxels);

   /// <summary>
   /// Multiplies the first factor then adds the second, in place.
   /// </summary>
   /// <remarks>This is equivalent of nifti scaling</remarks>
   /// <param name="multiplyBy">factor</param>
   /// <param name="thenAdd">added after multiplication</param>
   public void MulAddInPlace(double multiplyBy, double thenAdd)
   {
      _data.Storage.mul_(multiplyBy).add_(thenAdd);
   }

   #region Operators

   public static ImageDouble<TSpace> operator *(ImageDouble<TSpace> left, double right) => UnsafeCreateStatic(left.Data * right);
   public static ImageDouble<TSpace> operator *(double left, ImageDouble<TSpace> right) => UnsafeCreateStatic(right.Data * left);
   public static ImageDouble<TSpace> operator /(ImageDouble<TSpace> left, double right) => UnsafeCreateStatic(left.Data * (1.0/right));
   public static ImageDouble<TSpace> operator /(double left, ImageDouble<TSpace> right) => UnsafeCreateStatic(right.Data * (1.0/left));


   #endregion
}
