using System.Collections.Generic;

namespace FlipProof.Image;

internal class ArrayComparer_Int16 : IEqualityComparer<short[]>
{

   bool IEqualityComparer<short[]>.Equals(short[]? x, short[]? y)
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
   int IEqualityComparer<short[]>.GetHashCode(short[] array)
	{
		int hc = array.Length;
		for (int i = 0; i < array.Length; i++)
		{
			hc = hc * 17 + array[i];
		}
		return hc;
	}
}
