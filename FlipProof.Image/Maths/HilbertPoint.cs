using System.Numerics;

namespace FlipProof.Image.Maths;

internal class HilbertPoint : UnsignedPoint, IEquatable<HilbertPoint>, IComparable<HilbertPoint>
{
   public BigInteger HilbertIndex { get; private set; }

   public int BitsPerDimension { get; private set; }

   public static List<HilbertPoint> Transform(IList<IList<int>> points, int bitsPerDimension = 0)
   {
      if (bitsPerDimension <= 0)
      {
         bitsPerDimension = FindBitsPerDimension(points);
      }
      return points.Select((point) => new HilbertPoint(point, bitsPerDimension)).ToList();
   }

   public HilbertPoint(uint[] coordinates, int bitsPerDimension)
       : base(coordinates)
   {
      BitsPerDimension = bitsPerDimension;
      HilbertIndex = CalcHilbertIndex(coordinates, BitsPerDimension);
   }

   public HilbertPoint(IList<uint> coordinates, int bitsPerDimension)
       : this(coordinates.ToArray(), bitsPerDimension)
   {
   }

   public HilbertPoint(IList<int> coordinates, IEnumerable<int> coordinateRanges)
       : this(MakeUnsigned(coordinates), FindBitsPerDimension(coordinateRanges))
   {
   }

   public HilbertPoint(IList<int> coordinates, int bitsPerDimension)
       : this(MakeUnsigned(coordinates), bitsPerDimension)
   {
   }

   public static int FindBitsPerDimension(IEnumerable<int> maxValuePerDimension)
   {
      return SmallestPowerOfTwo(maxValuePerDimension.Max() + 1);
   }

   public static int FindBitsPerDimension(int max)
   {
      return SmallestPowerOfTwo(max + 1);
   }

   public static int FindBitsPerDimension(IList<IList<int>> points)
   {
      int max = 1;
      foreach (IList<int> point in points)
      {
         foreach (int coordinate in point)
         {
            max |= coordinate;
         }
      }
      return SmallestPowerOfTwo(max + 1);
   }

   public HilbertPoint(uint[] coordinates, int bitsPerDimension, long maxCoordinate, long squareMagnitude)
       : base(coordinates, maxCoordinate, squareMagnitude)
   {
      BitsPerDimension = bitsPerDimension;
      HilbertIndex = CalcHilbertIndex(coordinates, BitsPerDimension);
   }

   public HilbertPoint(uint[] coordinates, int bitsPerDimension, long maxCoordinate, long squareMagnitude, int key)
       : base(coordinates, maxCoordinate, squareMagnitude, key)
   {
      BitsPerDimension = bitsPerDimension;
      HilbertIndex = CalcHilbertIndex(coordinates, BitsPerDimension);
   }

   public static int SmallestPowerOfTwo(int n)
   {
      int logTwo = 0;
      uint r = 1u;
      while (r < n)
      {
         r <<= 1;
         logTwo++;
      }
      return logTwo;
   }

   public override bool Equals(object? obj)
   {
      if (obj is HilbertPoint p)
      {
         return Equals(p);
      }
      return false;
   }

   public bool Equals(HilbertPoint? other)
   {
      return Equals(other as UnsignedPoint);
   }
   public override int GetHashCode()
   {
      return HilbertIndex.GetHashCode();
   }
   public int CompareTo(HilbertPoint? other)
   {
      int cmp = other == null ? -1 : HilbertIndex.CompareTo(other.HilbertIndex);
      if (cmp == 0)
      {
         cmp = UniqueId.CompareTo(other!.UniqueId);
      }
      return cmp;
   }

   public BigInteger HilbertDistance(HilbertPoint other)
   {
      return BigInteger.Abs(HilbertIndex - other.HilbertIndex);
   }

   public static void SortByHilbertIndex(List<HilbertPoint> points)
   {
      points.Sort();
   }

   public long Measure(HilbertPoint reference)
   {
      return Measure((UnsignedPoint)reference);
   }

   public override UnsignedPoint AppendCoordinate(uint coordinate)
   {
      int limit = 1 << BitsPerDimension;
      if (limit <= coordinate)
      {
         throw new ArgumentOutOfRangeException(nameof(coordinate), $"Value must be smaller than 2^BitsPerDimension, which is {limit}");
      }
      List<uint> list = LazyCoordinates().ToList();
      list.Add(coordinate);
      return new HilbertPoint(list, BitsPerDimension);
   }
}
