using TorchSharp;
using static TorchSharp.torch;

namespace FlipProof.Torch;
public static class TensorExtensionMethods
{


   /// <summary>
   /// Gets elements indicated by the mask and range for a 2D tensor
   /// </summary>
   /// <param name="tensor">The source tensor</param>
   /// <param name="maskDim0">True/False to indicate which indices to get for dim 0</param>
   /// <param name="indicesDim1">Range of indices to get for dim 1</param>
   /// <exception cref="NotSupportedException">Tensor is larger than supported</exception>
   public static Tensor GetSubTensor(this Tensor tensor, Tensor maskDim0, Range indicesDim1)
   {
      if (tensor.shape[1] > int.MaxValue)
      {
         throw new NotSupportedException("This operation is not supported for Tensors size larger than Int32 for Dim 1");
      }

      Tensor t1 = RangeToMaskTensor((int)tensor.shape[1], indicesDim1);

      return tensor[TensorIndex.Tensor(maskDim0), TensorIndex.Tensor(t1)];

   }

   private static Tensor RangeToMaskTensor(int dimSize, Range indicesDim1)
   {
      bool[] dim1s = new bool[dimSize];

      (int offset, int len) = indicesDim1.GetOffsetAndLength(dim1s.Length);

      int to = offset + len;
      for (int i = offset; i < to; i++)
      {
         dim1s[i] = true;
      }

      Tensor t1 = dim1s.ToTensor([dimSize]);
      return t1;
   }

   /// <summary>
   /// Sets elements indicated by the mask and range for a 2D tensor
   /// </summary>
   /// <param name="tensor">The destination tensor</param>
   /// <param name="maskDim0">True/False to indicate which indices to get for dim 0</param>
   /// <param name="indicesDim1">Range of indices to get for dim 1</param>
   /// <param name="value">The new values</param>
   /// <exception cref="NotSupportedException">Tensor is larger than supported</exception>
   public static Tensor SetSubTensor(this Tensor tensor, Tensor maskDim0, Range indicesDim1, Tensor value)
   {
      if (tensor.shape[1] > int.MaxValue)
      {
         throw new NotSupportedException("This operation is not supported for Tensors size larger than Int32 for Dim 1");
      }

      return tensor[maskDim0, RangeToMaskTensor((int)tensor.shape[1], indicesDim1)] = value;
   }

   #region Wrapped Tensors


   #region Double

