using FlipProof.Torch;
using System.Numerics;
using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch.utils;

namespace FlipProof.Image;

public sealed partial class ImageComplex<TSpace> : ImageComplexType<Complex, TSpace, ImageComplex<TSpace>, ComplexTensor>
   where TSpace : struct, ISpace
{
   #region Constructors
#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageComplex<TSpace> UnsafeCreateStatic(ComplexTensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete


   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   [OrientationCheckedAtRuntime]
   public ImageComplex(ImageHeader header, Complex[] voxels) : base(header,
     ComplexTensor.CreateTensor(torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount), false))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   [OrientationCheckedAtRuntime]
   internal ImageComplex(ImageHeader header, ComplexTensor voxels) : base(header, voxels)
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageComplex(ComplexTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }

#pragma warning disable CS0618 // Type or member is obsolete
   internal override ImageComplex<TSpace> UnsafeCreate(ComplexTensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete

   #endregion

   public ImageDouble<TSpace> Angle() => ImageDouble<TSpace>.UnsafeCreateStatic(Data.Angle());

   public ImageDouble<TSpace> IFFT() => ImageDouble<TSpace>.UnsafeCreateStatic(Data.IFFTN([0, 1, 2]));

   #region Operators

   // TO DO:
   //public static ImageComplex<TSpace> operator +(ImageComplex<TSpace> left, ImageDouble<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   //public static ImageComplex<TSpace> operator +(ImageDouble<TSpace> left, ImageComplex<TSpace> right) => left.OperatorToNew(right, torch.add, Create);
   //public static ImageComplex<TSpace> operator +(ImageComplex<TSpace> left, ImageFloat<TSpace> right) => left.OperatorToNew(right, torch.add, Create);
   //public static ImageComplex<TSpace> operator +(ImageFloat<TSpace>  left, ImageComplex<TSpace> right) => left.OperatorToNew(right, torch.add, Create);


   //public static ImageComplex<TSpace> operator -(ImageComplex<TSpace> left, ImageDouble<TSpace> right) => left.OperatorToNew(right, torch.subtract, Create);
   //public static ImageComplex<TSpace> operator -(ImageDouble<TSpace> left, ImageComplex<TSpace> right) => left.OperatorToNew(right, torch.subtract, Create);
   //public static ImageComplex<TSpace> operator -(ImageComplex<TSpace> left, ImageFloat<TSpace>  right) => left.OperatorToNew(right, torch.subtract, Create);
   //public static ImageComplex<TSpace> operator -(ImageFloat<TSpace>  left, ImageComplex<TSpace> right) => left.OperatorToNew(right, torch.subtract, Create);


   //public static ImageComplex<TSpace> operator *(ImageComplex<TSpace> left, float right) => left.OperatorToNew(t => t.multiply(right), Create);
   //public static ImageComplex<TSpace> operator *(float left, ImageComplex<TSpace> right) => right.OperatorToNew(t => t.multiply(left), Create);
   //public static ImageComplex<TSpace> operator *(ImageComplex<TSpace> left, ImageDouble<TSpace> right) => left.OperatorToNew(right, torch.multiply, Create);
   //public static ImageComplex<TSpace> operator *(ImageDouble<TSpace> left, ImageComplex<TSpace> right) => left.OperatorToNew(right, torch.multiply, Create);
   //public static ImageComplex<TSpace> operator *(ImageComplex<TSpace> left, ImageFloat<TSpace>  right) => left.OperatorToNew(right, torch.multiply, Create);
   //public static ImageComplex<TSpace> operator *(ImageFloat<TSpace>  left, ImageComplex<TSpace> right) => left.OperatorToNew(right, torch.multiply, Create);


   //public static ImageComplex<TSpace> operator /(ImageComplex<TSpace> left, ImageDouble<TSpace> right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);
   //public static ImageComplex<TSpace> operator /(ImageDouble<TSpace> left, ImageComplex<TSpace> right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);
   //public static ImageComplex<TSpace> operator /(ImageComplex<TSpace> left, ImageFloat<TSpace>  right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);
   //public static ImageComplex<TSpace> operator /(ImageFloat<TSpace>  left, ImageComplex<TSpace> right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);


   #endregion


}
