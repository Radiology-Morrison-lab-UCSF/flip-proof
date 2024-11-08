﻿using FlipProof.Image.IO;
using FlipProof.Torch;
using System.Runtime.InteropServices.ObjectiveC;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.utils;

namespace FlipProof.Image;

/// <summary>
/// Base class for images with an known and enforced orientation
/// </summary>
public abstract class Image<TSpace> : IDisposable
   where TSpace : ISpace
{

   internal Image(Tensor _rawData) 
   {
      RawData = _rawData;
   }

   protected Tensor RawData { get; }
   public int NDims => TSpace.NDims;
   public ImageHeader Header => TSpace.Orientation!;


   /// <summary>
   /// Applies a <see cref="torch.Tensor"/> operator to create a new object from the resulting <see cref="torch.Tensor"/>
   /// </summary>
   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
   /// <typeparam name="TOut">The expected output datatype from the operation</typeparam>
   /// <param name="other">The second image</param>
   /// <param name="operation">The operation to apply to the two images</param>
   /// <returns></returns>
   [Obsolete("Use voxel safe method")]
   internal TOut TrustedOperatorToNew<TOut>(Image<TSpace> other, Func<Tensor, Tensor, Tensor> operation, Func<Tensor, TOut> constructor)
   {
      return constructor(operation(RawData, other.RawData));
   }

   public abstract ImageBool<TSpace> ToBool();
   public abstract ImageFloat<TSpace> ToFloat();
   public abstract ImageDouble<TSpace> ToDouble();

#pragma warning disable CS0618 // Type or member is obsolete
   protected ImageBool<TSpace> VoxelwiseEquals(Image<TSpace> other) => new(new BoolTensor( RawData.eq(other.RawData)), false);
#pragma warning restore CS0618 // Type or member is obsolete

   #region Comparison Operators



   public static ImageBool<TSpace> operator ==(Image<TSpace> left, Image<TSpace> right) => left.VoxelwiseEquals(right);
   public static ImageBool<TSpace> operator !=(Image<TSpace> left, Image<TSpace> right)
   {
      return (left == right).NotInPlace();
   }

   #endregion

   public abstract void Dispose();

}

