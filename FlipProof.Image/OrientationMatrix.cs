using FlipProof.Base;
using FlipProof.Image.Matrices;
using System.Numerics;

namespace FlipProof.Image;

class OrientationMatrix(Matrix4x4_Optimised<double> matrix) : IReadOnlyOrientation
{
   readonly Matrix4x4_Optimised<double> _matrix = matrix;
   public XYZ<double> VoxelSize { get; init; } = CalcVoxelSize(matrix);
   public XYZ<double> Translation { get;init; } = new(matrix.M14, matrix.M24, matrix.M34);
   public static XYZ<double> CalcVoxelSize(in Matrix4x4_Optimised<double> _matrix)
   {
      // We use cols to see how far moving 1 in each direction in voxel space moves us in world space
      // As, for example, 'i' is multiplied by elements in the first col
      return new(Norm(_matrix.M11, _matrix.M21, _matrix.M31),
                 Norm(_matrix.M12, _matrix.M22, _matrix.M32),
                 Norm(_matrix.M13, _matrix.M23, _matrix.M33));

      static double Norm(double x, double y, double z) => Math.Sqrt(x * x + y * y + z * z);

   }


   Matrix4x4_Optimised<double> IReadOnlyOrientation.GetMatrix() => _matrix.DeepClone();

   public IReadOnlyOrientation GetTranslated(double offsetX, double offsetY, double offsetZ)
   {
      var newMatrix = _matrix.DeepClone();
      newMatrix.M14 += (float)offsetX;
      newMatrix.M24 += (float)offsetY;
      newMatrix.M34 += (float)offsetZ;
      return new OrientationMatrix(newMatrix);
   }

   public XYZ<double> VoxelToWorldCoordinate(double x, double y, double z)
   {
      return new XYZ<double>(
         _matrix.M11 * x + _matrix.M12 * y + _matrix.M13 * z + _matrix.M14,
         _matrix.M21 * x + _matrix.M22 * y + _matrix.M23 * z + _matrix.M24,
         _matrix.M31 * x + _matrix.M32 * y + _matrix.M33 * z + _matrix.M34
         );
   }

   bool IReadOnlyOrientation.TryGetNiftiQuaternions(out double quartern_b, out double quartern_c, out double quartern_d, out double[] pixDims, out double[] translations, out double qFace)
   {
      // not supported
      quartern_b = quartern_c = quartern_d = qFace = 0;
      pixDims = translations = Array.Empty<double>();
      return false;
          
   }
}
