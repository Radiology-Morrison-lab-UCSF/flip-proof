using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FlipProof.Base;
using FlipProof.Base.Geometry;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace FlipProof.Image;

//TODO: Clean up old code/syntax, split into sub-classes
public static class GenMethods
{
   private struct SorterGenericArray<T, S>
   {
      private readonly List<T> keys;

      private readonly List<S> items;

      private readonly Comparison<T> comparer;

      internal SorterGenericArray(List<T> keys, List<S> items, Comparison<T> comparer)
      {
         this.keys = keys;
         this.items = items;
         this.comparer = comparer;
      }

      internal void SwapIfGreaterWithItems(int a, int b)
      {
         if (a != b && comparer(keys[a], keys[b]) > 0)
         {
            T key = keys[a];
            keys[a] = keys[b];
            keys[b] = key;
            if (items != null)
            {
               S item = items[a];
               items[a] = items[b];
               items[b] = item;
            }
         }
      }

      internal void QuickSort(int left, int right)
      {
         do
         {
            int i = left;
            int j = right;
            int middle = GetMedian(i, j);
            SwapIfGreaterWithItems(i, middle);
            SwapIfGreaterWithItems(i, j);
            SwapIfGreaterWithItems(middle, j);
            T x = keys[middle];
            while (true)
            {
               if (comparer(keys[i], x) < 0)
               {
                  i++;
                  continue;
               }
               while (comparer(x, keys[j]) < 0)
               {
                  j--;
               }
               if (i > j)
               {
                  break;
               }
               if (i < j)
               {
                  T key = keys[i];
                  keys[i] = keys[j];
                  keys[j] = key;
                  if (items != null)
                  {
                     S item = items[i];
                     items[i] = items[j];
                     items[j] = item;
                  }
               }
               if (i != int.MaxValue)
               {
                  i++;
               }
               if (j != int.MinValue)
               {
                  j--;
               }
               if (i > j)
               {
                  break;
               }
            }
            if (j - left <= right - i)
            {
               if (left < j)
               {
                  QuickSort(left, j);
               }
               left = i;
            }
            else
            {
               if (i < right)
               {
                  QuickSort(i, right);
               }
               right = j;
            }
         }
         while (left < right);
      }

      private static int GetMedian(int low, int hi)
      {
         return low + (hi - low >> 1);
      }
   }


   
   public static DateTime DateTimeFromMilliseconds(int totalMs) => new DateTime(1, 1, 1, 0, 0, 0, 0).AddMilliseconds(totalMs);




   public static LinkedListNode<T>? FirstOrDefault_Node<T>(this LinkedList<T> me, Func<T, int, bool> actionSubsequent)
   {
      if (me.Any())
      {
         LinkedListNode<T>? cur = me.First;
         int i = 0;
         while (cur != null)
         {
            if (actionSubsequent(cur.Value, i))
            {
               return cur;
            }
            i++;
            cur = cur.Next;
         }
      }
      return null;
   }

   public static int[] SplitIntoBlockOffsets(int lengthToSplit, int sizeOfBlocks)
   {
      int num = (int)Math.Ceiling((float)lengthToSplit / (float)sizeOfBlocks);
      int nextEntry = 0;
      int[] res = new int[num];
      for (int i = 0; i < res.Length; i++)
      {
         res[i] = nextEntry;
         nextEntry += sizeOfBlocks;
      }
      return res;
   }

   public static void SplitIntoTestAndTraining<T>(T[] source, int offset, T[] train, T[] test)
   {
      if (train.Length + test.Length > source.Length)
      {
         throw new ArgumentOutOfRangeException("No items to sampled is greater than the number available");
      }
      TakeCircular(source, offset, train);
      int testOffset = offset + train.Length;
      if (testOffset >= source.Length)
      {
         testOffset -= source.Length;
      }
      TakeCircular(source, testOffset, test);
   }

   public static void SplitIntoTestAndTraining_Random<T>(T[] source, T[] train, T[] test, int? randomSeed = null)
   {
      if (train.Length + test.Length > source.Length)
      {
         throw new ArgumentOutOfRangeException("No items to sampled is greater than the number available");
      }
      if (train.Length == 0)
      {
         source.CopyTo(test, 0);
         return;
      }
      if (test.Length == 0)
      {
         source.CopyTo(test, 0);
         return;
      }
      Random r = (randomSeed.HasValue ? new Random(randomSeed.Value) : new Random());
      int[] allIndices = CollectionCreation.IncrementalArray(source.Length);
      ScrambleOrder(allIndices, r);
      source.SubArray(allIndices.Take(train.Length), train);
      source.SubArray(allIndices.Skip(train.Length).Take(test.Length), test);
   }

   public static IEnumerator<T> CircularInfiniteIenum<T>(T[] source)
   {
      int i = 0;
      while (true)
      {
         yield return source[i];
         i = (i + 1) % source.Length;
      }
   }

   public static void TakeCircular<T>(T[] source, int offset, T[] destination)
   {
      if (destination.Length > source.Length)
      {
         throw new ArgumentOutOfRangeException("Destination is larger than source");
      }
      if (offset >= source.Length)
      {
         throw new ArgumentOutOfRangeException("offset");
      }
      if (offset + destination.Length > source.Length)
      {
         int takeFirst = source.Length - offset;
         Array.Copy(source, offset, destination, 0, takeFirst);
         Array.Copy(source, 0, destination, takeFirst, destination.Length - takeFirst);
      }
      else
      {
         Array.Copy(source, offset, destination, 0, destination.Length);
      }
   }

