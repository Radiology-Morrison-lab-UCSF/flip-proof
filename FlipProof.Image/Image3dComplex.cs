// TO DO
//using System.Numerics;
//using TorchSharp;
//using static TorchSharp.torch;

//namespace FlipProof.Image;

//public class Image3dComplex : Image3d<Complex>, IMultiplyOperators<Image3dComplex, float, Image3dComplex>
//{
//   internal static Image3dComplex Create(ImageHeader header, Tensor voxels) => new(header, voxels);
//   public Image3dComplex(ImageHeader header) : base(header)
//   {
//   }
//   public Image3dComplex(ImageHeader header, Tensor voxels) : base(header, voxels)
//   {
//   }

//   #region Operators

//   public static Image3dComplex operator +(Image3dComplex left, Image3dDouble right) => left.OperatorToNew(right, torch.add, Create);
//   public static Image3dComplex operator +(Image3dDouble left, Image3dComplex right) => left.OperatorToNew(right, torch.add, Create);
//   public static Image3dComplex operator +(Image3dComplex left, ImageFloat right) => left.OperatorToNew(right, torch.add, Create);
//   public static Image3dComplex operator +(ImageFloat left, Image3dComplex right) => left.OperatorToNew(right, torch.add, Create);


//   public static Image3dComplex operator -(Image3dComplex left, Image3dDouble right) => left.OperatorToNew(right, torch.subtract, Create);
//   public static Image3dComplex operator -(Image3dDouble left, Image3dComplex right) => left.OperatorToNew(right, torch.subtract, Create);
//   public static Image3dComplex operator -(Image3dComplex left, ImageFloat right) => left.OperatorToNew(right, torch.subtract, Create);
//   public static Image3dComplex operator -(ImageFloat left, Image3dComplex right) => left.OperatorToNew(right, torch.subtract, Create);


//   public static Image3dComplex operator *(Image3dComplex left, float right) => left.OperatorToNew(t=>t.multiply(right), Create);
//   public static Image3dComplex operator *(float left, Image3dComplex right) => right.OperatorToNew(t=>t.multiply(left), Create);
//   public static Image3dComplex operator *(Image3dComplex left, Image3dDouble right) => left.OperatorToNew(right, torch.multiply, Create);
//   public static Image3dComplex operator *(Image3dDouble  left, Image3dComplex right) => left.OperatorToNew(right, torch.multiply, Create);
//   public static Image3dComplex operator *(Image3dComplex left, ImageFloat right) => left.OperatorToNew(right, torch.multiply, Create);
//   public static Image3dComplex operator *(ImageFloat  left, Image3dComplex right) => left.OperatorToNew(right, torch.multiply, Create);


//   public static Image3dComplex operator /(Image3dComplex left, Image3dDouble right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);
//   public static Image3dComplex operator /(Image3dDouble left, Image3dComplex right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);
//   public static Image3dComplex operator /(Image3dComplex left, ImageFloat right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);
//   public static Image3dComplex operator /(ImageFloat left, Image3dComplex right) => left.OperatorToNew(right, (a, b) => torch.divide(a, b), Create);


//   #endregion


//}
