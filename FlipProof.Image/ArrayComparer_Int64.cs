using System.Collections.Generic;

namespace FlipProof.Image;

internal class ArrayComparer_Int64 : IEqualityComparer<long[]>
{
   bool IEqualityComparer<Int64[]>.Equals(Int64[]? x, Int64[]? y)
   {
      if (x == null)
      {
         return y == null;
      }
      if (y == null)
      {
         return false;
      }
      return x.SequenceEqual(y);
   }
   int IEqualityComparer<long[]>.GetHashCode(long[] array)
	{
		int hc = array.Length;
		for (int i = 0; i < array.Length; i++)
		{
			hc = hc * 17 + (int)array[i];
		}
		return hc;
	}
}
