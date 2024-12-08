using FlipProof.Torch;
using System.Numerics;
using TorchSharp;

namespace FlipProof.Image;

public class ImageComplex32<TSpace> : ImageComplexType<Complex32, TSpace, ImageComplex32<TSpace>, Complex32Tensor>
   where TSpace : struct, ISpace
{
   #region Constructors

#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageComplex32<TSpace> UnsafeCreateStatic(Complex32Tensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageComplex32(ImageHeader header, Complex32[] voxels) : base(header,
     Complex32Tensor.CreateTensor( torch.tensor(voxels.Select(a=>(a.Real, a.Imaginary)).ToArray()).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount), false))
   {
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageComplex32(Complex32Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }

   internal override ImageComplex32<TSpace> UnsafeCreate(Complex32Tensor voxels)
   {
#pragma warning disable CS0618 // Type or member is obsolete
      return new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   #endregion


   public ImageFloat<TSpace> IFFT() => ImageFloat<TSpace>.UnsafeCreateStatic(Data.IFFTN());


   #region Operators

   // TO DO:
   //public static ImageComplex32<TSpace> operator +(ImageComplex32<TSpace> left, ImageDouble<TSpace> right) => UnsafeCreateStatic(left.Data + right.Data);
   //public static ImageComplex32<TSpace> operator +(ImageDouble<TSpace> left, ImageComplex32<TSpace> right) => left.OperatorToNew(right, torch.add, Create);
   //public static ImageComplex32<TSpace> operator +(ImageComplex32<TSpace> left, ImageFloat<TSpace> right) => left.OperatorToNew(right, torch.add, Create);
   //public static ImageComplex32<TSpace> operator +(ImageFloat<TSpace>  left, ImageComplex32<TSpace> right) => left.OperatorToNew(right, torch.add, Create);


   //public static ImageComplex32<TSpace> operator -(ImageComplex32<TSpace> left, ImageDouble<TSpace> right) => left.OperatorToNew(right, torch.subtract, Create);
   //public static ImageComplex32<TSpace> operator -(ImageDouble<TSpace> left, ImageComplex32<TSpace> right) => left.OperatorToNew(right, torch.subtract, Create);
   //public static ImageComplex32<TSpace> operator -(ImageComplex32<TSpace> left, ImageFloat<TSpace>  right) => left.OperatorToNew(right, torch.subtract, Create);
   //public static ImageComplex32<TSpace> operator -(ImageFloat<TSpace>  left, ImageComplex32<TSpace> right) => left.OperatorToNew(right, torch.subtract, Create);


   //public static ImageComplex32<TSpace> operator *(ImageComplex32<TSpace> left, float right) => left.OperatorToNew(t => t.multiply(right), Create);
   //public static ImageComplex32<TSpace> operator *(float left, ImageComplex32<TSpace> right) => right.OperatorToNew(t => t.multiply(left), Create);
   //public static ImageComplex32<TSpace> operator *(ImageComplex32<TSpace> left, ImageDouble<TSpace> right) => left.OperatorToNew(right, torch.multiply, Create);
   //public static ImageComplex32<TSpace> operator *(ImageDouble<TSpace> left, ImageComplex32<TSpace> right) => left.OperatorToNew(right, torch.multiply, Create);
   //public static ImageComplex32<TSpace> operator *(ImageComplex32<TSpace> left, ImageFloat<TSpace>  right) => left.OperatorToNew(right, torch.multiply, Create);
   //public static ImageComplex32<TSpace> operator *(ImageFloat<TSpace>  left, ImageComplex32<TSpace> right) => left.OperatorToNew(right, torch.multiply, Create);


   //public static ImageComplex32<TSpace> operator /(ImageComplex32<TSpace> left, ImageDouble<TSpace> right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);
   //public static ImageComplex32<TSpace> operator /(ImageDouble<TSpace> left, ImageComplex32<TSpace> right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);
   //public static ImageComplex32<TSpace> operator /(ImageComplex32<TSpace> left, ImageFloat<TSpace>  right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);
   //public static ImageComplex32<TSpace> operator /(ImageFloat<TSpace>  left, ImageComplex32<TSpace> right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);


   #endregion


}