/// <summary>
/// Base class for images with an known and enforced orientation and voxel type
/// </summary>
/// <typeparam name="TVoxel">The voxel type, such as <see cref="double"/> </typeparam>
/// <typeparam name="TSpace">The physical space this is oriented to</typeparam>
public abstract class Image<TVoxel, TSpace> : Image<TSpace>
   where TSpace : ISpace
   where TVoxel : struct
{
   protected readonly Tensor<TVoxel> _data;

   #region Construction


   /// <summary>
   /// Creates a new image, explicitly stating the orientation. Appropriate 
   /// for use in I/O operations or when first stating the orientation of 
   /// this type. Provided voxels are cloned, not used directly.
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Voxel data - a copy of the provided object is made</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal Image(ImageHeader header, Tensor voxels) : this(header, Tensor<TVoxel>.CreateTensor(voxels, false))
   {

   }

   /// <summary>
   /// Creates a new image, explicitly stating the orientation. Appropriate 
   /// for use in I/O operations or when first stating the orientation of 
   /// this type. Provided voxels are cloned, not used directly.
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Voxel data - a copy of the provided object is made</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal Image(ImageHeader header, Tensor<TVoxel> voxels) : this(voxels.DeepClone(), false) // Space not initialised yet so can't be verified
   {
      if (header.NDims is not 3 and not 4)
      {
         throw new NotSupportedException("Only 3D and 4D images are supported. Use a flat dimension if wanting more");
      }
      ISpace.Initialise<TSpace>(header);
      VerifyVoxelArrayShape(header);
   }


   /// <summary>
   /// Creates a new Image from an operation known to not affect the shape or data-order of the tensor
   /// </summary>
   /// <param name="voxels">Voxel data. Data are used directly. Do not feed in a tensor accessible outside this object</param>
   /// <param name="verifyShape">Verify that the voxel array shape should be verified against the header. True if the operation is not trusted</param>
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal Image(Tensor<TVoxel> voxels, bool verifyShape):base(voxels.Storage)
   {
      _data = voxels;
      if(verifyShape)
      {
         VerifyVoxelArrayShape(TSpace.Orientation!);
      }
   }

   private void VerifyVoxelArrayShape(ImageHeader header)
   {
      if (_data.Storage.ndim != header.NDims)
      {
         throw new ArgumentException("The number of dimensions of the voxel data does not match the image header");
      }
      if (!_data.Storage.shape.Zip(header.Size, (a, b) => a == b).All(a => a))
      {
         throw new ArgumentException("Voxel data is wrong size");
      }
   }


   /// <summary>
   /// Applies a <see cref="torch.Tensor"/> operator to create a new object from the resulting <see cref="torch.Tensor"/>
   /// </summary>
   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
   /// <typeparam name="TOut">The expected output datatype from the operation</typeparam>
   /// <param name="other">The second image</param>
   /// <param name="operation">The operation to apply to the two images</param>
   /// <returns></returns>
   internal TOut TrustedOperatorToNew<TVoxelOut,TOut>(Func<Tensor<TVoxel>, Tensor<TVoxelOut>> operation,
                                                      Func<Tensor<TVoxelOut>, TOut> constructor)
      where TVoxelOut : struct
      where TOut: Image<TVoxelOut, TSpace>
   {
      return constructor(operation(_data));
   }
   /// <summary>
   /// Applies a <see cref="torch.Tensor"/> operator to create a new object from the resulting <see cref="torch.Tensor"/>
   /// </summary>
   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
   /// <typeparam name="TOut">The expected output datatype from the operation</typeparam>
   /// <param name="other">The second image</param>
   /// <param name="operation">The operation to apply to the two images</param>
   /// <returns></returns>
   internal TOut TrustedOperatorToNew<TVoxelOther,TVoxelOut,TOut>(Image<TVoxelOther,TSpace> other,
                                                      Func<Tensor<TVoxel>, Tensor<TVoxelOther>, Tensor<TVoxelOut>> operation,
                                                      Func<Tensor<TVoxelOut>, TOut> constructor)
      where TVoxelOther : struct
      where TVoxelOut : struct
      where TOut: Image<TVoxelOut, TSpace>
   {
      return constructor(operation(_data, other._data));
   }

   /// <summary>
   /// Applies a <see cref="torch.Tensor"/> operator to create a new object from the resulting <see cref="torch.Tensor"/>
   /// </summary>
   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
   /// <typeparam name="TOut">The expected output datatype from the operation</typeparam>
   /// <param name="other">The second image</param>
   /// <param name="operation">The operation to apply to the two images</param>
   /// <returns></returns>
   internal TOut TrustedOperatorToNew<TVoxelOther, TVoxelOut, TOut>(TVoxelOther other,
                                                      Func<Tensor<TVoxel>, TVoxelOther, Tensor<TVoxelOut>> operation,
                                                      Func<Tensor<TVoxelOut>, TOut> constructor)
      where TVoxelOut : struct
      where TOut : Image<TVoxelOut, TSpace>
   {
      return constructor(operation(_data, other));
   }

   protected void OperatorInPlace(Action<Tensor<TVoxel>> func) => func(_data);
   protected void OperatorInPlace(Tensor<TVoxel> other, Action<Tensor<TVoxel>, Tensor<TVoxel>> func) => func(_data, other);

   #endregion

   #region Cast
   /// <summary>
   /// If this is a float image, it is returned. Else, a copy cast to float is created
   /// </summary>
   /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
   public override ImageFloat<TSpace> ToFloat() => this is ImageFloat<TSpace> imF ? imF : new(_data.ToFloat(), false);
#pragma warning restore CS0618 // Type or member is obsolete

   /// <summary>
   /// If this is a double image, it is returned. Else, a copy cast to double is created
   /// </summary>
   /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
   public override ImageDouble<TSpace> ToDouble() => this is ImageDouble<TSpace> imF ? imF : new(_data.ToDouble(), false);
#pragma warning restore CS0618 // Type or member is obsolete

   /// <summary>
   /// Any non zero becomes true
   /// </summary>
   /// <returns></returns>
   public override ImageBool<TSpace> ToBool() => ImageBool<TSpace>.UnsafeCreate(_data.ToBool());

#pragma warning disable CS0618 // Type or member is obsolete
   public static explicit operator ImageInt8<TSpace>(Image<TVoxel,TSpace> value) => new(value._data.ToInt8(), false);
   // TO DO: Uncomment once implemented
   //public static explicit operator Image3dInt16<TSpace>(Image<TVoxel, TSpace> value) => new(value._data.ToInt16(), false);
   //public static explicit operator Image3dInt32<TSpace>(Image<TVoxel, TSpace> value) => new(value._data.ToInt32(), false);
   //public static explicit operator Image3dInt64<TSpace>(Image<TVoxel, TSpace> value) => new(value._data.ToInt64(), false);
   public static explicit operator ImageFloat<TSpace>(Image<TVoxel, TSpace> value) => new(value._data.ToFloat(), false);
   public static explicit operator ImageDouble<TSpace>(Image<TVoxel, TSpace> value) => new(value._data.ToDouble(), false);
#pragma warning restore CS0618 // Type or member is obsolete

   #endregion

   #region Operations

   public void AddInPlace(Image<TVoxel,TSpace> other) => _data.Storage.add_(other._data.Storage);
   public void SubtractInPlace(Image<TVoxel, TSpace> other) => _data.Storage.subtract_(other._data.Storage);
   public void MultiplyInPlace(Image<TVoxel, TSpace> other) => _data.Storage.mul_(other._data.Storage);
   public void DivideInPlace(Image<TVoxel, TSpace> other) => _data.Storage.divide_(other._data.Storage);


   #endregion

   #region Get / Calc

   public TVoxel GetMaxIntensity() => _data.GetScalarFromResult(torch.max);
   public TVoxel GetMinIntensity() => _data.GetScalarFromResult(torch.min);

   #endregion


   /// <summary>
   /// Reference equals
   /// </summary>
   /// <param name="obj">Other</param>
   /// <returns>True if images are the same object</returns>
   public override bool Equals(object? obj) => obj is not null && ReferenceEquals(this, obj);


   #region Dispose


   /// <summary>
   /// Prevents this becoming invalid when the torch dispose scope is disposed
   /// </summary>
   public void MoveToOuterDisposeScope() => _data.Storage.MoveToOuterDisposeScope();
   /// <summary>
   /// Dispose actively clears memory. If not called, GC will take care of it in time
   /// </summary>
   public override void Dispose()
   {
      _data.Dispose();
      GC.SuppressFinalize(this);
   }

   /// <summary>
   /// Returns a copy of the internal voxel data
   /// </summary>
   /// <returns></returns>
   internal Tensor<TVoxel> GetVoxelTensor()
   {
      return _data.DeepClone();
   }
   internal LargeMemoryStream GetVoxelBytes()
   {
      long totalSize = _data.Count * GenMethods.SizeOfType(Array.Empty<TVoxel>(), true);
      LargeMemoryStream voxels = new(totalSize, (totalSize > 2147483591L) ? (-1) : (int)totalSize)
      {
         Position = 0L
      };
      _data.WriteBytesToStream(voxels);
      return voxels;
   }


   #endregion
}
