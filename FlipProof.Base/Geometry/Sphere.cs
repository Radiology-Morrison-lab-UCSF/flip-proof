namespace FlipProof.Base.Geometry;

public class Sphere(XYZ<float> position, float radius)
{
   public XYZ<float> p = position;
   public float r = radius;

   public double GetVolume()
   {
      return Math.PI * 4.0 * (double)(r * r * r) / 3.0;
   }

   public bool Contains(XYZ<float> pt)
   {
      return pt.DistanceTo(p) < r;
   }



   public XYZ<float> Seed(Random rng)
   {
      return new XYZ<float>(p.X + r * ((float)rng.NextDouble() * 2f - 1f), p.Y + r * ((float)rng.NextDouble() * 2f - 1f), p.Z + r * ((float)rng.NextDouble() * 2f - 1f));
   }


   public Box<int> GetContainingBoxI()
   {
      XYZ<int> origin = new((int)Math.Floor(p.X - r), (int)Math.Floor(p.Y - r), (int)Math.Floor(p.Z - r));
      XYZ<int> farCorner = new((int)Math.Ceiling(p.X + r), (int)Math.Ceiling(p.Y + r), (int)Math.Ceiling(p.Z + r));
      return new Box<int>(origin, farCorner - origin);
   }

   public override string ToString() => p.ToString() + "\tRadius:\t" + r;

   public bool Intersects(Sphere cur)
   {
      float dist = (cur.p - p).NormF();
      if (!(dist <= r))
      {
         return dist <= cur.r;
      }
      return true;
   }
}
