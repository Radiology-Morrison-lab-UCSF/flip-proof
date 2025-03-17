using FlipProof.Base;
using System.Numerics;
using TorchSharp;
using static TorchSharp.torch;

namespace FlipProof.Torch;

public static partial class TensorExtensionMethods
{
   /// <summary>
   /// Deep clones a Tensor
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="tensor"></param>
   /// <returns></returns>
   public static Tensor<T> DeepClone<T>(this Tensor<T> tensor) where T:struct
   {
      return tensor.CreateFromTensorNew(tensor.Storage.clone());
   }

   /// <summary>
   /// Crops the tensor to the specified ranges
   /// </summary>
   /// <param name="tensor">The source tensor</param>
   /// <param name="dimensionSizes">Sizes of each dimension</param>
   [CLSCompliant(false)]
   public static Tensor GetSubTensor(this Tensor tensor, params long[] dimensionSizes)
   {
      return tensor[dimensionSizes.Select(a => TensorIndex.Slice(0, a)).ToArray()];
   }
   /// <summary>
   /// Gets elements indicated by the mask and range for a 2D tensor
   /// </summary>
   /// <param name="tensor">The source tensor</param>
   /// <param name="maskDim0">True/False to indicate which indices to get for dim 0</param>
   /// <param name="indicesDim1">Range of indices to get for dim 1</param>
   /// <exception cref="NotSupportedException">Tensor is larger than supported</exception>
   [CLSCompliant(false)]
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
   /// Replaces all instances of a value with another, in place, returning the original object
   /// </summary>
   /// <param name="replace">Must not be NaN</param>
   /// <param name="with"></param>
   /// <returns></returns>
   public static TTensor ReplaceInPlace<T,TTensor>(this TTensor tensor, T replace, T with)
      where T:struct,INumber<T>
      where TTensor:Tensor<T>
   {
      if(T.IsNaN(replace))
      {
         throw new NotSupportedException($"Cannot replace NaNs with this method. See {nameof(FloatTensor.ReplaceNaN)}");
      }
      using Tensor indices = tensor.Storage.equal(tensor.ScalarToTensor(replace));
      using Tensor val = tensor.ScalarToTensor(with);
      tensor.Storage[indices] = val;
      return tensor;
   }


   /// <summary>
   /// Sets elements indicated by the mask and range for a 2D tensor
   /// </summary>
   /// <param name="tensor">The destination tensor</param>
   /// <param name="maskDim0">True/False to indicate which indices to get for dim 0</param>
   /// <param name="indicesDim1">Range of indices to get for dim 1</param>
   /// <param name="value">The new values</param>
   /// <exception cref="NotSupportedException">Tensor is larger than supported</exception>
   [CLSCompliant(false)]
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

