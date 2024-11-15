namespace FlipProof.Base;

/// <summary>
/// Compares arrays with a tolerance
/// </summary>
/// <param name="tolerance"></param>
public class ArrayComparer_Double(double tolerance = 0f) : IEqualityComparer<double[]>
{
   public double Tolerance { get; init; } = tolerance < 0 ? throw new ArgumentException("Tolerance must be positive") : tolerance;

   public bool Equals(double[]? x, double[]? y)
   {
      if (x == null)
      {
         return y == null;
      }
      if (y == null)
      {
         return false;
      }
      return x.SequenceEqual_Fast(y, Tolerance);
   }
   int IEqualityComparer<double[]>.GetHashCode(double[] array)
	{
		int hc = array.Length;
		for (int i = 0; i < array.Length; i++)
		{
			hc = hc * 17 + (int)array[i];
		}
		return hc;
	}
}
