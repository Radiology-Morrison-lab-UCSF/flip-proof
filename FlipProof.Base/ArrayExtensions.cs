using FlipProof.Base.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.Base;

public static class ArrayExtensions
{
   public static bool Contains_AssumeSorted<T>(this T[] sortedArray, T lookFor) where T : IComparable<T> => Array.BinarySearch(sortedArray, lookFor) >= 0;


   public static void FillArray<T>(this T[] vals, T val)
   {
      for (int i = 0; i < vals.Length; i++)
      {
         vals[i] = val;
      }
   }

   public static void FillArray<T>(this T[] vals, Func<T> val)
   {
      for (int i = 0; i < vals.Length; i++)
      {
         vals[i] = val.Invoke();
      }
   }


   public static T[] Flatten<T>(this T[][] jagged)
   {
      int offset = 0;
      T[] result = new T[jagged.Sum((T[] a) => a.Length)];
      foreach (T[] from in jagged)
      {
         Array.Copy(from, 0, result, offset, from.Length);
         offset += from.Length;
      }
      return result;
   }

   public static string[][] Transpose(this string[][] bySubjectThenLine)
   {
      string[][] trans = CollectionCreation.JaggedArray<string>(bySubjectThenLine[0].Length, bySubjectThenLine.Length);
      for (int i = 0; i < bySubjectThenLine.Length; i++)
      {
         string[] curCol = bySubjectThenLine[i];
         if (trans.Length != curCol.Length)
         {
            throw new ArgumentException("Input is not rectangular. All arrays in jagged array must be the same length");
         }
         for (int j = 0; j < trans.Length; j++)
         {
            trans[j][i] = curCol[j];
         }
      }
      return trans;
   }

}


public static class IEnumerableExtensionMethods
{

   public static IEnumerable<int> Cumulative(IEnumerable<int> arr, bool startAtZeroAndSkipLastValue)
   {
      int curSum = 0;
      if (startAtZeroAndSkipLastValue)
      {
         foreach (int cur in arr)
         {
            yield return curSum;
            curSum += cur;
         }
         yield break;
      }
      foreach (int cur2 in arr)
      {
         curSum += cur2;
         yield return curSum;
      }
   }

   public static IEnumerable<double> Cumulative(IEnumerable<double> arr, bool startAtZeroAndSkipLastValue)
   {
      double curSum = 0.0;
      if (startAtZeroAndSkipLastValue)
      {
         foreach (double cur in arr)
         {
            yield return curSum;
            curSum += cur;
         }
         yield break;
      }
      foreach (double cur2 in arr)
      {
         curSum += cur2;
         yield return curSum;
      }
   }


   public static List<T> Distinct<T>(this IEnumerable<T> dat, Func<T, T, bool> equality)
   {
      List<T> found = new List<T>();
      foreach (T cur in dat)
      {
         if (!found.Any((T a) => equality(a, cur)))
         {
            found.Add(cur);
         }
      }
      return found;
   }

   public static IEnumerable<T> ForEach<T>(this IEnumerable<T> me, Action<T> a)
   {
      foreach (T cur in me)
      {
         a(cur);
      }
      return me;
   }


   public static IEnumerable<T> ForEach<T>(this IEnumerable<T> me, Action<T, int> a)
   {
      int count = 0;
      foreach (T cur in me)
      {
         a(cur, count);
         count++;
      }
      return me;
   }

   public static IEnumerable<T> ForEach<T>(this IEnumerable<T> me, Action<T> actionFirstElement, Action<T, T, int> actionSubsequent)
   {
      int count = 0;
      T prev = default!;// this value is never used
      foreach (T cur in me)
      {
         if (count == 0)
         {
            actionFirstElement(cur);
         }
         else
         {
            actionSubsequent(cur, prev, count);
         }
         count++;
         prev = cur;
      }
      return me;
   }

   public static int IndexOf<T>(this IEnumerable<T> coll, T lookFor) where T : IEquatable<T>
   {
      int i = 0;
      foreach (T item in coll)
      {
         if (item.Equals(lookFor))
         {
            return i;
         }
         i++;
      }
      return -1;
   }