   [CLSCompliant(false)]
   public static Int8Tensor Add(this Tensor<sbyte> left, Tensor<sbyte> right) => OperationToTensor(left, right, torch.add);
   [CLSCompliant(false)]
   public static Int8Tensor Subtract(this Tensor<sbyte> left, Tensor<sbyte> right) => OperationToTensor(left, right, torch.subtract);
   [CLSCompliant(false)]
   public static Int8Tensor Multiply(this Tensor<sbyte> left, Tensor<sbyte> right) => OperationToTensor(left, right, torch.multiply);
   [CLSCompliant(false)]
   public static Int8Tensor Divide(this Tensor<sbyte> left, Tensor<sbyte> right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));

   [CLSCompliant(false)]
   public static DoubleTensor Add(this Tensor<sbyte> left, double right) => OperationToTensor(left, right, torch.add);
   [CLSCompliant(false)]
   public static DoubleTensor Subtract(this Tensor<sbyte> left, double right) => OperationToTensor(left, right, torch.subtract);
   [CLSCompliant(false)]
   public static DoubleTensor Multiply(this Tensor<sbyte> left, double right) => OperationToTensor(left, right, torch.multiply);
   [CLSCompliant(false)]
   public static DoubleTensor Divide(this Tensor<sbyte> left, double right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   [CLSCompliant(false)]
   public static DoubleTensor LeftDivide(this Tensor<sbyte> right, double left) => new(torch.divide(left, right.Storage));

   [CLSCompliant(false)]
   public static FloatTensor Add(this Tensor<sbyte> left, float right) => OperationToTensor(left, right, torch.add);
   [CLSCompliant(false)]
   public static FloatTensor Subtract(this Tensor<sbyte> left, float right) => OperationToTensor(left, right, torch.subtract);
   [CLSCompliant(false)]
   public static FloatTensor Multiply(this Tensor<sbyte> left, float right) => OperationToTensor(left, right, torch.multiply);
   [CLSCompliant(false)]
   public static FloatTensor Divide(this Tensor<sbyte> left, float right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   [CLSCompliant(false)]
   public static FloatTensor LeftDivide(this Tensor<sbyte> right, float left) => new(torch.divide(left, right.Storage));

   [CLSCompliant(false)]
   public static Int64Tensor Add(this Tensor<sbyte> left, Int64 right) => OperationToTensor(left, right, torch.add);
   [CLSCompliant(false)]
   public static Int64Tensor Subtract(this Tensor<sbyte> left, Int64 right) => OperationToTensor(left, right, torch.subtract);
   [CLSCompliant(false)]
   public static Int64Tensor Multiply(this Tensor<sbyte> left, Int64 right) => OperationToTensor(left, right, torch.multiply);
   [CLSCompliant(false)]
   public static Int64Tensor Divide(this Tensor<sbyte> left, Int64 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   [CLSCompliant(false)]
   public static Int64Tensor LeftDivide(this Tensor<sbyte> right, Int64 left) => new(torch.divide(left, right.Storage));

   [CLSCompliant(false)]
   public static Int32Tensor Add(this Tensor<sbyte> left, Int32 right) => OperationToTensor(left, right, torch.add);
   [CLSCompliant(false)]
   public static Int32Tensor Subtract(this Tensor<sbyte> left, Int32 right) => OperationToTensor(left, right, torch.subtract);
   [CLSCompliant(false)]
   public static Int32Tensor Multiply(this Tensor<sbyte> left, Int32 right) => OperationToTensor(left, right, torch.multiply);
   [CLSCompliant(false)]
   public static Int32Tensor Divide(this Tensor<sbyte> left, Int32 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   [CLSCompliant(false)]
   public static Int32Tensor LeftDivide(this Tensor<sbyte> right, Int32 left) => new(torch.divide(left, right.Storage));

   [CLSCompliant(false)]
   public static Int16Tensor Add(this Tensor<sbyte> left, Int16 right) => OperationToTensor(left, right, torch.add);
   [CLSCompliant(false)]
   public static Int16Tensor Subtract(this Tensor<sbyte> left, Int16 right) => OperationToTensor(left, right, torch.subtract);
   [CLSCompliant(false)]
   public static Int16Tensor Multiply(this Tensor<sbyte> left, Int16 right) => OperationToTensor(left, right, torch.multiply);
   [CLSCompliant(false)]
   public static Int16Tensor Divide(this Tensor<sbyte> left, Int16 right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   [CLSCompliant(false)]
   public static Int16Tensor LeftDivide(this Tensor<sbyte> right, Int16 left) => new(torch.divide(left, right.Storage));

   [CLSCompliant(false)]
   public static Int8Tensor Add(this Tensor<sbyte> left, sbyte right) => OperationToTensor(left, right, torch.add);
   [CLSCompliant(false)]
   public static Int8Tensor Subtract(this Tensor<sbyte> left, sbyte right) => OperationToTensor(left, right, torch.subtract);
   [CLSCompliant(false)]
   public static Int8Tensor Multiply(this Tensor<sbyte> left, sbyte right) => OperationToTensor(left, right, torch.multiply);
   [CLSCompliant(false)]
   public static Int8Tensor Divide(this Tensor<sbyte> left, sbyte right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   [CLSCompliant(false)]
   public static Int8Tensor LeftDivide(this Tensor<sbyte> right, sbyte left) => new(torch.divide(left, right.Storage));

   [CLSCompliant(false)]
   public static Int8Tensor Add(this Tensor<sbyte> left, byte right) => OperationToTensor(left, right, torch.add);
   [CLSCompliant(false)]
   public static Int8Tensor Subtract(this Tensor<sbyte> left, byte right) => OperationToTensor(left, right, torch.subtract);
   [CLSCompliant(false)]
   public static Int8Tensor Multiply(this Tensor<sbyte> left, byte right) => OperationToTensor(left, right, torch.multiply);
   [CLSCompliant(false)]
   public static Int8Tensor Divide(this Tensor<sbyte> left, byte right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   [CLSCompliant(false)]
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

   [CLSCompliant(false)]
   public static Int8Tensor Add(this Tensor<byte> left, sbyte right) => OperationToTensor(left, right, torch.add);
   [CLSCompliant(false)]
   public static Int8Tensor Subtract(this Tensor<byte> left, sbyte right) => OperationToTensor(left, right, torch.subtract);
   [CLSCompliant(false)]
   public static Int8Tensor Multiply(this Tensor<byte> left, sbyte right) => OperationToTensor(left, right, torch.multiply);
   [CLSCompliant(false)]
   public static Int8Tensor Divide(this Tensor<byte> left, sbyte right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   [CLSCompliant(false)]
   public static Int8Tensor LeftDivide(this Tensor<byte> right, sbyte left) => new(torch.divide(left, right.Storage));

   public static UInt8Tensor Add(this Tensor<byte> left, byte right) => OperationToTensor(left, right, torch.add);
   public static UInt8Tensor Subtract(this Tensor<byte> left, byte right) => OperationToTensor(left, right, torch.subtract);
   public static UInt8Tensor Multiply(this Tensor<byte> left, byte right) => OperationToTensor(left, right, torch.multiply);
   public static UInt8Tensor Divide(this Tensor<byte> left, byte right) => OperationToTensor(left, right, (a, b) => torch.divide(a, b));
   public static UInt8Tensor LeftDivide(this Tensor<byte> right, byte left) => new(torch.divide(left, right.Storage));

   #endregion


   #region Bool

   /// <summary>
   /// Returns a box that tightly includes all true values. Dimensions above the 4th are ignored
   /// </summary>
   /// <param name="mask4d"></param>
   /// <returns></returns>
   public static Box4D<long> GetMaskBounds4D(this Tensor<bool> mask4d)
   {
      using var first = mask4d.Storage.nonzero().min(dim: 0).values;
      using var last = mask4d.Storage.nonzero().max(dim: 0).values;

      long from0 = first.ReadCpuInt64(0);
      long from1 = first.ReadCpuInt64(1);
      long from2 = first.ReadCpuInt64(2);
      long from3 = first.ReadCpuInt64(3);

      long to0 = last.ReadCpuInt64(0) + 1;
      long to1 = last.ReadCpuInt64(1) + 1;
      long to2 = last.ReadCpuInt64(2) + 1;
      long to3 = last.ReadCpuInt64(3) + 1;

      return new Box4D<long>(new(from0, from1, from2, from3), new(to0 - from0, to1 - from1, to2 - from2, to3 - from3));
   }


   private static BoolTensor OperationToTensor(this Tensor<bool> left, Tensor<bool> right, Func<Tensor, Tensor, Tensor> func) => new(func(left.Storage, right.Storage));

   /// <summary>
   /// Replaces values within the mask with another value
   /// </summary>
   /// <typeparam name="TTensor"></typeparam>
   /// <typeparam name="TValue"></typeparam>
   /// <param name="data"></param>
   /// <param name="mask"></param>
   /// <param name="maskTo"></param>
   /// <returns></returns>
   [CLSCompliant(false)]
   public static TTensor MaskedFillInPlace<TTensor,TValue>(this TTensor data, Tensor<bool> mask, Scalar maskTo)
      where TTensor : Tensor<TValue>
      where TValue : struct
   {
      data.Storage.masked_fill_(mask.Storage, maskTo);
      return data;
   }
   public static DoubleTensor Masked(this Tensor<double> data, Tensor<bool> mask) => new(torch.multiply(mask.ToDouble().Storage, data.Storage));
   public static FloatTensor Masked(this Tensor<float> data, Tensor<bool> mask) => new(torch.multiply(mask.ToFloat().Storage, data.Storage));
   public static Int64Tensor Masked(this Tensor<Int64> data, Tensor<bool> mask) => new(torch.multiply(mask.ToInt64().Storage, data.Storage));
   public static Int32Tensor Masked(this Tensor<Int32> data, Tensor<bool> mask) => new(torch.multiply(mask.ToInt32().Storage, data.Storage));
   public static Int16Tensor Masked(this Tensor<Int16> data, Tensor<bool> mask) => new(torch.multiply(mask.ToInt16().Storage, data.Storage));
   [CLSCompliant(false)]
   public static Int8Tensor Masked(this Tensor<sbyte> data, Tensor<bool> mask) => new(torch.multiply(mask.ToInt8().Storage, data.Storage));
   public static UInt8Tensor Masked(this Tensor<byte> data, Tensor<bool> mask) => new(torch.multiply(mask.ToUInt8().Storage, data.Storage));
   public static BoolTensor Masked(this Tensor<bool> data, Tensor<bool> mask) => And(mask,data);

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
