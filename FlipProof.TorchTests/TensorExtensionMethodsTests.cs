using FlipProof.Torch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TorchSharp;


namespace FlipProof.TorchTests;
[TestClass]
public class TensorExtensionMethodsTests
{
   //[TestMethod]
   //public void AddInPlace()
   //{
   //   Tensor addTo = new double[] { 1, 3, 5,
   //                                 7, 11, 13,
   //                                 17, 19, 23}.ToTensor<double>([3, 3]);

   //   Tensor vals = new double[] { 29, 31,
   //                                 37, 41}.ToTensor<double>([2, 2]);

   //   addTo.AddInPlace(new int[] { 1, 2 }, new Range(0, 2), vals);

   //   Assert.AreEqual(7 + 29, addTo[1, 0].ReadCpuDouble(0));
   //   Assert.AreEqual(11 + 31, addTo[1, 1].ReadCpuDouble(0));
   //   Assert.AreEqual(17 + 37, addTo[2, 0].ReadCpuDouble(0));
   //   Assert.AreEqual(19 + 41, addTo[2, 1].ReadCpuDouble(0));
   //}

   [TestMethod]
   public void ReplaceInPlace()
   {
      DoubleTensor t = new(torch.tensor(new double[]{ 1, 2, 3, 2, 5, 2, 2, double.PositiveInfinity }));

      t.ReplaceInPlace(2d, 77d);

      CollectionAssert.AreEqual(t.ToArray(), new double[] { 1d, 77, 3, 77, 5, 77, 77, double.PositiveInfinity });

   }
}