   public static void RunProcessAndWait_Parallel(string[][] command_args)
   {
      if (!RunProcessAndWait_Parallel(command_args, out var err))
      {
         throw new Exception(err);
      }
   }

   public static bool RunProcessAndWait_Parallel(string[][] command_args, [NotNullWhen(false)] out string? error)
   {
      bool allPass = true;
      string? errorCopy = null;
      Iteration.Loop_Parallel(0, command_args.Length, delegate (int i)
      {
         if (!RunProcessAndWait(command_args[i][0], command_args[i][1], out var err))
         {
            errorCopy = err;
            allPass = false;
         }
      });
      error = errorCopy;
      return allPass;
   }

   public static void RunProcessAndWait(string[][] command_args)
   {
      if (!RunProcessAndWait(command_args, out var err))
      {
         throw new Exception(err);
      }
   }

   public static bool RunProcessAndWait(string[][] command_args, [NotNullWhen(false)] out string? err)
   {
      for (int i = 0; i < command_args.Length; i++)
      {
         if (!RunProcessAndWait(command_args[i][0], command_args[i][1], out err))
         {
            return false;
         }
      }
      err = null;
      return true;
   }

   public static void RunProcessAndWait(string command, string args, bool redirectOutput = false)
   {
      if (!RunProcessAndWait(command, args, out var err, redirectOutput))
      {
         throw new Exception(err);
      }
   }

   public static bool RunProcessAndWait(string command, string args, [NotNullWhen(false)] out string? err, bool redirectOutput = false)
   {
      ProcessStartInfo psi = new ProcessStartInfo(command, args);
      if (redirectOutput)
      {
         psi.UseShellExecute = false;
         psi.RedirectStandardOutput = true;
      }
      using Process? p = Process.Start(psi);
      if (p != null)
      {
         p.WaitForExit();
         if (p.ExitCode == 0)
         {
            err = null;
            return true;
         }
         err = "Failed to run command (exit code was " + p.ExitCode + "): " + command;
      }
      else
      {
         err = "Failed to run command: " + command;
      }

      return false;
   }

   public static bool RunProcessAndWait(string command, [NotNullWhen(false)] out string? err)
   {
      using Process? p = Process.Start(new ProcessStartInfo(command));
      if (p != null)
      {
         p.WaitForExit();
         if (p.ExitCode == 0)
         {
            err = null;
            return true;
         }
         err = "Failed to run command (exit code was " + p.ExitCode + "): " + command;
      }
      else
      {
         err = "Failed to run command: " + command;
      }
   
      
      return false;
   }

   public static bool RunProcessAndWait_PipeIn(string command, string pipeIn, bool makeSilent, [NotNullWhen(false)] out string? err)
   {
      using Process? p = Process.Start(new ProcessStartInfo(command)
      {
         RedirectStandardInput = true,
         UseShellExecute = false,
         RedirectStandardOutput = makeSilent
      });
      if (p != null)
      {
         p.StandardInput.Write(pipeIn);
         p.StandardInput.Dispose();
         p.WaitForExit();
         if (p.ExitCode == 0)
         {
            err = null;
            return true;
         }
      }
      err = "Failed to run command " + command;
      return false;
   }

   public static IList<T> SortIListInPlace<T>(IList<T> vals) where T : IComparable<T>
   {
      if (vals is T[] arr)
      {
         Array.Sort(arr);
      }
      else if (vals is List<T> list)
      {
         list.Sort();
      }
      else
      {
         vals = vals.OrderBy((T a) => a).ToArray();
      }
      return vals;
   }


   public static T MaxValueOfType<T>() where T : struct
   {
      T example = default(T);
      if (example is byte)
      {
         return (T)Convert.ChangeType(byte.MaxValue, typeof(T));
      }
      if (example is sbyte)
      {
         return (T)Convert.ChangeType(sbyte.MaxValue, typeof(T));
      }
      if (example is short)
      {
         return (T)Convert.ChangeType(short.MaxValue, typeof(T));
      }
      if (example is ushort)
      {
         return (T)Convert.ChangeType(ushort.MaxValue, typeof(T));
      }
      if (example is int)
      {
         return (T)Convert.ChangeType(int.MaxValue, typeof(T));
      }
      if (example is uint)
      {
         return (T)Convert.ChangeType(uint.MaxValue, typeof(T));
      }
      if (example is long)
      {
         return (T)Convert.ChangeType(long.MaxValue, typeof(T));
      }
      if (example is ulong)
      {
         return (T)Convert.ChangeType(ulong.MaxValue, typeof(T));
      }
      if (example is float)
      {
         return (T)Convert.ChangeType(float.MaxValue, typeof(T));
      }
      if (example is double)
      {
         return (T)Convert.ChangeType(double.MaxValue, typeof(T));
      }
      if (example is decimal)
      {
         return (T)Convert.ChangeType(decimal.MaxValue, typeof(T));
      }
      if (example is char)
      {
         return (T)Convert.ChangeType('\uffff', typeof(T));
      }
      throw new NotSupportedException("Unsupported type");
   }

