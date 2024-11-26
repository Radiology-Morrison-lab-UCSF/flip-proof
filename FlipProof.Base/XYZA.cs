using System.Numerics;

namespace FlipProof.Base;

public readonly record struct XYZA<T>(T X, T Y, T Z, T A) where T : INumber<T>
{
    public static XYZA<T> operator +(XYZA<T> a, XYZA<T> b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.A + b.A);
    public static XYZA<T> operator *(XYZA<T> a, XYZA<T> b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.A + b.A);

    public static implicit operator XYZ<T>(XYZA<T> a) => new(a.X, a.Y, a.Z);
}
