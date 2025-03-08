using FlipProof.Base;
using System.Numerics;

namespace FlipProof.Image.Matrices;

class DecomposableNiftiTransformD : DecomposableNiftiTransform<double>, IReadOnlyOrientation
{
   public XYZ<double> VoxelSize => new(PixelDimensions[0], PixelDimensions[1], PixelDimensions[2]);

   private DecomposableNiftiTransformD(DenseMatrix<double> rot, double[] pixDimensionz, double[] translationz, double qfac) : base(rot, pixDimensionz, translationz, qfac)
   {

   }

   public new static DecomposableNiftiTransformD FromNiftiQuaternions(double quartern_b, double quartern_c, double quartern_d, double[] pixDims, double[] translations, double qFace)
   {
      var made = DecomposableNiftiTransform<double>.FromNiftiQuaternions(quartern_b, quartern_c, quartern_d, pixDims, translations, qFace);
      return new DecomposableNiftiTransformD(made.GetRotation(), made.GetPixDim(), made.GetTranslation(), made.Qfac);
   }

   Matrix4x4 IReadOnlyOrientation.GetMatrix() => throw new NotSupportedException();

   public IReadOnlyOrientation GetTranslated(double offsetX, double offsetY, double offsetZ)
   {
      var transl = GetTranslation();
      transl[0] += offsetX;
      transl[1] += offsetY;
      transl[2] += offsetZ;
      return new DecomposableNiftiTransformD(GetRotation(), GetPixDim(), transl, Qfac);
   }


   public XYZ<double> VoxelToWorldCoordinate(double x, double y, double z)
   {
      var rot = GetRotation();
      var trans = GetTranslation();

      DenseMatrix<double> ijk = new(3, 1);
      ijk.SetColumn(0, [x * PixelDimensions[0], y * PixelDimensions[1], z * PixelDimensions[2] * Qfac]);

      double[] xyz = (rot * ijk).GetColumn(0).Add(trans);

      return new(xyz[0], xyz[1], xyz[2]);

   }
}
