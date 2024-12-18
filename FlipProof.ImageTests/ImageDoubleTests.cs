using FlipProof.Image;
using FlipProof.Torch;
using System.Numerics;
using TorchSharp;

namespace FlipProof.ImageTests;

[TestClass]
public class ImageDoubleTests() : OperatorsTests(876)
{
   
   private static void OperatorsDifferentTypeTest<TImage, TVoxel, TSpace, TTensor>(Func<ImageDouble<TSpace>> getIm0, Func<ImageHeader, TImage> getIm1) 
      where TImage : Image_SimpleNumeric<TVoxel,TSpace, TImage, TTensor>
      where TSpace : struct, ISpace
      where TVoxel : struct, INumber<TVoxel>
      where TTensor : SimpleNumericTensor<TVoxel,TTensor>
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageDouble<TSpace> im0 = getIm0();
      TImage im1 = getIm1(im0.Header);

      DoubleTensor dat0 = im0.Data;
      TTensor dat1 = im1.Data;

      // add
      DoubleTensor expected = dat0 + dat1;
      AssertImagesMatch<ImageDouble<TSpace>,double, TSpace, DoubleTensor>(expected, im0 + im1);
      AssertImagesMatch<ImageDouble<TSpace>, double, TSpace, DoubleTensor>(expected, im1 + im0);

      // subtract
      AssertImagesMatch<ImageDouble<TSpace>, double, TSpace, DoubleTensor>(dat0 - dat1, im0 - im1);
      AssertImagesMatch<ImageDouble<TSpace>, double, TSpace, DoubleTensor>(dat1 - dat0, im1 - im0);

      // multiply
      expected = dat0 * dat1;
      AssertImagesMatch<ImageDouble<TSpace>, double, TSpace, DoubleTensor>(expected, im0 * im1);
      AssertImagesMatch<ImageDouble<TSpace>, double, TSpace, DoubleTensor>(expected, im1 * im0);

      // divide
      AssertImagesMatch<ImageDouble<TSpace>, double, TSpace, DoubleTensor>(dat0 / dat1, im0 / im1);
      AssertImagesMatch<ImageDouble<TSpace>, double, TSpace, DoubleTensor>(dat1 / dat0, im1 / im0);

   }

   // Double operations tested in double test class

   [TestMethod]
   public void OperatorsDouble()
   {
      OperatorsSameTypeTest<ImageDouble<TestSpace3D>, double, TestSpace3D, DoubleTensor>(
         () => GetRandom(out Tensor<double> _), 
         head => GetRandom(head, out double[] _));


   }
   
   [TestMethod]
   public void OperatorsFloat()
   {
      OperatorsDifferentTypeTest<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(
         () => GetRandom(out Tensor<double> _),
         head => GetRandom(head, out float[] _));
   }   

   [TestMethod]
   public void OperatorsInt64()
   {
      OperatorsDifferentTypeTest<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(
         () => GetRandom(out Tensor<double> _),
         head => GetRandom(head, out Int64[] _));
         
   }

   [TestMethod]
   public void OperatorsInt32()
   {
      OperatorsDifferentTypeTest<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(
         () => GetRandom(out Tensor<double> _),
         head => GetRandom(head, out Int32[] _));
   }
   [TestMethod]
   public void OperatorsInt16()
   {
      OperatorsDifferentTypeTest<ImageInt16<TestSpace3D>, Int16, TestSpace3D, Int16Tensor>(
         () => GetRandom(out Tensor<double> _),
         head => GetRandom(head, out Int16[] _));
   }


   [TestMethod]
   public void OperatorsInt8()
   {
      OperatorsDifferentTypeTest<ImageInt8<TestSpace3D>, sbyte, TestSpace3D, Int8Tensor>(
         () => GetRandom(out Tensor<double> _),
         head => GetRandom(head, out sbyte[] _));
   }


   [TestMethod]
   public void OperatorsUInt8()
   {
      OperatorsDifferentTypeTest<ImageUInt8<TestSpace3D>, byte, TestSpace3D, UInt8Tensor>(
         () => GetRandom(out Tensor<double> _),
         head => GetRandom(head, out byte[] _));
   }

   // TO DO: Bool operators


   #region Wrapped

   [TestMethod]
   public void FFT_IFFT() => FFT_IFFT<ImageDouble<TestSpace3D>, double, TestSpace3D, DoubleTensor>(() => GetRandom(out Tensor<double> _));
   [TestMethod]
   public void FFT_IFFT_D() => FFT_IFFT_D<ImageDouble<TestSpace3D>, double, TestSpace3D, DoubleTensor>(() => GetRandom(out Tensor<double> _));

   #endregion

}