   public static T MinValueOfType<T>() where T : struct
   {
      T example = default(T);
      if (example is byte)
      {
         return (T)Convert.ChangeType((byte)0, typeof(T));
      }
      if (example is sbyte)
      {
         return (T)Convert.ChangeType(sbyte.MinValue, typeof(T));
      }
      if (example is short)
      {
         return (T)Convert.ChangeType(short.MinValue, typeof(T));
      }
      if (example is ushort)
      {
         return (T)Convert.ChangeType((ushort)0, typeof(T));
      }
      if (example is int)
      {
         return (T)Convert.ChangeType(int.MinValue, typeof(T));
      }
      if (example is uint)
      {
         return (T)Convert.ChangeType(0u, typeof(T));
      }
      if (example is long)
      {
         return (T)Convert.ChangeType(long.MinValue, typeof(T));
      }
      if (example is ulong)
      {
         return (T)Convert.ChangeType(0uL, typeof(T));
      }
      if (example is float)
      {
         return (T)Convert.ChangeType(float.MinValue, typeof(T));
      }
      if (example is double)
      {
         return (T)Convert.ChangeType(double.MinValue, typeof(T));
      }
      if (example is decimal)
      {
         return (T)Convert.ChangeType(decimal.MinValue, typeof(T));
      }
      if (example is char)
      {
         return (T)Convert.ChangeType('\0', typeof(T));
      }
      throw new NotSupportedException("Unsupported type");
   }

   internal static int IntersectCount<T>(T[] curCell0, T[] v) where T : IEquatable<T>
   {
      int count = 0;
      for (int i = 0; i < curCell0.Length; i++)
      {
         for (int j = 0; j < v.Length; j++)
         {
            if (curCell0[i].Equals(v[j]))
            {
               count++;
            }
         }
      }
      return count;
   }

   internal static int IntersectCount3<T>(T[] arr0, T[] arr1) where T : IEquatable<T>
   {
      T a0 = arr0[0];
      T a1 = arr0[1];
      T a2 = arr0[2];
      T b0 = arr1[0];
      T b1 = arr1[1];
      T b2 = arr1[2];
      int count = 0;
      if (a0.Equals(b0))
      {
         count++;
      }
      else if (a0.Equals(b1))
      {
         count++;
      }
      else if (a0.Equals(b2))
      {
         count++;
      }
      if (a1.Equals(b0))
      {
         count++;
      }
      else if (a1.Equals(b1))
      {
         count++;
      }
      else if (a1.Equals(b2))
      {
         count++;
      }
      if (a2.Equals(b0))
      {
         count++;
      }
      else if (a2.Equals(b1))
      {
         count++;
      }
      else if (a2.Equals(b2))
      {
         count++;
      }
      return count;
   }

   public static void MinMax<T>(this IEnumerable<T> coll, out T min, out T max) where T : IComparable<T>
   {
      min = coll.First();
      max = min;
      foreach (T cur in coll.Skip(1L))
      {
         if (cur.CompareTo(min) < 0)
         {
            min = cur;
         }
         else if (cur.CompareTo(max) > 0)
         {
            max = cur;
         }
      }
   }

   public static T MinOrDefault<T>(this IEnumerable<T> coll) where T : struct, IComparable<T>
   {
      T min = coll.FirstOrDefault();
      foreach (T cur in coll)
      {
         if (cur.CompareTo(min) < 0)
         {
            min = cur;
         }
      }
      return min;
   }
   public static T? MinOrNull<T>(this IEnumerable<T> coll) where T : class, IComparable<T>
   {
      T? min = coll.FirstOrDefault();
      foreach (T cur in coll)
      {
         if (cur.CompareTo(min) < 0)
         {
            min = cur;
         }
      }
      return min;
   }

   public static T[] MinN<T>(this IList<T> coll, int nMin) where T : struct, IComparable<T>
   {
      if (coll.Count < nMin)
      {
         throw new ArgumentException("Array has too few entries for nMin");
      }
      if (coll.Count == nMin)
      {
         return coll.ToArray();
      }
      T[] min = new T[nMin];
      T largestMin = default;
      int indexOfLargestMin = 0;
      for (int j = 0; j < min.Length; j++)
      {
         T cur2 = (min[j] = coll[j]);
         if (j == 0 || cur2.CompareTo(largestMin) > 0)
         {
            largestMin = cur2;
            indexOfLargestMin = j;
         }
      }
      foreach (T cur in coll.Skip(nMin))
      {
         if (cur.CompareTo(largestMin) >= 0)
         {
            continue;
         }
         min[indexOfLargestMin] = cur;
         largestMin = min[0];
         indexOfLargestMin = 0;
         for (int i = 1; i < min.Length; i++)
         {
            if (min[i].CompareTo(largestMin) > 0)
            {
               indexOfLargestMin = i;
               largestMin = min[i];
            }
         }
      }
      return min;
   }