   public static int IndexOfClass<T>(this IEnumerable<T> coll, T lookFor) where T : class
   {
      int i = 0;
      foreach (T item in coll)
      {
         if (item == lookFor)
         {
            return i;
         }
         i++;
      }
      return -1;
   }

   public static int IndexOf<T>(this IEnumerable<T> coll, Func<T, bool> lookFor)
   {
      int i = 0;
      foreach (T cur in coll)
      {
         if (lookFor(cur))
         {
            return i;
         }
         i++;
      }
      return -1;
   }

   public static int IndexOf<T>(this IEnumerable<T> coll, Func<T, int, bool> lookFor)
   {
      int i = 0;
      foreach (T cur in coll)
      {
         if (lookFor(cur, i))
         {
            return i;
         }
         i++;
      }
      return -1;
   }
   public static IEnumerable<int> IndexOfAll<T>(this IEnumerable<T> coll, Func<T, bool> lookFor)
   {
      coll.Count();
      int i = 0;
      foreach (T cur in coll)
      {
         if (lookFor(cur))
         {
            yield return i;
         }
         i++;
      }
   }

   public static IEnumerable<int> IndexOfAll<T>(this IEnumerable<T> coll, Func<T, int, bool> lookFor)
   {
      coll.Count();
      int i = 0;
      foreach (T cur in coll)
      {
         if (lookFor(cur, i))
         {
            yield return i;
         }
         i++;
      }
   }


   public static int IndexOf_AssumeSorted<T>(this T[] coll, T lookFor) where T : IEquatable<T>, IComparable<T>
   {
      int index = Array.BinarySearch(coll, lookFor);
      if (index < 0)
      {
         return -1;
      }
      return index;
   }



   public static IEnumerable<int> IndexOfAll_Inverse<T>(this IEnumerable<T> coll, Func<T, int, bool> lookFor)
   {
      int count = coll.Count();
      int i = count - 1;
      foreach (T cur in coll.Reverse())
      {
         if (lookFor(cur, i))
         {
            yield return i;
         }
         i--;
      }
   }

   public static int IndexOfLast<T>(this IEnumerable<T> coll, Func<T, bool> lookFor)
   {
      int i = coll.Count() - 1;
      foreach (T cur in coll.Reverse())
      {
         if (lookFor(cur))
         {
            return i;
         }
         i--;
      }
      return -1;
   }

   public static int IndexOfLast<T>(this IEnumerable<T> coll, Func<T, int, bool> lookFor)
   {
      int i = coll.Count() - 1;
      foreach (T cur in coll.Reverse())
      {
         if (lookFor(cur, i))
         {
            return i;
         }
         i--;
      }
      return -1;
   }

   public static IEnumerable<S> Select<T, S>(this IEnumerable<T> me, Func<T, S> actionFirstElement, Func<T, T, int, S> actionSubsequent)
   {
      int count = 0;
      T prev = default!; // this value is never used
      foreach (T cur in me)
      {
         if (count == 0)
         {
            yield return actionFirstElement(cur);
         }
         else
         {
            yield return actionSubsequent(cur, prev, count);
         }
         count++;
         prev = cur;
      }
   }

