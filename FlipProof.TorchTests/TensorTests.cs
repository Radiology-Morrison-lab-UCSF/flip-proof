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
   public void SortInPlace_WithInvalidDimension()
   {
      var tensor = new DoubleTensor(new long[] { 3, 2 });
      tensor[0, 0] = 5.0;
      tensor[0, 1] = 1.0;
      tensor[1, 0] = 4.0;
      tensor[1, 1] = 2.0;
      tensor[2, 0] = 3.0;
      tensor[2, 1] = 6.0;

      var keys = new double[] { 1.0, 2.0, 3.0  };

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


}

