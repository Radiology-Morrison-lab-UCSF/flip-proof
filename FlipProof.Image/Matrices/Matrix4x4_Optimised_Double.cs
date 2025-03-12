using FlipProof.Base;
using System.Runtime.CompilerServices;

namespace FlipProof.Image.Matrices;

internal static class Matrix4x4_Optimised_ExtensionMethods
{
	public static Matrix4x4_Optimised<double> InterpolateRotationMatrix(this Matrix4x4_Optimised<double> mat, double factor)
	{
		Quaternion.FromMatrixValues(mat).ToAxisAngle(out var axes, out var angle);
		return Quaternion.FromAxisAngle_Normalised(axes, angle * factor).ToMatrixD(trustAlreadyNormalised: true, new XYZ<double>(mat.M14 * factor, mat.M24 * factor, mat.M34 * factor));
	}

	public static Matrix4x4_Optimised<float> ToFloat(this Matrix4x4_Optimised<double> m)
	{
		return new Matrix4x4_Optimised<float>
		{
			M11 = (float)m.M11,
			M12 = (float)m.M12,
			M13 = (float)m.M13,
			M14 = (float)m.M14,
			M21 = (float)m.M21,
			M22 = (float)m.M22,
			M23 = (float)m.M23,
			M24 = (float)m.M24,
			M31 = (float)m.M31,
			M32 = (float)m.M32,
			M33 = (float)m.M33,
			M34 = (float)m.M34,
			M41 = (float)m.M41,
			M42 = (float)m.M42,
			M43 = (float)m.M43,
			M44 = (float)m.M44
		};
	}
}
