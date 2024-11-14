using FlipProof.Base;
using System.Runtime.CompilerServices;

namespace FlipProof.Image.Matrices;

internal static class Matrix4x4_Optimised_ExtensionMethods
{
	public static Matrix4x4_Optimised<double> InterpolateRotationMatrix(this Matrix4x4_Optimised<double> mat, double factor)
	{
		Quaternion.FromMatrixValues(mat).ToAxisAngle(out var axes, out var angle);
		return Quaternion.FromAxisAngle_Normalised(axes, angle * factor).ToMatrixD(trustAlreadyNormalised: true, new XYZ<double>(mat.M0_3 * factor, mat.M1_3 * factor, mat.M2_3 * factor));
	}

	public static Matrix4x4_Optimised<float> ToFloat(this Matrix4x4_Optimised<double> m)
	{
		return new Matrix4x4_Optimised<float>
		{
			M0_0 = (float)m.M0_0,
			M0_1 = (float)m.M0_1,
			M0_2 = (float)m.M0_2,
			M0_3 = (float)m.M0_3,
			M1_0 = (float)m.M1_0,
			M1_1 = (float)m.M1_1,
			M1_2 = (float)m.M1_2,
			M1_3 = (float)m.M1_3,
			M2_0 = (float)m.M2_0,
			M2_1 = (float)m.M2_1,
			M2_2 = (float)m.M2_2,
			M2_3 = (float)m.M2_3,
			M3_0 = (float)m.M3_0,
			M3_1 = (float)m.M3_1,
			M3_2 = (float)m.M3_2,
			M3_3 = (float)m.M3_3
		};
	}
}
