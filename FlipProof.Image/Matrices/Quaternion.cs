using FlipProof.Base;
using System;

namespace FlipProof.Image.Matrices;

internal record Quaternion(double W, double X, double Y, double Z)
{

	public static Quaternion FromMatrixValues(Matrix4x4_Optimised<double> m) => FromMatrixValues(m.M11, m.M12, m.M13, m.M21, m.M22, m.M23, m.M31, m.M32, m.M33);

	public static Quaternion FromMatrixValues(double r11, double r12, double r13, double r21, double r22, double r23, double r31, double r32, double r33)
	{
		double q0 = (r11 + r22 + r33 + 1.0) / 4.0;
		double q1 = (r11 - r22 - r33 + 1.0) / 4.0;
		double q2 = (0.0 - r11 + r22 - r33 + 1.0) / 4.0;
		double q3 = (0.0 - r11 - r22 + r33 + 1.0) / 4.0;
		if (q0 < double.Epsilon)
		{
			q0 = 0.0;
		}
		if (q1 < double.Epsilon)
		{
			q1 = 0.0;
		}
		if (q2 < double.Epsilon)
		{
			q2 = 0.0;
		}
		if (q3 < double.Epsilon)
		{
			q3 = 0.0;
		}
		q0 = Math.Sqrt(q0);
		q1 = Math.Sqrt(q1);
		q2 = Math.Sqrt(q2);
		q3 = Math.Sqrt(q3);
		if (q0 >= q1 && q0 >= q2 && q0 >= q3)
		{
			q0 *= 1.0;
			q1 *= Math.Sign(r32 - r23);
			q2 *= Math.Sign(r13 - r31);
			q3 *= Math.Sign(r21 - r12);
		}
		else if (q1 >= q0 && q1 >= q2 && q1 >= q3)
		{
			q0 *= Math.Sign(r32 - r23);
			q1 *= 1.0;
			q2 *= Math.Sign(r21 + r12);
			q3 *= Math.Sign(r13 + r31);
		}
		else if (q2 >= q0 && q2 >= q1 && q2 >= q3)
		{
			q0 *= Math.Sign(r13 - r31);
			q1 *= Math.Sign(r21 + r12);
			q2 *= 1.0;
			q3 *= Math.Sign(r32 + r23);
		}
		else
		{
			if (!(q3 >= q0) || !(q3 >= q1) || !(q3 >= q2))
			{
				throw new Exception();
			}
			q0 *= Math.Sign(r21 - r12);
			q1 *= Math.Sign(r31 + r13);
			q2 *= Math.Sign(r32 + r23);
			q3 *= 1.0;
		}
		return new Quaternion(q0, q1, q2, q3).ToNormalised();
	}

	public static Quaternion FromAxisAngle_Normalised(XYZ<double> axes, double angle)
	{
		return FromAxisAngle_Normalised(axes.X, axes.Y, axes.Z, angle);
	}

	public static Quaternion FromAxisAngle_Normalised(double axisX, double axisY, double axisZ, double angle)
	{
		double s = Math.Sin(angle / 2.0);
		return new Quaternion
		(
			Math.Cos(angle / 2.0),
			axisX * s,
			axisY * s,
			axisZ * s
		);
	}

	public void ToAxisAngle(out XYZ<double> axes, out double angle)
	{
		Quaternion q = this;
		if (W > 1.0)
		{
			q = ToNormalised();
		}
		angle = 2.0 * Math.Acos(q.W);
		double s = Math.Sqrt(1.0 - q.W * q.W);
		if (s < 1E-05)
		{
			axes = new XYZ<double>(q.X, q.Y, q.Z);
		}
		else
		{
			axes = new XYZ<double>(q.X / s, q.Y / s, q.Z / s);
		}
	}

