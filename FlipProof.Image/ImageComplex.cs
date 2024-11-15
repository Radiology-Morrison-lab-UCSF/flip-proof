using FlipProof.Torch;
using System.Numerics;
using TorchSharp;

namespace FlipProof.Image;

public class ImageComplex<TSpace> : Image<Complex,TSpace>
   where TSpace : ISpace
{
#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageComplex<TSpace> UnsafeCreateStatic(ComplexTensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete

   ComplexTensor Data { get; }

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageComplex(ImageHeader header, Complex[] voxels) : base(header,
     torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.Volumes))
   {
      Data = ComplexTensor.CreateTensor(RawData, false);
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageComplex(ComplexTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
      Data = voxels;
   }




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