   public static T[] MinN<T>(this IList<T> coll, int nMin, Func<T, T, int> comparer) 
   {
      if (coll.Count < nMin)
      {
         throw new ArgumentException("Array has too few entries for nMin");
      }
      if (coll.Count == nMin)
      {
         return coll.ToArray();
      }
      T[] min = new T[nMin];
      T largestMin = coll[0];
      int indexOfLargestMin = 0;
      for (int j = 0; j < min.Length; j++)
      {
         T cur = min[j] = coll[j];
         if (j == 0 || comparer(cur, largestMin) > 0)
         {
            largestMin = cur;
            indexOfLargestMin = j;
         }
      }
      foreach (T cur in coll.Skip(nMin))
      {
         if (comparer(cur, largestMin) >= 0)
         {
            continue;
         }
         min[indexOfLargestMin] = cur;
         largestMin = min[0];
         indexOfLargestMin = 0;
         for (int i = 1; i < min.Length; i++)
         {
            if (comparer(min[i], largestMin) > 0)
            {
               indexOfLargestMin = i;
               largestMin = min[i];
            }
         }
      }
      return min;
   }

   public static T Max_Unchanged<T, S>(this IEnumerable<T> coll, Func<T, S> convert) where S : IComparable<S>
   {
      if (!coll.Any())
      {
         throw new Exception("No items");
      }
      T maxT = coll.First();
      S maxS = convert(maxT);
      foreach (T val in coll)
      {
         S curS = convert(val);
         if (curS.CompareTo(maxS) > 0)
         {
            maxS = curS;
            maxT = val;
         }
      }
      return maxT;
   }

   public static T Min_Unchanged<T, S>(this IEnumerable<T> coll, Func<T, S> convert) where S : IComparable<S>
   {
      if (!coll.Any())
      {
         throw new Exception("No items");
      }
      T minT = coll.First();
      S minS = convert(minT);
      foreach (T val in coll)
      {
         S curS = convert(val);
         if (curS.CompareTo(minS) < 0)
         {
            minS = curS;
            minT = val;
         }
      }
      return minT;
   }

   public static T Min_Unchanged<T>(this IEnumerable<T> coll, Func<T, T, int> compare)
   {
      if (!coll.Any())
      {
         throw new Exception("No items");
      }
      T min = coll.First();
      int i = 0;
      foreach (T val in coll)
      {
         if (compare(val, min) < 0)
         {
            min = val;
         }
         i++;
      }
      return min;
   }

   public static int IndexOfMax<T>(this IEnumerable<T> coll) where T : IComparable<T>
   {
      if (!coll.Any())
      {
         return -1;
      }
      int foundIndex = 0;
      T max = coll.First();
      int i = 0;
      foreach (T val in coll)
      {
         if (val.CompareTo(max) > 0)
         {
            max = val;
            foundIndex = i;
         }
         i++;
      }
      return foundIndex;
   }

   public static int IndexOfMin<T>(this IEnumerable<T> coll) where T : IComparable<T>
   {
      if (!coll.Any())
      {
         return -1;
      }
      int foundIndex = 0;
      T min = coll.First();
      int i = 0;
      foreach (T val in coll)
      {
         if (val.CompareTo(min) < 0)
         {
            min = val;
            foundIndex = i;
         }
         i++;
      }
      return foundIndex;
   }

   public static int IndexOfMin(this double[] coll)
   {
      if (coll.Length == 0)
      {
         return -1;
      }
      int foundIndex = 0;
      double min = coll[0];
      for (int i = 0; i < coll.Length; i++)
      {
         double val = coll[i];
         if (val < min)
         {
            min = val;
            foundIndex = i;
         }
      }
      return foundIndex;
   }

   public static int IndexOfMinNth<T>(this IEnumerable<T> coll, int minNo) where T : IComparable<T>
   {
      if (!coll.Any())
      {
         return -1;
      }
      return (from a in coll.Select((T a, int index) => new Tuple<T, int>(a, index))
              orderby a.Item1
              select a).Skip(minNo).First().Item2;
   }

   public static int[] IndexOfMinN<T>(this IEnumerable<T> coll, int minNo) where T : IComparable<T>
   {
      return (from a in coll.Select((T a, int index) => new Tuple<T, int>(a, index))
              orderby a.Item1
              select a.Item2).Take(minNo).ToArray();
   }

   public static int[] IndexOfClosestN_AssumeSorted_Generic<T>(this T[] coll, T closeTo, int minNo)
   {
      if (coll is int[] iArr)
      {
         return iArr.IndexOfClosestN_AssumeSorted(Convert.ToInt32(closeTo), minNo);
      }
      if (coll is double[] dArr)
      {
         return dArr.IndexOfClosestN_AssumeSorted(Convert.ToDouble(closeTo), minNo);
      }
      throw new NotSupportedException();
   }

