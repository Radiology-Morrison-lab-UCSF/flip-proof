
namespace FlipProof.Base;
public readonly record struct XYZf(float X, float Y, float Z)
{
   public float DistanceTo(XYZf p1)
   {
      float x = this.X - p1.X;
      float y = this.Y - p1.Y;
      float z = this.Z - p1.Z;
      return MathF.Sqrt(x * x + y * y + z * z);
   }

   public float LengthFrom000() => MathF.Sqrt(X * X + Y * Y + Z * Z);

   public static XYZf operator +(XYZf a, XYZf b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
   public static XYZf operator -(XYZf a, XYZf b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
   public static XYZf operator *(XYZf a, float b) => new(a.X * b, a.Y * b, a.Z * b);
   public static XYZf operator *(XYZf a, XYZf b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
   public static XYZf operator /(XYZf a, float b) => new(a.X / b, a.Y / b, a.Z / b);
   public static explicit operator XYZ<int> (XYZf a) => new XYZ<int>(Convert.ToInt32(a.X), Convert.ToInt32(a.Y), Convert.ToInt32(a.Z));
   public static explicit operator XYZ<long> (XYZf a) => new XYZ<long>(Convert.ToInt64(a.X), Convert.ToInt64(a.Y), Convert.ToInt64(a.Z));

   public override int GetHashCode()
   {
      unchecked
      {
         return Convert.ToInt32(X + Y * 3571 + Z * 1301081);
      }
   }

   /// <summary>
   /// Normalises to length of 1
   /// </summary>
   /// <returns></returns>
   public XYZf ToNormalised() => this / LengthFrom000();
}
