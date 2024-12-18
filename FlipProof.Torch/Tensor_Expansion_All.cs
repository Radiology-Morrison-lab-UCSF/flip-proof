#pragma expandtemplate typeToReplace=DoubleTensor
#pragma expandtemplate Int8Tensor UInt8Tensor Int16Tensor Int32Tensor Int64Tensor FloatTensor BoolTensor Complex32Tensor ComplexTensor
#pragma expandtemplate typeToReplace=double
#pragma expandtemplate Int8 UInt8 Int16 Int32 Int64 float bool Complex32 Complex
#pragma expandtemplate typeToReplace=Float64
#pragma expandtemplate Int8 Byte Int16 Int32 Int64 Float32 Bool ComplexFloat32 ComplexFloat64


using static TorchSharp.torch;
using TorchSharp.Modules;
using TorchSharp;
using Int8 = System.SByte;
using UInt8 = System.Byte;
using System.Numerics;
using static Tensorboard.TensorShapeProto.Types;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public partial class DoubleTensor
{
   /// <summary>
   /// Creates a new <see cref="DoubleTensor"/> containing default values
   /// </summary>
   /// <param name="dimSizes">Dimension sizes</param>
   [SetsRequiredMembers]
   public DoubleTensor(params long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Float64))
   {
   }

   /// <summary>
   /// Creates a new <see cref="DoubleTensor"/>
   /// </summary>
   /// <param name="t">Data to wrap</param>
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public DoubleTensor(Tensor t) : base(t) { }

   /// <summary>
   /// The pytorch scalar type contained within
   /// </summary>
   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Float64;


   [CLSCompliant(false)]
   protected override DoubleTensor CreateFromTensorSub(Tensor t) => new(t);

   /// <summary>
   /// Copies this object and its storage
   /// </summary>
   /// <returns></returns>
   public DoubleTensor DeepClone() => new(Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public DoubleTensor RowStack(DoubleTensor other) => new(torch.row_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public DoubleTensor ColumnStack( DoubleTensor other) =>new(torch.column_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public DoubleTensor Concat(int dim, params DoubleTensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static DoubleTensor Concat(DoubleTensor[] toStack, int dim) =>new (torch.concat(toStack.Select(static a =>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public DoubleTensor Stack(int dim, params DoubleTensor[] toStack) => Stack(toStack.Select(a => a).Prepend(this).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static DoubleTensor Stack(IList<DoubleTensor> toStack, int dim) =>new (torch.stack(toStack.Select(static a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public DoubleTensor Blank() => new (torch.zeros_like(Storage));


   /// <summary>
   /// Pads the tensor so it is centered in the result
   /// </summary>
   /// <param name="padBy">Amount to pad left and right for each dimension. Provide twice as many values as their are dimensions ordered LDim0, RDim0, LDim1, RDim1, etc</param>
   /// <remarks>This does not match behaviour of torch.pad, which has arguments in a sort of reverse order</remarks>
   /// <returns>A new, padded, tensor</returns>
   public DoubleTensor PadSurround(params long[] padBy)
   {
      if (padBy.Length != NDims * 2)
      {
         throw new ArgumentException($"Expected {NDims * 2} values");
      }

      // Torch orders things in reverse DimN L, DimN R, Dim N-1 L, Dim N-1 R, etc
      long[] padTorchOrder = padBy.Reverse().ToArray();

      for (int i = 0; i < padTorchOrder.Length;i+=2)
      {
         (padTorchOrder[i], padTorchOrder[i + 1]) = (padTorchOrder[i + 1], padTorchOrder[i]);
      }

      return CreateFromTensor(torch.nn.functional.pad(Storage, padTorchOrder, PaddingModes.Constant));
   }
}

#region TEMPLATE EXPANSION
public partial class Int8Tensor
{
   /// <summary>
   /// Creates a new <see cref="Int8Tensor"/> containing default values
   /// </summary>
   /// <param name="dimSizes">Dimension sizes</param>
   [SetsRequiredMembers]
   public Int8Tensor(params long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Int8))
   {
   }

   /// <summary>
   /// Creates a new <see cref="Int8Tensor"/>
   /// </summary>
   /// <param name="t">Data to wrap</param>
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public Int8Tensor(Tensor t) : base(t) { }

   /// <summary>
   /// The pytorch scalar type contained within
   /// </summary>
   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Int8;


   [CLSCompliant(false)]
   protected override Int8Tensor CreateFromTensorSub(Tensor t) => new(t);

   /// <summary>
   /// Copies this object and its storage
   /// </summary>
   /// <returns></returns>
   public Int8Tensor DeepClone() => new(Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public Int8Tensor RowStack(Int8Tensor other) => new(torch.row_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public Int8Tensor ColumnStack( Int8Tensor other) =>new(torch.column_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public Int8Tensor Concat(int dim, params Int8Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int8Tensor Concat(Int8Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(static a =>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public Int8Tensor Stack(int dim, params Int8Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(this).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int8Tensor Stack(IList<Int8Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(static a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public Int8Tensor Blank() => new (torch.zeros_like(Storage));


   /// <summary>
   /// Pads the tensor so it is centered in the result
   /// </summary>
   /// <param name="padBy">Amount to pad left and right for each dimension. Provide twice as many values as their are dimensions ordered LDim0, RDim0, LDim1, RDim1, etc</param>
   /// <remarks>This does not match behaviour of torch.pad, which has arguments in a sort of reverse order</remarks>
   /// <returns>A new, padded, tensor</returns>
   public Int8Tensor PadSurround(params long[] padBy)
   {
      if (padBy.Length != NDims * 2)
      {
         throw new ArgumentException($"Expected {NDims * 2} values");
      }

      // Torch orders things in reverse DimN L, DimN R, Dim N-1 L, Dim N-1 R, etc
      long[] padTorchOrder = padBy.Reverse().ToArray();

      for (int i = 0; i < padTorchOrder.Length;i+=2)
      {
         (padTorchOrder[i], padTorchOrder[i + 1]) = (padTorchOrder[i + 1], padTorchOrder[i]);
      }

      return CreateFromTensor(torch.nn.functional.pad(Storage, padTorchOrder, PaddingModes.Constant));
   }
}

public partial class UInt8Tensor
{
   /// <summary>
   /// Creates a new <see cref="UInt8Tensor"/> containing default values
   /// </summary>
   /// <param name="dimSizes">Dimension sizes</param>
   [SetsRequiredMembers]
   public UInt8Tensor(params long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Byte))
   {
   }

   /// <summary>
   /// Creates a new <see cref="UInt8Tensor"/>
   /// </summary>
   /// <param name="t">Data to wrap</param>
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public UInt8Tensor(Tensor t) : base(t) { }

   /// <summary>
   /// The pytorch scalar type contained within
   /// </summary>
   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Byte;


   [CLSCompliant(false)]
   protected override UInt8Tensor CreateFromTensorSub(Tensor t) => new(t);

   /// <summary>
   /// Copies this object and its storage
   /// </summary>
   /// <returns></returns>
   public UInt8Tensor DeepClone() => new(Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public UInt8Tensor RowStack(UInt8Tensor other) => new(torch.row_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public UInt8Tensor ColumnStack( UInt8Tensor other) =>new(torch.column_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public UInt8Tensor Concat(int dim, params UInt8Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static UInt8Tensor Concat(UInt8Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(static a =>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public UInt8Tensor Stack(int dim, params UInt8Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(this).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static UInt8Tensor Stack(IList<UInt8Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(static a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public UInt8Tensor Blank() => new (torch.zeros_like(Storage));


   /// <summary>
   /// Pads the tensor so it is centered in the result
   /// </summary>
   /// <param name="padBy">Amount to pad left and right for each dimension. Provide twice as many values as their are dimensions ordered LDim0, RDim0, LDim1, RDim1, etc</param>
   /// <remarks>This does not match behaviour of torch.pad, which has arguments in a sort of reverse order</remarks>
   /// <returns>A new, padded, tensor</returns>
   public UInt8Tensor PadSurround(params long[] padBy)
   {
      if (padBy.Length != NDims * 2)
      {
         throw new ArgumentException($"Expected {NDims * 2} values");
      }

      // Torch orders things in reverse DimN L, DimN R, Dim N-1 L, Dim N-1 R, etc
      long[] padTorchOrder = padBy.Reverse().ToArray();

      for (int i = 0; i < padTorchOrder.Length;i+=2)
      {
         (padTorchOrder[i], padTorchOrder[i + 1]) = (padTorchOrder[i + 1], padTorchOrder[i]);
      }

      return CreateFromTensor(torch.nn.functional.pad(Storage, padTorchOrder, PaddingModes.Constant));
   }
}

public partial class Int16Tensor
{
   /// <summary>
   /// Creates a new <see cref="Int16Tensor"/> containing default values
   /// </summary>
   /// <param name="dimSizes">Dimension sizes</param>
   [SetsRequiredMembers]
   public Int16Tensor(params long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Int16))
   {
   }

   /// <summary>
   /// Creates a new <see cref="Int16Tensor"/>
   /// </summary>
   /// <param name="t">Data to wrap</param>
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public Int16Tensor(Tensor t) : base(t) { }

   /// <summary>
   /// The pytorch scalar type contained within
   /// </summary>
   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Int16;


   [CLSCompliant(false)]
   protected override Int16Tensor CreateFromTensorSub(Tensor t) => new(t);

   /// <summary>
   /// Copies this object and its storage
   /// </summary>
   /// <returns></returns>
   public Int16Tensor DeepClone() => new(Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public Int16Tensor RowStack(Int16Tensor other) => new(torch.row_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public Int16Tensor ColumnStack( Int16Tensor other) =>new(torch.column_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public Int16Tensor Concat(int dim, params Int16Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int16Tensor Concat(Int16Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(static a =>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public Int16Tensor Stack(int dim, params Int16Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(this).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int16Tensor Stack(IList<Int16Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(static a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public Int16Tensor Blank() => new (torch.zeros_like(Storage));


   /// <summary>
   /// Pads the tensor so it is centered in the result
   /// </summary>
   /// <param name="padBy">Amount to pad left and right for each dimension. Provide twice as many values as their are dimensions ordered LDim0, RDim0, LDim1, RDim1, etc</param>
   /// <remarks>This does not match behaviour of torch.pad, which has arguments in a sort of reverse order</remarks>
   /// <returns>A new, padded, tensor</returns>
   public Int16Tensor PadSurround(params long[] padBy)
   {
      if (padBy.Length != NDims * 2)
      {
         throw new ArgumentException($"Expected {NDims * 2} values");
      }

      // Torch orders things in reverse DimN L, DimN R, Dim N-1 L, Dim N-1 R, etc
      long[] padTorchOrder = padBy.Reverse().ToArray();

      for (int i = 0; i < padTorchOrder.Length;i+=2)
      {
         (padTorchOrder[i], padTorchOrder[i + 1]) = (padTorchOrder[i + 1], padTorchOrder[i]);
      }

      return CreateFromTensor(torch.nn.functional.pad(Storage, padTorchOrder, PaddingModes.Constant));
   }
}

public partial class Int32Tensor
{
   /// <summary>
   /// Creates a new <see cref="Int32Tensor"/> containing default values
   /// </summary>
   /// <param name="dimSizes">Dimension sizes</param>
   [SetsRequiredMembers]
   public Int32Tensor(params long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Int32))
   {
   }

   /// <summary>
   /// Creates a new <see cref="Int32Tensor"/>
   /// </summary>
   /// <param name="t">Data to wrap</param>
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public Int32Tensor(Tensor t) : base(t) { }

   /// <summary>
   /// The pytorch scalar type contained within
   /// </summary>
   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Int32;


   [CLSCompliant(false)]
   protected override Int32Tensor CreateFromTensorSub(Tensor t) => new(t);

   /// <summary>
   /// Copies this object and its storage
   /// </summary>
   /// <returns></returns>
   public Int32Tensor DeepClone() => new(Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public Int32Tensor RowStack(Int32Tensor other) => new(torch.row_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public Int32Tensor ColumnStack( Int32Tensor other) =>new(torch.column_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public Int32Tensor Concat(int dim, params Int32Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int32Tensor Concat(Int32Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(static a =>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public Int32Tensor Stack(int dim, params Int32Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(this).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int32Tensor Stack(IList<Int32Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(static a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public Int32Tensor Blank() => new (torch.zeros_like(Storage));


   /// <summary>
   /// Pads the tensor so it is centered in the result
   /// </summary>
   /// <param name="padBy">Amount to pad left and right for each dimension. Provide twice as many values as their are dimensions ordered LDim0, RDim0, LDim1, RDim1, etc</param>
   /// <remarks>This does not match behaviour of torch.pad, which has arguments in a sort of reverse order</remarks>
   /// <returns>A new, padded, tensor</returns>
   public Int32Tensor PadSurround(params long[] padBy)
   {
      if (padBy.Length != NDims * 2)
      {
         throw new ArgumentException($"Expected {NDims * 2} values");
      }

      // Torch orders things in reverse DimN L, DimN R, Dim N-1 L, Dim N-1 R, etc
      long[] padTorchOrder = padBy.Reverse().ToArray();

      for (int i = 0; i < padTorchOrder.Length;i+=2)
      {
         (padTorchOrder[i], padTorchOrder[i + 1]) = (padTorchOrder[i + 1], padTorchOrder[i]);
      }

      return CreateFromTensor(torch.nn.functional.pad(Storage, padTorchOrder, PaddingModes.Constant));
   }
}

public partial class Int64Tensor
{
   /// <summary>
   /// Creates a new <see cref="Int64Tensor"/> containing default values
   /// </summary>
   /// <param name="dimSizes">Dimension sizes</param>
   [SetsRequiredMembers]
   public Int64Tensor(params long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Int64))
   {
   }

   /// <summary>
   /// Creates a new <see cref="Int64Tensor"/>
   /// </summary>
   /// <param name="t">Data to wrap</param>
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public Int64Tensor(Tensor t) : base(t) { }

   /// <summary>
   /// The pytorch scalar type contained within
   /// </summary>
   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Int64;


   [CLSCompliant(false)]
   protected override Int64Tensor CreateFromTensorSub(Tensor t) => new(t);

   /// <summary>
   /// Copies this object and its storage
   /// </summary>
   /// <returns></returns>
   public Int64Tensor DeepClone() => new(Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public Int64Tensor RowStack(Int64Tensor other) => new(torch.row_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public Int64Tensor ColumnStack( Int64Tensor other) =>new(torch.column_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public Int64Tensor Concat(int dim, params Int64Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Int64Tensor Concat(Int64Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(static a =>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public Int64Tensor Stack(int dim, params Int64Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(this).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Int64Tensor Stack(IList<Int64Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(static a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public Int64Tensor Blank() => new (torch.zeros_like(Storage));


   /// <summary>
   /// Pads the tensor so it is centered in the result
   /// </summary>
   /// <param name="padBy">Amount to pad left and right for each dimension. Provide twice as many values as their are dimensions ordered LDim0, RDim0, LDim1, RDim1, etc</param>
   /// <remarks>This does not match behaviour of torch.pad, which has arguments in a sort of reverse order</remarks>
   /// <returns>A new, padded, tensor</returns>
   public Int64Tensor PadSurround(params long[] padBy)
   {
      if (padBy.Length != NDims * 2)
      {
         throw new ArgumentException($"Expected {NDims * 2} values");
      }

      // Torch orders things in reverse DimN L, DimN R, Dim N-1 L, Dim N-1 R, etc
      long[] padTorchOrder = padBy.Reverse().ToArray();

      for (int i = 0; i < padTorchOrder.Length;i+=2)
      {
         (padTorchOrder[i], padTorchOrder[i + 1]) = (padTorchOrder[i + 1], padTorchOrder[i]);
      }

      return CreateFromTensor(torch.nn.functional.pad(Storage, padTorchOrder, PaddingModes.Constant));
   }
}

public partial class FloatTensor
{
   /// <summary>
   /// Creates a new <see cref="FloatTensor"/> containing default values
   /// </summary>
   /// <param name="dimSizes">Dimension sizes</param>
   [SetsRequiredMembers]
   public FloatTensor(params long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Float32))
   {
   }

   /// <summary>
   /// Creates a new <see cref="FloatTensor"/>
   /// </summary>
   /// <param name="t">Data to wrap</param>
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public FloatTensor(Tensor t) : base(t) { }

   /// <summary>
   /// The pytorch scalar type contained within
   /// </summary>
   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Float32;


   [CLSCompliant(false)]
   protected override FloatTensor CreateFromTensorSub(Tensor t) => new(t);

   /// <summary>
   /// Copies this object and its storage
   /// </summary>
   /// <returns></returns>
   public FloatTensor DeepClone() => new(Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public FloatTensor RowStack(FloatTensor other) => new(torch.row_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public FloatTensor ColumnStack( FloatTensor other) =>new(torch.column_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public FloatTensor Concat(int dim, params FloatTensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static FloatTensor Concat(FloatTensor[] toStack, int dim) =>new (torch.concat(toStack.Select(static a =>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public FloatTensor Stack(int dim, params FloatTensor[] toStack) => Stack(toStack.Select(a => a).Prepend(this).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static FloatTensor Stack(IList<FloatTensor> toStack, int dim) =>new (torch.stack(toStack.Select(static a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public FloatTensor Blank() => new (torch.zeros_like(Storage));


   /// <summary>
   /// Pads the tensor so it is centered in the result
   /// </summary>
   /// <param name="padBy">Amount to pad left and right for each dimension. Provide twice as many values as their are dimensions ordered LDim0, RDim0, LDim1, RDim1, etc</param>
   /// <remarks>This does not match behaviour of torch.pad, which has arguments in a sort of reverse order</remarks>
   /// <returns>A new, padded, tensor</returns>
   public FloatTensor PadSurround(params long[] padBy)
   {
      if (padBy.Length != NDims * 2)
      {
         throw new ArgumentException($"Expected {NDims * 2} values");
      }

      // Torch orders things in reverse DimN L, DimN R, Dim N-1 L, Dim N-1 R, etc
      long[] padTorchOrder = padBy.Reverse().ToArray();

      for (int i = 0; i < padTorchOrder.Length;i+=2)
      {
         (padTorchOrder[i], padTorchOrder[i + 1]) = (padTorchOrder[i + 1], padTorchOrder[i]);
      }

      return CreateFromTensor(torch.nn.functional.pad(Storage, padTorchOrder, PaddingModes.Constant));
   }
}

public partial class BoolTensor
{
   /// <summary>
   /// Creates a new <see cref="BoolTensor"/> containing default values
   /// </summary>
   /// <param name="dimSizes">Dimension sizes</param>
   [SetsRequiredMembers]
   public BoolTensor(params long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Bool))
   {
   }

   /// <summary>
   /// Creates a new <see cref="BoolTensor"/>
   /// </summary>
   /// <param name="t">Data to wrap</param>
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public BoolTensor(Tensor t) : base(t) { }

   /// <summary>
   /// The pytorch scalar type contained within
   /// </summary>
   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Bool;


   [CLSCompliant(false)]
   protected override BoolTensor CreateFromTensorSub(Tensor t) => new(t);

   /// <summary>
   /// Copies this object and its storage
   /// </summary>
   /// <returns></returns>
   public BoolTensor DeepClone() => new(Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public BoolTensor RowStack(BoolTensor other) => new(torch.row_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public BoolTensor ColumnStack( BoolTensor other) =>new(torch.column_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public BoolTensor Concat(int dim, params BoolTensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static BoolTensor Concat(BoolTensor[] toStack, int dim) =>new (torch.concat(toStack.Select(static a =>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public BoolTensor Stack(int dim, params BoolTensor[] toStack) => Stack(toStack.Select(a => a).Prepend(this).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static BoolTensor Stack(IList<BoolTensor> toStack, int dim) =>new (torch.stack(toStack.Select(static a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public BoolTensor Blank() => new (torch.zeros_like(Storage));


   /// <summary>
   /// Pads the tensor so it is centered in the result
   /// </summary>
   /// <param name="padBy">Amount to pad left and right for each dimension. Provide twice as many values as their are dimensions ordered LDim0, RDim0, LDim1, RDim1, etc</param>
   /// <remarks>This does not match behaviour of torch.pad, which has arguments in a sort of reverse order</remarks>
   /// <returns>A new, padded, tensor</returns>
   public BoolTensor PadSurround(params long[] padBy)
   {
      if (padBy.Length != NDims * 2)
      {
         throw new ArgumentException($"Expected {NDims * 2} values");
      }

      // Torch orders things in reverse DimN L, DimN R, Dim N-1 L, Dim N-1 R, etc
      long[] padTorchOrder = padBy.Reverse().ToArray();

      for (int i = 0; i < padTorchOrder.Length;i+=2)
      {
         (padTorchOrder[i], padTorchOrder[i + 1]) = (padTorchOrder[i + 1], padTorchOrder[i]);
      }

      return CreateFromTensor(torch.nn.functional.pad(Storage, padTorchOrder, PaddingModes.Constant));
   }
}

public partial class Complex32Tensor
{
   /// <summary>
   /// Creates a new <see cref="Complex32Tensor"/> containing default values
   /// </summary>
   /// <param name="dimSizes">Dimension sizes</param>
   [SetsRequiredMembers]
   public Complex32Tensor(params long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.ComplexFloat32))
   {
   }

   /// <summary>
   /// Creates a new <see cref="Complex32Tensor"/>
   /// </summary>
   /// <param name="t">Data to wrap</param>
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public Complex32Tensor(Tensor t) : base(t) { }

   /// <summary>
   /// The pytorch scalar type contained within
   /// </summary>
   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.ComplexFloat32;


   [CLSCompliant(false)]
   protected override Complex32Tensor CreateFromTensorSub(Tensor t) => new(t);

   /// <summary>
   /// Copies this object and its storage
   /// </summary>
   /// <returns></returns>
   public Complex32Tensor DeepClone() => new(Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public Complex32Tensor RowStack(Complex32Tensor other) => new(torch.row_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public Complex32Tensor ColumnStack( Complex32Tensor other) =>new(torch.column_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public Complex32Tensor Concat(int dim, params Complex32Tensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static Complex32Tensor Concat(Complex32Tensor[] toStack, int dim) =>new (torch.concat(toStack.Select(static a =>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public Complex32Tensor Stack(int dim, params Complex32Tensor[] toStack) => Stack(toStack.Select(a => a).Prepend(this).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static Complex32Tensor Stack(IList<Complex32Tensor> toStack, int dim) =>new (torch.stack(toStack.Select(static a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public Complex32Tensor Blank() => new (torch.zeros_like(Storage));


   /// <summary>
   /// Pads the tensor so it is centered in the result
   /// </summary>
   /// <param name="padBy">Amount to pad left and right for each dimension. Provide twice as many values as their are dimensions ordered LDim0, RDim0, LDim1, RDim1, etc</param>
   /// <remarks>This does not match behaviour of torch.pad, which has arguments in a sort of reverse order</remarks>
   /// <returns>A new, padded, tensor</returns>
   public Complex32Tensor PadSurround(params long[] padBy)
   {
      if (padBy.Length != NDims * 2)
      {
         throw new ArgumentException($"Expected {NDims * 2} values");
      }

      // Torch orders things in reverse DimN L, DimN R, Dim N-1 L, Dim N-1 R, etc
      long[] padTorchOrder = padBy.Reverse().ToArray();

      for (int i = 0; i < padTorchOrder.Length;i+=2)
      {
         (padTorchOrder[i], padTorchOrder[i + 1]) = (padTorchOrder[i + 1], padTorchOrder[i]);
      }

      return CreateFromTensor(torch.nn.functional.pad(Storage, padTorchOrder, PaddingModes.Constant));
   }
}

public partial class ComplexTensor
{
   /// <summary>
   /// Creates a new <see cref="ComplexTensor"/> containing default values
   /// </summary>
   /// <param name="dimSizes">Dimension sizes</param>
   [SetsRequiredMembers]
   public ComplexTensor(params long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.ComplexFloat64))
   {
   }

   /// <summary>
   /// Creates a new <see cref="ComplexTensor"/>
   /// </summary>
   /// <param name="t">Data to wrap</param>
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public ComplexTensor(Tensor t) : base(t) { }

   /// <summary>
   /// The pytorch scalar type contained within
   /// </summary>
   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.ComplexFloat64;


   [CLSCompliant(false)]
   protected override ComplexTensor CreateFromTensorSub(Tensor t) => new(t);

   /// <summary>
   /// Copies this object and its storage
   /// </summary>
   /// <returns></returns>
   public ComplexTensor DeepClone() => new(Storage.clone());

   #region COMBINE ACROSS DIMENSIONS
   /// <summary>
   /// Returns a new tensor that is this with the additional row added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public ComplexTensor RowStack(ComplexTensor other) => new(torch.row_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional column added
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public ComplexTensor ColumnStack( ComplexTensor other) =>new(torch.column_stack([Storage, other.Storage]));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public ComplexTensor Concat(int dim, params ComplexTensor[] toStack) =>new (torch.concat(toStack.Select(a => a.Storage).Prepend(Storage).ToList(), dim: dim));
   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with a new shape but the same dimensionality</returns>
   public static ComplexTensor Concat(ComplexTensor[] toStack, int dim) =>new (torch.concat(toStack.Select(static a =>a.Storage).ToList(), dim: dim));

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public ComplexTensor Stack(int dim, params ComplexTensor[] toStack) => Stack(toStack.Select(a => a).Prepend(this).ToList(), dim: dim);

   /// <summary>
   /// Returns a new tensor that is this with the additional tensors stacked in the dimension specified
   /// </summary>
   /// <param name="other"></param>
   /// <returns>A tensor with n+1 dimensions</returns>
   public static ComplexTensor Stack(IList<ComplexTensor> toStack, int dim) =>new (torch.stack(toStack.Select(static a => a.Storage).ToList(), dim: dim));

   #endregion


   /// <summary>
   /// Creates a tensor of zeros, or equivalent, of the same size / shape as this
   /// </summary>
   /// <returns></returns>
   public ComplexTensor Blank() => new (torch.zeros_like(Storage));


   /// <summary>
   /// Pads the tensor so it is centered in the result
   /// </summary>
   /// <param name="padBy">Amount to pad left and right for each dimension. Provide twice as many values as their are dimensions ordered LDim0, RDim0, LDim1, RDim1, etc</param>
   /// <remarks>This does not match behaviour of torch.pad, which has arguments in a sort of reverse order</remarks>
   /// <returns>A new, padded, tensor</returns>
   public ComplexTensor PadSurround(params long[] padBy)
   {
      if (padBy.Length != NDims * 2)
      {
         throw new ArgumentException($"Expected {NDims * 2} values");
      }

      // Torch orders things in reverse DimN L, DimN R, Dim N-1 L, Dim N-1 R, etc
      long[] padTorchOrder = padBy.Reverse().ToArray();

      for (int i = 0; i < padTorchOrder.Length;i+=2)
      {
         (padTorchOrder[i], padTorchOrder[i + 1]) = (padTorchOrder[i + 1], padTorchOrder[i]);
      }

      return CreateFromTensor(torch.nn.functional.pad(Storage, padTorchOrder, PaddingModes.Constant));
   }
}

#endregion TEMPLATE EXPANSION
