#warning clean up
//using System.Linq;
//using System.Numerics;
//using System.Reflection.PortableExecutable;
//using System.Runtime.CompilerServices;
//using TorchSharp;
//using static TorchSharp.torch;

//namespace FlipProof.Image;

//public abstract class ImageBase : IImage, IDisposable
//{
//   internal abstract Tensor Data { get; }

//   public abstract int NDims { get; } // Manually set to check header is sane

//   /// <summary>
//   /// Callers must call <see cref="VerifyConstruction"/> once ready
//   /// </summary>
//   internal ImageBase() { }

//   protected void VerifyConstruction()
//   {
//      if (Header.NDims != NDims)
//      {
//         throw new ArgumentException($"Header states voxel dimensionality of {Header.NDims} but expected {NDims} dimensions");
//      }
//      if (DataStorageMatchesHeader())
//      {
//         throw new ArgumentException("Data storage size or shape does not match header");
//      }

//   }

//   public abstract ImageHeader Header { get; }

//   #region Apply Operator


//   /// <summary>
//   /// Applies a <see cref="torch.Tensor"/> operator to create a new object
//   /// </summary>
//   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
//   /// <typeparam name="S">Expected output data type</typeparam>
//   /// <param name="operation">Voxel operation</param>
//   /// <returns>A new image with the altered voxels</returns>
//   public S OperatorToNew<S>(Func<Tensor, Tensor> operation, Func<ImageHeader, Tensor, S> constructor)
//   {
//      return constructor(_header, operation(Data));
//   }
//   /// <summary>
//   /// Applies a <see cref="torch.Tensor"/> operator to these voxels, in place
//   /// </summary>
//   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
//   /// <typeparam name="T"></typeparam>
//   /// <param name="other"></param>
//   /// <param name="operation"></param>
//   /// <returns></returns>
//   public void OperatorInPlace(Action<Tensor> operation)
//   {
//      operation(Data);
//      if (!DataStorageMatchesHeader())
//      {
//         throw new NotSupportedException("Operator altered the shape of data storage");
//      }
//   }


//   /// <summary>
//   /// Verifies images in same space and applies a <see cref="torch.Tensor"/> operator to create a new object from the resulting <see cref="torch.Tensor"/>
//   /// </summary>
//   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
//   /// <typeparam name="TOut">The expected output datatype from the operation</typeparam>
//   /// <param name="other"></param>
//   /// <param name="operation"></param>
//   /// <returns></returns>
//   public TOut OperatorToNew<TOut>(ImageBase other, Func<Tensor, Tensor, Tensor> operation, Func<ImageHeader, Tensor, TOut> constructor)
//   {
//      this.ThrowIfNotSameSpaceAs(other);
//      return constructor(_header, operation(Data, other.Data));
//   }
//   /// <summary>
//   /// Verifies images in same space and applies a <see cref="torch.Tensor"/> operator to the present data, altering it in place
//   /// </summary>
//   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
//   /// <typeparam name="T"></typeparam>
//   /// <param name="other"></param>
//   /// <param name="operation"></param>
//   /// <returns></returns>
//   public void OperatorInPlace(ImageBase other, Action<Tensor, Tensor> operation)
//   {
//      this.ThrowIfNotSameSpaceAs(other);
//      operation(Data, other.Data);
//      if (!DataStorageMatchesHeader())
//      {
//         throw new NotSupportedException("Operator altered the shape of data storage");
//      }
//   }


//   #endregion


//   #region Equality
//   protected bool DataStorageMatchesHeader()
//   {
//      return !Data.shape.SequenceEqual(_header.Size.Select(Convert.ToInt64));
//   }

//   /// <summary>
//   /// True if the voxels match without checking the orientation of the images
//   /// </summary>
//   /// <param name="other"></param>
//   /// <returns></returns>
//   private bool VoxelsEqual_Unsafe(ImageBase other) => Data.equal(other.Data).ToBoolean();

//   /// <summary>
//   /// True if the voxels match and are in the same space
//   /// </summary>
//   /// <param name="other"></param>
//   /// <returns></returns>
//   public bool Equals(ImageBase other) => ReferenceEquals(this, other) || (this.IsSameSpaceAs(other) && VoxelsEqual_Unsafe(other));

//   /// <summary>
//   /// True if the voxels match and are in the same space
//   /// </summary>
//   /// <param name="obj"></param>
//   /// <returns></returns>
//   public override bool Equals(object? obj) => obj is ImageBase ib && Equals(ib);
//   public override int GetHashCode()
//   {
//      // Hashing the voxel data is too expensive
//      return _header.GetHashCode();
//   }
//   #endregion

//   #region Dispose


//   /// <summary>
//   /// Prevents this becoming invalid when the torch dispose scope is disposed
//   /// </summary>
//   public void MoveToOuterDisposeScope() => Data.MoveToOuterDisposeScope();
//   /// <summary>
//   /// Dispose actively clears memory. If not called, GC will take care of it in time
//   /// </summary>
//   public void Dispose()
//   {
//      Data.Dispose();
//      GC.SuppressFinalize(this);
//   }


//   #endregion
//}
