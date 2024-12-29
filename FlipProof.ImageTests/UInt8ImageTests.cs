using FlipProof.Image;
using FlipProof.Torch;
using TorchSharp;
using UInt8 = System.Byte;
namespace FlipProof.ImageTests;

[TestClass]
public class UInt8ImageTests() : OperatorsTests(23354)
{

   // float, Double, Int64, Int32, int16, Int8 operations tested in other test classes

   [TestMethod]
   public void OperatorsUInt8()
   {
      OperatorsSameTypeTestInt<ImageUInt8<TestSpace3D>, UInt8, TestSpace3D, UInt8Tensor>(
         () => GetRandom(out Tensor<UInt8> _),
         head => GetRandom(head, out UInt8[] _));
   }


   // TO DO: Bool operators

   #region Wrapped


   [TestMethod]
   public void FFT_IFFT() => FFT_IFFT<ImageUInt8<TestSpace3D>, byte, TestSpace3D, UInt8Tensor>(() => GetRandom(out Tensor<byte> _), a => a.FFT());

   #endregion

}
