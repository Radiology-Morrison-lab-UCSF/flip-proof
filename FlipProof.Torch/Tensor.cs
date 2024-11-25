using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using static TorchSharp.torch.utils;

namespace FlipProof.Torch;


/// <summary>
/// Wraps a tensor to enforce type safety
/// </summary>
/// <typeparam name="T">The data type</typeparam>
public abstract class Tensor<T> : IDisposable, IEquatable<Tensor<T>>
   where T : struct
{

   [CLSCompliant(false)]
   public required Tensor Storage { get; init; }

   /// <summary>
   /// Returns the total number of datapoints in the tensor
   /// </summary>
   public long Count
   {
      get
      {
         bool any =false;
         long count = 1;
         foreach (var item in Storage.shape.Where(a => a != 0L))
         {
            count *= item;
            any = true;
         }
         return any ? count : 0L;
      }
   }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="storage"></param>
   /// <exception cref="ArgumentException">The dtype of the tensor does not match Dtype </exception>
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   protected Tensor(Tensor storage) 
   {
      if (storage.dtype != DType)
      {
         throw new ArgumentException($"Bad dtype: got {storage.dtype} but expected {DType}");
      }
      this.Storage = storage;
   }

   [CLSCompliant(false)]
   public virtual torch.ScalarType DType => throw new NotImplementedException("Must be overridden in non-abstract class");

   public T this[params long[] indices]
   {
      get
      {
         using Tensor t = Storage[indices];
         return ToScalar(t);
      }
      set
      {
         Set(value, indices);
      }
   }

   protected abstract void Set(T value, params long[] indices);

   public static Tensor<T> CreateTensor(params long[] size)
   {
      object o = default(T) switch
      {
         double => new DoubleTensor(size),
         float => new FloatTensor(size),
         sbyte => new Int8Tensor(size),
         Int16 => new Int16Tensor(size),
         Int32 => new Int32Tensor(size),
         Int64 => new Int64Tensor(size),
         byte => new UInt8Tensor(size),
         bool => new BoolTensor(size),
         _ => throw new NotSupportedException(typeof(T) + " is not supported")
      };

      return (Tensor<T>)o;
   }
   
   private static Tensor<T> CreateTensor(Tensor t)
   {
      object o = default(T) switch
      {
         double => new DoubleTensor(t),
         float => new FloatTensor(t),
         sbyte => new Int8Tensor(t),
         Int16 => new Int16Tensor(t),
         Int32 => new Int32Tensor(t),
         Int64 => new Int64Tensor(t),
         byte => new UInt8Tensor(t),
         bool => new BoolTensor(t),
         Complex => new ComplexTensor(t),
         Complex32 => new Complex32Tensor(t),
         _ => throw new NotSupportedException(typeof(T) + " is not supported")
      };

      return (Tensor<T>)o;
   }

   /// <summary>
   /// Creates <see cref="Tensor{T}"/> that wraps the input <see cref="Tensor"/>. Casting is not allowed
   /// </summary>
   /// <param name="t">Internal data. Type must must match T.</param>
   /// <param name="wrapCopy">If true, a copy of the tensor is wrapped</param>
   /// <exception cref="NotSupportedException">The type of T is not supported</exception>
   /// <returns></returns>
   [CLSCompliant(false)]
   public static Tensor<T> CreateTensor(Tensor t, bool wrapCopy)
   {
      if (TypeConversions.GetScalarType<T>() != t.dtype)
      {
         throw new ArgumentException($"Input tensor type is not {typeof(T).Name}");
      }
      return CreateTensor(wrapCopy ? t.clone() : t);
   }

   /// <summary>
   /// Creates a new <see cref="Tensor<typeparamref name="T"/>" from a given torch Tensor. If the type does not match, a copy cast to <typeparamref name="T"/> is used
   /// </summary>
   /// <param name="doNotCast">Diallows casting, guaranteeing the input tensor is wrapped, not a copy. If the input is the wrong type, an exception is thrown</param>
   /// <param name="t">The tensor to wrap</param>
   /// <returns></returns>
   [CLSCompliant(false)]
   public Tensor<T> CreateFromTensor(Tensor t, bool doNotCast=false)
   {
      if (t.dtype != DType)
      {
         if(doNotCast)
         {
            throw new ArgumentException("Input tensor is wrong data type");
         }
         t = t.to_type(DType);
      }
      return CreateFromTensor_Sub(t);
   }

   /// <summary>
   /// Creates a new <see cref="Tensor<typeparamref name="T"/>" from a given torch Tensor. 
   /// </summary>
   /// <param name="scalar">Guaranteed to be the correct DType</param>
   /// <returns></returns>
   [CLSCompliant(false)]
   protected virtual Tensor<T> CreateFromTensor_Sub(Tensor scalar) => throw new NotImplementedException("Must be overridden in non-abstract class");

   /// <summary>
   /// Creates a new 1D single value tensor from a given scalar
   /// </summary>
   /// <param name="scalar"></param>
   /// <returns></returns>
   public Tensor<T> CreateScalar(T scalar) => CreateFromTensor(ScalarToTensor(scalar));

   /// <summary>
   /// Creates a new 1D single value tensor from a given array
   /// </summary>
   /// <param name="scalar"></param>
   /// <returns></returns>
   public Tensor<T> Create1D(T[] array) => CreateFromTensor(ArrayToTensor(array));

   /// <summary>
   /// Calls torch.tensor on the input
   /// </summary>
   /// <param name="input"></param>
   /// <returns></returns>
   [CLSCompliant(false)]
   public virtual Tensor ScalarToTensor(T input) => throw new NotImplementedException("Must be overridden in non-abstract class");
   /// <summary>
   /// Calls torch.tensor on the input
   /// </summary>
   [CLSCompliant(false)]
   public virtual Tensor ArrayToTensor(T[] input) => throw new NotImplementedException("Must be overridden in non-abstract class");

   /// <summary>
   /// Applies a function to calculate a scalar, which is returned as a <see cref="T"/>
   /// </summary>
   /// <param name="func"></param>
   /// <returns></returns>
   /// <exception cref="ArgumentException">The function does not return the same dtype</exception>
   [CLSCompliant(false)]
   public T GetScalarFromResult(Func<Tensor, Tensor> func)
   {
      using Tensor result = func(Storage);
      if (result.dtype != DType)
      {
         throw new ArgumentException("Function does not return the same dtype");
      }
      return ToScalar(result);
   }

   /// <summary>
   /// Returns the scalar inside this Tensor, assuming it is a scalar
   /// </summary>
   public T GetScalar() => ToScalar(Storage);
   /// <summary>
   /// Returns the scalar inside the provided Tensor, assuming it is a scalar
   /// </summary>
   [CLSCompliant(false)]
   protected virtual T ToScalar(Tensor t) => throw new NotImplementedException("Must be overridden in non-abstract class");

   /// <summary>
   /// Converts this to a standard array
   /// </summary>
   /// <returns></returns>
   public virtual T[] ToArray() => Storage.ToArray<T>();
   #region Cast

   /// <summary>
   /// Any non zero becomes true
   /// </summary>
   /// <returns></returns>
   public BoolTensor ToBool()
   {
      Tensor asBool = Storage.not_equal(CreateScalar(default).Storage);
      return new BoolTensor(asBool);
   }
   /// <summary>
   /// Casts the internal data. If this is already the requested type, a clone is returned
   /// </summary>
   public DoubleTensor ToDouble() => Cast(torch.DoubleTensor, t=>DoubleTensor.CreateTensor(t,false));
   /// <summary>
   /// Casts the internal data. If this is already the requested type, a clone is returned
   /// </summary>
   public FloatTensor ToFloat() => Cast(torch.FloatTensor, t => FloatTensor.CreateTensor(t, false));
   /// <summary>
   /// Casts the internal data. If this is already the requested type, a clone is returned
   /// </summary>
   public UInt8Tensor ToUInt8() => Cast(torch.ByteTensor, t => UInt8Tensor.CreateTensor(t, false));
   /// <summary>
   /// Casts the internal data. If this is already the requested type, a clone is returned
   /// </summary>
   [CLSCompliant(false)]
   public Int8Tensor ToInt8() => Cast(a=>a.to(ScalarType.Int8), t => Int8Tensor.CreateTensor(t, false));
   /// <summary>
   /// Casts the internal data. If this is already the requested type, a clone is returned
   /// </summary>
   public Int16Tensor ToInt16() => Cast(torch.ShortTensor, t => Int16Tensor.CreateTensor(t, false));
   /// <summary>
   /// Casts the internal data. If this is already the requested type, a clone is returned
   /// </summary>
   public Int32Tensor ToInt32() => Cast(torch.IntTensor, t => Int32Tensor.CreateTensor(t, false));
   /// <summary>
   /// Casts the internal data. If this is already the requested type, a clone is returned
   /// </summary>
   public Int64Tensor ToInt64() => Cast(torch.LongTensor, t => Int64Tensor.CreateTensor(t, false));

   /// <summary>
   /// Casts the internal data. If <typeparamref name="S"/> is <typeparamref name="TVoxel" /> a clone is returned
   /// </summary>
   /// <typeparam name="S"></typeparam>
   /// <param name="conversion"></param>
   /// <returns></returns>
   private TOut Cast<TOut>(Func<Tensor, Tensor> conversion, Func<Tensor, TOut> createWrap) where TOut : class
   {
      Tensor t = conversion(Storage);
      if (ReferenceEquals(Storage, t))
      {
         // can happen when the requested type matches the value type of the tensor
         t = Storage.clone();
      }

      return createWrap(t);
   }

   #endregion

   #region Wrapped Methods
   public BoolTensor ValuewiseEquals<S>(Tensor<S> other)
      where S:struct
   {
      return new(Storage.eq(other.Storage));
   }

   public Tensor<T> RowStack(T[] other)
   {
      using Tensor<T> row = this.Create1D(other);
      
      return CreateFromTensor(torch.row_stack(Storage, row.Storage), doNotCast: true);
   }

   #endregion


   public override bool Equals(object? obj) => obj is Tensor<T> objT && Equals(objT);

   /// <summary>
   /// True if all elements and shapes are equal
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public bool Equals(Tensor<T>? other)
   {
      if(other == null || !ShapesEqual(other))
         return false;

      using Tensor match = Storage.eq(other.Storage);
      using Tensor result = match.all();
      return result.ToBoolean();
   }

   public override int GetHashCode()
   {
      return Storage.GetHashCode();
   }


   public bool ShapesEqual<S>(Tensor<S> other) where S:struct
   {
      return other.Storage.shape.SequenceEqual(other.Storage.shape);
   }

   /// <summary>
   /// Dispose actively clears memory. If not called, GC will take care of it in time
   /// </summary>
   public void Dispose()
   {
      Storage.Dispose();
      GC.SuppressFinalize(this);
   }

   public void WriteBytesToStream(Stream voxels) => Storage.WriteBytesToStream(voxels);
}
