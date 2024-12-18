using FlipProof.Image;
using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.ImageTests;


[TestClass]
public class Int64ImageTests() : OperatorsTests(876)
{

   // float, Double operations tested in float, double test classes

   [TestMethod]
   public void OperatorsInt64()
   {
      OperatorsSameTypeTestInt<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(
         () => GetRandom(out Tensor<Int64> _),
         head => GetRandom(head, out Int64[] _));
   }

   [TestMethod]
   public void OperatorsInt32()
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageInt64<TestSpace3D> im0 = GetRandom(out Tensor<Int64> _);
      ImageInt32<TestSpace3D> im1 = GetRandom(im0.Header, out int[] _);

      Int64Tensor dat0 = im0.Data;
      Int32Tensor dat1 = im1.Data;

      // add
      Int64Tensor expected = dat0 + dat1;
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im0 + im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im1 + im0);


      // subtract
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(dat0 - dat1, im0 - im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(dat1 - dat0, im1 - im0);

      // multiply
      expected = dat0 * dat1;
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im0 * im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im1 * im0);

      // divide
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat0 / dat1, im0 / im1);
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat1 / dat0, im1 / im0);
   }

   [TestMethod]
   public void OperatorsInt16()
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageInt64<TestSpace3D> im0 = GetRandom(out Tensor<Int64> _);
      ImageInt16<TestSpace3D> im1 = GetRandom(im0.Header, out Int16[] _);

      Int64Tensor dat0 = im0.Data;
      Int16Tensor dat1 = im1.Data;

      // add
      Int64Tensor expected = dat0 + dat1;
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im0 + im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im1 + im0);


      // subtract
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(dat0 - dat1, im0 - im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(dat1 - dat0, im1 - im0);

      // multiply
      expected = dat0 * dat1;
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im0 * im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im1 * im0);

      // divide
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat0 / dat1, im0 / im1);
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat1 / dat0, im1 / im0);
   }


   [TestMethod]
   public void OperatorsInt8()
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageInt64<TestSpace3D> im0 = GetRandom(out Tensor<Int64> _);
      ImageInt8<TestSpace3D> im1 = GetRandom(im0.Header, out sbyte[] _);

      Int64Tensor dat0 = im0.Data;
      Int8Tensor dat1 = im1.Data;

      // add
      Int64Tensor expected = dat0 + dat1;
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im0 + im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im1 + im0);


      // subtract
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(dat0 - dat1, im0 - im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(dat1 - dat0, im1 - im0);

      // multiply
      expected = dat0 * dat1;
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im0 * im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im1 * im0);

      // divide
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat0 / dat1, im0 / im1);
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat1 / dat0, im1 / im0);
   }


   [TestMethod]
   public void OperatorsUInt8()
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageInt64<TestSpace3D> im0 = GetRandom(out Tensor<Int64> _);
      ImageUInt8<TestSpace3D> im1 = GetRandom(im0.Header, out byte[] _);

      Int64Tensor dat0 = im0.Data;
      UInt8Tensor dat1 = im1.Data;

      // add
      Int64Tensor expected = dat0 + dat1;
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im0 + im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im1 + im0);


      // subtract
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(dat0 - dat1, im0 - im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(dat1 - dat0, im1 - im0);

      // multiply
      expected = dat0 * dat1;
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im0 * im1);
      AssertImagesMatch<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(expected, im1 * im0);

      // divide
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat0 / dat1, im0 / im1);
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat1 / dat0, im1 / im0);
   }

   // TO DO: Bool operators

   #region Wrapped

   [TestMethod]
   public void FFT_IFFT() => FFT_IFFT<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(() => GetRandom(out Tensor<Int64> _), 0.015f);// greater error allowance as not all values can be encoded as float
   [TestMethod]
   public void FFT_IFFT_D() => FFT_IFFT_D<ImageInt64<TestSpace3D>, Int64, TestSpace3D, Int64Tensor>(() => GetRandom(out Tensor<Int64> _));

   #endregion

}
