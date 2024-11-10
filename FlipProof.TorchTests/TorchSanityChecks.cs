using FlipProof.Torch;
using System;
using TorchSharp;

namespace FlipProof.TorchTests;

[TestClass]
public class TorchSanityChecks
{
   [TestMethod]
   public void Conj()
   {
      var orig = torch.tensor(new System.Numerics.Complex(3, 7));

      var conj = orig.conj();

      Assert.AreEqual(7d, orig.imag.ReadCpuDouble(0));
      Assert.IsTrue(conj.is_conj());

      var five = torch.tensor(5d);
      
      Assert.AreEqual(-35d, (conj * five).imag.ReadCpuDouble(0));// Fails
   }
}
