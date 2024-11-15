namespace FlipProof.Base;

/// <summary>
/// Compares arrays with a tolerance
/// </summary>
/// <param name="tolerance"></param>
public class ArrayComparer_Float(float tolerance = 0f) : IEqualityComparer<float[]>
{
   public float Tolerance { get; init; } = tolerance < 0 ? throw new ArgumentException("Tolerance must be positive") : tolerance;

   public bool Equals(float[]? x, float[]? y)
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
   int IEqualityComparer<float[]>.GetHashCode(float[] array)
	{
		int hc = array.Length;
		for (int i = 0; i < array.Length; i++)
		{
			hc = hc * 17 + (int)array[i];
		}
		return hc;
	}
}
