using System.Numerics;

namespace FlipProof.Base;

public readonly record struct XYZA<T>(T X, T Y, T Z, T A) where T : INumber<T>
{
   /// <summary>
   /// Creates an <see cref="XYZA{T}"/> from an array
   /// </summary>
   /// <param name="values">An array or equivalent with 4 elements</param>
   /// <exception cref="ArgumentException">Array contains more than 4 elements</exception>
   /// <exception cref="IndexOutOfRangeException">Array contains fewer than 4 elements</exception>
   public XYZA(IReadOnlyList<T> values) : this(values[0], values[1], values[2], values[3])
   {
      if (values.Count != 4)
      {
         throw new ArgumentException("Expected collection with 4 elements", nameof(values));
      }
   }
    public static XYZA<T> operator +(XYZA<T> a, XYZA<T> b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.A + b.A);
    public static XYZA<T> operator *(XYZA<T> a, XYZA<T> b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.A + b.A);

    public static implicit operator XYZ<T>(XYZA<T> a) => new(a.X, a.Y, a.Z);
}
