using FlipProof.Torch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.TorchTests;
[TestClass]
public class TensorTests
{
   [TestMethod]
   public void Concat()
   {
      // Arrange
      var tensor = new DoubleTensor(new long[] { 3, 2 });
      tensor[0, 0] = 1.0;
      tensor[0, 1] = 2.0;
      tensor[1, 0] = 3.0;
      tensor[1, 1] = 4.0;
      tensor[2, 0] = 5.0;
      tensor[2, 1] = 6.0;

      var tensor2 = new DoubleTensor(new long[] { 3, 2 });
      tensor2[0, 0] = 7.0;
      tensor2[0, 1] = 8.0;
      tensor2[1, 0] = 9.0;
      tensor2[1, 1] = 10.0;
      tensor2[2, 0] = 11.0;
      tensor2[2, 1] = 12.0;

      var tensor3 = new DoubleTensor(new long[] { 3, 2 });
      tensor3[0, 0] = 13;
      tensor3[0, 1] = 14;
      tensor3[1, 0] = 15;
      tensor3[1, 1] = 16.0;
      tensor3[2, 0] = 17.0;
      tensor3[2, 1] = 18.0;


      // Act
      var result = tensor.Concat(0, tensor2, tensor3);
      // Assert
      Assert.AreNotSame(result, tensor);
      Assert.AreNotSame(result, tensor2);
      Assert.AreNotSame(result, tensor3);

      Assert.AreEqual(2, result.Storage.ndim);
      CollectionAssert.AreEqual(new long[] { 9, 2 }, result.Storage.shape);

      // After sorting by the first dimension (rows), the tensor should have rows sorted according to 'keys'
      Assert.AreEqual(1.0, result[0, 0]);
      Assert.AreEqual(2.0, result[0, 1]);
      Assert.AreEqual(3.0, result[1, 0]);
      Assert.AreEqual(4.0, result[1, 1]);
      Assert.AreEqual(5.0, result[2, 0]);
      Assert.AreEqual(6.0, result[2, 1]);
      Assert.AreEqual(7.0, result[3, 0]);
      Assert.AreEqual(8.0, result[3, 1]);
      Assert.AreEqual(9.0, result[4, 0]);
      Assert.AreEqual(10.0, result[4, 1]);
      Assert.AreEqual(11.0, result[5, 0]);
      Assert.AreEqual(12.0, result[5, 1]);
      Assert.AreEqual(13.0, result[6, 0]);
      Assert.AreEqual(14.0, result[6, 1]);
      Assert.AreEqual(15.0, result[7, 0]);
      Assert.AreEqual(16.0, result[7, 1]);
      Assert.AreEqual(17.0, result[8, 0]);
      Assert.AreEqual(18.0, result[8, 1]);

   }