   public static int[] IndexOfClosestN_AssumeSorted(this int[] coll, int closeTo, int minNo)
   {
      if (minNo < 1 || coll.Length == 0)
      {
         return [];
      }
      minNo = Math.Min(minNo, coll.Length);
      int indexOfMatch = Array.BinarySearch(coll, closeTo);
      if (indexOfMatch < 0)
      {
         indexOfMatch = ~indexOfMatch;
         if (indexOfMatch == coll.Length)
         {
            indexOfMatch--;
         }
         else if (indexOfMatch > 0 && Math.Abs(closeTo - coll[indexOfMatch - 1]) < Math.Abs(closeTo - coll[indexOfMatch]))
         {
            indexOfMatch--;
         }
      }
      int[] found = new int[minNo];
      found[0] = indexOfMatch;
      int nextSaveTo = 1;
      int atOrAboveThisIndex = indexOfMatch;
      int atOrBelowThisIndex = indexOfMatch;
      int diffAbove = 0;
      int diffBelow = 0;
      Action UpdateDiffAbove = delegate
      {
         atOrAboveThisIndex--;
         diffAbove = ((atOrAboveThisIndex == -1) ? int.MaxValue : Math.Abs(closeTo - coll[atOrAboveThisIndex]));
      };
      Action UpdateDiffBelow = delegate
      {
         atOrBelowThisIndex++;
         diffBelow = ((atOrBelowThisIndex == coll.Length) ? int.MaxValue : Math.Abs(closeTo - coll[atOrBelowThisIndex]));
      };
      UpdateDiffAbove();
      UpdateDiffBelow();
      for (nextSaveTo = 1; nextSaveTo < minNo; nextSaveTo++)
      {
         if (diffBelow < diffAbove)
         {
            found[nextSaveTo] = atOrBelowThisIndex;
            UpdateDiffBelow();
         }
         else
         {
            found[nextSaveTo] = atOrAboveThisIndex;
            UpdateDiffAbove();
         }
      }
      return found;
   }

   public static int[] IndexOfClosestN_AssumeSorted(this double[] coll, double closeTo, int minNo)
   {
      if (minNo < 1 || coll.Length == 0)
      {
         return new int[0];
      }
      minNo = Math.Min(minNo, coll.Length);
      int indexOfMatch = Array.BinarySearch(coll, closeTo);
      if (indexOfMatch < 0)
      {
         indexOfMatch = ~indexOfMatch;
         if (indexOfMatch == coll.Length)
         {
            indexOfMatch--;
         }
         else if (indexOfMatch > 0 && Math.Abs(closeTo - coll[indexOfMatch - 1]) < Math.Abs(closeTo - coll[indexOfMatch]))
         {
            indexOfMatch--;
         }
      }
      int[] found = new int[minNo];
      found[0] = indexOfMatch;
      int nextSaveTo = 1;
      int atOrAboveThisIndex = indexOfMatch;
      int atOrBelowThisIndex = indexOfMatch;
      double diffAbove = 0.0;
      double diffBelow = 0.0;
      Action UpdateDiffAbove = delegate
      {
         atOrAboveThisIndex--;
         diffAbove = ((atOrAboveThisIndex == -1) ? double.MaxValue : Math.Abs(closeTo - coll[atOrAboveThisIndex]));
      };
      Action UpdateDiffBelow = delegate
      {
         atOrBelowThisIndex++;
         diffBelow = ((atOrBelowThisIndex == coll.Length) ? double.MaxValue : Math.Abs(closeTo - coll[atOrBelowThisIndex]));
      };
      UpdateDiffAbove();
      UpdateDiffBelow();
      for (nextSaveTo = 1; nextSaveTo < minNo; nextSaveTo++)
      {
         if (diffBelow < diffAbove)
         {
            found[nextSaveTo] = atOrBelowThisIndex;
            UpdateDiffBelow();
         }
         else
         {
            found[nextSaveTo] = atOrAboveThisIndex;
            UpdateDiffAbove();
         }
      }
      return found;
   }


   public static void PerformTasks(bool parallel, params Action[] toDos)
   {
      Iteration.Loop(0, toDos.Length, parallel, delegate (int i)
      {
         toDos[i]();
      });
   }

   public static void CreateOrClearDirectory(string dir)
   {
      if (Directory.Exists(dir))
      {
         Directory.Delete(dir, recursive: true);
      }
      Directory.CreateDirectory(dir);
   }

   public static T[] ConcatArrays<T>(this T[] first, params T[][] list)
   {
      T[] result = new T[first.Length + list.Sum((T[] a) => a.Length)];
      first.CopyTo(result, 0);
      int offset = first.Length;
      for (int x = 0; x < list.Length; x++)
      {
         list[x].CopyTo(result, offset);
         offset += list[x].Length;
      }
      return result;
   }

   public static T[] ConcatArrays<T>(params T[][] list)
   {
      T[] result = new T[list.Sum((T[] a) => a.Length)];
      int offset = 0;
      for (int x = 0; x < list.Length; x++)
      {
         list[x].CopyTo(result, offset);
         offset += list[x].Length;
      }
      return result;
   }

   public static T[] ConcatArr<T>(this IEnumerable<T> arr, T addAtEnd)
   {
      return arr.Concat(addAtEnd).ToArray();
   }

   [CLSCompliant(false)]
   public static T[] ConcatArrays<T>(params List<T>[] list)
   {
      T[] result = new T[list.Sum((List<T> a) => a.Count)];
      int offset = 0;
      for (int x = 0; x < list.Length; x++)
      {
         list[x].CopyTo(result, offset);
         offset += list[x].Count;
      }
      return result;
   }

   public static IEnumerable<T> Concat<T>(this IEnumerable<T> arr, T addAtEnd)
   {
      return arr.Concat(Enumerable.Repeat(addAtEnd, 1));
   }

   public static IEnumerable<T> Concat_Start<T>(this IEnumerable<T> arr, T addAtStart)
   {
      return Enumerable.Repeat(addAtStart, 1).Concat(arr);
   }

   public static IEnumerable<T> Concat_End<T>(this IEnumerable<T> arr, T addAtEnd)
   {
      return arr.Concat(Enumerable.Repeat(addAtEnd, 1));
   }

