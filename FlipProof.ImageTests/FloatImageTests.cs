using FlipProof.Image;
using FlipProof.Torch;
using System.Numerics;
using TorchSharp;
using static TorchSharp.torch;

namespace FlipProof.ImageTests;


[TestClass]
public class FloatImageTests() : OperatorsTests(876)
{
   
   private static void OperatorsDifferentTypeTest<TImage, TVoxel, TSpace, TTensor>(Func<ImageFloat<TSpace>> getIm0, Func<ImageHeader, TImage> getIm1) 
      where TImage : Image_Integer<TVoxel,TSpace, TImage, TTensor>
      where TSpace : ISpace
      where TVoxel : struct, INumber<TVoxel>, IBinaryInteger<TVoxel>
      where TTensor : IntegerTensor<TVoxel,TTensor>
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageFloat<TSpace> im0 = getIm0();
      TImage im1 = getIm1(im0.Header);

      FloatTensor dat0 = im0.Data;
      TTensor dat1 = im1.Data;
      // add
      FloatTensor expected = dat0 + dat1;
      AssertImagesMatch<ImageFloat<TSpace>,float, TSpace, FloatTensor>(expected, im0 + im1);
      AssertImagesMatch<ImageFloat<TSpace>, float, TSpace, FloatTensor>(expected, im1 + im0);


      // subtract
      AssertImagesMatch<ImageFloat<TSpace>, float, TSpace, FloatTensor>(dat0 - dat1, im0 - im1);
      AssertImagesMatch<ImageFloat<TSpace>, float, TSpace, FloatTensor>(dat1 - dat0, im1 - im0);

      // multiply
      expected = dat0 * dat1;
      AssertImagesMatch<ImageFloat<TSpace>, float, TSpace, FloatTensor>(expected, im0 * im1);
      AssertImagesMatch<ImageFloat<TSpace>, float, TSpace, FloatTensor>(expected, im1 * im0);

      // divide
      AssertImagesMatch<ImageFloat<TSpace>, float, TSpace, FloatTensor>(dat0 / dat1, im0 / im1);
      AssertImagesMatch<ImageFloat<TSpace>, float, TSpace, FloatTensor>(dat1 / dat0, im1 / im0);

   }

   // Double operations tested in double test class

   [TestMethod]
   public void OperatorsFloat()
   {
      OperatorsSameTypeTest<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(
         () => GetRandom(out Tensor<float> _), 
         head => GetRandom(head, out float[] _));
   }
   
   [TestMethod]
   public void OperatorsInt64()
   {
      OperatorsDifferentTypeTest<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(
         () => GetRandom(out Tensor<float> _),
         head => GetRandom(head, out Int64[] _));
         
   }

   [TestMethod]
   public void OperatorsInt32()
   {
      OperatorsDifferentTypeTest<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(
         () => GetRandom(out Tensor<float> _),
         head => GetRandom(head, out Int32[] _));

   }
   [TestMethod]
   public void OperatorsInt16()
   {
      OperatorsDifferentTypeTest<ImageInt16<TestSpace3D>, Int16, TestSpace3D, Int16Tensor>(
         () => GetRandom(out Tensor<float> _),
         head => GetRandom(head, out Int16[] _));

   }


   [TestMethod]
   public void OperatorsInt8()
   {
      OperatorsDifferentTypeTest<ImageInt8<TestSpace3D>, sbyte, TestSpace3D, Int8Tensor>(
         () => GetRandom(out Tensor<float> _),
         head => GetRandom(head, out sbyte[] _));

   }


   [TestMethod]
   public void OperatorsUInt8()
   {
      OperatorsDifferentTypeTest<ImageUInt8<TestSpace3D>, byte, TestSpace3D, UInt8Tensor>(
         () => GetRandom(out Tensor<float> _),
         head => GetRandom(head, out byte[] _));

   }

   // TO DO: Bool operators



}
