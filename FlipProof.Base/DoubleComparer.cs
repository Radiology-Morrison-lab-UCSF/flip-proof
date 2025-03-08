using System.Collections;

namespace FlipProof.Base;

public class DoubleComparer : IEqualityComparer<double>, IComparer<double>, IComparer
{
   public double Tolerance { get; init; }
   public DoubleComparer(double tolerance = 0f)
   {
      Tolerance = tolerance < 0 ? throw new ArgumentException("Tolerance must be positive") : tolerance;
   }
   public bool Equals(double x, double y) => Math.Abs(x - y) <= Tolerance;
   public int GetHashCode(double obj) => obj.GetHashCode();

   public int Compare(double x, double y)
   {
      // Compare with tolerance
      if (Equals(x, y))
      {
         return 0;
      }
      return x.CompareTo(y);
   }

   int IComparer.Compare(object? x, object? y)
   {
      if(x is double dx && y is double dy)
      {
         return Compare(dx, dy);
      }
      throw new NotSupportedException("Only supports doubles");
   }
}
