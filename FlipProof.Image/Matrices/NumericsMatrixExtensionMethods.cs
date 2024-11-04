using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.Image.Matrices;
public static class NumericsMatrixExtensionMethods
{
   /// <summary>
   /// Matrices are equal within a certain tolerance
   /// </summary>
   /// <param name="mat1"></param>
   /// <param name="mat2"></param>
   /// <param name="tolerance"></param>
   /// <returns></returns>
   public static bool Equals(this Matrix4x4 mat1, Matrix4x4 mat2, float tolerance)
   {
      bool equal = true;
      (mat2 - mat1).ForEach(val => 
      { 
         equal = equal && Math.Abs(val) <= tolerance; 
      });
      return equal;
   }

   /// <summary>
   /// Converts into an array
   /// </summary>
   /// <param name="mat"></param>
   /// <returns></returns>
   public static float[] ToArray(this Matrix4x4 mat)
   {
      return
      [
         mat.M11, mat.M12, mat.M13, mat.M14, 
         mat.M21, mat.M22, mat.M23, mat.M24, 
         mat.M31, mat.M32, mat.M33, mat.M34,
         mat.M41, mat.M42, mat.M43, mat.M44,
      ];
   }

   /// <summary>
   /// Performs an action for each value in the matrix
   /// </summary>
   /// <param name="m"></param>
   /// <param name="act"></param>
   public static void ForEach(this Matrix4x4 m, Action<float> act)
   {
      act(m.M11);
      act(m.M12);
      act(m.M13);
      act(m.M14);

      act(m.M21);
      act(m.M22);
      act(m.M23);
      act(m.M24);

      act(m.M31);
      act(m.M32);
      act(m.M33);
      act(m.M34);

      act(m.M41);
      act(m.M42);
      act(m.M43);
      act(m.M44);

   }

#warning need unit tests
   public static float GetVoxelSizeX(this Matrix4x4 mat) => MathF.Sqrt(mat.M11 * mat.M11 + mat.M21 * mat.M21 + mat.M31 * mat.M31);
   public static float GetVoxelSizeY(this Matrix4x4 mat) => MathF.Sqrt(mat.M12 * mat.M12 + mat.M22 * mat.M22 + mat.M32 * mat.M32);
   public static float GetVoxelSizeZ(this Matrix4x4 mat) => MathF.Sqrt(mat.M13 * mat.M13 + mat.M23 * mat.M23 + mat.M33 * mat.M33);
}
