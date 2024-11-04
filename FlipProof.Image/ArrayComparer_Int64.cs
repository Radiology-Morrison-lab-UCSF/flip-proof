using System.Collections.Generic;

namespace FlipProof.Image;

internal class ArrayComparer_Int64 : IEqualityComparer<long[]>
{
	bool IEqualityComparer<long[]>.Equals(long[] x, long[] y)
	{
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
