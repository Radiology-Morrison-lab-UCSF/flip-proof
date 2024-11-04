using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace FlipProof.Image.Maths;

internal class UnsignedPoint : IEquatable<UnsignedPoint>, IComparable<UnsignedPoint>, ICloneable
{
    private static int _counter;

    protected uint[] _coordinates;

    private double _magnitude = double.NaN;

    private int _hashCode;

    public static readonly int MaxCoordinatesToShow = 10;

    public int UniqueId { get; protected set; }

    public virtual uint[] Coordinates
    {
        get
        {
            return _coordinates;
        }
        protected set
        {
            _coordinates = value;
        }
    }

    public int Dimensions { get; private set; }

    public long MaxCoordinate { get; private set; }

    public long SquareMagnitude { get; protected set; }

    public double Magnitude
    {
        get
        {
            if (double.IsNaN(_magnitude))
            {
                _magnitude = Math.Sqrt(SquareMagnitude);
            }
            return _magnitude;
        }
    }

    public virtual int this[int i] => (int)Coordinates[i];

    protected static int NextId()
    {
        return Interlocked.Increment(ref _counter);
    }

    public virtual IEnumerable<uint> LazyCoordinates()
    {
        uint[] coordinates = _coordinates;
        for (int i = 0; i < coordinates.Length; i++)
        {
            yield return coordinates[i];
        }
    }

    protected void InitInvariants(bool lazy = false)
    {
        if (lazy)
        {
            _hashCode = ComputeHashCode(LazyCoordinates(), Dimensions);
            if (MaxCoordinate != 0L || SquareMagnitude != 0L)
            {
                return;
            }
            {
                foreach (uint x in LazyCoordinates())
                {
                    MaxCoordinate = Math.Max(MaxCoordinate, x);
                    SquareMagnitude += x * (long)x;
                }
                return;
            }
        }
        _hashCode = ComputeHashCode(_coordinates, Dimensions);
        if (MaxCoordinate == 0L && SquareMagnitude == 0L)
        {
            uint[] coordinates = _coordinates;
            foreach (uint x2 in coordinates)
            {
                MaxCoordinate = Math.Max(MaxCoordinate, x2);
                SquareMagnitude += x2 * (long)x2;
            }
        }
    }

    public UnsignedPoint(uint[] coordinates)
        : this(coordinates, 0L, 0L)
    {
        InitInvariants();
    }

    public UnsignedPoint(IList<uint> coordinates, int id = -1)
        : this(coordinates.ToArray())
    {
        if (id >= 0)
        {
            UniqueId = id;
        }
    }

    public UnsignedPoint(int[] coordinates)
        : this(coordinates.Select((i) => (uint)i).ToArray())
    {
    }

    protected UnsignedPoint(uint[] coordinates, long maxCoordinate, long squareMagnitude)
    {
        UniqueId = NextId();
        _coordinates = coordinates;
        Dimensions = _coordinates.Length;
        _hashCode = ComputeHashCode(coordinates, Dimensions);
        MaxCoordinate = maxCoordinate;
        SquareMagnitude = squareMagnitude;
    }

    protected UnsignedPoint(uint[] coordinates, long maxCoordinate, long squareMagnitude, int uniqueId)
    {
        UniqueId = uniqueId >= 0 ? uniqueId : NextId();
        _coordinates = coordinates;
        Dimensions = _coordinates.Length;
        _hashCode = ComputeHashCode(coordinates, Dimensions);
        MaxCoordinate = maxCoordinate;
        SquareMagnitude = squareMagnitude;
    }

    public static uint[] MakeUnsigned(IList<int> p)
    {
        int dimensions = p.Count;
        uint[] coordinates = new uint[dimensions];
        for (int i = 0; i < dimensions; i++)
        {
            coordinates[i] = (uint)p[i];
        }
        return coordinates;
    }

