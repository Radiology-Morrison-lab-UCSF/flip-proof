using System;

namespace FlipProof.Image.Maths;

public static class Number
{
    public const double SmallestNumberGreaterThanZero = double.Epsilon;

    public static readonly double RelativeAccuracy = EpsilonOf(1.0);

    public static readonly double PositiveRelativeAccuracy = PositiveEpsilonOf(1.0);

    public static readonly double DefaultRelativeAccuracy = 10.0 * PositiveRelativeAccuracy;

    public static double EpsilonOf(double value)
    {
        if (double.IsInfinity(value) || double.IsNaN(value))
        {
            return double.NaN;
        }
        long signed64 = BitConverter.DoubleToInt64Bits(value);
        if (signed64 == 0L)
        {
            signed64++;
            return BitConverter.Int64BitsToDouble(signed64) - value;
        }
        if (signed64-- < 0)
        {
            return BitConverter.Int64BitsToDouble(signed64) - value;
        }
        return value - BitConverter.Int64BitsToDouble(signed64);
    }

    public static double PositiveEpsilonOf(double value)
    {
        return 2.0 * EpsilonOf(value);
    }

    public static double Increment(double value)
    {
        if (double.IsInfinity(value) || double.IsNaN(value))
        {
            return value;
        }
        long signed64 = BitConverter.DoubleToInt64Bits(value);
        signed64 = signed64 >= 0 ? signed64 + 1 : signed64 - 1;
        if (signed64 == long.MinValue)
        {
            return 0.0;
        }
        value = BitConverter.Int64BitsToDouble(signed64);
        if (!double.IsNaN(value))
        {
            return value;
        }
        return double.NaN;
    }

    public static double Decrement(double value)
    {
        if (double.IsInfinity(value) || double.IsNaN(value))
        {
            return value;
        }
        long signed64 = BitConverter.DoubleToInt64Bits(value);
        if (signed64 == 0L)
        {
            return -5E-324;
        }
        signed64 = signed64 >= 0 ? signed64 - 1 : signed64 + 1;
        value = BitConverter.Int64BitsToDouble(signed64);
        if (!double.IsNaN(value))
        {
            return value;
        }
        return double.NaN;
    }

    [CLSCompliant(false)]
    public static ulong NumbersBetween(double a, double b)
    {
        if (double.IsNaN(a) || double.IsInfinity(a))
        {
            throw new ArgumentException("Value must neither be infinite nor NaN", "a");
        }
        if (double.IsNaN(b) || double.IsInfinity(b))
        {
            throw new ArgumentException("Value must neither be infinite nor NaN", "b");
        }
        ulong ua = ToLexicographicalOrderedUInt64(a);
        ulong ub = ToLexicographicalOrderedUInt64(b);
        if (!(a >= b))
        {
            return ub - ua;
        }
        return ua - ub;
    }

    [CLSCompliant(false)]
    public static ulong ToLexicographicalOrderedUInt64(double value)
    {
        ulong unsigned64;
        if ((long)(unsigned64 = (ulong)BitConverter.DoubleToInt64Bits(value)) < 0L)
        {
            return 9223372036854775808uL - unsigned64;
        }
        return unsigned64;
    }

    public static long ToLexicographicalOrderedInt64(double value)
    {
        long signed64 = BitConverter.DoubleToInt64Bits(value);
        if (signed64 < 0)
        {
            return long.MinValue - signed64;
        }
        return signed64;
    }

    [CLSCompliant(false)]
    public static ulong SignedMagnitudeToTwosComplementUInt64(long value)
    {
        if (value < 0)
        {
            return (ulong)(long.MinValue - value);
        }
        return (ulong)value;
    }

    public static long SignedMagnitudeToTwosComplementInt64(long value)
    {
        if (value < 0)
        {
            return long.MinValue - value;
        }
        return value;
    }

    public static bool AlmostEqual(double a, double b, int maxNumbersBetween)
    {
        return AlmostEqual(a, b, (ulong)maxNumbersBetween);
    }

    [CLSCompliant(false)]
    public static bool AlmostEqual(double a, double b, ulong maxNumbersBetween)
    {
        if (maxNumbersBetween < 0)
        {
            throw new ArgumentException("Value must not be negative (zero is ok)", "maxNumbersBetween");
        }
        if (double.IsNaN(a) || double.IsNaN(b))
        {
            return false;
        }
        if (a == b)
        {
            return true;
        }
        if (double.IsInfinity(a) || double.IsInfinity(b))
        {
            return false;
        }
        return NumbersBetween(a, b) <= maxNumbersBetween;
    }

    public static bool AlmostEqualNorm(double a, double b, double diff, double relativeAccuracy)
    {
        if (a == 0.0 && Math.Abs(b) < relativeAccuracy || b == 0.0 && Math.Abs(a) < relativeAccuracy)
        {
            return true;
        }
        return Math.Abs(diff) < relativeAccuracy * Math.Max(Math.Abs(a), Math.Abs(b));
    }

    public static bool AlmostEqualNorm(double a, double b, double diff)
    {
        return AlmostEqualNorm(a, b, diff, DefaultRelativeAccuracy);
    }

    public static bool AlmostEqual(double a, double b, double relativeAccuracy)
    {
        return AlmostEqualNorm(a, b, a - b, relativeAccuracy);
    }

    public static bool AlmostEqual(double a, double b)
    {
        return AlmostEqualNorm(a, b, a - b, DefaultRelativeAccuracy);
    }

    public static bool AlmostEqual(double[] x, double[] y)
    {
        if (x.Length != y.Length)
        {
            return false;
        }
        for (int i = 0; i < x.Length; i++)
        {
            if (!AlmostEqual(x[i], y[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool AlmostZero(double a, double absoluteAccuracy)
    {
        return Math.Abs(a) < absoluteAccuracy;
    }

    public static bool AlmostZero(double a)
    {
        return Math.Abs(a) < DefaultRelativeAccuracy;
    }

    internal static void Log2_Double(double d, out double mantissa, out int exponent)
    {
        Frxp_Double(d, out var mantissa_l, out exponent, out var _);
        mantissa = mantissa_l;
        while (mantissa >= 1.0)
        {
            mantissa /= 2.0;
            exponent++;
        }
    }

    internal static void Frxp_Double(double d, out long mantissa, out int exponent)
    {
        Frxp_Double(d, out mantissa, out exponent, out var _);
    }

    internal static void Frxp_Double(double d, out long mantissa, out int exponent, out bool negative)
    {
        long bits = BitConverter.DoubleToInt64Bits(d);
        negative = bits < 0;
        exponent = (int)(bits >> 52 & 0x7FF);
        mantissa = bits & 0xFFFFFFFFFFFFFL;
        if (exponent == 0)
        {
            exponent++;
        }
        else
        {
            mantissa |= 4503599627370496L;
        }
        exponent -= 1075;
        if (mantissa != 0L)
        {
            while ((mantissa & 1) == 0L)
            {
                mantissa >>= 1;
                exponent++;
            }
        }
    }
}