   [TestMethod]
   public void SortInPlace_WithInvalidDimension()
   {
      var tensor = new DoubleTensor(new long[] { 3, 2 });
      tensor[0, 0] = 5.0;
      tensor[0, 1] = 1.0;
      tensor[1, 0] = 4.0;
      tensor[1, 1] = 2.0;
      tensor[2, 0] = 3.0;
      tensor[2, 1] = 6.0;

      var keys = new double[] { 1.0, 2.0, 3.0 };

      // Act
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => tensor.SortInPlace(keys, 3)); // This should throw an ArgumentException due to invalid dimension
   }

   [TestMethod]
   public void SortInPlace_ShouldThrowArgumentExceptionIfKeysDimensionMismatch()
   {
      // Arrange
      var tensor = new DoubleTensor(new long[] { 3, 2 });
      var keys = new double[] { 1.0, 2.0 }; // Mismatch: tensor has size 3 along the dimension we're sorting

      // Act & Assert
      Assert.ThrowsException<ArgumentException>(() => tensor.SortInPlace(keys, 0)); // Should throw
   }

   [TestMethod]
   public void SortInPlace_Dim0()
   {
      // Arrange
      var tensor = new DoubleTensor(new long[] { 3, 2 });
      tensor[0, 0] = 5.0;
      tensor[0, 1] = 1.0;
      tensor[1, 0] = 4.0;
      tensor[1, 1] = 2.0;
      tensor[2, 0] = 3.0;
      tensor[2, 1] = 6.0;

      var keys = new double[] { 2.0, 3.0, 1.0 };

      // Act
      tensor.SortInPlace(keys, 0);

      // Assert
      // After sorting by the first dimension (rows), the tensor should have rows sorted according to 'keys'
      CollectionAssert.AreEqual(new long[] { 3, 2 }, tensor.Storage.shape);
      Assert.AreEqual(3.0, tensor[0, 0]);
      Assert.AreEqual(6.0, tensor[0, 1]);
      Assert.AreEqual(5.0, tensor[1, 0]);
      Assert.AreEqual(1.0, tensor[1, 1]);
      Assert.AreEqual(4.0, tensor[2, 0]);
      Assert.AreEqual(2.0, tensor[2, 1]);
   }

   [TestMethod]
   public void SortInPlace_Dim1()
   {
      // Arrange
      var tensor = new DoubleTensor(new long[] { 3, 2 });
      tensor[0, 0] = 5.0;
      tensor[0, 1] = 1.0;
      tensor[1, 0] = 4.0;
      tensor[1, 1] = 2.0;
      tensor[2, 0] = 3.0;
      tensor[2, 1] = 6.0;

      var keys = new double[] { 2, -1 };

      // Act
      tensor.SortInPlace(keys, 1);

      // Assert
      // After sorting by the first dimension (rows), the tensor should have rows sorted according to 'keys'
      CollectionAssert.AreEqual(new long[] { 3, 2 }, tensor.Storage.shape);
      Assert.AreEqual(1.0, tensor[0, 0]);
      Assert.AreEqual(5.0, tensor[0, 1]);
      Assert.AreEqual(2.0, tensor[1, 0]);
      Assert.AreEqual(4.0, tensor[1, 1]);
      Assert.AreEqual(6.0, tensor[2, 0]);
      Assert.AreEqual(3.0, tensor[2, 1]);
   }

   [TestMethod]
   public void Stack()
   {
      // Arrange
      var tensor = new DoubleTensor(new long[] { 3, 2 });
      tensor[0, 0] = 1.0;
      tensor[0, 1] = 2.0;
      tensor[1, 0] = 3.0;
      tensor[1, 1] = 4.0;
      tensor[2, 0] = 5.0;
      tensor[2, 1] = 6.0;

      var tensor2 = new DoubleTensor(new long[] { 3, 2 });
      tensor2[0, 0] = 7.0;
      tensor2[0, 1] = 8.0;
      tensor2[1, 0] = 9.0;
      tensor2[1, 1] = 10.0;
      tensor2[2, 0] = 11.0;
      tensor2[2, 1] = 12.0;

      var tensor3 = new DoubleTensor(new long[] { 3, 2 });
      tensor3[0, 0] = 13;
      tensor3[0, 1] = 14;
      tensor3[1, 0] = 15;
      tensor3[1, 1] = 16.0;
      tensor3[2, 0] = 17.0;
      tensor3[2, 1] = 18.0;


      // Act
      var result = tensor.Stack(0, tensor2, tensor3);
      // Assert
      Assert.AreNotSame(result, tensor);
      Assert.AreNotSame(result, tensor2);
      Assert.AreNotSame(result, tensor3);

      Assert.AreEqual(3, result.Storage.ndim);
      CollectionAssert.AreEqual(new long[] { 3, 3, 2 }, result.Storage.shape);

      // After sorting by the first dimension (rows), the tensor should have rows sorted according to 'keys'
      Assert.AreEqual(1.0, result[0, 0, 0]);
      Assert.AreEqual(2.0, result[0, 0, 1]);
      Assert.AreEqual(3.0, result[0, 1, 0]);
      Assert.AreEqual(4.0, result[0, 1, 1]);
      Assert.AreEqual(5.0, result[0, 2, 0]);
      Assert.AreEqual(6.0, result[0, 2, 1]);
      Assert.AreEqual(7.0, result[1, 0, 0]);
      Assert.AreEqual(8.0, result[1, 0, 1]);
      Assert.AreEqual(9.0, result[1, 1, 0]);
      Assert.AreEqual(10.0, result[1, 1, 1]);
      Assert.AreEqual(11.0, result[1, 2, 0]);
      Assert.AreEqual(12.0, result[1, 2, 1]);
      Assert.AreEqual(13.0, result[2, 0, 0]);
      Assert.AreEqual(14.0, result[2, 0, 1]);
      Assert.AreEqual(15.0, result[2, 1, 0]);
      Assert.AreEqual(16.0, result[2, 1, 1]);
      Assert.AreEqual(17.0, result[2, 2, 0]);
      Assert.AreEqual(18.0, result[2, 2, 1]);
   }

}

