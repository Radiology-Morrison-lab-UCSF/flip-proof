using FlipProof.Base;
using System.Numerics;

namespace FlipProof.Image;

class OrientationMatrix(Matrix4x4 matrix) : IReadOnlyOrientation
{
   readonly Matrix4x4 _matrix = matrix;
   readonly XYZ<double> _voxelSize = CalcVoxelSize(matrix);

   public XYZ<double> VoxelSize => _voxelSize;
   public static XYZ<double> CalcVoxelSize(Matrix4x4 _matrix)
   {
      //return new(Norm(_matrix.M11, _matrix.M21, _matrix.M31),
      //           Norm(_matrix.M12, _matrix.M22, _matrix.M32),
      //           Norm(_matrix.M13, _matrix.M23, _matrix.M33));
      return new(Norm(_matrix.M11, _matrix.M12, _matrix.M13),
                 Norm(_matrix.M21, _matrix.M22, _matrix.M23),
                 Norm(_matrix.M31, _matrix.M32, _matrix.M33));

      static double Norm(double x, double y, double z) => Math.Sqrt(x * x + y * y + z * z);

   }


   Matrix4x4 IReadOnlyOrientation.GetMatrix() => _matrix;

   public IReadOnlyOrientation GetTranslated(double offsetX, double offsetY, double offsetZ)
   {
      var newMatrix = _matrix;
      newMatrix.M41 += (float)offsetX;
      newMatrix.M42 += (float)offsetY;
      newMatrix.M43 += (float)offsetZ;
      return new OrientationMatrix(newMatrix);
   }

   public XYZ<double> VoxelToWorldCoordinate(double x, double y, double z)
   {
      //return new XYZ<double>(
      //   _matrix.M11 * x + _matrix.M12 * y + _matrix.M13 * z + _matrix.M14,
      //   _matrix.M21 * x + _matrix.M22 * y + _matrix.M23 * z + _matrix.M24,
      //   _matrix.M31 * x + _matrix.M32 * y + _matrix.M33 * z + _matrix.M34
      //   );

      //return new XYZ<double>(
      //   _matrix.M11 * x + _matrix.M12 * x + _matrix.M13 * x + _matrix.M14,
      //   _matrix.M21 * y + _matrix.M22 * y + _matrix.M23 * y + _matrix.M24,
      //   _matrix.M31 * z + _matrix.M32 * z + _matrix.M33 * z + _matrix.M34
      //   );

      //return new XYZ<double>(
      //   _matrix.M11 * x + _matrix.M21 * y + _matrix.M31 * z + _matrix.M41,
      //   _matrix.M12 * x + _matrix.M22 * y + _matrix.M32 * z + _matrix.M42,
      //   _matrix.M13 * x + _matrix.M32 * y + _matrix.M33 * z + _matrix.M43
      //   );

      return new XYZ<double>(
         _matrix.M11 * x + _matrix.M21 * y + _matrix.M31 * z + _matrix.M14,
         _matrix.M12 * x + _matrix.M22 * y + _matrix.M32 * z + _matrix.M24,
         _matrix.M13 * x + _matrix.M32 * y + _matrix.M33 * z + _matrix.M34
         );

      //return new XYZ<double>(
      //   _matrix.M21 * x + _matrix.M22 * y + _matrix.M13 * z + _matrix.M14,
      //   _matrix.M21 * x + _matrix.M22 * y + _matrix.M23 * z + _matrix.M24,
      //   _matrix.M31 * x + _matrix.M32 * y + _matrix.M33 * z + _matrix.M34
      //   );
   }

   bool IReadOnlyOrientation.TryGetNiftiQuaternions(out double quartern_b, out double quartern_c, out double quartern_d, out double[] pixDims, out double[] translations, out double qFace)
   {
      // not supported
      quartern_b = quartern_c = quartern_d = qFace = 0;
      pixDims = translations = Array.Empty<double>();
      return false;
          
   }
}
