using System.Numerics;
using static TorchSharp.torch;

namespace FlipProof.Torch;

public static class TypeConversions
{
   /// <summary>
   /// Converts a .NET type into a Torch scalar type
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <returns></returns>
   /// <exception cref="NotSupportedException"></exception>
   [CLSCompliant(false)]
   public static ScalarType GetScalarType<T>() 
   {
      return default(T) switch
      {
         bool => ScalarType.Bool,
         sbyte => ScalarType.Int8,
         short => ScalarType.Int16,
         int => ScalarType.Int32,
         long => ScalarType.Int64,
         byte => ScalarType.Byte,
         UInt16 => throw new NotSupportedException("Unsigned integers other than int8 are not supported"),
         UInt32 => throw new NotSupportedException("Unsigned integers other than int8 are not supported"),
         UInt64 => throw new NotSupportedException("Unsigned integers other than int8 are not supported"),
         float => ScalarType.Float32,
         double => ScalarType.Float64,
         Complex => ScalarType.ComplexFloat64,
         Complex32 => ScalarType.ComplexFloat32,
         _ => throw new NotSupportedException(typeof(T).FullName + " is not supported"),
      };
   }

}