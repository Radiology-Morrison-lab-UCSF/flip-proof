using FlipProof.Base;
using FlipProof.Image;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using TorchSharp;

namespace FlipProof.Image.Matrices;

class DecomposableNiftiTransform<T> : IEquatable<DecomposableNiftiTransform<T>> where T : struct, IFloatingPoint<T>
{
   private DenseMatrix<T> _rotation;
   private T[] _pixDim;
   private T[] _translation;
   private DenseMatrix<T> _fastCalcMat;
   private T _qfac;
   private DenseMatrix<T> _inverse;
   /// <summary>
   /// Q factorisation - changes interpretation of z slice order. Should be 1 or -1 (use sform)
   /// </summary>
   public T Qfac => _qfac;

   protected T[] PixelDimensions => _pixDim;

   public DenseMatrix<T> FastCalcMat
   {
      get => _fastCalcMat;
      private set
      {
         _fastCalcMat = value;
         _inverse = value.AppendRow(
         [
                default,
                default,
                default,
                (T)Convert.ChangeType(1, typeof(T))
         ]).Inverse();
      }
   }

   public DenseMatrix<T> Inverse => _inverse;

   public DecomposableNiftiTransform() : this(new DenseMatrix<T>(3, 3), [T.One, T.One, T.One], new T[3], T.One)
   {
   }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
   public DecomposableNiftiTransform(DenseMatrix<T> rot, T[] pixDimensionz, T[] translationz, T qfac)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
   {
      Set(rot, pixDimensionz, translationz, qfac);
   }

   public static DecomposableNiftiTransform<T> FromNiftiQuaternions(double quartern_b, double quartern_c, double quartern_d, T[] pixDims, T[] translations, T qFace)
   {
      double b_sq = quartern_b * quartern_b;
      double c_sq = quartern_c * quartern_c;
      double d_sq = quartern_d * quartern_d;
      double a_sq = 1.0 - b_sq - c_sq - d_sq;
      double num = a_sq <= 0.0 ? 0.0 : Math.Sqrt(a_sq);
      double ab = num * quartern_b;
      double ac = num * quartern_c;
      double ad = num * quartern_d;
      double bc = quartern_b * quartern_c;
      double bd = quartern_b * quartern_d;
      double cd = quartern_c * quartern_d;
      return new DecomposableNiftiTransform<T>(
         new DenseMatrix<double>(3, 3)
         {
            [0, 0] = a_sq + b_sq - c_sq - d_sq,
            [1, 0] = 2.0 * (bc + ad),
            [2, 0] = 2.0 * (bd - ac),
            [0, 1] = 2.0 * (bc - ad),
            [1, 1] = a_sq + c_sq - b_sq - d_sq,
            [2, 1] = 2.0 * (cd + ab),
            [0, 2] = 2.0 * (bd + ac),
            [1, 2] = 2.0 * (cd - ab),
            [2, 2] = a_sq + d_sq - b_sq - c_sq
         }.Cast<T>(), pixDims, translations, qFace);
   }

   public void Set(DenseMatrix<T> rot, T[] pixDimensionz, T[] translationz, T qfac)
   {
      if (qfac != T.One && qfac != -T.One)
      {
         throw new ArgumentException("Q factorisation expected only to be +1 or -1. See https://nifti.nimh.nih.gov/nifti-1/documentation/nifti1fields/nifti1fields_pages/qsform.html");
      }
      _rotation = rot.DeepClone();
      _pixDim = pixDimensionz.Duplicate();
      _translation = translationz.Duplicate();
      _qfac = qfac;
      FastCalcMat = NiftiMatToFastMat(rot, pixDimensionz.ToArray(), translationz.ToArray(), qfac);
   }

   public static DenseMatrix<T> NiftiMatToFastMat(DenseMatrix<T> rotate, T[] pixDim, T[] offset, T qfac)
   {
      if (!qfac.Equals(T.One) && !qfac.Equals(T.NegativeOne))
      {
         throw new ArgumentException("qfac should be -1 or 1");
      }

      DenseMatrix<T> fast = new DenseMatrix<T>(3, 4);
      rotate.CopyTo(fast);
      fast[0, 0] *= pixDim[0];
      fast[0, 1] *= pixDim[0];
      fast[0, 2] *= pixDim[0];
      fast[1, 0] *= pixDim[1];
      fast[1, 1] *= pixDim[1];
      fast[1, 2] *= pixDim[1];
      fast[2, 0] *= pixDim[2] * qfac;//qfac flips intepretation of z slice direction
      fast[2, 1] *= pixDim[2] * qfac;
      fast[2, 2] *= pixDim[2] * qfac;
      fast.SetColumn(3, offset);

      return fast;
   }

   public DenseMatrix<T> GetRotation() => _rotation.DeepClone();

   public T[] GetPixDim() => _pixDim.Duplicate();

   public T[] GetTranslation() => _translation.Duplicate();

   public void ApplyTransform(DenseMatrix<T> rotateAndTranslate)
   {
      ApplyTransform(rotateAndTranslate[0..3, 0..3], rotateAndTranslate.GetColumn(3)[0..3]);
   }

