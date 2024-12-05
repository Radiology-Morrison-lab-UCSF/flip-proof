using FlipProof.Torch;
using System.Numerics;
using TorchSharp;

namespace FlipProof.Image;

public class ImageComplex<TSpace> : Image<Complex, TSpace>
   where TSpace : struct, ISpace
{
   #region Constructors
#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageComplex<TSpace> UnsafeCreateStatic(ComplexTensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete


   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageComplex(ImageHeader header, Complex[] voxels) : base(header,
     torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount))
   {
      Data = ComplexTensor.CreateTensor(RawData, false);
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageComplex(ComplexTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
      Data = voxels;
   }
   #endregion

   #region Properties
   ComplexTensor Data { get; }

   #endregion

   /// <summary>
   /// Applies a tensor operator to create a new object from the resulting Tensor
   /// </summary>
   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
   /// <typeparam name="TOut">The expected output datatype from the operation</typeparam>
   /// <param name="other">The second image</param>
   /// <param name="operation">The operation to apply to the two images</param>
   /// <returns></returns>
   internal ImageComplex<TSpace> TrustedOperatorToNew(Func<ComplexTensor, ComplexTensor> operation)
   {
      return UnsafeCreateStatic(operation(this.Data));
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
