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
	public T M11;

	public T M12;

	public T M13;

	public T M14;

	public T M21;

	public T M22;

	public T M23;

	public T M24;

	public T M31;

	public T M32;

	public T M33;

	public T M34;

	public T M41;

	public T M42;

	public T M43;

	public T M44;

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
		M11 = m[0, 0];
		M12 = m[0, 1];
		M13 = m[0, 2];
		M21 = m[1, 0];
		M22 = m[1, 1];
		M23 = m[1, 2];
		M31 = m[2, 0];
		M32 = m[2, 1];
		M33 = m[2, 2];
		if (m.NoRows > 3)
		{
			M41 = m[3, 0];
			M42 = m[3, 1];
			M43 = m[3, 2];
		}
		if (m.NoCols > 3)
		{
			M14 = m[0, 3];
			M24 = m[1, 3];
			M34 = m[2, 3];
			if (m.NoRows > 3)
			{
				M44 = m[3, 3];
			}
		}
	}
   public Matrix4x4_Optimised(T[] columnwiseArray)
	{
		SetFromColumnwiseArray(columnwiseArray);
	}

   public Matrix4x4_Optimised(T m11, T m12, T m13, T m14, T m21, T m22, T m23, T m24, T m31, T m32, T m33, T m34, T m41, T m42, T m43, T m44)
   {
      M11 = m11;
      M12 = m12;
      M13 = m13;
      M14 = m14;
      M21 = m21;
      M22 = m22;
      M23 = m23;
      M24 = m24;
      M31 = m31;
      M32 = m32;
      M33 = m33;
      M34 = m34;
      M41 = m41;
      M42 = m42;
      M43 = m43;
      M44 = m44;
   }

   public static Matrix4x4_Optimised<T> FromTextFile(string loc_fsMatrix)
   {
      Matrix4x4_Optimised<T> i = new();
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
		M11 = columnwiseArray[0];
		M21 = columnwiseArray[1];
		M31 = columnwiseArray[2];
		M41 = columnwiseArray[3];
		M12 = columnwiseArray[4];
		M22 = columnwiseArray[5];
		M32 = columnwiseArray[6];
		M42 = columnwiseArray[7];
		M13 = columnwiseArray[8];
		M23 = columnwiseArray[9];
		M33 = columnwiseArray[10];
		M43 = columnwiseArray[11];
		M14 = columnwiseArray[12];
		M24 = columnwiseArray[13];
		M34 = columnwiseArray[14];
		M44 = columnwiseArray[15];
	}

	public void SetFromRowwiseArray(T[] rowwiseArray)
	{
		if (rowwiseArray.Length != 16)
		{
			throw new ArgumentException("Input array should have 16 elements");
		}
		M11 = rowwiseArray[0];
		M21 = rowwiseArray[4];
		M31 = rowwiseArray[8];
		M41 = rowwiseArray[12];
		M12 = rowwiseArray[1];
		M22 = rowwiseArray[5];
		M32 = rowwiseArray[9];
		M42 = rowwiseArray[13];
		M13 = rowwiseArray[2];
		M23 = rowwiseArray[6];
		M33 = rowwiseArray[10];
		M43 = rowwiseArray[14];
		M14 = rowwiseArray[3];
		M24 = rowwiseArray[7];
		M34 = rowwiseArray[11];
		M44 = rowwiseArray[15];
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
			0 => M11, 
			1 => M12, 
			2 => M13, 
			3 => M14, 
			10 => M21, 
			11 => M22, 
			12 => M23, 
			13 => M24, 
			20 => M31, 
			21 => M32, 
			22 => M33, 
			23 => M34, 
			30 => M41, 
			31 => M42, 
			32 => M43, 
			33 => M44, 
			_ => throw new IndexOutOfRangeException(), 
		};
	}

	public void SetDiagonal(T _0, T _1, T _2, T _3)
	{
		M11 = _0;
		M22 = _1;
		M33 = _2;
		M44 = _3;
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
			M11 = _0;
			M21 = _1;
			M31 = _2;
			M41 = _3;
			break;
		case 1:
			M12 = _0;
			M22 = _1;
			M32 = _2;
			M42 = _3;
			break;
		case 2:
			M13 = _0;
			M23 = _1;
			M33 = _2;
			M43 = _3;
			break;
		case 3:
			M14 = _0;
			M24 = _1;
			M34 = _2;
			M44 = _3;
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
			M11 = row[0];
			M12 = row[1];
			M13 = row[2];
			M14 = row[3];
			break;
		case 1:
			M21 = row[0];
			M22 = row[1];
			M23 = row[2];
			M24 = row[3];
			break;
		case 2:
			M31 = row[0];
			M32 = row[1];
			M33 = row[2];
			M34 = row[3];
			break;
		case 3:
			M41 = row[0];
			M42 = row[1];
			M43 = row[2];
			M44 = row[3];
			break;
		}
	}
	
	public XYZ<T> Multiply(XYZ<T> c)
	{
		return new XYZ<T>(c.X * M11 + c.Y * M12 + c.Z * M13 + M14, c.X * M21 + c.Y * M22 + c.Z * M23 + M24, c.X * M31 + c.Y * M32 + c.Z * M33 + M34);
	}

	public XYZ<T> Multiply(T x, T y, T z)
	{
		return new XYZ<T>(x * M11 + y * M12 + z * M13 + M14, x * M21 + y * M22 + z * M23 + M24, x * M31 + y * M32 + z * M33 + M34);
	}

	public Matrix4x4_Optimised<T> MultiplyByScalar(T s)
	{
		return new Matrix4x4_Optimised<T>
      {
			M11 = s * M11,
			M12 = s * M12,
			M13 = s * M13,
			M14 = s * M14,
			M21 = s * M21,
			M22 = s * M22,
			M23 = s * M23,
			M24 = s * M24,
			M31 = s * M31,
			M32 = s * M32,
			M33 = s * M33,
			M34 = s * M34,
			M41 = s * M41,
			M42 = s * M42,
			M43 = s * M43,
			M44 = s * M44
		};
	}


   public Matrix4x4_Optimised<T> DeepClone() => (Matrix4x4_Optimised<T>)MemberwiseClone();

   public T[] ToColumnWiseArray()
	{
		return
      [
         M11, M21, M31, M41, 
			M12, M22, M32, M42, 
			M13, M23, M33, M43, 
			M14, M24, M34, M44
		];
	}

	public T[] ToRowWiseArray()
	{
		return new T[16]
		{
			M11, M12, M13, M14, 
			M21, M22, M23, M24, 
			M31, M32, M33, M34, 
			M41, M42, M43, M44
		};
	}

	public DenseMatrix<T> ToMatrix()
	{
		DenseMatrix<T> mat = new(4, 4);
		mat.SetColumn(0, [M11, M21, M31, M41]);
		mat.SetColumn(1, [M12, M22, M32, M42]);
		mat.SetColumn(2, [M13, M23, M33, M43]);
		mat.SetColumn(3, [M14, M24, M34, M44]);
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
			M11 = M11 * right.M11 + M12 * right.M21 + M13 * right.M31 + M14 * right.M41,
			M21 = M21 * right.M11 + M22 * right.M21 + M23 * right.M31 + M24 * right.M41,
			M31 = M31 * right.M11 + M32 * right.M21 + M33 * right.M31 + M34 * right.M41,
			M41 = M41 * right.M11 + M42 * right.M21 + M43 * right.M31 + M44 * right.M41,
			M12 = M11 * right.M12 + M12 * right.M22 + M13 * right.M32 + M14 * right.M42,
			M22 = M21 * right.M12 + M22 * right.M22 + M23 * right.M32 + M24 * right.M42,
			M32 = M31 * right.M12 + M32 * right.M22 + M33 * right.M32 + M34 * right.M42,
			M42 = M41 * right.M12 + M42 * right.M22 + M43 * right.M32 + M44 * right.M42,
			M13 = M11 * right.M13 + M12 * right.M23 + M13 * right.M33 + M14 * right.M43,
			M23 = M21 * right.M13 + M22 * right.M23 + M23 * right.M33 + M24 * right.M43,
			M33 = M31 * right.M13 + M32 * right.M23 + M33 * right.M33 + M34 * right.M43,
			M43 = M41 * right.M13 + M42 * right.M23 + M43 * right.M33 + M44 * right.M43,
			M14 = M11 * right.M14 + M12 * right.M24 + M13 * right.M34 + M14 * right.M44,
			M24 = M21 * right.M14 + M22 * right.M24 + M23 * right.M34 + M24 * right.M44,
			M34 = M31 * right.M14 + M32 * right.M24 + M33 * right.M34 + M34 * right.M44,
			M44 = M41 * right.M14 + M42 * right.M24 + M43 * right.M34 + M44 * right.M44
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
			M11 = val,
			M22 = val,
			M33 = val,
			M44 = val
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
			M11 = val[0],
			M22 = val[1],
			M33 = val[2],
			M44 = val[3]
		};
	}

	public bool Equals(Matrix4x4_Optimised<T> obj)
	{
		if (M11 == obj.M11 && M12 == obj.M12 && M13 == obj.M13 && M14 == obj.M14 && M21 == obj.M21 && M22 == obj.M22 && M23 == obj.M23 && M24 == obj.M24 && M31 == obj.M31 && M32 == obj.M32 && M33 == obj.M33 && M34 == obj.M34 && M41 == obj.M41 && M42 == obj.M42 && M43 == obj.M43)
		{
			return M44 == obj.M44;
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
		inv.M11 = M22 * M33 * M44 - M22 * M43 * M34 - M23 * M32 * M44 + M23 * M42 * M34 + M24 * M32 * M43 - M24 * M42 * M33;
		inv.M12 = -M12 * M33 * M44 + M12 * M43 * M34 + M13 * M32 * M44 - M13 * M42 * M34 - M14 * M32 * M43 + M14 * M42 * M33;
		inv.M13 = M12 * M23 * M44 - M12 * M43 * M24 - M13 * M22 * M44 + M13 * M42 * M24 + M14 * M22 * M43 - M14 * M42 * M23;
		inv.M14 = -M12 * M23 * M34 + M12 * M33 * M24 + M13 * M22 * M34 - M13 * M32 * M24 - M14 * M22 * M33 + M14 * M32 * M23;
		inv.M21 = -M21 * M33 * M44 + M21 * M43 * M34 + M23 * M31 * M44 - M23 * M41 * M34 - M24 * M31 * M43 + M24 * M41 * M33;
		inv.M22 = M11 * M33 * M44 - M11 * M43 * M34 - M13 * M31 * M44 + M13 * M41 * M34 + M14 * M31 * M43 - M14 * M41 * M33;
		inv.M23 = -M11 * M23 * M44 + M11 * M43 * M24 + M13 * M21 * M44 - M13 * M41 * M24 - M14 * M21 * M43 + M14 * M41 * M23;
		inv.M24 = M11 * M23 * M34 - M11 * M33 * M24 - M13 * M21 * M34 + M13 * M31 * M24 + M14 * M21 * M33 - M14 * M31 * M23;
		inv.M31 = M21 * M32 * M44 - M21 * M42 * M34 - M22 * M31 * M44 + M22 * M41 * M34 + M24 * M31 * M42 - M24 * M41 * M32;
		inv.M32 = -M11 * M32 * M44 + M11 * M42 * M34 + M12 * M31 * M44 - M12 * M41 * M34 - M14 * M31 * M42 + M14 * M41 * M32;
		inv.M33 = M11 * M22 * M44 - M11 * M42 * M24 - M12 * M21 * M44 + M12 * M41 * M24 + M14 * M21 * M42 - M14 * M41 * M22;
		inv.M34 = -M11 * M22 * M34 + M11 * M32 * M24 + M12 * M21 * M34 - M12 * M31 * M24 - M14 * M21 * M32 + M14 * M31 * M22;
		inv.M41 = -M21 * M32 * M43 + M21 * M42 * M33 + M22 * M31 * M43 - M22 * M41 * M33 - M23 * M31 * M42 + M23 * M41 * M32;
		inv.M42 = M11 * M32 * M43 - M11 * M42 * M33 - M12 * M31 * M43 + M12 * M41 * M33 + M13 * M31 * M42 - M13 * M41 * M32;
		inv.M43 = -M11 * M22 * M43 + M11 * M42 * M23 + M12 * M21 * M43 - M12 * M41 * M23 - M13 * M21 * M42 + M13 * M41 * M22;
		inv.M44 = M11 * M22 * M33 - M11 * M32 * M23 - M12 * M21 * M33 + M12 * M31 * M23 + M13 * M21 * M32 - M13 * M31 * M22;
		T det = M11 * inv.M11 + M21 * inv.M12 + M31 * inv.M13 + M41 * inv.M14;
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

   internal void DecomposeRotationMatrix(out XYZ<double> translations, out XYZ<double> scale, out Quaternion q)
	{
		DecomposeRotationMatrix(ToDouble(), out translations, out scale, out q);
	}

   public Matrix4x4_Optimised<double> ToDouble() => new(this.ToMatrix().Cast<double>());

   static void DecomposeRotationMatrix(Matrix4x4_Optimised<double> me, out XYZ<double> translations, out XYZ<double> scale, out Quaternion q)
   {
      Matrix4x4_Optimised<double> rotMat = me.DeepClone();
      translations = new XYZ<double>(me.M14, me.M24, me.M34);
      double scaleX = new XYZ<double>(me.M11, me.M21, me.M31).Norm();
      double scaleY = new XYZ<double>(me.M12, me.M22, me.M32).Norm();
      double scaleZ = new XYZ<double>(me.M13, me.M23, me.M33).Norm();
      scale = new XYZ<double>(scaleX, scaleY, scaleZ);
      rotMat.M14 = 0d;
      rotMat.M24 = 0d;
      rotMat.M34 = 0d;
      q = Quaternion.FromMatrixValues(rotMat.M11 / scaleX, rotMat.M12 / scaleY, rotMat.M13 / scaleZ,
                                      rotMat.M21 / scaleX, rotMat.M22 / scaleY, rotMat.M23 / scaleZ,
                                      rotMat.M31 / scaleX, rotMat.M32 / scaleY, rotMat.M33 / scaleZ);
   }
   public override string ToString()
	{
		return string.Concat(M11, "\t", M12, "\t", M13, "\t", M14, "\r\n",
									M21, "\t", M22, "\t", M23, "\t", M24, "\r\n", 
									M31, "\t", M32, "\t", M33, "\t", M34, "\r\n", 
									M41, "\t", M42, "\t", M43, "\t", M44);
	}


}
