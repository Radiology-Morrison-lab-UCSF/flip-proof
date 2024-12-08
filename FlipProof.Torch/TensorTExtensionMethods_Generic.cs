#pragma expandtemplate typeToReplace=double
#pragma expandtemplate Int8 UInt8 Int16 Int32 Int64 float bool Complex

using System.Numerics;
using Int8 = System.SByte;
using UInt8 = System.Byte;

namespace FlipProof.Torch;

public static partial class TensorExtensionMethods
{
   /// <summary>
   /// Returns a boolean for each value indicating whether it matches the provided value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static BoolTensor ValuewiseEquals(this Tensor<double> me, double value) => new(me.Storage == value);
}

#region TEMPLATE EXPANSION
public static partial class TensorExtensionMethods_Int8
{
   /// <summary>
   /// Returns a boolean for each value indicating whether it matches the provided value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static BoolTensor ValuewiseEquals(this Tensor<Int8> me, Int8 value) => new(me.Storage == value);
}

public static partial class TensorExtensionMethods_UInt8
{
   /// <summary>
   /// Returns a boolean for each value indicating whether it matches the provided value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static BoolTensor ValuewiseEquals(this Tensor<UInt8> me, UInt8 value) => new(me.Storage == value);
}

public static partial class TensorExtensionMethods_Int16
{
   /// <summary>
   /// Returns a boolean for each value indicating whether it matches the provided value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static BoolTensor ValuewiseEquals(this Tensor<Int16> me, Int16 value) => new(me.Storage == value);
}

public static partial class TensorExtensionMethods_Int32
{
   /// <summary>
   /// Returns a boolean for each value indicating whether it matches the provided value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static BoolTensor ValuewiseEquals(this Tensor<Int32> me, Int32 value) => new(me.Storage == value);
}

public static partial class TensorExtensionMethods_Int64
{
   /// <summary>
   /// Returns a boolean for each value indicating whether it matches the provided value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static BoolTensor ValuewiseEquals(this Tensor<Int64> me, Int64 value) => new(me.Storage == value);
}

public static partial class TensorExtensionMethods_Float
{
   /// <summary>
   /// Returns a boolean for each value indicating whether it matches the provided value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static BoolTensor ValuewiseEquals(this Tensor<float> me, float value) => new(me.Storage == value);
}

public static partial class TensorExtensionMethods_Bool
{
   /// <summary>
   /// Returns a boolean for each value indicating whether it matches the provided value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static BoolTensor ValuewiseEquals(this Tensor<bool> me, bool value) => new(me.Storage == value);
}

public static partial class TensorExtensionMethods_Complex
{
   /// <summary>
   /// Returns a boolean for each value indicating whether it matches the provided value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="others"></param>
   /// <param name="dimension"></param>
   /// <returns></returns>
   public static BoolTensor ValuewiseEquals(this Tensor<Complex> me, Complex value) => new(me.Storage == value);
}

#endregion TEMPLATE EXPANSION