    public static int[] MakeSigned(IList<uint> p)
    {
        int dimensions = p.Count;
        int[] coordinates = new int[dimensions];
        for (int i = 0; i < dimensions; i++)
        {
            coordinates[i] = (int)p[i];
        }
        return coordinates;
    }

    protected UnsignedPoint(UnsignedPoint original)
        : this((uint[])original.Coordinates.Clone(), original.MaxCoordinate, original.SquareMagnitude)
    {
    }

    public virtual object Clone()
    {
        return new UnsignedPoint(this);
    }

    private static int ComputeHashCode(IEnumerable<uint> vector, int vectorLength)
    {
        uint seed = (uint)vectorLength;
        foreach (uint i in vector)
        {
            seed ^= (uint)((int)i + -1640531527 + (int)(seed << 6)) + (seed >> 2);
        }
        return (int)seed;
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }

    public override bool Equals(object obj)
    {
        if (obj is UnsignedPoint p)
        {
            return Equals(p);
        }
        return false;
    }

    public bool Equals(UnsignedPoint other)
    {
        return UniqueId == other.UniqueId;
    }

    public virtual int CompareTo(UnsignedPoint other)
    {
        return UniqueId.CompareTo(other.UniqueId);
    }

    public virtual long SquareDistance(UnsignedPoint other)
    {
        return SquareDistanceDotProduct(Coordinates, other.Coordinates, SquareMagnitude, other.SquareMagnitude, MaxCoordinate, other.MaxCoordinate);
    }

    private static long SquareDistanceDotProduct(uint[] x, uint[] y, long xMag2, long yMag2, long xMax, long yMax)
    {
        if (xMax * yMax * 4 < uint.MaxValue)
        {
            return SquareDistanceDotProductNoOverflow(x, y, xMag2, yMag2);
        }
        ulong dotProduct = 0uL;
        int leftovers = x.Length % 4;
        int dimensions = x.Length;
        int roundDimensions = dimensions - leftovers;
        for (int j = 0; j < roundDimensions; j += 4)
        {
            uint x2 = x[j];
            ulong y2 = y[j];
            uint x3 = x[j + 1];
            ulong y3 = y[j + 1];
            uint x4 = x[j + 2];
            ulong y4 = y[j + 2];
            uint x5 = x[j + 3];
            ulong y5 = y[j + 3];
            dotProduct += x2 * y2 + x3 * y3 + x4 * y4 + x5 * y5;
        }
        for (int i = roundDimensions; i < dimensions; i++)
        {
            dotProduct += (ulong)(x[i] * (long)y[i]);
        }
        return xMag2 + yMag2 - (long)(2 * dotProduct);
    }

    private static long SquareDistanceDotProductNoOverflow(uint[] x, uint[] y, long xMag2, long yMag2)
    {
        ulong dotProduct = 0uL;
        int leftovers = x.Length % 4;
        int dimensions = x.Length;
        int roundDimensions = dimensions - leftovers;
        for (int j = 0; j < roundDimensions; j += 4)
        {
            dotProduct += x[j] * y[j] + x[j + 1] * y[j + 1] + x[j + 2] * y[j + 2] + x[j + 3] * y[j + 3];
        }
        for (int i = roundDimensions; i < dimensions; i++)
        {
            dotProduct += x[i] * y[i];
        }
        return xMag2 + yMag2 - (long)(2 * dotProduct);
    }

    public virtual int SquareDistanceCompare(UnsignedPoint other, long squareDistance)
    {
        double num = Magnitude - other.Magnitude;
        long low = (long)Math.Floor(num * num);
        if (squareDistance < low)
        {
            return 1;
        }
        long high = SquareMagnitude + other.SquareMagnitude;
        if (squareDistance > high)
        {
            return -1;
        }
        return SquareDistance(other).CompareTo(squareDistance);
    }

    public long Measure(UnsignedPoint reference)
    {
        return SquareDistance(reference);
    }

    public double Distance(UnsignedPoint other)
    {
        return Math.Sqrt(SquareDistance(other));
    }

