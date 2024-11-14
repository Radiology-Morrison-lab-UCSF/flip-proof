using FlipProof.Base;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace FlipProof.Image.Matrices;

/// <summary>
/// A mutable 4x4 matrix. Consider instead using <see cref="System.Numerics.Matrix4x4"/> if simple operations are only needed
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class Matrix4x4_Optimised<T> where T:struct,IFloatingPoint<T>
{
	public T _0_0;

	public T _0_1;

	public T _0_2;

	public T _0_3;

	public T _1_0;

	public T _1_1;

	public T _1_2;

	public T _1_3;

	public T _2_0;

	public T _2_1;

	public T _2_2;

	public T _2_3;

	public T _3_0;

	public T _3_1;

	public T _3_2;

	public T _3_3;

	public Matrix4x4_Optimised()
	{
	}

	public Matrix4x4_Optimised(DenseMatrix<T> m)
	{
		if (m.NoRows > 4 || m.NoCols > 4)
		{
			throw new ArgumentException("Matrix too large");
		}
		if (m.NoRows < 3 || m.NoCols < 3)
		{
			throw new NotSupportedException("Matrix too small. 3x3, 3x3 and 4x4 supported only");
		}
		_0_0 = m[0, 0];
		_0_1 = m[0, 1];
		_0_2 = m[0, 2];
		_1_0 = m[1, 0];
		_1_1 = m[1, 1];
		_1_2 = m[1, 2];
		_2_0 = m[2, 0];
		_2_1 = m[2, 1];
		_2_2 = m[2, 2];
		if (m.NoRows > 3)
		{
			_3_0 = m[3, 0];
			_3_1 = m[3, 1];
			_3_2 = m[3, 2];
		}
		if (m.NoCols > 3)
		{
			_0_3 = m[0, 3];
			_1_3 = m[1, 3];
			_2_3 = m[2, 3];
			if (m.NoRows > 3)
			{
				_3_3 = m[3, 3];
			}
		}
	}
   public Matrix4x4_Optimised(T[] columnwiseArray)
	{
		SetFromColumnwiseArray(columnwiseArray);
	}

   public static Matrix4x4_Optimised<T> FromTextFile(string loc_fsMatrix)
   {
      Matrix4x4_Optimised<T> i = new Matrix4x4_Optimised<T>();
      T[] rowWiseArr = (from a in File.ReadAllText(loc_fsMatrix).Split(new char[4] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                             select T.Parse(a,CultureInfo.InvariantCulture)).ToArray();
      i.SetFromRowwiseArray(rowWiseArr);
      return i;
   }


   public void SaveToTextFile(string loc_fsMatrix)
	{
		File.WriteAllText(loc_fsMatrix, ToString());
	}

	public void SetFromColumnwiseArray(T[] columnwiseArray)
	{
		if (columnwiseArray.Length != 16)
		{
			throw new ArgumentException("Input array should have 16 elements");
		}
		_0_0 = columnwiseArray[0];
		_1_0 = columnwiseArray[1];
		_2_0 = columnwiseArray[2];
		_3_0 = columnwiseArray[3];
		_0_1 = columnwiseArray[4];
		_1_1 = columnwiseArray[5];
		_2_1 = columnwiseArray[6];
		_3_1 = columnwiseArray[7];
		_0_2 = columnwiseArray[8];
		_1_2 = columnwiseArray[9];
		_2_2 = columnwiseArray[10];
		_3_2 = columnwiseArray[11];
		_0_3 = columnwiseArray[12];
		_1_3 = columnwiseArray[13];
		_2_3 = columnwiseArray[14];
		_3_3 = columnwiseArray[15];
	}

	public void SetFromRowwiseArray(T[] rowwiseArray)
	{
		if (rowwiseArray.Length != 16)
		{
			throw new ArgumentException("Input array should have 16 elements");
		}
		_0_0 = rowwiseArray[0];
		_1_0 = rowwiseArray[4];
		_2_0 = rowwiseArray[8];
		_3_0 = rowwiseArray[12];
		_0_1 = rowwiseArray[1];
		_1_1 = rowwiseArray[5];
		_2_1 = rowwiseArray[9];
		_3_1 = rowwiseArray[13];
		_0_2 = rowwiseArray[2];
		_1_2 = rowwiseArray[6];
		_2_2 = rowwiseArray[10];
		_3_2 = rowwiseArray[14];
		_0_3 = rowwiseArray[3];
		_1_3 = rowwiseArray[7];
		_2_3 = rowwiseArray[11];
		_3_3 = rowwiseArray[15];
	}

	public void TransposeInPlace()
	{
		T[] arr = ToRowWiseArray();
		SetFromColumnwiseArray(arr);
	}

	public T At(int rowNo, int colNo)
	{
		return (rowNo * 10 + colNo) switch
		{
			0 => _0_0, 
			1 => _0_1, 
			2 => _0_2, 
			3 => _0_3, 
			10 => _1_0, 
			11 => _1_1, 
			12 => _1_2, 
			13 => _1_3, 
			20 => _2_0, 
			21 => _2_1, 
			22 => _2_2, 
			23 => _2_3, 
			30 => _3_0, 
			31 => _3_1, 
			32 => _3_2, 
			33 => _3_3, 
			_ => throw new IndexOutOfRangeException(), 
		};
	}

	public void SetDiagonal(T _0, T _1, T _2, T _3)
	{
		_0_0 = _0;
		_1_1 = _1;
		_2_2 = _2;
		_3_3 = _3;
	}

	public void SetCol(int colNo, T[] col)
	{
		if (col.Length != 4)
		{
			throw new ArgumentException("Wrong size for col");
		}
		SetCol(colNo, col[0], col[1], col[2], col[3]);
	}

	public void SetCol(int colNo, XYZ<T> _012, T _3)
	{
		SetCol(colNo, _012.X, _012.Y, _012.Z, _3);
	}

	public void SetCol(int colNo, T _0, T _1, T _2, T _3)
	{
		switch (colNo)
		{
		default:
			throw new ArgumentOutOfRangeException("Col must be between 0 and 3 inclusive");
		case 0:
			_0_0 = _0;
			_1_0 = _1;
			_2_0 = _2;
			_3_0 = _3;
			break;
		case 1:
			_0_1 = _0;
			_1_1 = _1;
			_2_1 = _2;
			_3_1 = _3;
			break;
		case 2:
			_0_2 = _0;
			_1_2 = _1;
			_2_2 = _2;
			_3_2 = _3;
			break;
		case 3:
			_0_3 = _0;
			_1_3 = _1;
			_2_3 = _2;
			_3_3 = _3;
			break;
		}
	}

	public void SetRow(int rowNo, T[] row)
	{
		if (row.Length != 4)
		{
			throw new ArgumentException("Wrong size for row");
		}
		switch (rowNo)
		{
		default:
			throw new ArgumentOutOfRangeException("Row must be between 0 and 3");
		case 0:
			_0_0 = row[0];
			_0_1 = row[1];
			_0_2 = row[2];
			_0_3 = row[3];
			break;
		case 1:
			_1_0 = row[0];
			_1_1 = row[1];
			_1_2 = row[2];
			_1_3 = row[3];
			break;
		case 2:
			_2_0 = row[0];
			_2_1 = row[1];
			_2_2 = row[2];
			_2_3 = row[3];
			break;
		case 3:
			_3_0 = row[0];
			_3_1 = row[1];
			_3_2 = row[2];
			_3_3 = row[3];
			break;
		}
	}
	
	public XYZ<T> Multiply(XYZ<T> c)
	{
		return new XYZ<T>(c.X * _0_0 + c.Y * _0_1 + c.Z * _0_2 + _0_3, c.X * _1_0 + c.Y * _1_1 + c.Z * _1_2 + _1_3, c.X * _2_0 + c.Y * _2_1 + c.Z * _2_2 + _2_3);
	}

	public XYZ<T> Multiply(T x, T y, T z)
	{
		return new XYZ<T>(x * _0_0 + y * _0_1 + z * _0_2 + _0_3, x * _1_0 + y * _1_1 + z * _1_2 + _1_3, x * _2_0 + y * _2_1 + z * _2_2 + _2_3);
	}

	public Matrix4x4_Optimised<T> MultiplyByScalar(T s)
	{
		return new Matrix4x4_Optimised<T>
      {
			_0_0 = s * _0_0,
			_0_1 = s * _0_1,
			_0_2 = s * _0_2,
			_0_3 = s * _0_3,
			_1_0 = s * _1_0,
			_1_1 = s * _1_1,
			_1_2 = s * _1_2,
			_1_3 = s * _1_3,
			_2_0 = s * _2_0,
			_2_1 = s * _2_1,
			_2_2 = s * _2_2,
			_2_3 = s * _2_3,
			_3_0 = s * _3_0,
			_3_1 = s * _3_1,
			_3_2 = s * _3_2,
			_3_3 = s * _3_3
		};
	}


	public Matrix4x4_Optimised<T> DeepClone()
	{
		return MemberwiseClone() as Matrix4x4_Optimised<T>;
	}

	public T[] ToColumnWiseArray()
	{
		return new T[16]
		{
			_0_0, _1_0, _2_0, _3_0, _0_1, _1_1, _2_1, _3_1, _0_2, _1_2,
			_2_2, _3_2, _0_3, _1_3, _2_3, _3_3
		};
	}

	public T[] ToRowWiseArray()
	{
		return new T[16]
		{
			_0_0, _0_1, _0_2, _0_3, _1_0, _1_1, _1_2, _1_3, _2_0, _2_1,
			_2_2, _2_3, _3_0, _3_1, _3_2, _3_3
		};
	}

	public DenseMatrix<T> ToMatrix()
	{
		DenseMatrix<T> mat = new DenseMatrix<T>(4, 4);
		mat.SetColumn(0, [_0_0, _1_0, _2_0, _3_0]);
		mat.SetColumn(1, [_0_1, _1_1, _2_1, _3_1]);
		mat.SetColumn(2, [_0_2, _1_2, _2_2, _3_2]);
		mat.SetColumn(3, [_0_3, _1_3, _2_3, _3_3]);
		return mat;
	}

	internal Matrix4x4_Optimised<T> Multiply(DenseMatrix<T> trans)
	{
		Matrix4x4_Optimised<T> m2 = new(trans);
		return Multiply(m2);
	}

	public Matrix4x4_Optimised<T> Multiply(Matrix4x4_Optimised<T> right)
	{
		return new Matrix4x4_Optimised<T>
		{
			_0_0 = _0_0 * right._0_0 + _0_1 * right._1_0 + _0_2 * right._2_0 + _0_3 * right._3_0,
			_1_0 = _1_0 * right._0_0 + _1_1 * right._1_0 + _1_2 * right._2_0 + _1_3 * right._3_0,
			_2_0 = _2_0 * right._0_0 + _2_1 * right._1_0 + _2_2 * right._2_0 + _2_3 * right._3_0,
			_3_0 = _3_0 * right._0_0 + _3_1 * right._1_0 + _3_2 * right._2_0 + _3_3 * right._3_0,
			_0_1 = _0_0 * right._0_1 + _0_1 * right._1_1 + _0_2 * right._2_1 + _0_3 * right._3_1,
			_1_1 = _1_0 * right._0_1 + _1_1 * right._1_1 + _1_2 * right._2_1 + _1_3 * right._3_1,
			_2_1 = _2_0 * right._0_1 + _2_1 * right._1_1 + _2_2 * right._2_1 + _2_3 * right._3_1,
			_3_1 = _3_0 * right._0_1 + _3_1 * right._1_1 + _3_2 * right._2_1 + _3_3 * right._3_1,
			_0_2 = _0_0 * right._0_2 + _0_1 * right._1_2 + _0_2 * right._2_2 + _0_3 * right._3_2,
			_1_2 = _1_0 * right._0_2 + _1_1 * right._1_2 + _1_2 * right._2_2 + _1_3 * right._3_2,
			_2_2 = _2_0 * right._0_2 + _2_1 * right._1_2 + _2_2 * right._2_2 + _2_3 * right._3_2,
			_3_2 = _3_0 * right._0_2 + _3_1 * right._1_2 + _3_2 * right._2_2 + _3_3 * right._3_2,
			_0_3 = _0_0 * right._0_3 + _0_1 * right._1_3 + _0_2 * right._2_3 + _0_3 * right._3_3,
			_1_3 = _1_0 * right._0_3 + _1_1 * right._1_3 + _1_2 * right._2_3 + _1_3 * right._3_3,
			_2_3 = _2_0 * right._0_3 + _2_1 * right._1_3 + _2_2 * right._2_3 + _2_3 * right._3_3,
			_3_3 = _3_0 * right._0_3 + _3_1 * right._1_3 + _3_2 * right._2_3 + _3_3 * right._3_3
		};
	}

	public static Matrix4x4_Optimised<T> operator *(Matrix4x4_Optimised<T> left, Matrix4x4_Optimised<T> right)
	{
		return left.Multiply(right);
	}

	internal static Matrix4x4_Optimised<T> Diagonal(T val)
	{
		return new Matrix4x4_Optimised<T>
		{
			_0_0 = val,
			_1_1 = val,
			_2_2 = val,
			_3_3 = val
		};
	}

	internal static Matrix4x4_Optimised<T> Diagonal(T[] val)
	{
		if (val.Length != 4)
		{
			throw new ArgumentException("Array should have four items - one per diagonal position");
		}
		return new Matrix4x4_Optimised<T>
		{
			_0_0 = val[0],
			_1_1 = val[1],
			_2_2 = val[2],
			_3_3 = val[3]
		};
	}

	public bool Equals(Matrix4x4_Optimised<T> obj)
	{
		if (_0_0 == obj._0_0 && _0_1 == obj._0_1 && _0_2 == obj._0_2 && _0_3 == obj._0_3 && _1_0 == obj._1_0 && _1_1 == obj._1_1 && _1_2 == obj._1_2 && _1_3 == obj._1_3 && _2_0 == obj._2_0 && _2_1 == obj._2_1 && _2_2 == obj._2_2 && _2_3 == obj._2_3 && _3_0 == obj._3_0 && _3_1 == obj._3_1 && _3_2 == obj._3_2)
		{
			return _3_3 == obj._3_3;
		}
		return false;
	}

	public Matrix4x4_Optimised<T> Inverse()
	{
		if (Inverse(out var res))
		{
			return res;
		}
		throw new Exception("No inverse could be found");
	}

	public bool Inverse([NotNullWhen(true)] out Matrix4x4_Optimised<T>? invOut)
	{
		Matrix4x4_Optimised<T> inv = new ();
		inv._0_0 = _1_1 * _2_2 * _3_3 - _1_1 * _3_2 * _2_3 - _1_2 * _2_1 * _3_3 + _1_2 * _3_1 * _2_3 + _1_3 * _2_1 * _3_2 - _1_3 * _3_1 * _2_2;
		inv._0_1 = -_0_1 * _2_2 * _3_3 + _0_1 * _3_2 * _2_3 + _0_2 * _2_1 * _3_3 - _0_2 * _3_1 * _2_3 - _0_3 * _2_1 * _3_2 + _0_3 * _3_1 * _2_2;
		inv._0_2 = _0_1 * _1_2 * _3_3 - _0_1 * _3_2 * _1_3 - _0_2 * _1_1 * _3_3 + _0_2 * _3_1 * _1_3 + _0_3 * _1_1 * _3_2 - _0_3 * _3_1 * _1_2;
		inv._0_3 = -_0_1 * _1_2 * _2_3 + _0_1 * _2_2 * _1_3 + _0_2 * _1_1 * _2_3 - _0_2 * _2_1 * _1_3 - _0_3 * _1_1 * _2_2 + _0_3 * _2_1 * _1_2;
		inv._1_0 = -_1_0 * _2_2 * _3_3 + _1_0 * _3_2 * _2_3 + _1_2 * _2_0 * _3_3 - _1_2 * _3_0 * _2_3 - _1_3 * _2_0 * _3_2 + _1_3 * _3_0 * _2_2;
		inv._1_1 = _0_0 * _2_2 * _3_3 - _0_0 * _3_2 * _2_3 - _0_2 * _2_0 * _3_3 + _0_2 * _3_0 * _2_3 + _0_3 * _2_0 * _3_2 - _0_3 * _3_0 * _2_2;
		inv._1_2 = -_0_0 * _1_2 * _3_3 + _0_0 * _3_2 * _1_3 + _0_2 * _1_0 * _3_3 - _0_2 * _3_0 * _1_3 - _0_3 * _1_0 * _3_2 + _0_3 * _3_0 * _1_2;
		inv._1_3 = _0_0 * _1_2 * _2_3 - _0_0 * _2_2 * _1_3 - _0_2 * _1_0 * _2_3 + _0_2 * _2_0 * _1_3 + _0_3 * _1_0 * _2_2 - _0_3 * _2_0 * _1_2;
		inv._2_0 = _1_0 * _2_1 * _3_3 - _1_0 * _3_1 * _2_3 - _1_1 * _2_0 * _3_3 + _1_1 * _3_0 * _2_3 + _1_3 * _2_0 * _3_1 - _1_3 * _3_0 * _2_1;
		inv._2_1 = -_0_0 * _2_1 * _3_3 + _0_0 * _3_1 * _2_3 + _0_1 * _2_0 * _3_3 - _0_1 * _3_0 * _2_3 - _0_3 * _2_0 * _3_1 + _0_3 * _3_0 * _2_1;
		inv._2_2 = _0_0 * _1_1 * _3_3 - _0_0 * _3_1 * _1_3 - _0_1 * _1_0 * _3_3 + _0_1 * _3_0 * _1_3 + _0_3 * _1_0 * _3_1 - _0_3 * _3_0 * _1_1;
		inv._2_3 = -_0_0 * _1_1 * _2_3 + _0_0 * _2_1 * _1_3 + _0_1 * _1_0 * _2_3 - _0_1 * _2_0 * _1_3 - _0_3 * _1_0 * _2_1 + _0_3 * _2_0 * _1_1;
		inv._3_0 = -_1_0 * _2_1 * _3_2 + _1_0 * _3_1 * _2_2 + _1_1 * _2_0 * _3_2 - _1_1 * _3_0 * _2_2 - _1_2 * _2_0 * _3_1 + _1_2 * _3_0 * _2_1;
		inv._3_1 = _0_0 * _2_1 * _3_2 - _0_0 * _3_1 * _2_2 - _0_1 * _2_0 * _3_2 + _0_1 * _3_0 * _2_2 + _0_2 * _2_0 * _3_1 - _0_2 * _3_0 * _2_1;
		inv._3_2 = -_0_0 * _1_1 * _3_2 + _0_0 * _3_1 * _1_2 + _0_1 * _1_0 * _3_2 - _0_1 * _3_0 * _1_2 - _0_2 * _1_0 * _3_1 + _0_2 * _3_0 * _1_1;
		inv._3_3 = _0_0 * _1_1 * _2_2 - _0_0 * _2_1 * _1_2 - _0_1 * _1_0 * _2_2 + _0_1 * _2_0 * _1_2 + _0_2 * _1_0 * _2_1 - _0_2 * _2_0 * _1_1;
		T det = _0_0 * inv._0_0 + _1_0 * inv._0_1 + _2_0 * inv._0_2 + _3_0 * inv._0_3;
		if (det == T.Zero)
		{
			invOut = null;
			return false;
		}
		det = T.One / det;
		invOut = inv.MultiplyByScalar(det);
		return true;
	}

	public void DecomposeRotationMatrix(out XYZ<double> rotationAxes, out double rotationRads, out XYZ<double> translations, out XYZ<double> scale)
	{
		DecomposeRotationMatrix(out translations, out scale, out var q);
		q.ToAxisAngle(out rotationAxes, out rotationRads);
	}

	public void DecomposeRotationMatrix(out XYZ<double> translations, out XYZ<double> scale, out Quaternion q)
	{
		DecomposeRotationMatrix(ToDouble(), out translations, out scale, out q);
	}

   public Matrix4x4_Optimised<double> ToDouble() => new Matrix4x4_Optimised<double>(this.ToMatrix().Cast<double>());

   static void DecomposeRotationMatrix(Matrix4x4_Optimised<double> me, out XYZ<double> translations, out XYZ<double> scale, out Quaternion q)
   {
      Matrix4x4_Optimised<double> rotMat = me.DeepClone();
      translations = new XYZ<double>(me._0_3, me._1_3, me._2_3);
      double scaleX = new XYZ<double>(me._0_0, me._1_0, me._2_0).LengthFrom000();
      double scaleY = new XYZ<double>(me._0_1, me._1_1, me._2_1).LengthFrom000();
      double scaleZ = new XYZ<double>(me._0_2, me._1_2, me._2_2).LengthFrom000();
      scale = new XYZ<double>(scaleX, scaleY, scaleZ);
      rotMat._0_3 = 0d;
      rotMat._1_3 = 0d;
      rotMat._2_3 = 0d;
      q = Quaternion.FromMatrixValues(rotMat._0_0 / scaleX, rotMat._0_1 / scaleY, rotMat._0_2 / scaleZ,
                                      rotMat._1_0 / scaleX, rotMat._1_1 / scaleY, rotMat._1_2 / scaleZ,
                                      rotMat._2_0 / scaleX, rotMat._2_1 / scaleY, rotMat._2_2 / scaleZ);
   }
   public override string ToString()
	{
		return string.Concat(_0_0, "\t", _0_1, "\t", _0_2, "\t", _0_3, "\r\n",
									_1_0, "\t", _1_1, "\t", _1_2, "\t", _1_3, "\r\n", 
									_2_0, "\t", _2_1, "\t", _2_2, "\t", _2_3, "\r\n", 
									_3_0, "\t", _3_1, "\t", _3_2, "\t", _3_3);
	}


}