   public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> me)
   {
      return me.SelectMany((IEnumerable<T> a) => a);
   }


   public unsafe static bool SequenceEqual_Fast(this Line[] a1, Line[] a2)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (Line* a1_source = a1)
      {
         fixed (Line* a2_source = a2)
         {
            Line* p_a1 = a1_source;
            Line* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (!p_a1->Equals(*p_a2))
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   public unsafe static bool SequenceEqual_Fast(this bool[] a1, bool[] a2)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (bool* a1_source = a1)
      {
         fixed (bool* a2_source = a2)
         {
            bool* p_a1 = a1_source;
            bool* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (!p_a1->Equals(*p_a2))
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   [CLSCompliant(false)]
   public unsafe static bool SequenceEqual_Fast(this sbyte[] a1, sbyte[] a2)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (sbyte* a1_source = a1)
      {
         fixed (sbyte* a2_source = a2)
         {
            sbyte* p_a1 = a1_source;
            sbyte* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (!p_a1->Equals(*p_a2))
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   public unsafe static bool SequenceEqual_Fast(this byte[] a1, byte[] a2)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (byte* a1_source = a1)
      {
         fixed (byte* a2_source = a2)
         {
            byte* p_a1 = a1_source;
            byte* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (!p_a1->Equals(*p_a2))
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   [CLSCompliant(false)]
   public unsafe static bool SequenceEqual_Fast(this ushort[] a1, ushort[] a2)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (ushort* a1_source = a1)
      {
         fixed (ushort* a2_source = a2)
         {
            ushort* p_a1 = a1_source;
            ushort* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (!p_a1->Equals(*p_a2))
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   [CLSCompliant(false)]
   public unsafe static bool SequenceEqual_Fast(this uint[] a1, uint[] a2)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (uint* a1_source = a1)
      {
         fixed (uint* a2_source = a2)
         {
            uint* p_a1 = a1_source;
            uint* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (!p_a1->Equals(*p_a2))
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   public unsafe static bool SequenceEqual_Fast(this float[] a1, float[] a2)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (float* a1_source = a1)
      {
         fixed (float* a2_source = a2)
         {
            float* p_a1 = a1_source;
            float* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (!p_a1->Equals(*p_a2))
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   public unsafe static bool SequenceEqual_Fast(this float[] a1, float[] a2, float tolerance = 1E-07f)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (float* a1_source = a1)
      {
         fixed (float* a2_source = a2)
         {
            float* p_a1 = a1_source;
            float* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (Math.Abs(*p_a1 - *p_a2) > tolerance)
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   public unsafe static bool SequenceEqual_Fast(this int[] a1, int[] a2, int tolerance = 0)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (int* a1_source = a1)
      {
         fixed (int* a2_source = a2)
         {
            int* p_a1 = a1_source;
            int* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (Math.Abs(*p_a1 - *p_a2) > tolerance)
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   public unsafe static bool SequenceEqual_Fast(this long[] a1, long[] a2, long tolerance = 0L)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (long* a1_source = a1)
      {
         fixed (long* a2_source = a2)
         {
            long* p_a1 = a1_source;
            long* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (Math.Abs(*p_a1 - *p_a2) > tolerance)
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }


   public unsafe static bool SequenceEqual_Fast(this double[] a1, double[] a2, double tolerance = 1E-07)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (double* a1_source = a1)
      {
         fixed (double* a2_source = a2)
         {
            double* p_a1 = a1_source;
            double* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (Math.Abs(*p_a1 - *p_a2) > tolerance)
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   public unsafe static bool SequenceEqual_Fast(this short[] a1, short[] a2)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (short* a1_source = a1)
      {
         fixed (short* a2_source = a2)
         {
            short* p_a1 = a1_source;
            short* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if ((double)Math.Abs(*p_a1 - *p_a2) > 1E-07)
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   public unsafe static bool SequenceEqual_Fast(this int[] a1, int[] a2)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (int* a1_source = a1)
      {
         fixed (int* a2_source = a2)
         {
            int* p_a1 = a1_source;
            int* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if (*p_a1 != *p_a2)
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   public unsafe static bool SequenceEqual_Fast(this long[] a1, long[] a2)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      fixed (long* a1_source = a1)
      {
         fixed (long* a2_source = a2)
         {
            long* p_a1 = a1_source;
            long* p_a2 = a2_source;
            int to = a1.Length;
            for (int i = 0; i < to; i++)
            {
               if ((double)Math.Abs(*p_a1 - *p_a2) > 1E-07)
               {
                  return false;
               }
               p_a1++;
               p_a2++;
            }
         }
      }
      return true;
   }

   public static bool SequenceEqual<T>(this T[] a1, T[] a2)
   {
      if (a1 is Line[] al)
      {
         return al.SequenceEqual_Fast((a2 as Line[])!);
      }
      if (a1 is byte[] ab)
      {
         return SequenceEqual_Fast(ab, (a2 as byte[])!);
      }
      if (a1 is sbyte[] asb)
      {
         return SequenceEqual_Fast(asb, (a2 as sbyte[])!);
      }
      if (a1 is ushort[] aus)
      {
         return SequenceEqual_Fast(aus, (a2 as ushort[])!);
      }
      if (a1 is int[] ai)
      {
         return SequenceEqual_Fast(ai, (a2 as int[])!);
      }
      if (a1 is uint[] au)
      {
         return SequenceEqual_Fast(au, (a2 as uint[])!);
      }
      if (a1 is float[] af)
      {
         return SequenceEqual_Fast(af, (a2 as float[])!, 0f);
      }
      if (a1 is double[] ad)
      {
         return SequenceEqual_Fast(ad, (a2 as double[])!, 0.0);
      }
      if (a1 is bool[] abool)
      {
         return SequenceEqual_Fast(abool, (a2 as bool[])!);
      }
      return Enumerable.SequenceEqual(a1, a2);
   }

   public static bool SequenceEqual_JaggedElementWise(this double[][] a1, double[][] a2, double tolerance)
   {
      if (a1.Length != a2.Length)
      {
         return false;
      }
      for (int i = 0; i < a1.Length; i++)
      {
         double[] a3 = a1[i];
         double[] curB = a2[i];
         if (!a3.SequenceEqual_Fast(curB, tolerance))
         {
            return false;
         }
      }
      return true;
   }


   public static void ZipAction<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Action<TFirst, TSecond> action)
   {
      if (first == null)
      {
         throw new ArgumentNullException("first");
      }
      if (second == null)
      {
         throw new ArgumentNullException("second");
      }
      if (action == null)
      {
         throw new ArgumentNullException("resultSelector");
      }
      using IEnumerator<TFirst> e1 = first.GetEnumerator();
      using IEnumerator<TSecond> e2 = second.GetEnumerator();
      while (e1.MoveNext() && e2.MoveNext())
      {
         action(e1.Current, e2.Current);
      }
   }
}






public static class Iteration
{

   public static void Loop3DXYZ(int xFrom, int xToExclusive, int yFrom, int yToExclusive, int zFrom, int zToExclusive, Action<int, int, int> act)
   {
      for (int x = xFrom; x < xToExclusive; x++)
      {
         for (int y = yFrom; y < yToExclusive; y++)
         {
            for (int z = zFrom; z < zToExclusive; z++)
            {
               act(x, y, z);
            }
         }
      }
   }

   public static void Loop3DXYZ(int xFrom, int xToExclusive, int yFrom, int yToExclusive, int zFrom, int zToExclusive, Action<int, int, int, int> act)
   {
      int i = 0;
      for (int x = xFrom; x < xToExclusive; x++)
      {
         for (int y = yFrom; y < yToExclusive; y++)
         {
            for (int z = zFrom; z < zToExclusive; z++)
            {
               act(x, y, z, i);
               i++;
            }
         }
      }
   }

   public static IEnumerable<XYZ<int>> Loop3DXYZ_Yield(int xFrom, int xToExclusive, int yFrom, int yToExclusive, int zFrom, int zToExclusive)
   {
      int i = 0;
      for (int x = xFrom; x < xToExclusive; x++)
      {
         for (int y = yFrom; y < yToExclusive; y++)
         {
            for (int z = zFrom; z < zToExclusive; z++)
            {
               yield return new XYZ<int>(x, y, z);
               i++;
            }
         }
      }
   }

   public static IEnumerable<XYZ<float>> Loop3DXYZ_Yield_f(int xFrom, int xToExclusive, int yFrom, int yToExclusive, int zFrom, int zToExclusive)
   {
      int i = 0;
      for (int x = xFrom; x < xToExclusive; x++)
      {
         for (int y = yFrom; y < yToExclusive; y++)
         {
            for (int z = zFrom; z < zToExclusive; z++)
            {
               yield return new XYZ<float>(x, y, z);
               i++;
            }
         }
      }
   }

   public static void Loop3D(int xFrom, int xToExclusive, int yFrom, int yToExclusive, int zFrom, int zToExclusive, int noThreads, bool oneThreadPerXRange, bool printProgress, Action<int, int, int> act)
   {
      Action<int, int, int, int> a = delegate (int x, int y, int z, int tid)
      {
         act(x, y, z);
      };
      Loop3D(xFrom, xToExclusive, yFrom, yToExclusive, zFrom, zToExclusive, noThreads, oneThreadPerXRange, printProgress, a);
   }

   public static void Loop3D(int xFrom, int xToExclusive, int yFrom, int yToExclusive, int zFrom, int zToExclusive, int noThreads, bool oneThreadPerXRange, bool printProgress, Action<int, int, int, int> act)
   {
      object lockObj = new object();
      int countDone = 0;
      float loopsf = 1f;
      int xWid = xToExclusive - xFrom;
      int yWid = yToExclusive - yFrom;
      int zWid = zToExclusive - zFrom;
      int stride_Z = yWid * xWid;
      int progressIncrement = 1;
      void UpdateProgress(int i)
      {
         if (printProgress && i % progressIncrement == 0)
         {
            lock (lockObj)
            {
               countDone += progressIncrement;
               Console.WriteLine("Progress: \t" + (float)countDone / loopsf * 100f + "%");
            }
         }
      }
      void ConvertTo3DAndExecute(int i, int threadID)
      {
         int result2;
         int num3 = Math.DivRem(i, stride_Z, out result2);
         int result3;
         int num4 = Math.DivRem(result2, xWid, out result3);
         act(result3 + xFrom, num4 + yFrom, num3 + zFrom, threadID);
         UpdateProgress(i);
      }
      Action<int, int> ConvertTo3DAndExecute_Chunked = delegate (int i, int threadID)
      {
         int result;
         int num = Math.DivRem(i, yWid, out result);
         int num2 = result;
         for (int l = xFrom; l < xToExclusive; l++)
         {
            act(l, num2 + yFrom, num + zFrom, threadID);
         }
         UpdateProgress(i);
      };
      Action<int, int> ConvertTo3DAndExecute_ChunkedXY = delegate (int z, int threadID)
      {
         for (int j = xFrom; j < xToExclusive; j++)
         {
            for (int k = yFrom; k < yToExclusive; k++)
            {
               act(j, k, z + zFrom, threadID);
            }
         }
         UpdateProgress(z);
      };
      if (oneThreadPerXRange)
      {
         int loops = yWid * zWid;
         if (loops > 100)
         {
            loopsf = zWid;
            progressIncrement = loops / 50;
            Loop(0, zWid, noThreads, ConvertTo3DAndExecute_ChunkedXY);
         }
         else
         {
            loopsf = loops;
            progressIncrement = loops / 50;
            Loop(0, loops, noThreads, ConvertTo3DAndExecute_Chunked);
         }
      }
      else
      {
         int loops = xWid * yWid * (zToExclusive - zFrom);
         loopsf = loops;
         progressIncrement = loops / 50;
         Loop(0, loops, noThreads, ConvertTo3DAndExecute);
      }
   }

   public static void Loop3D(int xFrom, int xToExclusive, int xStepSize, int yFrom, int yToExclusive, int yStepSize, int zFrom, int zToExclusive, int zStepSize, int noThreads, bool printProgress, Func<int, int, int, bool> xyzCriteria, Action<int, int, int, int> act)
   {
      object lockObj = new();
      if (xStepSize < 1 || yStepSize < 1 || zStepSize < 1)
      {
         throw new ArgumentException("Step size < 1");
      }
      if (xToExclusive <= xFrom || yToExclusive <= yFrom || zToExclusive <= zFrom)
      {
         return;
      }
      int countDone = 0;
      float loopsf = 1f;
      int xWid = (xToExclusive - xFrom - 1) / xStepSize + 1;
      int yWid = (yToExclusive - yFrom - 1) / yStepSize + 1;
      int zWid = (zToExclusive - zFrom - 1) / zStepSize + 1;
      int stride_Z = yWid * xWid;
      int progressIncrement = 1;
      List<int> iToRun = new List<int>();
      Action<int> UpdateProgress = delegate (int i)
      {
         if (printProgress && i % progressIncrement == 0 && i != 0)
         {
            lock (lockObj)
            {
               countDone += progressIncrement;
            }
         }
      };
      void ConvertTo3DAndExecute(int i, int threadID)
      {
         Loop3DIToXYZ(i, xWid, stride_Z, out var x2, out var y2, out var z2);
         act(x2 * xStepSize + xFrom, y2 * yStepSize + yFrom, z2 * zStepSize + zFrom, threadID);
         UpdateProgress(i);
      }
      void ConvertTo3DAndExecuteWithCriteria(int iIniToRun, int threadID)
      {
         int arg = iToRun[iIniToRun];
         ConvertTo3DAndExecute(arg, threadID);
      }
      int loops;
      if (xyzCriteria == null)
      {
         loops = xWid * yWid * zWid;
         loopsf = loops;
         progressIncrement = loops / 50;
         Loop(0, loops, noThreads, ConvertTo3DAndExecute);
         return;
      }
      loops = xWid * yWid * zWid;

      for (int j = 0; j < loops; j++)
      {
         Loop3DIToXYZ(j, xWid, stride_Z, out var x, out var y, out var z);
         if (xyzCriteria(x, y, z))
         {
            iToRun.Add(j);
         }
      }
      loops = iToRun.Count;
      loopsf = loops;
      progressIncrement = loops / 50;
      Loop(0, loops, noThreads, ConvertTo3DAndExecuteWithCriteria);
   }

   private static void Loop3DIToXYZ(int i, int xWid, int stride_Z, out int x, out int y, out int z)
   {
      z = Math.DivRem(i, stride_Z, out var remainder);
      y = Math.DivRem(remainder, xWid, out x);
   }

   public static void Loop(int from, int toExclusive, int maxNoThreads, Action<int> act)
   {
      if (maxNoThreads > 0)
      {
         Loop_Parallel(from, toExclusive, Math.Min(toExclusive - from, maxNoThreads), act);
         return;
      }
      for (int i = from; i < toExclusive; i++)
      {
         act(i);
      }
   }

   public static void Loop(int from, int toExclusive, int maxNoThreads, Action<int, int> act)
   {
      if (maxNoThreads > 1)
      {
         Loop_Parallel(from, toExclusive, maxNoThreads, act);
         return;
      }
      for (int i = from; i < toExclusive; i++)
      {
         act(i, 0);
      }
   }

   public static void LoopNonParallel<T>(int from, int toExclusive, T input, Action<int, T> act)
   {
      for (int i = from; i < toExclusive; i++)
      {
         act(i, input);
      }
   }

   public static void Loop(int from, int toExclusive, bool parallel, Action<int> act)
   {
      if (parallel)
      {
         Parallel.For(from, toExclusive, act);
         return;
      }
      for (int i = from; i < toExclusive; i++)
      {
         act(i);
      }
   }

   public static void Loop<T>(int from, int toExclusive, bool parallel, Func<T> runAtAstartOncePerThread, Func<int, T, T> act, Action<T> onceCompletedRunOncePerThread)
   {
      if (parallel)
      {
         Parallel.For(from, toExclusive, runAtAstartOncePerThread, (int a, ParallelLoopState b, T c) => act(a, c), onceCompletedRunOncePerThread);
         return;
      }
      T generated = runAtAstartOncePerThread();
      for (int i = from; i < toExclusive; i++)
      {
         generated = act(i, generated);
      }
      onceCompletedRunOncePerThread(generated);
   }

   public static void Loop<T>(long from, long toExclusive, bool parallel, Func<T> runAtAstartOncePerThread, Func<long, T, T> act, Action<T> onceCompletedRunOncePerThread)
   {
      if (parallel)
      {
         Parallel.For(from, toExclusive, runAtAstartOncePerThread, (long a, ParallelLoopState b, T c) => act(a, c), onceCompletedRunOncePerThread);
         return;
      }
      T generated = runAtAstartOncePerThread();
      for (long i = from; i < toExclusive; i++)
      {
         generated = act(i, generated);
      }
      onceCompletedRunOncePerThread(generated);
   }


   public static void Loop_Parallel(int from, int toExclusive, Action<int> act)
   {
      Loop(from, toExclusive, parallel: true, act);
   }

   public static void Loop_Parallel(int from, int toExclusive, int noThreads, Action<int> act)
   {
      Loop_Parallel(from, toExclusive, noThreads, delegate (int i, int threadId)
      {
         act(i);
      });
   }

   public static void Loop_Parallel(int from, int toExclusive, int noThreads, Action<int, int> act)
   {
      if (from == toExclusive)
      {
         return;
      }
      if (from > toExclusive)
      {
         throw new ArgumentException("to must be larger than from");
      }
      if (from == toExclusive - 1)
      {
         act(from, 0);
         return;
      }
      int noActs = toExclusive - from + 1;
      if (noThreads > noActs)
      {
         noThreads = noActs;
      }
      int noActsPerThread = noActs / noThreads;
      Task[] tasks = new Task[noThreads - 1];
      for (int i = 0; i < noThreads - 1; i++)
      {
         int thisFrom = i * noActsPerThread + from;
         int thisTo = thisFrom + noActsPerThread;
         int threadId = i;
         tasks[i] = new Task(delegate
         {
            LoopNonParallel(thisFrom, thisTo, threadId, act);
         });
         tasks[i].Start();
      }
      LoopNonParallel((noThreads - 1) * noActsPerThread + from, toExclusive, noThreads - 1, act);
      tasks.ForEach(delegate (Task a)
      {
         a.Wait();
      });
      tasks.ForEach(delegate (Task a)
      {
         a.Dispose();
      });
   }

   [CLSCompliant(false)]
   public static IList<T> Parallel_InversePyramid<T>(IList<T> previousResult, Func<int, int, IList<T>, uint, IList<T>> act, Func<IList<T>[], IList<T>> merge, ushort noThreads_initial = 16, ushort divNoThreadsByPerIteration = 2, bool multiThread = true, int iFrom = 0, int count = -1)
   {
      if (count == -1)
      {
         count = previousResult.Count;
      }
      if (noThreads_initial == 1)
      {
         return act(iFrom, count, previousResult, noThreads_initial);
      }
      if (divNoThreadsByPerIteration < 2)
      {
         throw new ArgumentException("divNoThreadsByPerIteration must be at least 2");
      }
      uint noThreads = noThreads_initial;
      int range = previousResult.Count;
      int each = (int)(range / noThreads);
      TaskFactory<IList<T>> factory = new TaskFactory<IList<T>>();
      Task<IList<T>>[] allTasks = new Task<IList<T>>[noThreads_initial - 1];
      IList<T>[] unmerged = new IList<T>[noThreads];
      for (int iThread = 0; iThread < allTasks.Length; iThread++)
      {
         int from2 = each * iThread + iFrom;
         Func<IList<T>> func = () => act(from2, each, previousResult, noThreads);
         if (multiThread)
         {
            allTasks[iThread] = factory.StartNew(func);
         }
         else
         {
            unmerged[iThread] = func();
         }
      }
      int from = (int)(each * (noThreads - 1)) + iFrom;
      unmerged[^1] = act(from, previousResult.Count - from, previousResult, noThreads);
      if (multiThread)
      {
         Task[] tasks = allTasks;
         Task.WaitAll(tasks);
         for (int i = 0; i < allTasks.Length; i++)
         {
            unmerged[i] = allTasks[i].Result;
         }
      }
      return Parallel_InversePyramid(merge(unmerged), act, merge, (ushort)Math.Max(1, noThreads_initial / divNoThreadsByPerIteration), divNoThreadsByPerIteration, multiThread);
   }

}