   public static string ConcatString<T>(this IEnumerable<T> arr, string addBetween)
   {
      StringBuilder sb = new StringBuilder();
      bool anyAdded = false;
      foreach (T cur in arr)
      {
         if (anyAdded)
         {
            sb.Append(addBetween);
         }
         sb.Append(cur);
         anyAdded = true;
      }
      return sb.ToString();
   }



   public static IEnumerable<T> CastIEnum<T, U>(this IEnumerable<U> arr)
   {
      if (arr is IEnumerable<T> ie)
      {
         return ie;
      }
      return arr.Select((U a) => (T)Convert.ChangeType(a, typeof(T))!);
   }


   public static T[] Duplicate_NonPrimitive<T>(this T[] arr)
   {
      T[] clone = new T[arr.Length];
      Array.Copy(arr, clone, arr.Length);
      return clone;
   }

   public static T HighestScore<T>(ICollection<T> col, Func<T, int> convertToScore)
   {
      int index = col.Select((T a) => convertToScore(a)).IndexOfMax();
      return col.ElementAt(index);
   }

   public static T LowestScore<T>(ICollection<T> col, Func<T, int> convertToScore)
   {
      int index = col.Select((T a) => convertToScore(a)).IndexOfMin();
      return col.ElementAt(index);
   }

   internal static float Multiply(IEnumerable<float> p)
   {
      float res = 1f;
      foreach (float item in p)
      {
         res *= item;
      }
      return res;
   }

   internal static double Multiply(IEnumerable<double> p)
   {
      double res = 1.0;
      foreach (double item in p)
      {
         res *= item;
      }
      return res;
   }

   public static IEnumerable<int> RankOrderUnique<T>(T[] orig) where T : IComparable<T>, IEquatable<T>
   {
      T[] sorted = new T[orig.Length];
      Array.Copy(orig, sorted, orig.Length);
      Array.Sort(sorted);
      T[] ordered = sorted.Distinct_AssumeSorted().ToArray();
      return orig.Select((T a) => ordered.IndexOf(a));
   }

   public static IEnumerable<int> RankOrderUnique_ICollection<T>(ICollection<T> ienum) where T : IComparable<T>, IEquatable<T>
   {
      T[] ordered = (from a in ienum.Distinct()
                     orderby a
                     select a).ToArray();
      return ienum.Select((T a) => ordered.IndexOf(a));
   }

   public static IEnumerable<T> Distinct_AssumeSorted<T>(this T[] arr) where T : IEquatable<T>
   {
      if (!arr.Any())
      {
         yield break;
      }
      T last = arr[0];
      yield return last;
      for (int i = 1; i < arr.Length; i++)
      {
         if (!arr[i].Equals(last))
         {
            last = arr[i];
            yield return last;
         }
      }
   }

   public static IEnumerable<T> Distinct_AssumeSorted<T>(this IEnumerable<T> arr) where T : IEquatable<T>
   {
      if (arr is IOrderedEnumerable<T>)
      {
         return arr.Distinct();
      }
      return Distinct_AssumeSorted_Sub(arr);
   }

   private static IEnumerable<T> Distinct_AssumeSorted_Sub<T>(IEnumerable<T> arr) where T : IEquatable<T>
   {
      if (!arr.Any())
      {
         yield break;
      }
      bool first = true;
      T last = default(T)!;// value is never used
      foreach (T cur in arr)
      {
         if (first || !cur.Equals(last))
         {
            last = cur;
            yield return last;
         }
         first = false;
      }
   }

   public static T[] ReverseInPlace<T>(T[] arr)
   {
      int halfway = arr.Length / 2;
      if (arr.Length % 2 == 0)
      {
         for (int i = 0; i < halfway; i++)
         {
            int i_end = arr.Length - 1 - i;
            T oldStart = arr[i];
            arr[i] = arr[i_end];
            arr[i_end] = oldStart;
         }
      }
      else
      {
         for (int j = 0; j <= halfway; j++)
         {
            int i_end2 = arr.Length - 1 - j;
            T oldStart2 = arr[j];
            arr[j] = arr[i_end2];
            arr[i_end2] = oldStart2;
         }
      }
      return arr;
   }

   public static T[] ReverseInPlace_2<T>(T[] arr)
   {
      T start = arr[0];
      arr[0] = arr[1];
      arr[1] = start;
      return arr;
   }

   public static T[] ReverseInPlace_4<T>(T[] arr)
   {
      T start = arr[0];
      arr[0] = arr[3];
      arr[3] = start;
      start = arr[1];
      arr[1] = arr[2];
      arr[2] = start;
      return arr;
   }

   public static T[] ReverseInPlace_8<T>(T[] arr)
   {
      T start = arr[0];
      arr[0] = arr[7];
      arr[7] = start;
      start = arr[1];
      arr[1] = arr[6];
      arr[6] = start;
      start = arr[2];
      arr[2] = arr[5];
      arr[5] = start;
      start = arr[3];
      arr[3] = arr[4];
      arr[4] = start;
      return arr;
   }

   static T[] SubArray<T>(this T[] data, int fromInclusive, int count) => data[fromInclusive..(fromInclusive + count)];

