namespace FlipProof.Base;
public static class XYZExtensionMethods
{
   public static double LengthFrom000(this XYZ<double> xyz) => Math.Sqrt(xyz.X * xyz.X + xyz.Y * xyz.Y + xyz.Z * xyz.Z);

}