   private static DoubleTensor OperationToTensor(this Tensor<double> left, Tensor<double> right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right.Storage));
   private static DoubleTensor OperationToTensor(this Tensor<double> left, double right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
  
   public static DoubleTensor Add(this Tensor<double> left, Tensor<double> right) => OperationToTensor(left, right, torch.add);
   public static DoubleTensor Subtract(this Tensor<double> left, Tensor<double> right) => OperationToTensor(left, right, torch.subtract);
   public static DoubleTensor Multiply(this Tensor<double> left, Tensor<double> right) => OperationToTensor(left, right, torch.multiply);
   public static DoubleTensor Divide(this Tensor<double> left, Tensor<double> right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));

   public static DoubleTensor Add(this Tensor<double> left, double right) => OperationToTensor(left, right, torch.add);
   public static DoubleTensor Subtract(this Tensor<double> left, double right) => OperationToTensor(left, right, torch.subtract);
   public static DoubleTensor Multiply(this Tensor<double> left, double right) => OperationToTensor(left, right, torch.multiply);
   public static DoubleTensor Divide(this Tensor<double> left, double right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static DoubleTensor LeftDivide(this Tensor<double> right, double left) => new (torch.divide(left, right.Storage));

   // Other types are implicitly casted to double
   #endregion


   #region Float

   private static FloatTensor OperationToTensor(this Tensor<float> left, Tensor<float> right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right.Storage));
   private static DoubleTensor OperationToTensor(this Tensor<float> left, double right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static FloatTensor OperationToTensor(this Tensor<float> left, float right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));

   public static FloatTensor Add(this Tensor<float> left, Tensor<float> right) => OperationToTensor(left, right, torch.add);
   public static FloatTensor Subtract(this Tensor<float> left, Tensor<float> right) => OperationToTensor(left, right, torch.subtract);
   public static FloatTensor Multiply(this Tensor<float> left, Tensor<float> right) => OperationToTensor(left, right, torch.multiply);
   public static FloatTensor Divide(this Tensor<float> left, Tensor<float> right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));

   public static DoubleTensor Add(this Tensor<float> left, double right) => OperationToTensor(left, right, torch.add);
   public static DoubleTensor Subtract(this Tensor<float> left, double right) => OperationToTensor(left, right, torch.subtract);
   public static DoubleTensor Multiply(this Tensor<float> left, double right) => OperationToTensor(left, right, torch.multiply);
   public static DoubleTensor Divide(this Tensor<float> left, double right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static DoubleTensor LeftDivide(this Tensor<float> right, double left) => new (torch.divide(left, right.Storage));

   public static FloatTensor Add(this Tensor<float> left, float right) => OperationToTensor(left, right, torch.add);
   public static FloatTensor Subtract(this Tensor<float> left, float right) => OperationToTensor(left, right, torch.subtract);
   public static FloatTensor Multiply(this Tensor<float> left, float right) => OperationToTensor(left, right, torch.multiply);
   public static FloatTensor Divide(this Tensor<float> left, float right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static FloatTensor LeftDivide(this Tensor<float> right, float left) => new (torch.divide(left, right.Storage));

   // Other types are implicitly casted to float

   #endregion


   #region Int64

   private static Int64Tensor OperationToTensor(this Tensor<Int64> left, Tensor<Int64> right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right.Storage));
   private static DoubleTensor OperationToTensor(this Tensor<Int64> left, double right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static FloatTensor OperationToTensor(this Tensor<Int64> left, float right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int64Tensor OperationToTensor(this Tensor<Int64> left, Int64 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   
   public static Int64Tensor Add(this Tensor<Int64> left, Tensor<Int64> right) => OperationToTensor(left, right, torch.add);
   public static Int64Tensor Subtract(this Tensor<Int64> left, Tensor<Int64> right) => OperationToTensor(left, right, torch.subtract);
   public static Int64Tensor Multiply(this Tensor<Int64> left, Tensor<Int64> right) => OperationToTensor(left, right, torch.multiply);
   public static Int64Tensor Divide(this Tensor<Int64> left, Tensor<Int64> right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));

   public static DoubleTensor Add(this Tensor<Int64> left, double right) => OperationToTensor(left, right, torch.add);
   public static DoubleTensor Subtract(this Tensor<Int64> left, double right) => OperationToTensor(left, right, torch.subtract);
   public static DoubleTensor Multiply(this Tensor<Int64> left, double right) => OperationToTensor(left, right, torch.multiply);
   public static DoubleTensor Divide(this Tensor<Int64> left, double right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static DoubleTensor LeftDivide(this Tensor<Int64> right, double left) => new(torch.divide(left, right.Storage));

   public static FloatTensor Add(this Tensor<Int64> left, float right) => OperationToTensor(left, right, torch.add);
   public static FloatTensor Subtract(this Tensor<Int64> left, float right) => OperationToTensor(left, right, torch.subtract);
   public static FloatTensor Multiply(this Tensor<Int64> left, float right) => OperationToTensor(left, right, torch.multiply);
   public static FloatTensor Divide(this Tensor<Int64> left, float right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static FloatTensor LeftDivide(this Tensor<Int64> right, float left) => new(torch.divide(left, right.Storage));

   public static Int64Tensor Add(this Tensor<Int64> left, Int64 right) => OperationToTensor(left, right, torch.add);
   public static Int64Tensor Subtract(this Tensor<Int64> left, Int64 right) => OperationToTensor(left, right, torch.subtract);
   public static Int64Tensor Multiply(this Tensor<Int64> left, Int64 right) => OperationToTensor(left, right, torch.multiply);
   public static Int64Tensor Divide(this Tensor<Int64> left, Int64 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int64Tensor LeftDivide(this Tensor<Int64> right, Int64 left) => new(torch.divide(left, right.Storage));

   // Other types implicitly casted to long or float

   #endregion
   
   #region Int32

   private static Int32Tensor OperationToTensor(this Tensor<Int32> left, Tensor<Int32> right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right.Storage));
   private static DoubleTensor OperationToTensor(this Tensor<Int32> left, double right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static FloatTensor OperationToTensor(this Tensor<Int32> left, float right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int64Tensor OperationToTensor(this Tensor<Int32> left, Int64 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int32Tensor OperationToTensor(this Tensor<Int32> left, Int32 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
 
   // Other types implicitly case to Int64 or Int32

   public static Int32Tensor Add(this Tensor<Int32> left, Tensor<Int32> right) => OperationToTensor(left, right, torch.add);
   public static Int32Tensor Subtract(this Tensor<Int32> left, Tensor<Int32> right) => OperationToTensor(left, right, torch.subtract);
   public static Int32Tensor Multiply(this Tensor<Int32> left, Tensor<Int32> right) => OperationToTensor(left, right, torch.multiply);
   public static Int32Tensor Divide(this Tensor<Int32> left, Tensor<Int32> right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));

   public static DoubleTensor Add(this Tensor<Int32> left, double right) => OperationToTensor(left, right, torch.add);
   public static DoubleTensor Subtract(this Tensor<Int32> left, double right) => OperationToTensor(left, right, torch.subtract);
   public static DoubleTensor Multiply(this Tensor<Int32> left, double right) => OperationToTensor(left, right, torch.multiply);
   public static DoubleTensor Divide(this Tensor<Int32> left, double right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static DoubleTensor LeftDivide(this Tensor<Int32> right, double left) => new(torch.divide(left, right.Storage));

   public static FloatTensor Add(this Tensor<Int32> left, float right) => OperationToTensor(left, right, torch.add);
   public static FloatTensor Subtract(this Tensor<Int32> left, float right) => OperationToTensor(left, right, torch.subtract);
   public static FloatTensor Multiply(this Tensor<Int32> left, float right) => OperationToTensor(left, right, torch.multiply);
   public static FloatTensor Divide(this Tensor<Int32> left, float right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static FloatTensor LeftDivide(this Tensor<Int32> right, float left) => new(torch.divide(left, right.Storage));

   public static Int64Tensor Add(this Tensor<Int32> left, Int64 right) => OperationToTensor(left, right, torch.add);
   public static Int64Tensor Subtract(this Tensor<Int32> left, Int64 right) => OperationToTensor(left, right, torch.subtract);
   public static Int64Tensor Multiply(this Tensor<Int32> left, Int64 right) => OperationToTensor(left, right, torch.multiply);
   public static Int64Tensor Divide(this Tensor<Int32> left, Int64 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int64Tensor LeftDivide(this Tensor<Int32> right, Int64 left) => new(torch.divide(left, right.Storage));

   public static Int32Tensor Add(this Tensor<Int32> left, Int32 right) => OperationToTensor(left, right, torch.add);
   public static Int32Tensor Subtract(this Tensor<Int32> left, Int32 right) => OperationToTensor(left, right, torch.subtract);
   public static Int32Tensor Multiply(this Tensor<Int32> left, Int32 right) => OperationToTensor(left, right, torch.multiply);
   public static Int32Tensor Divide(this Tensor<Int32> left, Int32 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int32Tensor LeftDivide(this Tensor<Int32> right, Int32 left) => new(torch.divide(left, right.Storage));

   #endregion

   #region Int16

   private static Int16Tensor OperationToTensor(this Tensor<Int16> left, Tensor<Int16> right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right.Storage));
   private static DoubleTensor OperationToTensor(this Tensor<Int16> left, double right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static FloatTensor OperationToTensor(this Tensor<Int16> left, float right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int64Tensor OperationToTensor(this Tensor<Int16> left, Int64 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int32Tensor OperationToTensor(this Tensor<Int16> left, Int32 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int16Tensor OperationToTensor(this Tensor<Int16> left, Int16 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
 
   // Other types implicitly cast to Int16, Int32, Int64 or float

   public static Int16Tensor Add(this Tensor<Int16> left, Tensor<Int16> right) => OperationToTensor(left, right, torch.add);
   public static Int16Tensor Subtract(this Tensor<Int16> left, Tensor<Int16> right) => OperationToTensor(left, right, torch.subtract);
   public static Int16Tensor Multiply(this Tensor<Int16> left, Tensor<Int16> right) => OperationToTensor(left, right, torch.multiply);
   public static Int16Tensor Divide(this Tensor<Int16> left, Tensor<Int16> right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));

   public static DoubleTensor Add(this Tensor<Int16> left, double right) => OperationToTensor(left, right, torch.add);
   public static DoubleTensor Subtract(this Tensor<Int16> left, double right) => OperationToTensor(left, right, torch.subtract);
   public static DoubleTensor Multiply(this Tensor<Int16> left, double right) => OperationToTensor(left, right, torch.multiply);
   public static DoubleTensor Divide(this Tensor<Int16> left, double right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static DoubleTensor LeftDivide(this Tensor<Int16> right, double left) => new(torch.divide(left, right.Storage));

   public static FloatTensor Add(this Tensor<Int16> left, float right) => OperationToTensor(left, right, torch.add);
   public static FloatTensor Subtract(this Tensor<Int16> left, float right) => OperationToTensor(left, right, torch.subtract);
   public static FloatTensor Multiply(this Tensor<Int16> left, float right) => OperationToTensor(left, right, torch.multiply);
   public static FloatTensor Divide(this Tensor<Int16> left, float right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static FloatTensor LeftDivide(this Tensor<Int16> right, float left) => new(torch.divide(left, right.Storage));

   public static Int64Tensor Add(this Tensor<Int16> left, Int64 right) => OperationToTensor(left, right, torch.add);
   public static Int64Tensor Subtract(this Tensor<Int16> left, Int64 right) => OperationToTensor(left, right, torch.subtract);
   public static Int64Tensor Multiply(this Tensor<Int16> left, Int64 right) => OperationToTensor(left, right, torch.multiply);
   public static Int64Tensor Divide(this Tensor<Int16> left, Int64 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int64Tensor LeftDivide(this Tensor<Int16> right, Int64 left) => new(torch.divide(left, right.Storage));

   public static Int32Tensor Add(this Tensor<Int16> left, Int32 right) => OperationToTensor(left, right, torch.add);
   public static Int32Tensor Subtract(this Tensor<Int16> left, Int32 right) => OperationToTensor(left, right, torch.subtract);
   public static Int32Tensor Multiply(this Tensor<Int16> left, Int32 right) => OperationToTensor(left, right, torch.multiply);
   public static Int32Tensor Divide(this Tensor<Int16> left, Int32 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int32Tensor LeftDivide(this Tensor<Int16> right, Int32 left) => new(torch.divide(left, right.Storage));

   public static Int16Tensor Add(this Tensor<Int16> left, Int16 right) => OperationToTensor(left, right, torch.add);
   public static Int16Tensor Subtract(this Tensor<Int16> left, Int16 right) => OperationToTensor(left, right, torch.subtract);
   public static Int16Tensor Multiply(this Tensor<Int16> left, Int16 right) => OperationToTensor(left, right, torch.multiply);
   public static Int16Tensor Divide(this Tensor<Int16> left, Int16 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int16Tensor LeftDivide(this Tensor<Int16> right, Int16 left) => new(torch.divide(left, right.Storage));

   #endregion


   #region Int8
   
   private static Int8Tensor OperationToTensor(this Tensor<sbyte> left, Tensor<sbyte> right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right.Storage));
   private static DoubleTensor OperationToTensor(this Tensor<sbyte> left, double right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static FloatTensor OperationToTensor(this Tensor<sbyte> left, float right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int64Tensor OperationToTensor(this Tensor<sbyte> left, Int64 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int32Tensor OperationToTensor(this Tensor<sbyte> left, Int32 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int16Tensor OperationToTensor(this Tensor<sbyte> left, Int16 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int8Tensor OperationToTensor(this Tensor<sbyte> left, sbyte right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int8Tensor OperationToTensor(this Tensor<sbyte> left, byte right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));

   public static Int8Tensor Add(this Tensor<sbyte> left, Tensor<sbyte> right) => OperationToTensor(left, right, torch.add);
   public static Int8Tensor Subtract(this Tensor<sbyte> left, Tensor<sbyte> right) => OperationToTensor(left, right, torch.subtract);
   public static Int8Tensor Multiply(this Tensor<sbyte> left, Tensor<sbyte> right) => OperationToTensor(left, right, torch.multiply);
   public static Int8Tensor Divide(this Tensor<sbyte> left, Tensor<sbyte> right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));

   public static DoubleTensor Add(this Tensor<sbyte> left, double right) => OperationToTensor(left, right, torch.add);
   public static DoubleTensor Subtract(this Tensor<sbyte> left, double right) => OperationToTensor(left, right, torch.subtract);
   public static DoubleTensor Multiply(this Tensor<sbyte> left, double right) => OperationToTensor(left, right, torch.multiply);
   public static DoubleTensor Divide(this Tensor<sbyte> left, double right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static DoubleTensor LeftDivide(this Tensor<sbyte> right, double left) => new(torch.divide(left, right.Storage));

   public static FloatTensor Add(this Tensor<sbyte> left, float right) => OperationToTensor(left, right, torch.add);
   public static FloatTensor Subtract(this Tensor<sbyte> left, float right) => OperationToTensor(left, right, torch.subtract);
   public static FloatTensor Multiply(this Tensor<sbyte> left, float right) => OperationToTensor(left, right, torch.multiply);
   public static FloatTensor Divide(this Tensor<sbyte> left, float right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static FloatTensor LeftDivide(this Tensor<sbyte> right, float left) => new(torch.divide(left, right.Storage));

   public static Int64Tensor Add(this Tensor<sbyte> left, Int64 right) => OperationToTensor(left, right, torch.add);
   public static Int64Tensor Subtract(this Tensor<sbyte> left, Int64 right) => OperationToTensor(left, right, torch.subtract);
   public static Int64Tensor Multiply(this Tensor<sbyte> left, Int64 right) => OperationToTensor(left, right, torch.multiply);
   public static Int64Tensor Divide(this Tensor<sbyte> left, Int64 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int64Tensor LeftDivide(this Tensor<sbyte> right, Int64 left) => new(torch.divide(left, right.Storage));

   public static Int32Tensor Add(this Tensor<sbyte> left, Int32 right) => OperationToTensor(left, right, torch.add);
   public static Int32Tensor Subtract(this Tensor<sbyte> left, Int32 right) => OperationToTensor(left, right, torch.subtract);
   public static Int32Tensor Multiply(this Tensor<sbyte> left, Int32 right) => OperationToTensor(left, right, torch.multiply);
   public static Int32Tensor Divide(this Tensor<sbyte> left, Int32 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int32Tensor LeftDivide(this Tensor<sbyte> right, Int32 left) => new(torch.divide(left, right.Storage));

   public static Int16Tensor Add(this Tensor<sbyte> left, Int16 right) => OperationToTensor(left, right, torch.add);
   public static Int16Tensor Subtract(this Tensor<sbyte> left, Int16 right) => OperationToTensor(left, right, torch.subtract);
   public static Int16Tensor Multiply(this Tensor<sbyte> left, Int16 right) => OperationToTensor(left, right, torch.multiply);
   public static Int16Tensor Divide(this Tensor<sbyte> left, Int16 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int16Tensor LeftDivide(this Tensor<sbyte> right, Int16 left) => new(torch.divide(left, right.Storage));

   public static Int8Tensor Add(this Tensor<sbyte> left, sbyte right) => OperationToTensor(left, right, torch.add);
   public static Int8Tensor Subtract(this Tensor<sbyte> left, sbyte right) => OperationToTensor(left, right, torch.subtract);
   public static Int8Tensor Multiply(this Tensor<sbyte> left, sbyte right) => OperationToTensor(left, right, torch.multiply);
   public static Int8Tensor Divide(this Tensor<sbyte> left, sbyte right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int8Tensor LeftDivide(this Tensor<sbyte> right, sbyte left) => new(torch.divide(left, right.Storage));

   public static Int8Tensor Add(this Tensor<sbyte> left, byte right) => OperationToTensor(left, right, torch.add);
   public static Int8Tensor Subtract(this Tensor<sbyte> left, byte right) => OperationToTensor(left, right, torch.subtract);
   public static Int8Tensor Multiply(this Tensor<sbyte> left, byte right) => OperationToTensor(left, right, torch.multiply);
   public static Int8Tensor Divide(this Tensor<sbyte> left, byte right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int8Tensor LeftDivide(this Tensor<sbyte> right, byte left) => new(torch.divide(left, right.Storage));

   #endregion



   #region UInt8

   private static UInt8Tensor OperationToTensor(this Tensor<byte> left, Tensor<byte> right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right.Storage));
   private static DoubleTensor OperationToTensor(this Tensor<byte> left, double right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static FloatTensor OperationToTensor(this Tensor<byte> left, float right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int64Tensor OperationToTensor(this Tensor<byte> left, Int64 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int32Tensor OperationToTensor(this Tensor<byte> left, Int32 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int16Tensor OperationToTensor(this Tensor<byte> left, Int16 right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static Int8Tensor OperationToTensor(this Tensor<byte> left, sbyte right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));
   private static UInt8Tensor OperationToTensor(this Tensor<byte> left, byte right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right));

   public static UInt8Tensor Add(this Tensor<byte> left, Tensor<byte> right) => OperationToTensor(left, right, torch.add);
   public static UInt8Tensor Subtract(this Tensor<byte> left, Tensor<byte> right) => OperationToTensor(left, right, torch.subtract);
   public static UInt8Tensor Multiply(this Tensor<byte> left, Tensor<byte> right) => OperationToTensor(left, right, torch.multiply);
   public static UInt8Tensor Divide(this Tensor<byte> left, Tensor<byte> right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));

   public static DoubleTensor Add(this Tensor<byte> left, double right) => OperationToTensor(left, right, torch.add);
   public static DoubleTensor Subtract(this Tensor<byte> left, double right) => OperationToTensor(left, right, torch.subtract);
   public static DoubleTensor Multiply(this Tensor<byte> left, double right) => OperationToTensor(left, right, torch.multiply);
   public static DoubleTensor Divide(this Tensor<byte> left, double right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static DoubleTensor LeftDivide(this Tensor<byte> right, double left) => new(torch.divide(left, right.Storage));

   public static FloatTensor Add(this Tensor<byte> left, float right) => OperationToTensor(left, right, torch.add);
   public static FloatTensor Subtract(this Tensor<byte> left, float right) => OperationToTensor(left, right, torch.subtract);
   public static FloatTensor Multiply(this Tensor<byte> left, float right) => OperationToTensor(left, right, torch.multiply);
   public static FloatTensor Divide(this Tensor<byte> left, float right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static FloatTensor LeftDivide(this Tensor<byte> right, float left) => new(torch.divide(left, right.Storage));

   public static Int64Tensor Add(this Tensor<byte> left, Int64 right) => OperationToTensor(left, right, torch.add);
   public static Int64Tensor Subtract(this Tensor<byte> left, Int64 right) => OperationToTensor(left, right, torch.subtract);
   public static Int64Tensor Multiply(this Tensor<byte> left, Int64 right) => OperationToTensor(left, right, torch.multiply);
   public static Int64Tensor Divide(this Tensor<byte> left, Int64 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int64Tensor LeftDivide(this Tensor<byte> right, Int64 left) => new(torch.divide(left, right.Storage));

   public static Int32Tensor Add(this Tensor<byte> left, Int32 right) => OperationToTensor(left, right, torch.add);
   public static Int32Tensor Subtract(this Tensor<byte> left, Int32 right) => OperationToTensor(left, right, torch.subtract);
   public static Int32Tensor Multiply(this Tensor<byte> left, Int32 right) => OperationToTensor(left, right, torch.multiply);
   public static Int32Tensor Divide(this Tensor<byte> left, Int32 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int32Tensor LeftDivide(this Tensor<byte> right, Int32 left) => new(torch.divide(left, right.Storage));

   public static Int16Tensor Add(this Tensor<byte> left, Int16 right) => OperationToTensor(left, right, torch.add);
   public static Int16Tensor Subtract(this Tensor<byte> left, Int16 right) => OperationToTensor(left, right, torch.subtract);
   public static Int16Tensor Multiply(this Tensor<byte> left, Int16 right) => OperationToTensor(left, right, torch.multiply);
   public static Int16Tensor Divide(this Tensor<byte> left, Int16 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int16Tensor LeftDivide(this Tensor<byte> right, Int16 left) => new(torch.divide(left, right.Storage));

   public static Int8Tensor Add(this Tensor<byte> left, sbyte right) => OperationToTensor(left, right, torch.add);
   public static Int8Tensor Subtract(this Tensor<byte> left, sbyte right) => OperationToTensor(left, right, torch.subtract);
   public static Int8Tensor Multiply(this Tensor<byte> left, sbyte right) => OperationToTensor(left, right, torch.multiply);
   public static Int8Tensor Divide(this Tensor<byte> left, sbyte right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static Int8Tensor LeftDivide(this Tensor<byte> right, sbyte left) => new(torch.divide(left, right.Storage));

   public static UInt8Tensor Add(this Tensor<byte> left, byte right) => OperationToTensor(left, right, torch.add);
   public static UInt8Tensor Subtract(this Tensor<byte> left, byte right) => OperationToTensor(left, right, torch.subtract);
   public static UInt8Tensor Multiply(this Tensor<byte> left, byte right) => OperationToTensor(left, right, torch.multiply);
   public static UInt8Tensor Divide(this Tensor<byte> left, byte right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static UInt8Tensor LeftDivide(this Tensor<byte> right, byte left) => new(torch.divide(left, right.Storage));

   #endregion


   #region Bool

   private static BoolTensor OperationToTensor(this Tensor<bool> left, Tensor<bool> right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right.Storage));

   public static DoubleTensor Mask(this Tensor<double> right, Tensor<bool> left) => new(torch.multiply(left.ToDouble().Storage, right.Storage));
   public static FloatTensor Mask(this Tensor<float> right, Tensor<bool> left) => new(torch.multiply(left.ToFloat().Storage, right.Storage));
   public static Int64Tensor Mask(this Tensor<Int64> right, Tensor<bool> left) => new(torch.multiply(left.ToInt64().Storage, right.Storage));
   public static Int32Tensor Mask(this Tensor<Int32> right, Tensor<bool> left) => new(torch.multiply(left.ToInt32().Storage, right.Storage));
   public static Int16Tensor Mask(this Tensor<Int16> right, Tensor<bool> left) => new(torch.multiply(left.ToInt16().Storage, right.Storage));
   public static Int8Tensor Mask(this Tensor<sbyte> right, Tensor<bool> left) => new(torch.multiply(left.ToInt8().Storage, right.Storage));
   public static UInt8Tensor Mask(this Tensor<byte> right, Tensor<bool> left) => new(torch.multiply(left.ToUInt8().Storage, right.Storage));

   public static BoolTensor And(this Tensor<bool> left, Tensor<bool> right) => OperationToTensor(left, right, torch.logical_and);
   public static BoolTensor Or(this Tensor<bool> left, Tensor<bool> right) => OperationToTensor(left, right, torch.logical_or);
   public static BoolTensor XOr(this Tensor<bool> left, Tensor<bool> right) => OperationToTensor(left, right, torch.logical_xor);
   public static BoolTensor Not(this Tensor<bool> left) => new(torch.logical_not(left.Storage));
   public static Tensor<bool> NotInPlace(this Tensor<bool> left)
   {
      left.Storage.logical_not_();
      return left;
   }
   #endregion
   #endregion
}