   public static T[] SubArray<T>(this T[] data, long fromInclusive, long count)
   {
      T[] result = new T[count];
      Array.Copy(data, fromInclusive, result, 0L, count);
      return result;
   }

   public static T[] SubArray<T>(this HashSet<T> data, int fromInclusive, int count)
   {
      T[] arr = new T[count];
      int i = 0;
      foreach (T cur in data.Skip(fromInclusive).Take(count))
      {
         arr[i] = cur;
         i++;
      }
      return arr;
   }

   public static T[] SubArray<T>(this ICollection<T> data, int fromInclusive, int count)
   {
      return data.Skip(fromInclusive).Take(count).ToArray();
   }

   public static T[] SubArray<T>(this ICollection<T> data, IEnumerable<int> indices)
   {
      return data.SubIEnum(indices).ToArray();
   }

   public static T[] SubArray<T>(this T[] data, IEnumerable<long> indices)
   {
      return data.SubIEnum(indices).ToArray();
   }

   public static void SubArray<T>(this T[] data, IEnumerable<int> indices, T[] destination)
   {
      int i = 0;
      foreach (int curI in indices)
      {
         destination[i] = data[curI];
         i++;
      }
   }

   internal static bool HasAllFlags(uint num, uint flags)
   {
      return (num & flags) == flags;
   }

   public static T[] SubArray_Inverse<T>(this T[] data, IEnumerable<int> indices)
   {
      return data.SubIEnum_Inverse(indices).ToArray();
   }

   public static T[] SubArray_Inverse<T>(this T[] data, HashSet<int> indicesToExclude)
   {
      T[] result = new T[data.Length - indicesToExclude.Count];
      data.SubArray_Inverse(indicesToExclude, result);
      return result;
   }

   public static void SubArray_Inverse<T>(this T[] data, HashSet<int> indicesToExclude, T[] fillMe)
   {
      int next = 0;
      if (fillMe.Length < data.Length - indicesToExclude.Count)
      {
         throw new ArgumentOutOfRangeException("fillMe is too small");
      }
      for (int i_data = 0; i_data < data.Length; i_data++)
      {
         if (!indicesToExclude.Contains(i_data))
         {
            fillMe[next] = data[i_data];
            next++;
         }
      }
   }

   public static List<T> SubArray_Inverse<T>(this List<T> data, HashSet<int> indicesToExclude)
   {
      List<T> result = new List<T>(data.Count - indicesToExclude.Count);
      for (int i_data = 0; i_data < data.Count; i_data++)
      {
         if (!indicesToExclude.Contains(i_data))
         {
            result.Add(data[i_data]);
         }
      }
      return result;
   }

   public static IEnumerable<T> SubIEnum<T>(this ICollection<T> data, IEnumerable<int> indices)
   {
      return indices.Select((int a) => data.ElementAt(a));
   }

   public static IEnumerable<T> SubIEnum<T>(this T[] data, IEnumerable<long> indices)
   {
      return indices.Select((long a) => data[a]);
   }

   public static IEnumerable<T> SubIEnum_Inverse<T>(this T[] data, IEnumerable<int> indices)
   {
      return from a in data.Index_Inverse(indices)
             select data[a];
   }

   public static IEnumerable<int> Index_Inverse<T>(this T[] data, IEnumerable<int> indicesToExclude)
   {
      return Enumerable.Range(0, data.Length).Except(indicesToExclude);
   }

   public static IEnumerable<T> SubIEnum<T>(this IEnumerable<T> data, IOrderedEnumerable<int> indices)
   {
      return data.SubIEnum_AssumedIndicesAreOrdered(indices);
   }

   public static IEnumerable<T> SubIEnum_AssumedIndicesAreOrdered<T>(this IEnumerable<T> data, IEnumerable<int> indices)
   {
      int lastIndex = -1;
      using IEnumerator<T> e2 = data.GetEnumerator();
      foreach (int curIndex in indices)
      {
         int move = curIndex - lastIndex;
         for (int i = 0; i < move; i++)
         {
            e2.MoveNext();
         }
         yield return e2.Current;
         lastIndex = curIndex;
      }
   }

   public static void ScrambleOrder<T>(this T[] a1, int randomSeed = 4985791)
   {
      Random r = new Random(randomSeed);
      ScrambleOrder(a1, r);
   }

   public static void ScrambleOrder<T>(T[] a1, Random r)
   {
      for (int i = 0; i < a1.Length; i++)
      {
         int swapWith = r.Next(a1.Length);
         T temp = a1[i];
         a1[i] = a1[swapWith];
         a1[swapWith] = temp;
      }
   }

   public static void ScrambleOrder<T, U>(this T[] keys, IList<U> values)
   {
      Random r = new Random(4985791);
      for (int i = 0; i < keys.Length; i++)
      {
         int swapWith = r.Next(keys.Length);
         T temp2 = keys[i];
         keys[i] = keys[swapWith];
         keys[swapWith] = temp2;
         U temp = values[i];
         values[i] = values[swapWith];
         values[swapWith] = temp;
      }
   }



   public static IEnumerable<T> Skip<T>(this IEnumerable<T> vals, long skip)
   {
      using IEnumerator<T> e = vals.GetEnumerator();
      for (long i = 0L; i < skip; i++)
      {
         if (!e.MoveNext())
         {
            yield break;
         }
      }
      while (e.MoveNext())
      {
         yield return e.Current;
      }
   }

