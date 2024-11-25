using FlipProof.Torch;
using System.Numerics;
using TorchSharp;

namespace FlipProof.Image;

public class ImageComplex32<TSpace> : Image<Complex32,TSpace>
   where TSpace : ISpace
{
   #region Constructors

#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageComplex32<TSpace> UnsafeCreateStatic(Complex32Tensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageComplex32(ImageHeader header, Complex32[] voxels) : base(header,
     torch.tensor(voxels.Select(a=>(a.Real, a.Imaginary)).ToArray()).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.Volumes))
   {
      Data = Complex32Tensor.CreateTensor(RawData, false);
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageComplex32(Complex32Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
      Data = voxels;
   }

   #endregion

   #region Properties
   Complex32Tensor Data { get; }


   #endregion

   public ImageFloat<TSpace> IFFT() => ImageFloat<TSpace>.UnsafeCreateStatic(Data.IFFTN());

   /// <summary>
   /// Applies a tensor operator to create a new object from the resulting Tensor
   /// </summary>
   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
   /// <typeparam name="TOut">The expected output datatype from the operation</typeparam>
   /// <param name="other">The second image</param>
   /// <param name="operation">The operation to apply to the two images</param>
   /// <returns></returns>
   internal ImageComplex32<TSpace> TrustedOperatorToNew(Func<Complex32Tensor, Complex32Tensor> operation)
   {
      return UnsafeCreateStatic(operation(Data));
   }

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
