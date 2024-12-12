using FlipProof.Torch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TorchSharp;
using static TorchSharp.torch;

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
   public void ArgMin()
   {
      // Create three 2D tensors with small sizes
      var tensor1 = new float[,] { { 4, -2, 7 }, { -10, 0, 9 } };
      var tensor2 = new float[,] { { 10, -3, -5 }, { -8, 2, 4 } };
      var tensor3 = new float[,] { { -7, 3, 8 }, { 1, -4, -6 } };

      // Convert these arrays to TorchSharp tensors
      FloatTensor[] tensor1Torch = [
         new(torch.tensor(tensor1)),
         new(torch.tensor(tensor2)),
         new(torch.tensor(tensor3))];

      Int64Tensor indices = tensor1Torch.ArgMin();

      CollectionAssert.AreEqual(new long[] { 2, 3 }, indices.Storage.shape);



      // Assert the correct results for argmin
      Assert.AreEqual(2L, indices[0,0]); 
      Assert.AreEqual(1L, indices[0,1]); 
      Assert.AreEqual(1L, indices[0,2]); 

      Assert.AreEqual(0L, indices[1,0]); 
      Assert.AreEqual(2L, indices[1, 1]);
      Assert.AreEqual(2L, indices[1, 2]); 

   }
   [TestMethod]
   public void ArgMax()
   {
      // Create three 2D tensors with small sizes
      var tensor1 = new float[,] { { 4, -2, 7 }, { -10, 0, 9 } };
      var tensor2 = new float[,] { { 10, -3, -5 }, { -8, 2, 4 } };
      var tensor3 = new float[,] { { -7, 3, 8 }, { 1, -4, -6 } };

      // Convert these arrays to TorchSharp tensors
      FloatTensor[] tensor1Torch = [
         new(torch.tensor(tensor1)),
         new(torch.tensor(tensor2)),
         new(torch.tensor(tensor3))];

      Int64Tensor indices = tensor1Torch.ArgMax();

      CollectionAssert.AreEqual(new long[] { 2, 3 }, indices.Storage.shape);



      // Assert the correct results for argmin
      Assert.AreEqual(1L, indices[0,0]); 
      Assert.AreEqual(2L, indices[0,1]); 
      Assert.AreEqual(2L, indices[0,2]); 

      Assert.AreEqual(2L, indices[1,0]); 
      Assert.AreEqual(1L, indices[1, 1]);
      Assert.AreEqual(0L, indices[1, 2]); 

   }

   [TestMethod]
   public void Masked()
   {
      var data = new float[,] { { 4, -2, 7 }, { -2, 1, 9 } };
      FloatTensor dataTensor = new(torch.tensor(data));

      var mask = new bool[,] { { true, false, true }, { true, true, false } };
      BoolTensor maskTensor = new(torch.tensor(mask));

      FloatTensor result = dataTensor.Masked(maskTensor);

      var expected = new float[,] { { 4, 0, 7 }, { -2, 1, 0 } };
      FloatTensor expectedTensor = new(torch.tensor(expected));

      Assert.AreNotSame(dataTensor, result);
      Assert.IsTrue(expectedTensor.Equals(result));

   }
   [TestMethod]
   public void MaskedFillInPlace()
   {
      var data = new float[,] { { 4, -2, 7 }, { -2, 1, 9 } };
      FloatTensor dataTensor = new(torch.tensor(data));

      var mask = new bool[,] { { true, false, true }, { true, true, false } };
      BoolTensor maskTensor = new(torch.tensor(mask));

      FloatTensor result = dataTensor.MaskedFillInPlace(maskTensor,44);

      var expected = new float[,] { { 44, -2, 44 }, { 44, 44, 9 } };
      FloatTensor expectedTensor = new(torch.tensor(expected));

      Assert.AreSame(dataTensor, result);
      Assert.IsTrue(expectedTensor.Equals(result));

   }

   [TestMethod]
   public void ValuewiseEquals()
   {
      var tensor1 = new float[,] { { 4, -2, 7 }, { -2, 0, 9 } };
      FloatTensor tensor1Torch = new(torch.tensor(tensor1));

      BoolTensor result = tensor1Torch.ValuewiseEquals(-2f);

      CollectionAssert.AreEqual(new long[] { 2, 3 }, result.Storage.shape);

      Assert.IsFalse(result[0,0]); 
      Assert.IsTrue(result[0,1]); 
      Assert.IsFalse(result[0,2]); 

      Assert.IsTrue(result[1,0]); 
      Assert.IsFalse(result[1,1]); 
      Assert.IsFalse(result[1,2]); 
   }

  

   [TestMethod]
   public void ReplaceInPlace()
   {
      DoubleTensor t = new(torch.tensor(new double[]{ 1, 2, 3, 2, 5, 2, 2, double.PositiveInfinity }));

      t.ReplaceInPlace(2d, 77d);

      CollectionAssert.AreEqual(t.ToArray(), new double[] { 1d, 77, 3, 77, 5, 77, 77, double.PositiveInfinity });

   }
}