    public int Range()
    {
        return (int)LazyCoordinates().Max();
    }

    public int GetDimensions()
    {
        return Dimensions;
    }

    public IEnumerable<int> GetCoordinates()
    {
        return from u in LazyCoordinates()
               select (int)u;
    }

    public override string ToString()
    {
        return AsString(MaxCoordinatesToShow);
    }

    public string AsString(int maxCoordinatesToShow = 0)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('[');
        int limit = Math.Min(maxCoordinatesToShow == 0 ? Dimensions : maxCoordinatesToShow, Dimensions);
        int dim = 0;
        foreach (uint x in LazyCoordinates().Take(limit))
        {
            dim++;
            if (dim > 0)
            {
                sb.Append(',');
            }
            sb.Append(x);
        }
        if (limit < Dimensions)
        {
            sb.Append(",...");
        }
        sb.Append(']');
        return sb.ToString();
    }

    public virtual UnsignedPoint AppendCoordinate(uint coordinate)
    {
        List<uint> list = Coordinates.ToList();
        list.Add(coordinate);
        return new UnsignedPoint(list);
    }

    public static UnsignedPoint Centroid(IEnumerable<UnsignedPoint> points)
    {
        double[] sums = null;
        int numPoints = 0;
        foreach (UnsignedPoint p in points)
        {
            if (sums == null)
            {
                sums = new double[p.Dimensions];
            }
            int dim2 = 0;
            foreach (uint x in p.LazyCoordinates())
            {
                sums[dim2++] += x;
            }
            numPoints++;
        }
        if (numPoints == 0)
        {
            return null;
        }
        uint[] coords = new uint[sums.Length];
        for (int dim = 0; dim < sums.Length; dim++)
        {
            coords[dim] = (uint)Math.Round(sums[dim] / numPoints);
        }
        return new UnsignedPoint(coords);
    }

    public static BigInteger CalcHilbertIndex(uint[] hilbertAxes, int bits)
    {
        return HilbertInterleaver.Instance(hilbertAxes.Length, bits).Untranspose(HilbertIndexTransposed(hilbertAxes, bits));
    }

    public static uint[] HilbertAxes(uint[] transposedIndex, int bits)
    {
        uint[] X = (uint[])transposedIndex.Clone();
        int j = X.Length;
        uint N = (uint)(2 << bits - 1);
        uint t = X[j - 1] >> 1;
        for (int i = j - 1; i > 0; i--)
        {
            X[i] ^= X[i - 1];
        }
        X[0] ^= t;
        for (uint Q = 2u; Q != N; Q <<= 1)
        {
            uint P = Q - 1;
            for (int i = j - 1; i >= 0; i--)
            {
                if ((X[i] & Q) != 0)
                {
                    X[0] ^= P;
                }
                else
                {
                    t = (X[0] ^ X[i]) & P;
                    X[0] ^= t;
                    X[i] ^= t;
                }
            }
        }
        return X;
    }

    public static uint[] HilbertIndexTransposed(uint[] hilbertAxes, int bits)
    {
        uint[] X = (uint[])hilbertAxes.Clone();
        int j = hilbertAxes.Length;
        uint M = (uint)(1 << bits - 1);
        uint t;
        for (uint Q = M; Q > 1; Q >>= 1)
        {
            uint P = Q - 1;
            for (int i = 0; i < j; i++)
            {
                if ((X[i] & Q) != 0)
                {
                    X[0] ^= P;
                    continue;
                }
                t = (X[0] ^ X[i]) & P;
                X[0] ^= t;
                X[i] ^= t;
            }
        }
        for (int i = 1; i < j; i++)
        {
            X[i] ^= X[i - 1];
        }
        t = 0u;
        for (uint Q = M; Q > 1; Q >>= 1)
        {
            if ((X[j - 1] & Q) != 0)
            {
                t ^= Q - 1;
            }
        }
        for (int i = 0; i < j; i++)
        {
            X[i] ^= t;
        }
        return X;
    }
}
