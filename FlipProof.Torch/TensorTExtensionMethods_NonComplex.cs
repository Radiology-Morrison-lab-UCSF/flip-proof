#pragma expandtemplate typeToReplace=DoubleTensor
#pragma expandtemplate Int8Tensor UInt8Tensor Int16Tensor Int32Tensor Int64Tensor FloatTensor BoolTensor
#pragma expandtemplate typeToReplace=double
#pragma expandtemplate Int8 UInt8 Int16 Int32 Int64 float bool


using TorchSharp;
using Int8 = System.SByte;
using UInt8 = System.Byte;

namespace FlipProof.Torch;

public static partial class TensorExtensionMethods
{
   /// <summary>
   /// Returns the index of tensor containing the maximum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMax(this IList<DoubleTensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmax(concatenated.Storage, 0));
   }   
   
   /// <summary>
   /// Returns the index of tensor containing the minimum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMin(this IList<DoubleTensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmin(concatenated.Storage, 0));
   }

   /// <summary>
   /// Replaces values in the mask with another value
   /// </summary>
   /// <param name="toMask">To be altered</param>
   /// <param name="mask">Where to set values</param>
   /// <param name="replaceWith">The value to fill with</param>
   /// <returns></returns>
   public static DoubleTensor MaskedFillInPlace(this DoubleTensor toMask, Tensor<bool> mask, double replaceWith = default) => toMask.MaskedFillInPlace<DoubleTensor, double>(mask, replaceWith);

}

#region TEMPLATE EXPANSION
public static partial class TensorExtensionMethods_Int8Tensor
{
   /// <summary>
   /// Returns the index of tensor containing the maximum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMax(this IList<Int8Tensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmax(concatenated.Storage, 0));
   }   
   
   /// <summary>
   /// Returns the index of tensor containing the minimum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMin(this IList<Int8Tensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmin(concatenated.Storage, 0));
   }

   /// <summary>
   /// Replaces values in the mask with another value
   /// </summary>
   /// <param name="toMask">To be altered</param>
   /// <param name="mask">Where to set values</param>
   /// <param name="replaceWith">The value to fill with</param>
   /// <returns></returns>
   public static Int8Tensor MaskedFillInPlace(this Int8Tensor toMask, Tensor<bool> mask, Int8 replaceWith = default) => toMask.MaskedFillInPlace<Int8Tensor, Int8>(mask, replaceWith);

}

public static partial class TensorExtensionMethods_UInt8Tensor
{
   /// <summary>
   /// Returns the index of tensor containing the maximum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMax(this IList<UInt8Tensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmax(concatenated.Storage, 0));
   }   
   
   /// <summary>
   /// Returns the index of tensor containing the minimum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMin(this IList<UInt8Tensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmin(concatenated.Storage, 0));
   }

   /// <summary>
   /// Replaces values in the mask with another value
   /// </summary>
   /// <param name="toMask">To be altered</param>
   /// <param name="mask">Where to set values</param>
   /// <param name="replaceWith">The value to fill with</param>
   /// <returns></returns>
   public static UInt8Tensor MaskedFillInPlace(this UInt8Tensor toMask, Tensor<bool> mask, UInt8 replaceWith = default) => toMask.MaskedFillInPlace<UInt8Tensor, UInt8>(mask, replaceWith);

}

public static partial class TensorExtensionMethods_Int16Tensor
{
   /// <summary>
   /// Returns the index of tensor containing the maximum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMax(this IList<Int16Tensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmax(concatenated.Storage, 0));
   }   
   
   /// <summary>
   /// Returns the index of tensor containing the minimum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMin(this IList<Int16Tensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmin(concatenated.Storage, 0));
   }

   /// <summary>
   /// Replaces values in the mask with another value
   /// </summary>
   /// <param name="toMask">To be altered</param>
   /// <param name="mask">Where to set values</param>
   /// <param name="replaceWith">The value to fill with</param>
   /// <returns></returns>
   public static Int16Tensor MaskedFillInPlace(this Int16Tensor toMask, Tensor<bool> mask, Int16 replaceWith = default) => toMask.MaskedFillInPlace<Int16Tensor, Int16>(mask, replaceWith);

}

