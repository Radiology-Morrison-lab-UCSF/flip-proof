using System.Collections.Generic;

namespace FlipProof.Image;

internal class ArrayComparer_Byte : IEqualityComparer<byte[]>
{
   bool IEqualityComparer<byte[]>.Equals(byte[] x, byte[] y)
   {
      return x.SequenceEqual(y);
   }

   int IEqualityComparer<byte[]>.GetHashCode(byte[] array)
   {
      int hc = array.Length;
      for (int i = 0; i < array.Length; i++)
      {
         hc = hc * 17 + array[i];
      }
      return hc;
   }
}
