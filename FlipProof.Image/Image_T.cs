﻿using FlipProof.Base;
using FlipProof.Image.IO;
using FlipProof.Torch;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.utils;

namespace FlipProof.Image;

/// <summary>
/// Base class for images with an known and enforced orientation
/// </summary>
public abstract class Image<TSpace> : IDisposable
   where TSpace : struct, ISpace
{

   internal Image(Tensor _rawData) 
   {
      if(_rawData.ndim != 4)
      {
         throw new ArgumentException($"Tensor must be 4D, ordered x,y,z,volume. Got {_rawData.ndim} dimensions");
      }
      RawData = _rawData;
      
   }

   [CLSCompliant(false)]
   protected Tensor RawData { get; }
   public ImageHeader Header => ISpace.GetOrientation<TSpace>()!;


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
   public abstract ImageUInt8<TSpace> ToUInt8();

#pragma warning disable CS0618 // Type or member is obsolete
   protected ImageBool<TSpace> VoxelwiseEquals(Image<TSpace> other) => new(new BoolTensor( RawData.eq(other.RawData)), false);
#pragma warning restore CS0618 // Type or member is obsolete

   /// <summary>
   /// Returns a 4D tensor containing the single volume requested. The returned value is not linked to this object
   /// </summary>
   /// <param name="index">The volume of interest</param>
   internal Tensor ExtractVolumeAsTensor(long index) => RawData[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3);

   #region Comparison Operators


   /// <summary>
   /// Voxelwise comparison
   /// </summary>
   public static ImageBool<TSpace> operator ==(Image<TSpace> left, Image<TSpace> right) => left.VoxelwiseEquals(right);
   /// <summary>
   /// Voxelwise comparison
   /// </summary>
   public static ImageBool<TSpace> operator !=(Image<TSpace> left, Image<TSpace> right)
   {
      return (left == right).NotInPlace();
   }

   #endregion


   /// <summary>
   /// Reference equals
   /// </summary>
   /// <param name="obj">Other</param>
   /// <returns>True if images are the same object</returns>
   public override bool Equals(object? obj) => obj is not null && ReferenceEquals(this, obj);

   /// <summary>
   /// Returns a hash code based on the storage
   /// </summary>
   /// <returns></returns>
   public override int GetHashCode() => RawData.GetHashCode();
   public abstract void Dispose();

}