   public void ApplyTransform(DenseMatrix<T> rotate, T[] translate)
   {
      Set(_rotation * rotate, _pixDim, _translation.Add(translate), Qfac);
   }

   /// <summary>
   /// Mimics behaviour of MRtrix' MRTransform operation
   /// </summary>
   /// <param name="orig"></param>
   /// <param name="applyMe"></param>
   /// <param name="inverse"></param>
   /// <param name="flipX"></param>
   /// <param name="imageWidth_vox_thisIm"></param>
   /// <param name="pixDim_this"></param>
   /// <param name="imageWidth_vox_referenceIm">The image width in voxels of the reference image</param>
   /// <param name="mFromRefImage"></param>
   /// <param name="pixDim_refImage"></param>
   /// <returns></returns>
   public static DenseMatrix<T> MRTransform(DenseMatrix<T> orig,
                                             DenseMatrix<double> applyMe,
                                             bool inverse,
                                             bool flipX,
                                             int imageWidth_vox_thisIm,
                                             T[] pixDim_this,
                                             int imageWidth_vox_referenceIm,
                                             Matrix4x4_Optimised<T> mFromRefImage,
                                             T[] pixDim_refImage)
   {
      return MRTransformD(
         orig.Cast<double>(),
         applyMe,
         inverse,
         flipX,
         imageWidth_vox_thisIm,
         pixDim_this.CastArrayToDouble(),
         imageWidth_vox_referenceIm,
         mFromRefImage.ToDouble(),
         pixDim_refImage.CastArrayToDouble()).Cast<T>();
   }

   /// <summary>
   /// Mimics behaviour of MRtrix' MRTransform operation
   /// </summary>
   /// <param name="orig"></param>
   /// <param name="applyMe"></param>
   /// <param name="inverse"></param>
   /// <param name="flipX"></param>
   /// <param name="imageWidth_vox_thisIm"></param>
   /// <param name="pixDim_this"></param>
   /// <param name="imageWidth_vox_referenceIm">The image width in voxels of the reference image</param>
   /// <param name="mFromRefImage"></param>
   /// <param name="pixDim_refImage"></param>
   /// <returns></returns>
   public static DenseMatrix<double> MRTransformD(DenseMatrix<double> orig,
                                             DenseMatrix<double> applyMe,
                                             bool inverse,
                                             bool flipX,
                                             int imageWidth_vox_thisIm,
                                             double[] pixDim_this,
                                             int imageWidth_vox_referenceIm,
                                             Matrix4x4_Optimised<double> mFromRefImage,
                                             double[] pixDim_refImage)
   {
      bool replace = true;
      if (inverse)
      {
         applyMe = applyMe.Inverse();
      }
      if (flipX)
      {
         DenseMatrix<double> R = new DenseMatrix<double>(4, 4);
         R.SetDiagonal([-1.0, 1.0, 1.0, 1.0]);
         R[0, 3] = (imageWidth_vox_referenceIm - 1) * pixDim_refImage[0];
         DenseMatrix<double> denseMatrix = R * applyMe;
         R[0, 3] = (imageWidth_vox_thisIm - 1) * pixDim_this[0];
         applyMe = denseMatrix * R;
      }
      DenseMatrix<double> old = orig.DeepClone();
      DeScaleMatrix(pixDim_this.ToArray(), old);
      if (old.NoRows == 3)
      {
         old = old.AppendRow([0.0, 0.0, 0.0, 1.0]);
      }
      DeScaleMatrix(pixDim_refImage, mFromRefImage);
      DenseMatrix<double> M = mFromRefImage.ToMatrix() * applyMe;
      DenseMatrix<double> newQuickM = !replace ? M * old : M;
      DecomposableNiftiTransform<double>.DeScaleMatrix(pixDim_this.Select((a) => 1.0 / a).ToArray(), newQuickM);
      return newQuickM;
   }
   public bool Equals(DecomposableNiftiTransform<T>? other) => other is null ? false : FastCalcMat.Equals(other.FastCalcMat);

   public static void DeScaleMatrix(double[] pixDim, DenseMatrix<double> fastMat)
   {
      double pixSize_x = pixDim[0];
      double pixSize_y = pixDim[1];
      double pixSize_z = pixDim[2];
      fastMat[0, 0] = fastMat[0, 0] / pixSize_x;
      fastMat[0, 1] = fastMat[0, 1] / pixSize_x;
      fastMat[0, 2] = fastMat[0, 2] / pixSize_x;
      fastMat[1, 0] = fastMat[1, 0] / pixSize_y;
      fastMat[1, 1] = fastMat[1, 1] / pixSize_y;
      fastMat[1, 2] = fastMat[1, 2] / pixSize_y;
      fastMat[2, 0] = fastMat[2, 0] / pixSize_z;
      fastMat[2, 1] = fastMat[2, 1] / pixSize_z;
      fastMat[2, 2] = fastMat[2, 2] / pixSize_z;
   }

   public static void DeScaleMatrix(float[] pixDim, DenseMatrix<float> fastMat)
   {
      DeScaleMatrix(pixDim.CastArray<float, double>(), fastMat);
   }

