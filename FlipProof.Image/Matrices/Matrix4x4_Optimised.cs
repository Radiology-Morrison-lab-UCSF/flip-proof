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
	public T M0_0;

	public T M0_1;

	public T M0_2;

	public T M0_3;

	public T M1_0;

	public T M1_1;

	public T M1_2;

	public T M1_3;

	public T M2_0;

	public T M2_1;

	public T M2_2;

	public T M2_3;

	public T M3_0;

	public T M3_1;

	public T M3_2;

	public T M3_3;

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
		M0_0 = m[0, 0];
		M0_1 = m[0, 1];
		M0_2 = m[0, 2];
		M1_0 = m[1, 0];
		M1_1 = m[1, 1];
		M1_2 = m[1, 2];
		M2_0 = m[2, 0];
		M2_1 = m[2, 1];
		M2_2 = m[2, 2];
		if (m.NoRows > 3)
		{
			M3_0 = m[3, 0];
			M3_1 = m[3, 1];
			M3_2 = m[3, 2];
		}
		if (m.NoCols > 3)
		{
			M0_3 = m[0, 3];
			M1_3 = m[1, 3];
			M2_3 = m[2, 3];
			if (m.NoRows > 3)
			{
				M3_3 = m[3, 3];
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
		M0_0 = columnwiseArray[0];
		M1_0 = columnwiseArray[1];
		M2_0 = columnwiseArray[2];
		M3_0 = columnwiseArray[3];
		M0_1 = columnwiseArray[4];
		M1_1 = columnwiseArray[5];
		M2_1 = columnwiseArray[6];
		M3_1 = columnwiseArray[7];
		M0_2 = columnwiseArray[8];
		M1_2 = columnwiseArray[9];
		M2_2 = columnwiseArray[10];
		M3_2 = columnwiseArray[11];
		M0_3 = columnwiseArray[12];
		M1_3 = columnwiseArray[13];
		M2_3 = columnwiseArray[14];
		M3_3 = columnwiseArray[15];
	}

	public void SetFromRowwiseArray(T[] rowwiseArray)
	{
		if (rowwiseArray.Length != 16)
		{
			throw new ArgumentException("Input array should have 16 elements");
		}
		M0_0 = rowwiseArray[0];
		M1_0 = rowwiseArray[4];
		M2_0 = rowwiseArray[8];
		M3_0 = rowwiseArray[12];
		M0_1 = rowwiseArray[1];
		M1_1 = rowwiseArray[5];
		M2_1 = rowwiseArray[9];
		M3_1 = rowwiseArray[13];
		M0_2 = rowwiseArray[2];
		M1_2 = rowwiseArray[6];
		M2_2 = rowwiseArray[10];
		M3_2 = rowwiseArray[14];
		M0_3 = rowwiseArray[3];
		M1_3 = rowwiseArray[7];
		M2_3 = rowwiseArray[11];
		M3_3 = rowwiseArray[15];
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
			0 => M0_0, 
			1 => M0_1, 
			2 => M0_2, 
			3 => M0_3, 
			10 => M1_0, 
			11 => M1_1, 
			12 => M1_2, 
			13 => M1_3, 
			20 => M2_0, 
			21 => M2_1, 
			22 => M2_2, 
			23 => M2_3, 
			30 => M3_0, 
			31 => M3_1, 
			32 => M3_2, 
			33 => M3_3, 
			_ => throw new IndexOutOfRangeException(), 
		};
	}

	public void SetDiagonal(T _0, T _1, T _2, T _3)
	{
		M0_0 = _0;
		M1_1 = _1;
		M2_2 = _2;
		M3_3 = _3;
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
			M0_0 = _0;
			M1_0 = _1;
			M2_0 = _2;
			M3_0 = _3;
			break;
		case 1:
			M0_1 = _0;
			M1_1 = _1;
			M2_1 = _2;
			M3_1 = _3;
			break;
		case 2:
			M0_2 = _0;
			M1_2 = _1;
			M2_2 = _2;
			M3_2 = _3;
			break;
		case 3:
			M0_3 = _0;
			M1_3 = _1;
			M2_3 = _2;
			M3_3 = _3;
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
			M0_0 = row[0];
			M0_1 = row[1];
			M0_2 = row[2];
			M0_3 = row[3];
			break;
		case 1:
			M1_0 = row[0];
			M1_1 = row[1];
			M1_2 = row[2];
			M1_3 = row[3];
			break;
		case 2:
			M2_0 = row[0];
			M2_1 = row[1];
			M2_2 = row[2];
			M2_3 = row[3];
			break;
		case 3:
			M3_0 = row[0];
			M3_1 = row[1];
			M3_2 = row[2];
			M3_3 = row[3];
			break;
		}
	}
	
	public XYZ<T> Multiply(XYZ<T> c)
	{
		return new XYZ<T>(c.X * M0_0 + c.Y * M0_1 + c.Z * M0_2 + M0_3, c.X * M1_0 + c.Y * M1_1 + c.Z * M1_2 + M1_3, c.X * M2_0 + c.Y * M2_1 + c.Z * M2_2 + M2_3);
	}

	public XYZ<T> Multiply(T x, T y, T z)
	{
		return new XYZ<T>(x * M0_0 + y * M0_1 + z * M0_2 + M0_3, x * M1_0 + y * M1_1 + z * M1_2 + M1_3, x * M2_0 + y * M2_1 + z * M2_2 + M2_3);
	}

	public Matrix4x4_Optimised<T> MultiplyByScalar(T s)
	{
		return new Matrix4x4_Optimised<T>
      {
			M0_0 = s * M0_0,
			M0_1 = s * M0_1,
			M0_2 = s * M0_2,
			M0_3 = s * M0_3,
			M1_0 = s * M1_0,
			M1_1 = s * M1_1,
			M1_2 = s * M1_2,
			M1_3 = s * M1_3,
			M2_0 = s * M2_0,
			M2_1 = s * M2_1,
			M2_2 = s * M2_2,
			M2_3 = s * M2_3,
			M3_0 = s * M3_0,
			M3_1 = s * M3_1,
			M3_2 = s * M3_2,
			M3_3 = s * M3_3
		};
	}


   public Matrix4x4_Optimised<T> DeepClone() => (Matrix4x4_Optimised<T>)MemberwiseClone();

   public T[] ToColumnWiseArray()
	{
		return new T[16]
		{
			M0_0, M1_0, M2_0, M3_0, M0_1, M1_1, M2_1, M3_1, M0_2, M1_2,
			M2_2, M3_2, M0_3, M1_3, M2_3, M3_3
		};
	}

	public T[] ToRowWiseArray()
	{
		return new T[16]
		{
			M0_0, M0_1, M0_2, M0_3, M1_0, M1_1, M1_2, M1_3, M2_0, M2_1,
			M2_2, M2_3, M3_0, M3_1, M3_2, M3_3
		};
	}

	public DenseMatrix<T> ToMatrix()
	{
		DenseMatrix<T> mat = new DenseMatrix<T>(4, 4);
		mat.SetColumn(0, [M0_0, M1_0, M2_0, M3_0]);
		mat.SetColumn(1, [M0_1, M1_1, M2_1, M3_1]);
		mat.SetColumn(2, [M0_2, M1_2, M2_2, M3_2]);
		mat.SetColumn(3, [M0_3, M1_3, M2_3, M3_3]);
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
			M0_0 = M0_0 * right.M0_0 + M0_1 * right.M1_0 + M0_2 * right.M2_0 + M0_3 * right.M3_0,
			M1_0 = M1_0 * right.M0_0 + M1_1 * right.M1_0 + M1_2 * right.M2_0 + M1_3 * right.M3_0,
			M2_0 = M2_0 * right.M0_0 + M2_1 * right.M1_0 + M2_2 * right.M2_0 + M2_3 * right.M3_0,
			M3_0 = M3_0 * right.M0_0 + M3_1 * right.M1_0 + M3_2 * right.M2_0 + M3_3 * right.M3_0,
			M0_1 = M0_0 * right.M0_1 + M0_1 * right.M1_1 + M0_2 * right.M2_1 + M0_3 * right.M3_1,
			M1_1 = M1_0 * right.M0_1 + M1_1 * right.M1_1 + M1_2 * right.M2_1 + M1_3 * right.M3_1,
			M2_1 = M2_0 * right.M0_1 + M2_1 * right.M1_1 + M2_2 * right.M2_1 + M2_3 * right.M3_1,
			M3_1 = M3_0 * right.M0_1 + M3_1 * right.M1_1 + M3_2 * right.M2_1 + M3_3 * right.M3_1,
			M0_2 = M0_0 * right.M0_2 + M0_1 * right.M1_2 + M0_2 * right.M2_2 + M0_3 * right.M3_2,
			M1_2 = M1_0 * right.M0_2 + M1_1 * right.M1_2 + M1_2 * right.M2_2 + M1_3 * right.M3_2,
			M2_2 = M2_0 * right.M0_2 + M2_1 * right.M1_2 + M2_2 * right.M2_2 + M2_3 * right.M3_2,
			M3_2 = M3_0 * right.M0_2 + M3_1 * right.M1_2 + M3_2 * right.M2_2 + M3_3 * right.M3_2,
			M0_3 = M0_0 * right.M0_3 + M0_1 * right.M1_3 + M0_2 * right.M2_3 + M0_3 * right.M3_3,
			M1_3 = M1_0 * right.M0_3 + M1_1 * right.M1_3 + M1_2 * right.M2_3 + M1_3 * right.M3_3,
			M2_3 = M2_0 * right.M0_3 + M2_1 * right.M1_3 + M2_2 * right.M2_3 + M2_3 * right.M3_3,
			M3_3 = M3_0 * right.M0_3 + M3_1 * right.M1_3 + M3_2 * right.M2_3 + M3_3 * right.M3_3
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
			M0_0 = val,
			M1_1 = val,
			M2_2 = val,
			M3_3 = val
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
			M0_0 = val[0],
			M1_1 = val[1],
			M2_2 = val[2],
			M3_3 = val[3]
		};
	}

	public bool Equals(Matrix4x4_Optimised<T> obj)
	{
		if (M0_0 == obj.M0_0 && M0_1 == obj.M0_1 && M0_2 == obj.M0_2 && M0_3 == obj.M0_3 && M1_0 == obj.M1_0 && M1_1 == obj.M1_1 && M1_2 == obj.M1_2 && M1_3 == obj.M1_3 && M2_0 == obj.M2_0 && M2_1 == obj.M2_1 && M2_2 == obj.M2_2 && M2_3 == obj.M2_3 && M3_0 == obj.M3_0 && M3_1 == obj.M3_1 && M3_2 == obj.M3_2)
		{
			return M3_3 == obj.M3_3;
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
		inv.M0_0 = M1_1 * M2_2 * M3_3 - M1_1 * M3_2 * M2_3 - M1_2 * M2_1 * M3_3 + M1_2 * M3_1 * M2_3 + M1_3 * M2_1 * M3_2 - M1_3 * M3_1 * M2_2;
		inv.M0_1 = -M0_1 * M2_2 * M3_3 + M0_1 * M3_2 * M2_3 + M0_2 * M2_1 * M3_3 - M0_2 * M3_1 * M2_3 - M0_3 * M2_1 * M3_2 + M0_3 * M3_1 * M2_2;
		inv.M0_2 = M0_1 * M1_2 * M3_3 - M0_1 * M3_2 * M1_3 - M0_2 * M1_1 * M3_3 + M0_2 * M3_1 * M1_3 + M0_3 * M1_1 * M3_2 - M0_3 * M3_1 * M1_2;
		inv.M0_3 = -M0_1 * M1_2 * M2_3 + M0_1 * M2_2 * M1_3 + M0_2 * M1_1 * M2_3 - M0_2 * M2_1 * M1_3 - M0_3 * M1_1 * M2_2 + M0_3 * M2_1 * M1_2;
		inv.M1_0 = -M1_0 * M2_2 * M3_3 + M1_0 * M3_2 * M2_3 + M1_2 * M2_0 * M3_3 - M1_2 * M3_0 * M2_3 - M1_3 * M2_0 * M3_2 + M1_3 * M3_0 * M2_2;
		inv.M1_1 = M0_0 * M2_2 * M3_3 - M0_0 * M3_2 * M2_3 - M0_2 * M2_0 * M3_3 + M0_2 * M3_0 * M2_3 + M0_3 * M2_0 * M3_2 - M0_3 * M3_0 * M2_2;
		inv.M1_2 = -M0_0 * M1_2 * M3_3 + M0_0 * M3_2 * M1_3 + M0_2 * M1_0 * M3_3 - M0_2 * M3_0 * M1_3 - M0_3 * M1_0 * M3_2 + M0_3 * M3_0 * M1_2;
		inv.M1_3 = M0_0 * M1_2 * M2_3 - M0_0 * M2_2 * M1_3 - M0_2 * M1_0 * M2_3 + M0_2 * M2_0 * M1_3 + M0_3 * M1_0 * M2_2 - M0_3 * M2_0 * M1_2;
		inv.M2_0 = M1_0 * M2_1 * M3_3 - M1_0 * M3_1 * M2_3 - M1_1 * M2_0 * M3_3 + M1_1 * M3_0 * M2_3 + M1_3 * M2_0 * M3_1 - M1_3 * M3_0 * M2_1;
		inv.M2_1 = -M0_0 * M2_1 * M3_3 + M0_0 * M3_1 * M2_3 + M0_1 * M2_0 * M3_3 - M0_1 * M3_0 * M2_3 - M0_3 * M2_0 * M3_1 + M0_3 * M3_0 * M2_1;
		inv.M2_2 = M0_0 * M1_1 * M3_3 - M0_0 * M3_1 * M1_3 - M0_1 * M1_0 * M3_3 + M0_1 * M3_0 * M1_3 + M0_3 * M1_0 * M3_1 - M0_3 * M3_0 * M1_1;
		inv.M2_3 = -M0_0 * M1_1 * M2_3 + M0_0 * M2_1 * M1_3 + M0_1 * M1_0 * M2_3 - M0_1 * M2_0 * M1_3 - M0_3 * M1_0 * M2_1 + M0_3 * M2_0 * M1_1;
		inv.M3_0 = -M1_0 * M2_1 * M3_2 + M1_0 * M3_1 * M2_2 + M1_1 * M2_0 * M3_2 - M1_1 * M3_0 * M2_2 - M1_2 * M2_0 * M3_1 + M1_2 * M3_0 * M2_1;
		inv.M3_1 = M0_0 * M2_1 * M3_2 - M0_0 * M3_1 * M2_2 - M0_1 * M2_0 * M3_2 + M0_1 * M3_0 * M2_2 + M0_2 * M2_0 * M3_1 - M0_2 * M3_0 * M2_1;
		inv.M3_2 = -M0_0 * M1_1 * M3_2 + M0_0 * M3_1 * M1_2 + M0_1 * M1_0 * M3_2 - M0_1 * M3_0 * M1_2 - M0_2 * M1_0 * M3_1 + M0_2 * M3_0 * M1_1;
		inv.M3_3 = M0_0 * M1_1 * M2_2 - M0_0 * M2_1 * M1_2 - M0_1 * M1_0 * M2_2 + M0_1 * M2_0 * M1_2 + M0_2 * M1_0 * M2_1 - M0_2 * M2_0 * M1_1;
		T det = M0_0 * inv.M0_0 + M1_0 * inv.M0_1 + M2_0 * inv.M0_2 + M3_0 * inv.M0_3;
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
      translations = new XYZ<double>(me.M0_3, me.M1_3, me.M2_3);
      double scaleX = new XYZ<double>(me.M0_0, me.M1_0, me.M2_0).LengthFrom000();
      double scaleY = new XYZ<double>(me.M0_1, me.M1_1, me.M2_1).LengthFrom000();
      double scaleZ = new XYZ<double>(me.M0_2, me.M1_2, me.M2_2).LengthFrom000();
      scale = new XYZ<double>(scaleX, scaleY, scaleZ);
      rotMat.M0_3 = 0d;
      rotMat.M1_3 = 0d;
      rotMat.M2_3 = 0d;
      q = Quaternion.FromMatrixValues(rotMat.M0_0 / scaleX, rotMat.M0_1 / scaleY, rotMat.M0_2 / scaleZ,
                                      rotMat.M1_0 / scaleX, rotMat.M1_1 / scaleY, rotMat.M1_2 / scaleZ,
                                      rotMat.M2_0 / scaleX, rotMat.M2_1 / scaleY, rotMat.M2_2 / scaleZ);
   }
   public override string ToString()
	{
		return string.Concat(M0_0, "\t", M0_1, "\t", M0_2, "\t", M0_3, "\r\n",
									M1_0, "\t", M1_1, "\t", M1_2, "\t", M1_3, "\r\n", 
									M2_0, "\t", M2_1, "\t", M2_2, "\t", M2_3, "\r\n", 
									M3_0, "\t", M3_1, "\t", M3_2, "\t", M3_3);
	}


}
