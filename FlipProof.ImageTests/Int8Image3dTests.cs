using FlipProof.Image;
using FlipProof.Torch;
using TorchSharp;
using Int8 = System.SByte;
namespace FlipProof.ImageTests;


[TestClass]
public class Int8ImageTests() : OperatorsTests(98882)
{

   // float, Double, Int64, Int32, nt16 operations tested in other test classes

   [TestMethod]
   public void OperatorsInt8()
   {
      OperatorsSameTypeTestInt<ImageInt8<TestSpace3D>, Int8, TestSpace3D, Int8Tensor>(
         () => GetRandom(out Tensor<Int8> _),
         head => GetRandom(head, out Int8[] _));
   }


   [TestMethod]
   public void OperatorsUInt8()
   {
      using DisposeScope scope = torch.NewDisposeScope();

      ImageInt8<TestSpace3D> im0 = GetRandom(out Tensor<Int8> _);
      ImageUInt8<TestSpace3D> im1 = GetRandom(im0.Header, out byte[] _);

      Int8Tensor dat0 = im0.Data;
      UInt8Tensor dat1 = im1.Data;

      // add
      Int16Tensor expected = dat0 + dat1;
      AssertImagesMatch<ImageInt16<TestSpace3D>, Int16, TestSpace3D, Int16Tensor>(expected, im0 + im1);
      AssertImagesMatch<ImageInt16<TestSpace3D>, Int16, TestSpace3D, Int16Tensor>(expected, im1 + im0);


      // subtract
      AssertImagesMatch<ImageInt16<TestSpace3D>, Int16, TestSpace3D, Int16Tensor>(dat0 - dat1, im0 - im1);
      AssertImagesMatch<ImageInt16<TestSpace3D>, Int16, TestSpace3D, Int16Tensor>(dat1 - dat0, im1 - im0);

      // multiply
      expected = dat0 * dat1;
      AssertImagesMatch<ImageInt16<TestSpace3D>, Int16, TestSpace3D, Int16Tensor>(expected, im0 * im1);
      AssertImagesMatch<ImageInt16<TestSpace3D>, Int16, TestSpace3D, Int16Tensor>(expected, im1 * im0);

      // divide
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat0 / dat1, im0 / im1);
      AssertImagesMatch<ImageFloat<TestSpace3D>, float, TestSpace3D, FloatTensor>(dat1 / dat0, im1 / im0);
   }

   // TO DO: Bool operators

}
