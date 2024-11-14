using System.Collections.Generic;

namespace FlipProof.Image;

internal class ArrayComparer_Int32 : IEqualityComparer<int[]>
{
   bool IEqualityComparer<int[]>.Equals(int[]? x, int[]? y)
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
   int IEqualityComparer<int[]>.GetHashCode(int[] array)
	{
		int hc = array.Length;
		for (int i = 0; i < array.Length; i++)
		{
			hc = hc * 17 + array[i];
		}
		return hc;
	}
}