public static partial class TensorExtensionMethods_Int32Tensor
{
   /// <summary>
   /// Returns the index of tensor containing the maximum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMax(this IList<Int32Tensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmax(concatenated.Storage, 0));
   }   
   
   /// <summary>
   /// Returns the index of tensor containing the minimum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMin(this IList<Int32Tensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmin(concatenated.Storage, 0));
   }

   /// <summary>
   /// Replaces values in the mask with another value
   /// </summary>
   /// <param name="toMask">To be altered</param>
   /// <param name="mask">Where to set values</param>
   /// <param name="replaceWith">The value to fill with</param>
   /// <returns></returns>
   public static Int32Tensor MaskedFillInPlace(this Int32Tensor toMask, Tensor<bool> mask, Int32 replaceWith = default) => toMask.MaskedFillInPlace<Int32Tensor, Int32>(mask, replaceWith);

}

public static partial class TensorExtensionMethods_Int64Tensor
{
   /// <summary>
   /// Returns the index of tensor containing the maximum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMax(this IList<Int64Tensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmax(concatenated.Storage, 0));
   }   
   
   /// <summary>
   /// Returns the index of tensor containing the minimum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMin(this IList<Int64Tensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmin(concatenated.Storage, 0));
   }

   /// <summary>
   /// Replaces values in the mask with another value
   /// </summary>
   /// <param name="toMask">To be altered</param>
   /// <param name="mask">Where to set values</param>
   /// <param name="replaceWith">The value to fill with</param>
   /// <returns></returns>
   public static Int64Tensor MaskedFillInPlace(this Int64Tensor toMask, Tensor<bool> mask, Int64 replaceWith = default) => toMask.MaskedFillInPlace<Int64Tensor, Int64>(mask, replaceWith);

}

public static partial class TensorExtensionMethods_FloatTensor
{
   /// <summary>
   /// Returns the index of tensor containing the maximum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMax(this IList<FloatTensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmax(concatenated.Storage, 0));
   }   
   
   /// <summary>
   /// Returns the index of tensor containing the minimum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMin(this IList<FloatTensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmin(concatenated.Storage, 0));
   }

   /// <summary>
   /// Replaces values in the mask with another value
   /// </summary>
   /// <param name="toMask">To be altered</param>
   /// <param name="mask">Where to set values</param>
   /// <param name="replaceWith">The value to fill with</param>
   /// <returns></returns>
   public static FloatTensor MaskedFillInPlace(this FloatTensor toMask, Tensor<bool> mask, float replaceWith = default) => toMask.MaskedFillInPlace<FloatTensor, float>(mask, replaceWith);

}

public static partial class TensorExtensionMethods_BoolTensor
{
   /// <summary>
   /// Returns the index of tensor containing the maximum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMax(this IList<BoolTensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmax(concatenated.Storage, 0));
   }   
   
   /// <summary>
   /// Returns the index of tensor containing the minimum value for each entry
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static Int64Tensor ArgMin(this IList<BoolTensor> others)
   {
      for (int i = 0;i < others.Count;i++)
      {
         if(!others[i].ShapesEqual(others[0]))
         {
            throw new ArgumentException("Tensors must be same size and shape");
         }
      }
      using var concatenated = Stack(others, 0);
      return new Int64Tensor(torch.argmin(concatenated.Storage, 0));
   }

   /// <summary>
   /// Replaces values in the mask with another value
   /// </summary>
   /// <param name="toMask">To be altered</param>
   /// <param name="mask">Where to set values</param>
   /// <param name="replaceWith">The value to fill with</param>
   /// <returns></returns>
   public static BoolTensor MaskedFillInPlace(this BoolTensor toMask, Tensor<bool> mask, bool replaceWith = default) => toMask.MaskedFillInPlace<BoolTensor, bool>(mask, replaceWith);

}

#endregion TEMPLATE EXPANSION