	internal Matrix4x4_Optimised<double> ToMatrixD(bool trustAlreadyNormalised, XYZ<double> translate)
	{
		double sqw = W * W;
		double sqx = X * X;
		double sqy = Y * Y;
		double sqz = Z * Z;
		double m0;
		double m4;
		double m8;
		double m3;
		double m;
		double m6;
		double m2;
		double m7;
		double m5;
		if (trustAlreadyNormalised)
		{
			m0 = sqx - sqy - sqz + sqw;
			m4 = 0.0 - sqx + sqy - sqz + sqw;
			m8 = 0.0 - sqx - sqy + sqz + sqw;
			double tmp1 = X * Y;
			double tmp3 = Z * W;
			m3 = 2.0 * (tmp1 + tmp3);
			m = 2.0 * (tmp1 - tmp3);
			tmp1 = X * Z;
			tmp3 = Y * W;
			m6 = 2.0 * (tmp1 - tmp3);
			m2 = 2.0 * (tmp1 + tmp3);
			tmp1 = Y * Z;
			tmp3 = X * W;
			m7 = 2.0 * (tmp1 + tmp3);
			m5 = 2.0 * (tmp1 - tmp3);
		}
		else
		{
			double invs = 1.0 / (sqx + sqy + sqz + sqw);
			m0 = (sqx - sqy - sqz + sqw) * invs;
			m4 = (0.0 - sqx + sqy - sqz + sqw) * invs;
			m8 = (0.0 - sqx - sqy + sqz + sqw) * invs;
			double tmp2 = X * Y;
			double tmp4 = Z * W;
			m3 = 2.0 * (tmp2 + tmp4) * invs;
			m = 2.0 * (tmp2 - tmp4) * invs;
			tmp2 = X * Z;
			tmp4 = Y * W;
			m6 = 2.0 * (tmp2 - tmp4) * invs;
			m2 = 2.0 * (tmp2 + tmp4) * invs;
			tmp2 = Y * Z;
			tmp4 = X * W;
			m7 = 2.0 * (tmp2 + tmp4) * invs;
			m5 = 2.0 * (tmp2 - tmp4) * invs;
		}
		return new Matrix4x4_Optimised<double>
		{
			M11 = m0,
			M12 = m,
			M13 = m2,
			M14 = translate.X,
			M21 = m3,
			M22 = m4,
			M23 = m5,
			M24 = translate.Y,
			M31 = m6,
			M32 = m7,
			M33 = m8,
			M34 = translate.Z,
			M44 = 1.0
		};
	}

	public Quaternion ToNormalised()
	{
		Quaternion quat = DeepClone();
		double qmagsq = quat.GetSquareMagnitude();
		double factor = Math.Abs(1.0 - qmagsq) < 2.107342E-08 ? 2.0 / (1.0 + qmagsq) : 1.0 / (qmagsq * qmagsq);
		return quat.Scale(factor);
	}

	public double GetMagnitude()
	{
		return Math.Sqrt(GetSquareMagnitude());
	}

	public double GetSquareMagnitude() => W * W + X * X + Y * Y + Z * Z;

	public Quaternion Scale(double a)
	{
		return new(W * a, X * a, Y * a, Z * a);
	}

	public Quaternion DeepClone() => new Quaternion(W, X, Y, Z);

	public override string ToString() => "Real: " + W + " Imaginary: (" + X + ",\t" + Y + ",\t" + Z + ")";

	public static bool TryMatrixToQuaternionValues(DenseMatrix<double> m, out double q0, out double q1, out double q2, out double q3)
	{
		double m2 = m[0, 0];
		double r12 = m[0, 1];
		double r13 = m[0, 2];
		double r14 = m[1, 0];
		double r15 = m[1, 1];
		double r16 = m[1, 2];
		double r17 = m[2, 0];
		double r18 = m[2, 1];
		double r19 = m[2, 2];
		return TryMatrixToQuaternionValues(m2, r12, r13, r14, r15, r16, r17, r18, r19, out q0, out q1, out q2, out q3);
	}

	public static bool TryMatrixToQuaternionValues(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22, out double q0, out double q1, out double q2, out double q3)
	{
		q0 = Math.Sqrt(Math.Max(0.0, 1.0 + m00 + m11 + m22)) / 2.0;
		q1 = Math.Sqrt(Math.Max(0.0, 1.0 + m00 - m11 - m22)) / 2.0;
		q2 = Math.Sqrt(Math.Max(0.0, 1.0 - m00 + m11 - m22)) / 2.0;
		q3 = Math.Sqrt(Math.Max(0.0, 1.0 - m00 - m11 + m22)) / 2.0;
		q1 = CopySign(q1, m21 - m12);
		q2 = CopySign(q2, m02 - m20);
		q3 = CopySign(q3, m10 - m01);

		return !(double.IsNaN(q0) || double.IsNaN(q1) || double.IsNaN(q2));


      static double CopySign(double num, double sign)
		{
			return sign == 0.0 ? num : num * (double)Math.Sign(sign);
		}
	}
}
