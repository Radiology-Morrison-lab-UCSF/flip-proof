using FlipProof.Base;
using FlipProof.Image;
using FlipProof.Torch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using TorchSharp;
using static TorchSharp.torch;

namespace FlipProof.Image.Matrices;

public class DenseMatrix<T> where T:struct, IFloatingPoint<T>
{
	protected readonly Tensor<T> storage;

   public DenseMatrix(int rows, int cols, T diagonalVal = default)
   : this(Tensor<T>.CreateTensor(rows,cols))
   {
		if(diagonalVal != default)
		{
			SetDiagonal(diagonalVal);
		}
   }

   private DenseMatrix(Tensor<T> storage)
   {
      this.storage = storage;
		if(storage.Storage.Dimensions != 2)
		{
			throw new ArgumentException("Tensor not 2d");
		}
		if(storage.Storage.shape.Any(a=>a==0))
		{
			throw new ArgumentException("Matrix cannot have a size of 0 for any dimension");
		}
   }
	public long NoRows => storage.Storage.shape[0];
	public long NoCols => storage.Storage.shape[1];
   public T this[int row, int col]
   {
		get
      {
         return storage[row,col];
      }
      set
      {
         storage[row,col] = value;
      }
   }

   public DenseMatrix<T> this[Range row, Range col]
	{
		get
		{
			Tensor sliced =storage.Storage[TensorIndex.Slice(row.Start.GetOffset((int)NoRows), row.End.GetOffset((int)NoRows)),
													 TensorIndex.Slice(row.Start.GetOffset((int)NoCols), row.End.GetOffset((int)NoCols))];
			return new(Tensor<T>.CreateTensor(sliced, true));
		}
	}

	public DenseMatrix<S> Cast<S>() where S: struct, IFloatingPoint<S>
	{
		if(typeof(T).Equals(typeof(S)))
		{
			return (DeepClone() as DenseMatrix<S>)!;
		}
		using Tensor castData = storage.Storage.to_type(TypeConversions.GetScalarType<S>());
		Tensor<S> dest = Tensor<S>.CreateTensor(castData.shape);
		dest.Storage.copy_(castData);

		return new DenseMatrix<S>(dest);
   }


   /// <summary>
   /// Copies this into the top left corner of the argument provided. 
   /// </summary>
   /// <param name="destination">Where to copy to</param>
   /// <exception cref="ArgumentException">Destination is too small</exception>
   public void CopyTo(DenseMatrix<T> destination)
   {
      if (destination.NoRows < NoRows || destination.NoCols < NoCols)
      {
         throw new ArgumentException("Argument is too small");
      }

		destination.storage.Storage[TensorIndex.Slice(0, NoRows), TensorIndex.Slice(0, NoCols)] = this.storage.Storage;
   }
	public T[] GetRow(int row) => storage.Storage[row, TensorIndex.Colon].ToArray<T>();
	public T[] GetColumn(int column) => storage.Storage[TensorIndex.Colon, column].ToArray<T>();
   public void SetColumn(int column, T[] value)
	{
		if(value.Length != NoRows)
		{
			throw new ArgumentException($"Expected {NoRows} but got {value.Length}");
		}
		storage.Storage[TensorIndex.Colon,column] = storage.ArrayToTensor(value);
	}
	/// <summary>
	/// Calculates the inverse of a square matrix, if one exists
	/// </summary>
	/// <returns></returns>
	public DenseMatrix<T> Inverse() => new(Tensor<T>.CreateTensor(storage.Storage.inverse(), false));
	public double Trace() => storage.Storage.trace().ToDouble();
	public DenseMatrix<T> AppendRow(T[] row)
	{
		if(row.Length != NoCols)
		{
			throw new ArgumentException($"Expected {NoCols} but got {row.Length}");
		}
      return new DenseMatrix<T>(storage.RowStack(row));
   }

   public DenseMatrix<T> MatMul(DenseMatrix<T> right) => NewFromResultOfOperation(torch.matmul, right);
   public DenseMatrix<T> MultiplyPointwise(DenseMatrix<T> right) => NewFromResultOfOperation(torch.mul, right);

   private DenseMatrix<T> NewFromResultOfOperation(Func<Tensor, Tensor, Tensor> action, DenseMatrix<T> right) => NewFromResultOfOperation(action, right.storage.Storage);
   private DenseMatrix<T> NewFromResultOfOperation(Func<Tensor, Tensor, Tensor> action, Tensor<T> right) => NewFromResultOfOperation(action, right.Storage);
	private DenseMatrix<T> NewFromResultOfOperation(Func<Tensor, Tensor, Tensor> action, Tensor right)
	{
		Tensor result = action(storage.Storage, right);
		bool isInput = !(object.ReferenceEquals(result, storage.Storage) || ReferenceEquals(result, right));
      return new DenseMatrix<T>(Tensor<T>.CreateTensor(result, wrapCopy: isInput));
	}
	public void SetDiagonal(T diagonalVal)
	{
		using Tensor val = storage.CreateScalar(diagonalVal).Storage;

		long diagSize = Math.Min(NoRows, NoCols);
		for (int i = 0; i < diagSize; i++)
		{
			storage.Storage[i, i] = val;
		}
	}
   public void SetDiagonal(T[] diagonalVals)
   {
		long diagSize = Math.Min(NoRows, NoCols);
		if(diagonalVals.Length != diagSize)
		{
			throw new ArgumentOutOfRangeException($"Diagonal is size {diagSize} but {diagonalVals.Length} were provided");
		}
		using Tensor<T> setTo = storage.Create1D(diagonalVals);
		for (int i = 0; i < diagSize; i++)
		{
			storage.Storage[i, i] = setTo.Storage[i];
		}
   }

   internal DenseMatrix<T> DeepClone() => new(storage.DeepClone());

	public bool Equals(DenseMatrix<T> other, double tolerance)
   {
		using var absDiff = (storage.Storage - other.storage.Storage).abs_();
      return !absDiff.greater(tolerance).any().ToBoolean();
   }


   /// <summary>
   /// Matrix multiplication
   /// </summary>
   public static DenseMatrix<T> operator *(DenseMatrix<T> left, DenseMatrix<T> right) => left.MatMul(right);
	/// <summary>
	/// Matrix multiplication
	/// </summary>
	public static DenseMatrix<double> operator *(DenseMatrix<T> left, DenseMatrix<double> right) => left.Cast<double>().MatMul(right);
	public static DenseMatrix<T> operator *(DenseMatrix<T> left, XYZ<T> right) => left.NewFromResultOfOperation(torch.matmul, right.ToTensor());
	public static DenseMatrix<T> operator +(DenseMatrix<T> left, DenseMatrix<T> right) => left.NewFromResultOfOperation(torch.add, right);
	public static DenseMatrix<T> operator -(DenseMatrix<T> left, DenseMatrix<T> right) => left.NewFromResultOfOperation(torch.sub, right);
}
