using FlipProof.Torch;
using System.Numerics;
using TorchSharp;

namespace FlipProof.TorchTests;

public abstract class SimpleNumericTensorTests<T,S> where T:struct,INumber<T>
   where S:SimpleNumericTensor<T,S>
{


   public T[] GetTestArray() => Cast(new int[] { 1, 3, 5, 7, 11, 13, 17, 19 });


   protected T[] Cast(int[] inputs) => inputs.Select(Cast).ToArray();
   protected abstract T Cast(int input);
   protected abstract T[] Cast(S inputs);
   protected abstract S ToTensor(T[] inputs);

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

}


[TestClass]
public class FloatTensorTests : SimpleNumericTensorTests<float, FloatTensor>
{
   protected override float Cast(int input) => Convert.ToSingle(input);

   protected override float[] Cast(FloatTensor inputs) => inputs.Storage.ToArray<float>();

   protected override FloatTensor ToTensor(float[] inputs) => new(torch.tensor(inputs));
}

[TestClass]
public class DoubleTensorTests : SimpleNumericTensorTests<double, DoubleTensor>
{
   protected override double Cast(int input) => Convert.ToDouble(input);

   protected override double[] Cast(DoubleTensor inputs) => inputs.Storage.ToArray<double>();

   protected override DoubleTensor ToTensor(double[] inputs) => new(torch.tensor(inputs));
}

[TestClass]
public class Int64TensorTests : SimpleNumericTensorTests<Int64, Int64Tensor>
{
   protected override Int64 Cast(int input) => Convert.ToInt64(input);

   protected override Int64[] Cast(Int64Tensor inputs) => inputs.Storage.ToArray<Int64>();

   protected override Int64Tensor ToTensor(Int64[] inputs) => new(torch.tensor(inputs));
}

[TestClass]
public class Int32TensorTests : SimpleNumericTensorTests<Int32, Int32Tensor>
{
   protected override Int32 Cast(int input) => Convert.ToInt32(input);

   protected override Int32[] Cast(Int32Tensor inputs) => inputs.Storage.ToArray<Int32>();

   protected override Int32Tensor ToTensor(Int32[] inputs) => new(torch.tensor(inputs));
}

[TestClass]
public class Int16TensorTests : SimpleNumericTensorTests<Int16, Int16Tensor>
{
   protected override Int16 Cast(int input) => Convert.ToInt16(input);

   protected override Int16[] Cast(Int16Tensor inputs) => inputs.Storage.ToArray<Int16>();

   protected override Int16Tensor ToTensor(Int16[] inputs) => new(torch.tensor(inputs));
}


[TestClass]
public class Int8TensorTests : SimpleNumericTensorTests<SByte, Int8Tensor>
{
   protected override SByte Cast(int input) => Convert.ToSByte(input);

   protected override SByte[] Cast(Int8Tensor inputs) => inputs.Storage.ToArray<SByte>();

   protected override Int8Tensor ToTensor(SByte[] inputs) => new(torch.tensor(inputs));
}

[TestClass]
public class UInt8TensorTests : SimpleNumericTensorTests<byte, UInt8Tensor>
{
   protected override Byte Cast(int input) => Convert.ToByte(input);

   protected override Byte[] Cast(UInt8Tensor inputs) => inputs.Storage.ToArray<Byte>();

   protected override UInt8Tensor ToTensor(Byte[] inputs) => new(torch.tensor(inputs));

   protected override byte[] GetWithSomeValuesNegated(byte[] inputArray) => inputArray;
}