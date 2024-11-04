using System;

namespace FlipProof.Image.Maths;

internal class Snorm
{
    private const ulong uint64_1 = 1uL;

    private short m_bits;

    private const int bitcount = 16;

    internal short bits => m_bits;

    private Snorm(short b)
    {
        m_bits = b;
    }

    public Snorm()
    {
    }

    public static Snorm fromBits(short b)
    {
        return new Snorm(b);
    }

    public Snorm(float f)
    {
        m_bits = (short)Math.Round(clamp(f, -1f, 1f) * 32767f);
    }

    public static Snorm flooredSnorms(float f)
    {
        return fromBits((short)Math.Floor(clamp(f, -1f, 1f) * 32767f));
    }

    public static explicit operator float(Snorm s)
    {
        return Convert.ToSingle(clamp(s.m_bits * 3.051851E-05f, -1f, 1f));
    }

    public static float clamp(float val, float low, float hi)
    {
        if (val <= low)
        {
            return low;
        }
        if (val >= hi)
        {
            return hi;
        }
        return val;
    }
}
