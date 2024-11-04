using FlipProof.Image;
using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.ImageTests;


[TestClass]
public class Int32ImageTests() : OperatorsTests(9814)
{

   // float, Double, Int64 operations tested in other test classes

   [TestMethod]
   public void OperatorsInt32()
   {
      OperatorsSameTypeTestInt<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(
         () => GetRandom(out Tensor<Int32> _),
         head => GetRandom(head, out Int32[] _));
   }


   [TestMethod]
   public void OperatorsInt16()
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageInt32<TestSpace3D> im0 = GetRandom(out Tensor<Int32> _);
      ImageInt16<TestSpace3D> im1 = GetRandom(im0.Header, out Int16[] _);

      Int32Tensor dat0 = im0.Data;
      Int16Tensor dat1 = im1.Data;

      // add
      Int32Tensor expected = dat0 + dat1;
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im0 + im1);
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im1 + im0);


      // subtract
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(dat0 - dat1, im0 - im1);
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(dat1 - dat0, im1 - im0);

      // multiply
      expected = dat0 * dat1;
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im0 * im1);
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im1 * im0);

      // divide
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat0 / dat1, im0 / im1);
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat1 / dat0, im1 / im0);
   }


   [TestMethod]
   public void OperatorsInt8()
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageInt32<TestSpace3D> im0 = GetRandom(out Tensor<Int32> _);
      ImageInt8<TestSpace3D> im1 = GetRandom(im0.Header, out sbyte[] _);

      Int32Tensor dat0 = im0.Data;
      Int8Tensor dat1 = im1.Data;

      // add
      Int32Tensor expected = dat0 + dat1;
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im0 + im1);
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im1 + im0);


      // subtract
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(dat0 - dat1, im0 - im1);
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(dat1 - dat0, im1 - im0);

      // multiply
      expected = dat0 * dat1;
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im0 * im1);
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im1 * im0);

      // divide
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat0 / dat1, im0 / im1);
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat1 / dat0, im1 / im0);
   }


   [TestMethod]
   public void OperatorsUInt8()
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageInt32<TestSpace3D> im0 = GetRandom(out Tensor<Int32> _);
      ImageUInt8<TestSpace3D> im1 = GetRandom(im0.Header, out byte[] _);

      Int32Tensor dat0 = im0.Data;
      UInt8Tensor dat1 = im1.Data;

      // add
      Int32Tensor expected = dat0 + dat1;
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im0 + im1);
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im1 + im0);


      // subtract
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(dat0 - dat1, im0 - im1);
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(dat1 - dat0, im1 - im0);

      // multiply
      expected = dat0 * dat1;
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im0 * im1);
      AssertImagesMatch<ImageInt32<TestSpace3D>, Int32, TestSpace3D, Int32Tensor>(expected, im1 * im0);

      // divide
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat0 / dat1, im0 / im1);
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat1 / dat0, im1 / im0);
   }

   // TO DO: Bool operators

}