   public static void Sort<T, S>(List<T> keys, List<S> items) where T : IComparable<T>
   {
      Sort(keys, items, 0, keys.Count, (T a, T b) => a.CompareTo(b));
   }

   public static void Sort<T, S>(List<T> keys, List<S> items, Comparison<T> comparer)
   {
      Sort(keys, items, 0, keys.Count, comparer);
   }

   public static void Sort<T, S>(List<T> keys, List<S> items, int index, int length, Comparison<T> comparer)
   {
      if (keys.Count != items.Count)
      {
         throw new ArgumentException("Lists are different sizes");
      }
      if (index >= keys.Count || length < 0)
      {
         throw new ArgumentOutOfRangeException();
      }
      if (keys.Count - (index - keys.Count) < length || (index - items.Count > items.Count - length))
      {
         throw new ArgumentException("length");
      }
      if (length > 1)
      {
         new SorterGenericArray<T, S>(keys, items, comparer).QuickSort(index, index + length - 1);
      }
   }

   public static T[][] SplitArray<T>(T[] arr, int nArrays, bool allowReturningOriginalArray)
   {
      if (allowReturningOriginalArray && nArrays == 1)
      {
         return new T[1][] { arr };
      }
      int nInEach = arr.Length / nArrays;
      int startNext = 0;
      T[][] res = new T[nArrays][];
      for (int i = 0; i < nArrays - 1; i++)
      {
         res[i] = arr.SubArray(startNext, nInEach);
         startNext += nInEach;
      }
      res[nArrays - 1] = arr.SubArray(startNext, arr.Length - startNext);
      return res;
   }

   public static T[][] SplitArray<T>(HashSet<T> arr, int nArrays, bool allowReturningOriginalArray)
   {
      if (allowReturningOriginalArray && nArrays == 1)
      {
         return new T[1][] { arr.ToArray() };
      }
      int nInEach = arr.Count / nArrays;
      int startNext = 0;
      T[][] res = new T[nArrays][];
      for (int i = 0; i < nArrays - 1; i++)
      {
         res[i] = arr.SubArray(startNext, nInEach);
         startNext += nInEach;
      }
      res[nArrays - 1] = arr.SubArray(startNext, arr.Count - startNext);
      return res;
   }

   public static T[][] SplitArray_ByLength<T>(IEnumerable<T> arr, int arrayLength)
   {
      return arr.TakeToJagged(arrayLength).ToArray();
   }

   [CLSCompliant(false)]
   public static uint SumBytes(byte[] arr)
   {
      uint sum = 0u;
      for (int i = 0; i < arr.Length; i++)
      {
         sum += arr[i];
      }
      return sum;
   }

   [CLSCompliant(false)]
   public static uint SumBytes(IEnumerable<byte> arr)
   {
      uint sum = 0u;
      foreach (byte cur in arr)
      {
         sum += cur;
      }
      return sum;
   }

   [CLSCompliant(false)]
   public static ulong SumUInt64(ulong[] arr)
   {
      ulong sum = 0uL;
      for (int i = 0; i < arr.Length; i++)
      {
         sum += arr[i];
      }
      return sum;
   }

   [CLSCompliant(false)]
   public static ulong SumUInt64(IEnumerable<ulong> arr)
   {
      ulong sum = 0uL;
      foreach (ulong cur in arr)
      {
         sum += cur;
      }
      return sum;
   }


   public static IEnumerable<T> Take<T>(this IEnumerable<T> vals, long take)
   {
      using IEnumerator<T> e = vals.GetEnumerator();
      for (long i = 0L; i < take; i++)
      {
         if (e.MoveNext())
         {
            yield return e.Current;
            continue;
         }
         yield break;
      }
   }

   public static IEnumerable<T> TakeSkip<T>(this IEnumerable<T> vals, int take, int skip)
   {
      using IEnumerator<T> e = vals.GetEnumerator();
      while (true)
      {
         for (int j = 0; j < take; j++)
         {
            if (e.MoveNext())
            {
               yield return e.Current;
               continue;
            }
            yield break;
         }
         for (int i = 0; i < skip; i++)
         {
            if (!e.MoveNext())
            {
               yield break;
            }
         }
      }
   }

   public static IEnumerable<T[]> TakeToJagged<T>(this IEnumerable<T> vals, int take)
   {
      using IEnumerator<T> e = vals.GetEnumerator();
      while (true)
      {
         T[] arrToFill = new T[take];
         for (int i = 0; i < take; i++)
         {
            if (e.MoveNext())
            {
               arrToFill[i] = e.Current;
               continue;
            }
            if (i != 0)
            {
               Array.Resize(ref arrToFill, i);
               yield return arrToFill;
            }
            yield break;
         }
         yield return arrToFill;
      }
   }



   public static List<S> ToList<T, S>(this List<T> arr, Func<T, S> conversion)
   {
      List<S> result = new(arr.Count);
      for (int i = 0; i < arr.Count; i++)
      {
         result.Add(conversion(arr[i]));
      }
      return result;
   }

   public static IEnumerable<float> ToRelative(this IEnumerable<float> absolute)
   {
      return absolute.Select((float a) => 0f, (float cur, float prev, int i) => cur - prev);
   }




}
