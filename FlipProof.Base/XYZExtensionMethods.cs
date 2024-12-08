namespace FlipProof.Base;
public static class XYZExtensionMethods
{
   public static float LengthFrom000F(this XYZ<float> xyz) => MathF.Sqrt(xyz.X * xyz.X + xyz.Y * xyz.Y + xyz.Z * xyz.Z);

   public static float DistanceToF(this XYZ<float> p1, XYZ<float> p2) => (p1 - p2).NormF();
   /// <summary>
   /// Distance from (0,0,0) to (1,1,1)
   /// </summary>
   /// <returns></returns>
   public static float NormF(this XYZ<float> p1) => MathF.Sqrt(p1.X * p1.X + p1.Y * p1.Y + p1.Z * p1.Z);
}
