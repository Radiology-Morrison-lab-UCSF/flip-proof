using FlipProof.Base;
using FlipProof.Torch;
using System.Numerics;
using TorchSharp;

namespace FlipProof.TorchTests;

public abstract class SimpleNumericTensorTests<T,S>
   where T:struct,INumber<T>
   where S:SimpleNumericTensor<T,S>
{

   protected abstract bool TIsRepresentableAsFloat { get; }
   public T[] GetTestArray() => Cast([1, 3, 5, 7, 11, 13, 17, 19]);


   protected T[] Cast(int[] inputs) => inputs.Select(Cast).ToArray();
   protected abstract T Cast(int input);
   protected abstract T[] Cast(S inputs);
   protected virtual bool IsSigned => true;
   protected abstract S ToTensor(T[] inputs);

   [TestMethod]
   public void AddScalar()
   {
      T[] inputArray = GetTestArray();
      T[] expected = GetWithValueAdded(GetTestArray(), Get3());

      using S tensor = ToTensor(inputArray);
      S result = tensor + Get3();

      Assert.AreNotSame(tensor, result); 
      CollectionAssert.AreEqual(expected, result.ToArray());
   }

   [TestMethod]
   public void AddTensors()
   {
      T[] inputArray = Cast([43,47,13,23]);
      T[] added = Cast([23,27,29,31]);
      T[] expected = Cast([23+43,27+47,29+13,31+23]);

      using S result = ToTensor(inputArray) + ToTensor(added);

      CollectionAssert.AreEqual(expected, result.ToArray());
   }


   [TestMethod]
   public void Abs()
   {
      T[] expected = GetTestArray();
      T[] inputArray = GetWithSomeValuesNegated(GetTestArray());

      using S tensor = ToTensor(inputArray);

      T[] result = Cast(tensor.Abs());

      CollectionAssert.AreEqual(expected, result);
   }
   [TestMethod]
   public void AbsInPlace()
   {
      T[] expected = GetTestArray();
      T[] inputArray = GetWithSomeValuesNegated(GetTestArray());

      using S tensor = ToTensor(inputArray);

      Assert.ReferenceEquals(tensor, tensor.AbsInPlace());
      T[] result = Cast(tensor);

      CollectionAssert.AreEqual(expected, result);
   }

   [TestMethod]
   public void MulScalar()
   {
      T[] inputArray = Cast([3, 4, 3, 2]);
      T[] expected = Cast([3 * 3, 4 * 3, 3 * 3, 2 * 3]);

      using S result = ToTensor(inputArray) * Get3();

      CollectionAssert.AreEqual(expected, result.ToArray());
   }

   [TestMethod]
   public void MulTensors()
   {
      T[] inputArray = Cast([3, 4, 3, 2]);
      T[] added = Cast([23, 27, 29, 31]);
      T[] expected = Cast([23 * 3, 27 * 4, 29 * 3, 31 * 2]);

      using S result = ToTensor(inputArray) * ToTensor(added);

      CollectionAssert.AreEqual(expected, result.ToArray());
   }

   
   [TestMethod]
   public void SubtractScalar()
   {
      T[] inputArray, expected;
      if (IsSigned)
      {
         inputArray = Cast([-30, 1, 30, 20]);
         expected = Cast([-30 - 3, 1 - 3, 30 - 3, 20 - 3]);
      }
      else
      {
         inputArray = Cast([30, 40, 30, 20]);
         expected = Cast([30 - 3, 40 - 3, 30 - 3, 20 - 3]);
      }
      using S result = ToTensor(inputArray) - Get3();

      CollectionAssert.AreEqual(expected, result.ToArray());
   }

   [TestMethod]
   public void SubtractTensors()
   {
      T[] left, right, expected;

      if (IsSigned)
      {
         left = Cast([3, 4, 3, 2]);
         right = Cast([23, 27, 29, 31]);
         expected = Cast([3 - 23, 4 - 27, 3 - 29, 2 - 31]);
      }
      else
      {
         left = Cast([3, 27, 127, 200]);
         right = Cast([3, 4, 29, 31]);
         expected = Cast([3 - 3, 27 - 4, 127 - 29, 200 - 31]);
      }

      using S result = ToTensor(left) - ToTensor(right);

      CollectionAssert.AreEqual(expected, result.ToArray());
   }


   /// <summary>
   /// Tests that IFFTN(FFTN(input)) == input
   /// </summary>
   [TestMethod]
   public void FFTN32()
   {
      if(!TIsRepresentableAsFloat)
      {
         // test not relevant. See 64bit test
         return;
      }
      T[] expected = GetWithSomeValuesNegated(GetTestArray());
      T[] inputArray = expected.ToArray();

      float[] expectedF = expected.Select(a => Convert.ToSingle(a)).ToArray();

      using S tensor = ToTensor(inputArray);


      float tolerance = Math.Abs(expectedF.Select(MathF.Abs).Min() * 0.0001f);

      // Forward
      Complex32Tensor fwd = FFT32(tensor);
      Complex32[] fwdArray = fwd.ToArray();
      Assert.IsFalse(new ArrayComparer_Float(tolerance).Equals(expectedF, fwdArray.ToArray(a=>a.Real)), "FFT does nothing");
      Assert.IsFalse(new ArrayComparer_Float(tolerance).Equals(expectedF, fwdArray.ToArray(a => a.Imaginary)), "FFT does nothing");

      Assert.IsTrue(fwd.ShapesEqual(tensor));


      // Inverse
      FloatTensor inverse = fwd.IFFTN();
      float[] invArr = inverse.ToArray();

      Assert.IsTrue(new ArrayComparer_Float(tolerance).Equals(expectedF, invArr));
      Assert.IsTrue(inverse.ShapesEqual(tensor));
   }

   /// <summary>
   /// Tests that IFFTN(FFTN(input)) == input
   /// </summary>
   [TestMethod]
   public void FFTN64()
   {
      if(TIsRepresentableAsFloat)
      {
         // test not relevant. See 32bit test
         return;
      }
      T[] expected = GetWithSomeValuesNegated(GetTestArray());
      T[] inputArray = expected.ToArray();

      double[] expectedF = expected.Select(a => Convert.ToDouble(a)).ToArray();

      using S tensor = ToTensor(inputArray);


      double tolerance = Math.Abs(expectedF.Select(Math.Abs).Min() * 0.0001);

      // Forward
      ComplexTensor fwd = FFT64(tensor);
      Complex[] fwdArray = fwd.ToArray();
      Assert.IsFalse(new ArrayComparer_Double(tolerance).Equals(expectedF, fwdArray.ToArray(a=>a.Real)), "FFT does nothing");
      Assert.IsFalse(new ArrayComparer_Double(tolerance).Equals(expectedF, fwdArray.ToArray(a => a.Imaginary)), "FFT does nothing");

      Assert.IsTrue(fwd.ShapesEqual(tensor));


      // Inverse
      DoubleTensor inverse = fwd.IFFTN();
      double[] invArr = inverse.ToArray();

      Assert.IsTrue(new ArrayComparer_Double(tolerance).Equals(expectedF, invArr));
      Assert.IsTrue(inverse.ShapesEqual(tensor));
   }

   protected abstract Complex32Tensor FFT32(S t);
   protected abstract ComplexTensor FFT64(S t);

   /// <summary>
   /// Negates some values in the array and returns it
   /// </summary>
   /// <param name="inputArray"></param>
   /// <returns></returns>
   protected virtual T[] GetWithSomeValuesNegated(T[] inputArray)
   {
      inputArray[1] = -inputArray[1];
      inputArray[2] = -inputArray[2];
      inputArray[5] = -inputArray[5];
      return inputArray;
   }


   protected T Get3() => Cast(3);

   protected virtual T[] GetWithValueAdded(T[] inputArray, T add)
   {
      for (int i = 0;i< inputArray.Length;i++)
      {
         inputArray[i] += add;
      }
      return inputArray;
   }

}