   public static void DeScaleMatrix(double[] pixDim, DenseMatrix<float> fastMat)
   {
      double pixSize_x = pixDim[0];
      double pixSize_y = pixDim[1];
      double pixSize_z = pixDim[2];
      fastMat[0, 0] = (float)((double)fastMat[0, 0] / pixSize_x);
      fastMat[0, 1] = (float)((double)fastMat[0, 1] / pixSize_x);
      fastMat[0, 2] = (float)((double)fastMat[0, 2] / pixSize_x);
      fastMat[1, 0] = (float)((double)fastMat[1, 0] / pixSize_y);
      fastMat[1, 1] = (float)((double)fastMat[1, 1] / pixSize_y);
      fastMat[1, 2] = (float)((double)fastMat[1, 2] / pixSize_y);
      fastMat[2, 0] = (float)((double)fastMat[2, 0] / pixSize_z);
      fastMat[2, 1] = (float)((double)fastMat[2, 1] / pixSize_z);
      fastMat[2, 2] = (float)((double)fastMat[2, 2] / pixSize_z);
   }

   internal static void DeScaleMatrix(float[] pixDim, Matrix4x4_Optimised<float> fastMat)
   {
      DeScaleMatrix(pixDim.CastArray<float, double>(), fastMat);
   }

   internal static void DeScaleMatrix(double[] pixDim, Matrix4x4_Optimised<float> fastMat)
   {
      double pixSize_x = pixDim[0];
      double pixSize_y = pixDim[1];
      double pixSize_z = pixDim[2];
      fastMat.M0_0 = (float)(fastMat.M0_0 / pixSize_x);
      fastMat.M0_1 = (float)(fastMat.M0_1 / pixSize_x);
      fastMat.M0_2 = (float)(fastMat.M0_2 / pixSize_x);
      fastMat.M1_0 = (float)(fastMat.M1_0 / pixSize_y);
      fastMat.M1_1 = (float)(fastMat.M1_1 / pixSize_y);
      fastMat.M1_2 = (float)(fastMat.M1_2 / pixSize_y);
      fastMat.M2_0 = (float)(fastMat.M2_0 / pixSize_z);
      fastMat.M2_1 = (float)(fastMat.M2_1 / pixSize_z);
      fastMat.M2_2 = (float)(fastMat.M2_2 / pixSize_z);
   }

   internal static void DeScaleMatrix(double[] pixDim, Matrix4x4_Optimised<double> fastMat)
   {
      double pixSize_x = pixDim[0];
      double pixSize_y = pixDim[1];
      double pixSize_z = pixDim[2];
      fastMat.M0_0 /= pixSize_x;
      fastMat.M0_1 /= pixSize_x;
      fastMat.M0_2 /= pixSize_x;
      fastMat.M1_0 /= pixSize_y;
      fastMat.M1_1 /= pixSize_y;
      fastMat.M1_2 /= pixSize_y;
      fastMat.M2_0 /= pixSize_z;
      fastMat.M2_1 /= pixSize_z;
      fastMat.M2_2 /= pixSize_z;
   }

   internal void FlipX(T imWidth_rawCoords)
   {
      Flip(0, imWidth_rawCoords);
   }

   internal void FlipY(T imHeight_rawCoords)
   {
      Flip(1, imHeight_rawCoords);
   }

   internal void FlipZ(T imDepth_rawCoords)
   {
      Flip(2, imDepth_rawCoords);
   }

   internal void Flip(int dimension, T sizeOfThisDim_rawCoords)
   {
      DenseMatrix<T> rotate_new = _rotation.DeepClone();
      rotate_new[dimension, dimension] *= -T.One;
      if (rotate_new[0, 1] != T.Zero ||
         rotate_new[0, 2] != T.Zero ||
         rotate_new[1, 0] != T.Zero ||
         rotate_new[1, 2] != T.Zero ||
         rotate_new[2, 0] != T.Zero ||
         rotate_new[2, 1] != T.Zero)
      {
         throw new NotImplementedException("Not sure how to do this when other values in the array are set");
      }
      T[] translation_new = _translation.Duplicate();

      T offset = T.CopySign(sizeOfThisDim_rawCoords * _pixDim[dimension], -rotate_new[dimension, dimension]);
      translation_new[dimension] = translation_new[dimension] * -T.One + offset;
      Set(rotate_new, _pixDim, translation_new, Qfac);
   }

   public bool TryGetNiftiQuaternions(out double quartern_b, out double quartern_c, out double quartern_d, out T[] pixDims, out T[] translations, out T qFace)
   {
      bool success = Quaternion.TryMatrixToQuaternionValues(_rotation.Cast<double>(), out _, out quartern_b, out quartern_c, out quartern_d);
      System.Diagnostics.Debug.Assert(success, "Should always be decomposable");
      pixDims = _pixDim.ToArray();
      qFace = Qfac;
      translations = _translation.ToArray();
      return success;
   }

   public DecomposableNiftiTransform<T> DeepClone() => new DecomposableNiftiTransform<T>(_rotation.DeepClone(), _pixDim.Duplicate(), _translation.Duplicate(), Qfac);
}
