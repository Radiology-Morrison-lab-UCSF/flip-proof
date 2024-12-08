using System.Collections;
using System.Numerics;
using System.Runtime.InteropServices;

namespace FlipProof.Base;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)] // for span
public readonly record struct XYZ<T>(T X, T Y, T Z) : IReadOnlyList<T>
   where T : INumber<T>, IMultiplicativeIdentity<T,T>
{
   /// <summary>
   /// 
   /// </summary>
   /// <param name="index"></param>
   /// <returns></returns>
   /// <exception cref="IndexOutOfRangeException">Any value that is not 0, 1 or 2</exception>
   public T this[int index] => index switch
   {
      0 => X,
      1 => Y,
      2 => Z,
      _ => throw new IndexOutOfRangeException(),
   };

   int IReadOnlyCollection<T>.Count => 3;

   IEnumerator<T> IEnumerable<T>.GetEnumerator()
   {
      yield return X; 
      yield return Y; 
      yield return Z;
   }

   IEnumerator IEnumerable.GetEnumerator()
   {
      yield return X;
      yield return Y;
      yield return Z;
   }

   /// <summary>
   /// Casts to an int and throws if the conversion is not lossless
   /// </summary>
   /// <exception cref="InvalidCastException">Cast is not lossless</exception>
   public XYZ<int> ToIntLosslessly()
   {
      int x = Convert.ToInt32(X);
      int y = Convert.ToInt32(Y);
      int z = Convert.ToInt32(Z);

      if(x.CompareTo(X) != 0 || y.CompareTo(Y) != 0 || z.CompareTo(Z) != 0)
      {
         throw new InvalidCastException($"Cannot convert {x},{y},{z} to int losslessly");
      }

      return new XYZ<int>(x, y, z);
   }
   /// <summary>
   /// Casts to an int and throws if the conversion is not lossless
   /// </summary>
   /// <exception cref="InvalidCastException">Cast is not lossless</exception>
   /// <exception cref="OverflowException">Cast is not lossless</exception>
   [CLSCompliant(false)]
   public XYZ<uint> ToUIntLosslessly()
   {
      uint x = Convert.ToUInt32(X);
      uint y = Convert.ToUInt32(Y);
      uint z = Convert.ToUInt32(Z);

      if(T.CreateChecked(x) != X || T.CreateChecked(y) != Y || T.CreateChecked(z) != Z)
      {
         throw new InvalidCastException($"Cannot convert {x},{y},{z} to int losslessly");
      }

      return new XYZ<uint>(x, y, z);
   }

   /// <summary>
   /// Distance from (0,0,0) to (1,1,1)
   /// </summary>
   /// <returns></returns>
   public double Norm() => Math.Sqrt(Convert.ToDouble( X * X + Y * Y + Z * Z));


   public double DistanceTo(XYZ<T> p1) => (this - p1).Norm();





   /// <summary>
   /// Normalises to length of 1
   /// </summary>
   /// <returns></returns>
   public XYZ<double> ToNormalised()
   {
      XYZ<double> asD = (XYZ<double>)this;
      return asD / asD.Norm();
   }
   public static XYZ<T> operator *(XYZ<T> a, T b) => new(a.X * b, a.Y * b, a.Z * b);
   public static XYZ<T> operator *(XYZ<T> a, XYZ<T> b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
   public static XYZ<T> operator +(XYZ<T> a, XYZ<T> b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
   public static XYZ<T> operator -(XYZ<T> a, XYZ<T> b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
   public static XYZ<T> operator /(XYZ<T> a, T b) => new(a.X / b, a.Y / b, a.Z / b);
   public static explicit operator XYZ<int>(XYZ<T> a) => new XYZ<int>(Convert.ToInt32(a.X), Convert.ToInt32(a.Y), Convert.ToInt32(a.Z));
   public static explicit operator XYZ<long>(XYZ<T> a) => new XYZ<long>(Convert.ToInt64(a.X), Convert.ToInt64(a.Y), Convert.ToInt64(a.Z));
   public static explicit operator XYZ<double>(XYZ<T> a) => new XYZ<double>(Convert.ToDouble(a.X), Convert.ToDouble(a.Y), Convert.ToDouble(a.Z));

}