[TestClass]
public class FloatTensorTests : SimpleNumericTensorTests<float, FloatTensor>
{
   protected override bool TIsRepresentableAsFloat => true;
   protected override float Cast(int input) => Convert.ToSingle(input);

   protected override float[] Cast(FloatTensor inputs) => inputs.Storage.ToArray<float>();

   protected override FloatTensor ToTensor(float[] inputs) => new(torch.tensor(inputs));

   protected override Complex32Tensor FFT32(FloatTensor t) => t.FFTN();
   protected override ComplexTensor FFT64(FloatTensor t) => throw new NotSupportedException();
}

[TestClass]
public class DoubleTensorTests : SimpleNumericTensorTests<double, DoubleTensor>
{
   protected override bool TIsRepresentableAsFloat => false;
   protected override double Cast(int input) => Convert.ToDouble(input);

   protected override double[] Cast(DoubleTensor inputs) => inputs.Storage.ToArray<double>();

   protected override DoubleTensor ToTensor(double[] inputs) => new(torch.tensor(inputs));

   protected override Complex32Tensor FFT32(DoubleTensor t) => throw new NotSupportedException();
   protected override ComplexTensor FFT64(DoubleTensor t) => t.FFTN();


}

[TestClass]
public class Int64TensorTests : SimpleNumericTensorTests<Int64, Int64Tensor>
{

