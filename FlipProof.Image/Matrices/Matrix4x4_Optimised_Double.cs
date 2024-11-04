using FlipProof.Base;
using System.Runtime.CompilerServices;

namespace FlipProof.Image.Matrices;

internal static class Matrix4x4_Optimised_ExtensionMethods
{
	public static Matrix4x4_Optimised<double> InterpolateRotationMatrix(this Matrix4x4_Optimised<double> mat, double factor)
	{
		Quaternion.FromMatrixValues(mat).ToAxisAngle(out var axes, out var angle);
		return Quaternion.FromAxisAngle_Normalised(axes, angle * factor).ToMatrixD(trustAlreadyNormalised: true, new XYZ<double>(mat._0_3 * factor, mat._1_3 * factor, mat._2_3 * factor));
	}

	public static Matrix4x4_Optimised<float> ToFloat(this Matrix4x4_Optimised<double> m)
	{
		return new Matrix4x4_Optimised<float>
		{
			_0_0 = (float)m._0_0,
			_0_1 = (float)m._0_1,
			_0_2 = (float)m._0_2,
			_0_3 = (float)m._0_3,
			_1_0 = (float)m._1_0,
			_1_1 = (float)m._1_1,
			_1_2 = (float)m._1_2,
			_1_3 = (float)m._1_3,
			_2_0 = (float)m._2_0,
			_2_1 = (float)m._2_1,
			_2_2 = (float)m._2_2,
			_2_3 = (float)m._2_3,
			_3_0 = (float)m._3_0,
			_3_1 = (float)m._3_1,
			_3_2 = (float)m._3_2,
			_3_3 = (float)m._3_3
		};
	}
}
