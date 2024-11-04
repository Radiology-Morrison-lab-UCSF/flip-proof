using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.Base;
public static class Array4DExtensionMethods
{
   public static IEnumerable<XYZA<int>> GetIndicesSortedByValueDescending<T>(this Array4D<T> arr, Array3D<bool>? mask = null) where T:struct, IComparable<T>
   {

      XYZ<int>[][] byArray = arr.Data.Select(a => a.GetIndicesSortedByValueDescending()).ToArray();

      int[] offsets = new int[byArray.Length];
      bool maskNull = mask == null;
      while (TryGetNextLargestIndex(out XYZA<int> nextLargest))
      {
         yield return nextLargest;

         offsets[nextLargest.A]++;
      }

      bool TryGetNextLargestIndex(out XYZA<int> largestIndex)
      {
         bool any = false;
         T largestVal = default;
         largestIndex = default;
         for (int i = 0; i < byArray.Length; i++)
         {
            XYZ<int>[] curCoordsList = byArray[i];
            int curOffset = offsets[i];
            if (curOffset < curCoordsList.Length)
            {
               XYZ<int> curCoord = curCoordsList[curOffset];
               var currentVal = arr.Data[i][curCoord];
               if ((maskNull || mask![curCoord.X, curCoord.Y, curCoord.Z]) &&
                  ((!any) || currentVal.CompareTo(largestVal) > 0))
               {
                  largestVal = currentVal;
                  largestIndex = new(curCoord.X, curCoord.Y, curCoord.Z, i);
                  any = true;
               }
            }
         }

         return any;
      }
   }
}