/// <summary>
/// Base class for images with an known and enforced orientation and voxel type
/// </summary>
/// <typeparam name="TVoxel">The voxel type, such as <see cref="double"/> </typeparam>
/// <typeparam name="TSpace">The physical space this is oriented to</typeparam>
public abstract class Image<TVoxel, TSpace> : Image<TSpace>
   where TSpace : struct, ISpace
   where TVoxel : struct
{
   [CLSCompliant(false)]
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
   [OrientationCheckedAtRuntime]
   internal Image(ImageHeader header, Tensor<TVoxel> voxels) : this(voxels.DeepClone(), false) // Space not initialised yet so can't be verified
   {
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
         VerifyVoxelArrayShape(ISpace.GetOrientation<TSpace>() ?? throw new OrientationException("Orientation is not yet registered"));
      }
   }

   private void VerifyVoxelArrayShape(ImageHeader header) => VerifyVoxelArrayShape(header, _data);
   private static void VerifyVoxelArrayShape(ImageHeader header, Tensor<TVoxel> voxels)
   {
      // Base class enforces 4D tensor
      if (header.NDims == 3 && voxels.Storage.shape[3] != 1)
      {
         throw new ArgumentException($"The image is 3D and so the fourth dimension of the voxel data should be size 1 but it is {voxels.Storage.shape[3]}");
      }
      if (!voxels.Storage.shape.Zip(header.Size, (a, b) => a == b).All(a => a))
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
   public override ImageBool<TSpace> ToBool() => ImageBool<TSpace>.UnsafeCreateStatic(_data.ToBool());

#pragma warning disable CS0618 // Type or member is obsolete
   public override ImageUInt8<TSpace> ToUInt8() => this is ImageUInt8<TSpace> imF ? imF : new(_data.ToUInt8(), false);


#pragma warning disable CS0618 // Type or member is obsolete
   [CLSCompliant(false)]
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




   /// <summary>
   /// Sorts the volumes of this image, in place, using the keys provided
   /// </summary>
   /// <typeparam name="TKey">Data type of key</typeparam>
   /// <param name="keys">Used to sort this' volumes</param>
   public void SortVolumesInPlace<TKey>(TKey[] keys) where TKey:struct,IComparable<TKey> => _data.SortInPlace(keys, 3);

   #endregion

   #region Get / Calc

   public TVoxel this[int x, int y, int z, int volume]
   {
      get => _data[x, y, z, volume];
      set => _data[x, y, z, volume] = value;
   }
   
   public TVoxel this[long x, long y, long z, long volume]
   {
      get => _data[x, y, z, volume];
      set => _data[x, y, z, volume] = value;
   }
   public TVoxel this[XYZA<int> index]
   {
      get => this[index.X, index.Y, index.Z, index.A];
      set => this[index.X, index.Y, index.Z, index.A] = value;
   }
   public TVoxel this[XYZ<int> index, int volume = 0]
   {
      get => this[index.X, index.Y, index.Z, volume];
      set => this[index.X, index.Y, index.Z, volume] = value;
   }
   
   public TVoxel this[XYZA<long> index]
   {
      get => this[index.X, index.Y, index.Z, index.A];
      set => this[index.X, index.Y, index.Z, index.A] = value;
   }
   public TVoxel this[XYZ<long> index, long volume = 0]
   {
      get => this[index.X, index.Y, index.Z, volume];
      set => this[index.X, index.Y, index.Z, volume] = value;
   }

   public IEnumerable<XYZA<long>> GetAllVoxelIndices() => Header.Size.GetAllVoxelIndices();

   public TVoxel[] GetAllVoxels() => _data.ToArray();
   /// <summary>
   /// Returns a copy of the voxels as a wrapped tensor
   /// </summary>
   /// <returns></returns>
   public Tensor<TVoxel> GetAllVoxelsAsTensor() => _data.DeepClone();

   /// <summary>
   /// Returns a copy of the voxels as a 4D array in managed memory
   /// </summary>
   /// <returns></returns>
   public Array4D<TVoxel> GetVoxelsAsArray4D() => _data.Storage.ToArray<Array4D<TVoxel>, TVoxel>(arr => new Array4D<TVoxel>(arr[0], arr[1], arr[2], arr[3]), 4);

   public TVoxel GetMaxIntensity() => _data.GetScalarFromResult(torch.max);
   public TVoxel GetMinIntensity() => _data.GetScalarFromResult(torch.min);

   /// <summary>
   /// Gets voxel indices connected with a full face that are larger in one dimension than this (i.e. voxel.X+1, voxel.Y+1, or voxel.Z+1)
   /// </summary>
   /// <param name="voxel"></param>
   /// <returns></returns>
   public IEnumerable<XYZ<int>> GetConnectedVoxels_3Wise(XYZ<int> voxel)
   {
      ImageSize size = Header.Size;
      if (voxel.X + 1 < size.X)
      {
         yield return voxel with { X = voxel.X + 1 };
      }
      if (voxel.Y + 1 < size.Y)
      {
         yield return voxel with { Y = voxel.Y + 1 };
      }
      if (voxel.Z + 1 < size.Z)
      {
         yield return voxel with { Z = voxel.Z + 1 };
      }
   }
   /// <summary>
   /// Gets voxel indices connected with a full face that are larger in one dimension than this (i.e. voxel.X+1, voxel.Y+1, or voxel.Z+1)
   /// </summary>
   /// <param name="voxel"></param>
   /// <returns></returns>
   public IEnumerable<XYZ<long>> GetConnectedVoxels_3Wise(XYZ<long> voxel)
   {
      ImageSize size = Header.Size;
      if (voxel.X + 1 < size.X)
      {
         yield return voxel with { X = voxel.X + 1 };
      }
      if (voxel.Y + 1 < size.Y)
      {
         yield return voxel with { Y = voxel.Y + 1 };
      }
      if (voxel.Z + 1 < size.Z)
      {
         yield return voxel with { Z = voxel.Z + 1 };
      }
   }
   /// <summary>
   /// Gets voxel indices connected with a full face (i.e. no diagonals)
   /// </summary>
   /// <param name="voxel"></param>
   /// <returns></returns>
   internal IEnumerable<XYZ<int>> GetConnectedVoxels_6Wise(XYZ<int> voxel)
   {
      ImageSize size = Header.Size;
      if (voxel.X > 0)
      {
         yield return voxel with { X = voxel.X - 1 };
      }
      if (voxel.Y > 0)
      {
         yield return voxel with { Y = voxel.Y - 1 };
      }
      if (voxel.Z > 0)
      {
         yield return voxel with { Z = voxel.Z - 1 };
      }
      if (voxel.X + 1 < size.X)
      {
         yield return voxel with { X = voxel.X + 1 };
      }
      if (voxel.Y + 1 < size.Y)
      {
         yield return voxel with { Y = voxel.Y + 1 };
      }
      if (voxel.Z + 1 < size.Z)
      {
         yield return voxel with { Z = voxel.Z + 1 };
      }
   }



   #endregion

   /// <summary>
   /// Set voxels for a given 3D volume. 
   /// </summary>
   /// <remarks>Orientation unsafe. Use of this method should be reserved for situations where Image level operations were not supported and tensor operations were the only option</remarks>
   /// <param name="index">The volume to set</param>
   /// <param name="newData">The new data. This is copied, not used directly</param>
   [OrientationCheckedAtRuntime]
   [Obsolete("Orientation unsafe")]
   public void SetVolume(int index, Tensor<TVoxel> newData)
   {
      VerifyVoxelArrayShape(Header, newData);
      _data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index] = newData.Storage;
   }

   

   /// <summary>
   /// Copies all values from the provided image that are true in the mask into this image
   /// </summary>
   /// <param name="values"></param>
   /// <param name="mask"></param>
   public void Set(Image<TVoxel, TSpace> values, ImageBool<TSpace> mask)
   {
      this._data.Storage[mask._data.Storage] = values._data.Storage[mask._data.Storage];   
   }

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

   internal LargeMemoryStream GetVoxelBytes()
   {
      long totalSize = _data.Count * CollectionCreation.SizeOfType(Array.Empty<TVoxel>(), true);
      LargeMemoryStream voxels = new(totalSize, (totalSize > 2147483591L) ? (-1) : (int)totalSize)
      {
         Position = 0L
      };
      _data.WriteBytesToStream(voxels);
      return voxels;
   }


   #endregion
}


