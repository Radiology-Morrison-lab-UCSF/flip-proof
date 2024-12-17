#pragma expandtemplate typeToReplace=DoubleTensor
#pragma expandtemplate Int8Tensor UInt8Tensor Int16Tensor Int32Tensor Int64Tensor FloatTensor BoolTensor Complex32Tensor ComplexTensor
#pragma expandtemplate typeToReplace=double
#pragma expandtemplate Int8 UInt8 Int16 Int32 Int64 float bool Complex Complex32


using static TorchSharp.torch;
using TorchSharp.Modules;
using TorchSharp;
using Int8 = System.SByte;
using UInt8 = System.Byte;
using System.Numerics;

namespace FlipProof.Torch;

public static partial class TensorExtensionMethods
{

   public static DoubleTensor DeepClone(this DoubleTensor toClone) => new(toClone.Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static DoubleTensor RowStack(this DoubleTensor me, DoubleTensor other) => new(torch.row_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static DoubleTensor ColumnStack(this DoubleTensor me, DoubleTensor other) =>new(torch.column_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static DoubleTensor Concat(this DoubleTensor me, int dim, params DoubleTensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(me.Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static DoubleTensor Concat(this DoubleTensor[] toStack, int dim) =>new (torch.concat(toStack.Select(a=>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static DoubleTensor Stack(this DoubleTensor me, int dim, params DoubleTensor[] toStack) => Stack(toStack.Select(a => a).Prepend(me).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static DoubleTensor Stack(this IList<DoubleTensor> toStack, int dim) =>new (torch.stack(toStack.Select(a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public static DoubleTensor Blank(this DoubleTensor me) => new (torch.zeros_like(me.Storage));
}

#region TEMPLATE EXPANSION
public static partial class TensorExtensionMethods_Int8Tensor
{

   public static Int8Tensor DeepClone(this Int8Tensor toClone) => new(toClone.Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static Int8Tensor RowStack(this Int8Tensor me, Int8Tensor other) => new(torch.row_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static Int8Tensor ColumnStack(this Int8Tensor me, Int8Tensor other) =>new(torch.column_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int8Tensor Concat(this Int8Tensor me, int dim, params Int8Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(me.Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int8Tensor Concat(this Int8Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(a=>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int8Tensor Stack(this Int8Tensor me, int dim, params Int8Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(me).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int8Tensor Stack(this IList<Int8Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public static Int8Tensor Blank(this Int8Tensor me) => new (torch.zeros_like(me.Storage));
}

public static partial class TensorExtensionMethods_UInt8Tensor
{

   public static UInt8Tensor DeepClone(this UInt8Tensor toClone) => new(toClone.Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static UInt8Tensor RowStack(this UInt8Tensor me, UInt8Tensor other) => new(torch.row_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static UInt8Tensor ColumnStack(this UInt8Tensor me, UInt8Tensor other) =>new(torch.column_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static UInt8Tensor Concat(this UInt8Tensor me, int dim, params UInt8Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(me.Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static UInt8Tensor Concat(this UInt8Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(a=>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static UInt8Tensor Stack(this UInt8Tensor me, int dim, params UInt8Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(me).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static UInt8Tensor Stack(this IList<UInt8Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public static UInt8Tensor Blank(this UInt8Tensor me) => new (torch.zeros_like(me.Storage));
}

public static partial class TensorExtensionMethods_Int16Tensor
{

   public static Int16Tensor DeepClone(this Int16Tensor toClone) => new(toClone.Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static Int16Tensor RowStack(this Int16Tensor me, Int16Tensor other) => new(torch.row_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static Int16Tensor ColumnStack(this Int16Tensor me, Int16Tensor other) =>new(torch.column_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int16Tensor Concat(this Int16Tensor me, int dim, params Int16Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(me.Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int16Tensor Concat(this Int16Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(a=>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int16Tensor Stack(this Int16Tensor me, int dim, params Int16Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(me).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int16Tensor Stack(this IList<Int16Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public static Int16Tensor Blank(this Int16Tensor me) => new (torch.zeros_like(me.Storage));
}

public static partial class TensorExtensionMethods_Int32Tensor
{

   public static Int32Tensor DeepClone(this Int32Tensor toClone) => new(toClone.Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static Int32Tensor RowStack(this Int32Tensor me, Int32Tensor other) => new(torch.row_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static Int32Tensor ColumnStack(this Int32Tensor me, Int32Tensor other) =>new(torch.column_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int32Tensor Concat(this Int32Tensor me, int dim, params Int32Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(me.Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int32Tensor Concat(this Int32Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(a=>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int32Tensor Stack(this Int32Tensor me, int dim, params Int32Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(me).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int32Tensor Stack(this IList<Int32Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public static Int32Tensor Blank(this Int32Tensor me) => new (torch.zeros_like(me.Storage));
}

public static partial class TensorExtensionMethods_Int64Tensor
{

   public static Int64Tensor DeepClone(this Int64Tensor toClone) => new(toClone.Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static Int64Tensor RowStack(this Int64Tensor me, Int64Tensor other) => new(torch.row_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static Int64Tensor ColumnStack(this Int64Tensor me, Int64Tensor other) =>new(torch.column_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int64Tensor Concat(this Int64Tensor me, int dim, params Int64Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(me.Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int64Tensor Concat(this Int64Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(a=>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int64Tensor Stack(this Int64Tensor me, int dim, params Int64Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(me).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int64Tensor Stack(this IList<Int64Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public static Int64Tensor Blank(this Int64Tensor me) => new (torch.zeros_like(me.Storage));
}

public static partial class TensorExtensionMethods_FloatTensor
{

   public static FloatTensor DeepClone(this FloatTensor toClone) => new(toClone.Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static FloatTensor RowStack(this FloatTensor me, FloatTensor other) => new(torch.row_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static FloatTensor ColumnStack(this FloatTensor me, FloatTensor other) =>new(torch.column_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static FloatTensor Concat(this FloatTensor me, int dim, params FloatTensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(me.Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static FloatTensor Concat(this FloatTensor[] toStack, int dim) =>new (torch.concat(toStack.Select(a=>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static FloatTensor Stack(this FloatTensor me, int dim, params FloatTensor[] toStack) => Stack(toStack.Select(a => a).Prepend(me).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static FloatTensor Stack(this IList<FloatTensor> toStack, int dim) =>new (torch.stack(toStack.Select(a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public static FloatTensor Blank(this FloatTensor me) => new (torch.zeros_like(me.Storage));
}

public static partial class TensorExtensionMethods_BoolTensor
{

   public static BoolTensor DeepClone(this BoolTensor toClone) => new(toClone.Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static BoolTensor RowStack(this BoolTensor me, BoolTensor other) => new(torch.row_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static BoolTensor ColumnStack(this BoolTensor me, BoolTensor other) =>new(torch.column_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static BoolTensor Concat(this BoolTensor me, int dim, params BoolTensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(me.Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static BoolTensor Concat(this BoolTensor[] toStack, int dim) =>new (torch.concat(toStack.Select(a=>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static BoolTensor Stack(this BoolTensor me, int dim, params BoolTensor[] toStack) => Stack(toStack.Select(a => a).Prepend(me).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static BoolTensor Stack(this IList<BoolTensor> toStack, int dim) =>new (torch.stack(toStack.Select(a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public static BoolTensor Blank(this BoolTensor me) => new (torch.zeros_like(me.Storage));
}

public static partial class TensorExtensionMethods_Complex32Tensor
{

   public static Complex32Tensor DeepClone(this Complex32Tensor toClone) => new(toClone.Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static Complex32Tensor RowStack(this Complex32Tensor me, Complex32Tensor other) => new(torch.row_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static Complex32Tensor ColumnStack(this Complex32Tensor me, Complex32Tensor other) =>new(torch.column_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Complex32Tensor Concat(this Complex32Tensor me, int dim, params Complex32Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(me.Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Complex32Tensor Concat(this Complex32Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(a=>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Complex32Tensor Stack(this Complex32Tensor me, int dim, params Complex32Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(me).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Complex32Tensor Stack(this IList<Complex32Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public static Complex32Tensor Blank(this Complex32Tensor me) => new (torch.zeros_like(me.Storage));
}

public static partial class TensorExtensionMethods_ComplexTensor
{

   public static ComplexTensor DeepClone(this ComplexTensor toClone) => new(toClone.Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static ComplexTensor RowStack(this ComplexTensor me, ComplexTensor other) => new(torch.row_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public static ComplexTensor ColumnStack(this ComplexTensor me, ComplexTensor other) =>new(torch.column_stack([me.Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static ComplexTensor Concat(this ComplexTensor me, int dim, params ComplexTensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(me.Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static ComplexTensor Concat(this ComplexTensor[] toStack, int dim) =>new (torch.concat(toStack.Select(a=>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static ComplexTensor Stack(this ComplexTensor me, int dim, params ComplexTensor[] toStack) => Stack(toStack.Select(a => a).Prepend(me).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static ComplexTensor Stack(this IList<ComplexTensor> toStack, int dim) =>new (torch.stack(toStack.Select(a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public static ComplexTensor Blank(this ComplexTensor me) => new (torch.zeros_like(me.Storage));
}

#endregion TEMPLATE EXPANSION