   protected override bool TIsRepresentableAsFloat => false;

   protected override Int64 Cast(int input) => Convert.ToInt64(input);

   protected override Int64[] Cast(Int64Tensor inputs) => inputs.Storage.ToArray<Int64>();

   protected override Int64Tensor ToTensor(Int64[] inputs) => new(torch.tensor(inputs));

   protected override Complex32Tensor FFT32(Int64Tensor t) => throw new NotSupportedException();
   protected override ComplexTensor FFT64(Int64Tensor t) => t.FFTN();

}

[TestClass]
public class Int32TensorTests : SimpleNumericTensorTests<Int32, Int32Tensor>
{
   protected override bool TIsRepresentableAsFloat => false;

   protected override Int32 Cast(int input) => Convert.ToInt32(input);

   protected override Int32[] Cast(Int32Tensor inputs) => inputs.Storage.ToArray<Int32>();

   protected override Int32Tensor ToTensor(Int32[] inputs) => new(torch.tensor(inputs));

   protected override Complex32Tensor FFT32(Int32Tensor t) => throw new NotSupportedException();
   protected override ComplexTensor FFT64(Int32Tensor t) => t.FFTN();

}

[TestClass]
public class Int16TensorTests : SimpleNumericTensorTests<Int16, Int16Tensor>
{
   protected override bool TIsRepresentableAsFloat => true;

   protected override Int16 Cast(int input) => Convert.ToInt16(input);

   protected override Int16[] Cast(Int16Tensor inputs) => inputs.Storage.ToArray<Int16>();

   protected override Int16Tensor ToTensor(Int16[] inputs) => new(torch.tensor(inputs));

   protected override Complex32Tensor FFT32(Int16Tensor t) => t.FFTN();
   protected override ComplexTensor FFT64(Int16Tensor t) => throw new NotSupportedException();

}


[TestClass]
public class Int8TensorTests : SimpleNumericTensorTests<SByte, Int8Tensor>
{
   protected override bool TIsRepresentableAsFloat => true;

   protected override SByte Cast(int input) => Convert.ToSByte(input);

   protected override SByte[] Cast(Int8Tensor inputs) => inputs.Storage.ToArray<SByte>();

   protected override Int8Tensor ToTensor(SByte[] inputs) => new(torch.tensor(inputs));

   protected override Complex32Tensor FFT32(Int8Tensor t) => t.FFTN();
   protected override ComplexTensor FFT64(Int8Tensor t) => throw new NotSupportedException();

}

[TestClass]
public class UInt8TensorTests : SimpleNumericTensorTests<byte, UInt8Tensor>
{
   protected override bool TIsRepresentableAsFloat => true;

   protected override bool IsSigned => false;
   protected override Byte Cast(int input) => Convert.ToByte(input);

   protected override Byte[] Cast(UInt8Tensor inputs) => inputs.Storage.ToArray<Byte>();

   protected override UInt8Tensor ToTensor(Byte[] inputs) => new(torch.tensor(inputs));

   protected override byte[] GetWithSomeValuesNegated(byte[] inputArray) => inputArray;

   protected override Complex32Tensor FFT32(UInt8Tensor t) => t.FFTN();
   protected override ComplexTensor FFT64(UInt8Tensor t) => throw new NotSupportedException();

}