[CLSCompliant(true)]
public abstract class Image<TVoxel, TSpace, TSelf, TTensor> : Image<TVoxel, TSpace>
   where TVoxel : struct
   where TSpace : struct, ISpace
   where TTensor : Tensor<TVoxel, TTensor>
   where TSelf : Image<TVoxel, TSpace, TSelf, TTensor>
{
   internal TTensor Data { get; }

   #region Construction

   /// <summary>
   /// Creates a new image, explicitly stating the orientation. Appropriate 
   /// for use in I/O operations or when first stating the orientation of 
   /// this type. Provided voxels are cloned, not used directly.
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Voxel data - a copy of the provided object is made</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal Image(ImageHeader header, Tensor voxels) : base(header, voxels)
   {
      Data = (TTensor)_data;
   }

   /// <summary>
   /// Creates a new image, explicitly stating the orientation. Appropriate 
   /// for use in I/O operations or when first stating the orientation of 
   /// this type. Provided voxels are cloned, not used directly.
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Voxel data - a copy of the provided object is made</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal Image(ImageHeader header, TTensor voxels) : base(header, voxels)
   {
      Data = voxels;
   }


   /// <summary>
   /// Creates a new Image from an operation known to not affect the shape or data-order of the tensor
   /// </summary>
   /// <param name="voxels">Voxel data. Data are used directly. Do not feed in a tensor accessible outside this object</param>
   /// <param name="verifyShape">Verify that the voxel array shape should be verified against the header. True if the operation is not trusted</param>
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal Image(TTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
      Data = voxels;
#if DEBUG
      // If verify is true the orientation will have already been checked in the base class constructor
      Debug.Assert(verifyShape || ISpace.GetOrientation<TSpace>() is not null, "Space is not initialised but using unsafe constructor");
#endif
   }

   /// <summary>
   /// Creates an object like this with the provided voxels
   /// </summary>
   /// <param name="voxels"></param>
   /// <returns></returns>
   internal abstract TSelf UnsafeCreate(TTensor voxels);

   /// <summary>
   /// Creates a copy of this object
   /// </summary>
   /// <returns></returns>
   public TSelf DeepClone() => UnsafeCreate((TTensor)Data.DeepClone());
   #endregion

   /// <summary>
   /// Returns a copy of the voxels as a wrapped tensor
   /// </summary>
   /// <returns></returns>
   public new TTensor GetAllVoxelsAsTensor() => Data.DeepClone();
}
