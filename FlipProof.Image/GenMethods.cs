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

   public static T[] ArrayOf<T>(T val, int count)
   {
      T[] array = new T[count];
      FillArray(array, val);
      return array;
   }

   public unsafe static float[] ArrayOfFloat(float val, int count)
   {
      float[] vals = new float[count];
      fixed (float* ptr = vals)
      {
         for (int i = 0; i < count; i++)
         {
            ptr[i] = val;
         }
      }
      return vals;
   }

   public static double[] ArrayOfRandomD(int count, int? seed = null)
   {
      Random r = (seed.HasValue ? new Random(seed.Value) : new Random());
      double[] vals = new double[count];
      for (int i = 0; i < vals.Length; i++)
      {
         vals[i] = r.NextDouble();
      }
      return vals;
   }

   public static float[] ArrayOfRandomF(int count, int? seed = null)
   {
      Random r = (seed.HasValue ? new Random(seed.Value) : new Random());
      float[] vals = new float[count];
      for (int i = 0; i < vals.Length; i++)
      {
         vals[i] = (float)r.NextDouble();
      }
      return vals;
   }

   public static int[] ArrayOfRandomI(int count, int? seed = null)
   {
      Random r = (seed.HasValue ? new Random(seed.Value) : new Random());
      int[] vals = new int[count];
      for (int i = 0; i < vals.Length; i++)
      {
         vals[i] = r.Next();
      }
      return vals;
   }

   public static bool Contains_AssumeSorted<T>(this T[] sortedArray, T lookFor) where T : IComparable<T> => Array.BinarySearch(sortedArray, lookFor) >= 0;

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

   public static DateTime DateTimeFromMilliseconds(int totalMs) => new DateTime(1, 1, 1, 0, 0, 0, 0).AddMilliseconds(totalMs);

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

   public static void FillArray<T>(T[] vals, T val)
   {
      for (int i = 0; i < vals.Length; i++)
      {
         vals[i] = val;
      }
   }

   public static int[] IncrementalArray(int length)
   {
      int[] arr = new int[length];
      for (int i = 0; i < length; i++)
      {
         arr[i] = i;
      }
      return arr;
   }

   [CLSCompliant(false)]
   public static uint[] IncrementalArrayU(int length)
   {
      uint[] arr = new uint[length];
      for (uint i = 0u; i < length; i++)
      {
         arr[i] = i;
      }
      return arr;
   }

   public static float[] IncrementalArray_StartStop_f(float start, float end, float? increment = null, int? noSteps = null)
   {
      if (!increment.HasValue)
      {
         increment = ((!noSteps.HasValue) ? new float?(1f) : new float?((end - start) / (float)noSteps.Value));
      }
      else if (increment.HasValue == noSteps.HasValue)
      {
         throw new ArgumentException("Can supply only one of increment and noSteps");
      }
      if (!noSteps.HasValue)
      {
         noSteps = (int)Math.Floor((end - start) / increment.Value);
      }
      if (increment == 0f)
      {
         throw new ArgumentOutOfRangeException(nameof(increment), "increment is zero");
      }
      if (end > start && increment < 0f)
      {
         throw new ArgumentOutOfRangeException(nameof(start), "end is greater than start but increment is negative");
      }
      if (end < start && increment > 0f)
      {
         throw new ArgumentOutOfRangeException(nameof(start), "end is less than start but increment is positive");
      }
      float[] arr = new float[noSteps.Value];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = start + increment.Value * (float)i;
      }
      return arr;
   }

   public static int[] IncrementalArray(int start, int length)
   {
      int[] arr = new int[length];
      for (int i = 0; i < length; i++)
      {
         arr[i] = i + start;
      }
      return arr;
   }

   public static int[] IncrementalArray(int start, int length, int increment)
   {
      int[] arr = new int[length];
      for (int i = 0; i < length; i++)
      {
         arr[i] = i * increment + start;
      }
      return arr;
   }

   public static float[] IncrementalArray_f(int start, int length, float increment = 1f)
   {
      float[] arr = new float[length];
      for (int i = 0; i < length; i++)
      {
         arr[i] = (float)i * increment + (float)start;
      }
      return arr;
   }

   public static double[] IncrementalArray_d(int start, int length, double increment = 1.0)
   {
      double[] arr = new double[length];
      for (int i = 0; i < length; i++)
      {
         arr[i] = (double)i * increment + (double)start;
      }
      return arr;
   }

   public static int[] DecrementalArray(int start, int length)
   {
      return IncrementalArray(start, length, -1);
   }

   public static IEnumerable<float> IncrementalIEnum_F(int length)
   {
      for (int i = 0; i < length; i++)
      {
         yield return i;
      }
   }

   public static IEnumerable<float> IncrementalIEnum_F(int startAt, int length)
   {
      for (int i = 0; i < length; i++)
      {
         yield return i + startAt;
      }
   }

   public static IEnumerable<double> IncrementalIEnum_D(int length)
   {
      for (int i = 0; i < length; i++)
      {
         yield return i;
      }
   }

   public static IEnumerable<double> IncrementalIEnum_D(int startAt, int length)
   {
      for (int i = 0; i < length; i++)
      {
         yield return i + startAt;
      }
   }

   public static IEnumerable<T> ForEach<T>(this IEnumerable<T> me, Action<T> a)
   {
      foreach (T cur in me)
      {
         a(cur);
      }
      return me;
   }

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

   public static IEnumerable<XYZf> Loop3DXYZ_Yield_f(int xFrom, int xToExclusive, int yFrom, int yToExclusive, int zFrom, int zToExclusive)
   {
      int i = 0;
      for (int x = xFrom; x < xToExclusive; x++)
      {
         for (int y = yFrom; y < yToExclusive; y++)
         {
            for (int z = zFrom; z < zToExclusive; z++)
            {
               yield return new XYZf(x, y, z);
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
      int[] allIndices = IncrementalArray(source.Length);
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
      Loop_Parallel(0, command_args.Length, delegate (int i)
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

   public static int IndexOf_AssumeSorted<T>(this T[] coll, T lookFor) where T : IEquatable<T>, IComparable<T>
   {
      int index = Array.BinarySearch(coll, lookFor);
      if (index < 0)
      {
         return -1;
      }
      return index;
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

   public static IEnumerable<int> IndexOfAll<T>(this T[] coll, Func<T, bool> lookFor, int[]? onlyInvestigateTheseIndices = null)
   {
      if (onlyInvestigateTheseIndices == null)
      {
         for (int i = 0; i < coll.Length; i++)
         {
            if (lookFor(coll[i]))
            {
               yield return i;
            }
         }
         yield break;
      }
      foreach (int curIndex in onlyInvestigateTheseIndices)
      {
         if (lookFor(coll[curIndex]))
         {
            yield return curIndex;
         }
      }
   }

   public static IEnumerable<int> IndexOfAll<T>(this T[] coll, Func<T, int, bool> lookFor)
   {
      for (int i = 0; i < coll.Length; i++)
      {
         if (lookFor(coll[i], i))
         {
            yield return i;
         }
      }
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

   public static int IndexOfLast<T>(this T[] coll, Func<T, int, bool> lookFor)
   {
      return coll.IndexOfLast(lookFor, coll.Length);
   }

   public static int IndexOfLast<T>(this T[] coll, Func<T, int, bool> lookFor, int largestIndexToSearchExclusive)
   {
      for (int i = Math.Min(coll.Length, largestIndexToSearchExclusive) - 1; i >= 0; i--)
      {
         if (lookFor(coll[i], i))
         {
            return i;
         }
      }
      return -1;
   }

   public static int IndexOfNth<T>(this T[] coll, Func<T, bool> lookFor, int nth, int[]? onlyInvestigateTheseIndices = null)
   {
      int countFound = 0;
      if (onlyInvestigateTheseIndices == null)
      {
         for (int i = 0; i < coll.Length; i++)
         {
            if (lookFor(coll[i]))
            {
               if (countFound == nth)
               {
                  return i;
               }
               countFound++;
            }
         }
      }
      else
      {
         foreach (int curIndex in onlyInvestigateTheseIndices)
         {
            if (lookFor(coll[curIndex]))
            {
               if (countFound == nth)
               {
                  return curIndex;
               }
               countFound++;
            }
         }
      }
      return -1;
   }

   public static T[][] JaggedArray<T>(int length0, int length1)
   {
      T[][] jagged = new T[length0][];
      for (int i = 0; i < jagged.Length; i++)
      {
         jagged[i] = new T[length1];
      }
      return jagged;
   }

   public static T[][] JaggedArray<T>(int length0, int length1, T val)
   {
      T[][] jagged = new T[length0][];
      for (int i = 0; i < jagged.Length; i++)
      {
         jagged[i] = ArrayOf(val, length1);
      }
      return jagged;
   }

   public static T[][][] JaggedArray<T>(int length0, int length1, int length2)
   {
      T[][][] jagged = new T[length0][][];
      for (int i = 0; i < jagged.Length; i++)
      {
         jagged[i] = JaggedArray<T>(length1, length2);
      }
      return jagged;
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
      Loop(0, toDos.Length, parallel, delegate (int i)
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

   public static T[] CastArray<U, T>(this U[] arr, bool returnSameArrayIfSameType = true) where U : struct
   {
      if (arr is T[] at)
      {
         if (returnSameArrayIfSameType)
         {
            return at;
         }
         return (arr.Duplicate() as T[])!;
      }
      return Array.ConvertAll(arr, (U a) => (T)Convert.ChangeType(a, typeof(T)));
   }

   public static double[] CastArrayToDouble<T>(this T[] arr)
   {
      if (arr is float[] af)
      {
         return Array.ConvertAll(af, Convert.ToDouble);
      }
      if (arr is sbyte[] asb)
      {
         return Array.ConvertAll(asb, Convert.ToDouble);
      }
      if (arr is byte[] ab)
      {
         return Array.ConvertAll(ab, Convert.ToDouble);
      }
      if (arr is ushort[] au)
      {
         return Array.ConvertAll(au, Convert.ToDouble);
      }
      if (arr is short[] ash)
      {
         return Array.ConvertAll(ash, Convert.ToDouble);
      }
      if (arr is int[] ai)
      {
         return Array.ConvertAll(ai, Convert.ToDouble);
      }
      if (arr is uint[] aui)
      {
         return Array.ConvertAll(aui, Convert.ToDouble);
      }
      if (arr is long[] al)
      {
         return Array.ConvertAll(al, Convert.ToDouble);
      }
      if (arr is ulong[] aul)
      {
         return Array.ConvertAll(aul, Convert.ToDouble);
      }
      throw new NotSupportedException(typeof(T).Name + " is not supported");
   }

   public static float[] CastArrayToFloat<T>(this T[] arr)
   {
      if (arr is double[] d)
      {
         return Array.ConvertAll(d, Convert.ToSingle);
      }
      if (arr is sbyte[] sb)
      {
         return Array.ConvertAll(sb, Convert.ToSingle);
      }
      if (arr is byte[] b)
      {
         return Array.ConvertAll(b, Convert.ToSingle);
      }
      if (arr is ushort[] us)
      {
         return Array.ConvertAll(us, Convert.ToSingle);
      }
      if (arr is short[] s)
      {
         return Array.ConvertAll(s, Convert.ToSingle);
      }
      if (arr is int[] ai)
      {
         return Array.ConvertAll(ai, Convert.ToSingle);
      }
      if (arr is uint[] ui)
      {
         return Array.ConvertAll(ui, Convert.ToSingle);
      }
      if (arr is long[] l )
      {
         return Array.ConvertAll(l, Convert.ToSingle);
      }
      if (arr is ulong[] ul)
      {
         return Array.ConvertAll(ul, Convert.ToSingle);
      }
      throw new NotSupportedException(typeof(T).Name + " is not supported");
   }

   public static double[] CastArrayToDouble(this float[] arr)
   {
      return Array.ConvertAll(arr, Convert.ToDouble);
   }

   internal static T[][] Duplicate<T>(T[] orig, int clones) where T : struct
   {
      T[][] result = new T[clones][];
      for (int i = 0; i < clones; i++)
      {
         result[i] = orig.Duplicate();
      }
      return result;
   }

   [CLSCompliant(false)]
   public unsafe static IntPtr Duplicate_Generic<T>(void* orig, int size, out GCHandle freeMeWhenDone) where T : struct
   {
      new IntPtr(orig);
      if (typeof(T).Equals(typeof(byte[])))
      {
         return Duplicate(orig, size, out freeMeWhenDone, (byte)0);
      }
      if (typeof(T).Equals(typeof(short[])))
      {
         return Duplicate(orig, size, out freeMeWhenDone, (short)0);
      }
      if (typeof(T).Equals(typeof(int[])))
      {
         return Duplicate(orig, size, out freeMeWhenDone, 0);
      }
      if (typeof(T).Equals(typeof(long[])))
      {
         return Duplicate(orig, size, out freeMeWhenDone, 0L);
      }
      if (typeof(T).Equals(typeof(float[])))
      {
         return Duplicate(orig, size, out freeMeWhenDone, 0f);
      }
      if (typeof(T).Equals(typeof(double[])))
      {
         return Duplicate(orig, size, out freeMeWhenDone, 0.0);
      }
      if (typeof(T).Equals(typeof(char[])))
      {
         return Duplicate(orig, size, out freeMeWhenDone, 0);
      }
      throw new NotSupportedException();
   }

   public static T[] Duplicate<T>(this T[] arr)
   {
      T[] clone = new T[arr.Length];
      Buffer.BlockCopy(arr, 0, clone, 0, arr.Length * SizeOfType(arr, dotNetSizeForBoolean: true));
      return clone;
   }

   public static byte[] Duplicate(this byte[] arr)
   {
      byte[] clone = new byte[arr.Length];
      Buffer.BlockCopy(arr, 0, clone, 0, arr.Length);
      return clone;
   }

   public static float[] Duplicate(this float[] arr)
   {
      float[] clone = new float[arr.Length];
      Buffer.BlockCopy(arr, 0, clone, 0, arr.Length * 4);
      return clone;
   }

   public static T[] DuplicateAndResize<T>(this T[] arr, int newSize) where T : struct
   {
      T[] clone = new T[newSize];
      Buffer.BlockCopy(arr, 0, clone, 0, Math.Min(arr.Length, newSize) * SizeOfType(arr, dotNetSizeForBoolean: true));
      return clone;
   }

   [CLSCompliant(false)]
   public static T[][] Duplicate<T>(this T[][] arr) where T : struct
   {
      return arr.Select((T[] a) => a.Duplicate()).ToArray();
   }

   public static int SizeOfType<T>( bool dotNetSizeForBoolean) => SizeOfType(Array.Empty<T>(), dotNetSizeForBoolean);
   public static int SizeOfType<T>(T[] arr, bool dotNetSizeForBoolean)
   {
      if (arr is float[])
      {
         return 4;
      }
      if (arr is double[])
      {
         return 8;
      }
      if (arr is ulong[])
      {
         return 8;
      }
      if (arr is long[])
      {
         return 8;
      }
      if (arr is uint[])
      {
         return 4;
      }
      if (arr is int[])
      {
         return 4;
      }
      if (arr is ushort[])
      {
         return 2;
      }
      if (arr is short[])
      {
         return 2;
      }
      if (arr is byte[])
      {
         return 1;
      }
      if (arr is sbyte[])
      {
         return 1;
      }
      if (arr is bool[] && dotNetSizeForBoolean)
      {
         return 1;
      }
      throw new NotImplementedException("Type " + typeof(T).Name + " not implemented");
   }

   public static int SizeOfType_Bits<T>(bool dotNetSizeForBoolean)
   {
      Type t = typeof(T);
      int size;
      if (t.Equals(typeof(float)))
      {
         size = 4;
      }
      else if (t.Equals(typeof(double)))
      {
         size = 8;
      }
      else if (t.Equals(typeof(ulong)))
      {
         size = 8;
      }
      else if (t.Equals(typeof(long)))
      {
         size = 8;
      }
      else if (t.Equals(typeof(uint)))
      {
         size = 4;
      }
      else if (t.Equals(typeof(int)))
      {
         size = 4;
      }
      else if (t.Equals(typeof(ushort)))
      {
         size = 2;
      }
      else if (t.Equals(typeof(short)))
      {
         size = 2;
      }
      else if (t.Equals(typeof(byte)))
      {
         size = 1;
      }
      else if (t.Equals(typeof(sbyte)))
      {
         size = 1;
      }
      else
      {
         if (!t.Equals(typeof(bool)))
         {
            throw new NotImplementedException("Type " + typeof(T).Name + " not implemented");
         }
         if (!dotNetSizeForBoolean)
         {
            return 1;
         }
         size = 1;
      }
      return size * 8;
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

   public static bool SequenceEqual<T>(this T[] a1, T[] a2)
   {
      if (a1 is Line[] al)
      {
         return al.SequenceEqual_Fast((a2 as Line[])!);
      }
      if (a1 is byte[] ab)
      {
         return (ab).SequenceEqual_Fast((a2 as byte[])!);
      }
      if (a1 is sbyte[] asb)
      {
         return (asb).SequenceEqual_Fast((a2 as sbyte[])!);
      }
      if (a1 is ushort[] aus)
      {
         return (aus).SequenceEqual_Fast((a2 as ushort[])!);
      }
      if (a1 is int[] ai)
      {
         return (ai).SequenceEqual_Fast((a2 as int[])!);
      }
      if (a1 is uint[] au)
      {
         return (au).SequenceEqual_Fast((a2 as uint[])!);
      }
      if (a1 is float[] af)
      {
         return (af).SequenceEqual_Fast((a2 as float[])!, 0f);
      }
      if (a1 is double[] ad)
      {
         return (ad).SequenceEqual_Fast((a2 as double[])!, 0.0);
      }
      if (a1 is bool[] abool)
      {
         return abool.SequenceEqual_Fast((a2 as bool[])!);
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

   public static S[] ToArrayMultithread<T, S>(this T[] arr, Func<T, S> conversion)
   {
      S[] result = new S[arr.Length];
      Loop_Parallel(0, result.Length, delegate (int i)
      {
         result[i] = conversion(arr[i]);
      });
      return result;
   }

   public static S[] ToArray<T, S>(this T[] arr, Func<T, S> conversion, bool multithread = false)
   {
      S[] result = new S[arr.Length];
      if (multithread)
      {
         Loop(0, arr.Length, parallel: true, delegate (int i)
         {
            result[i] = conversion(arr[i]);
         });
      }
      else
      {
         for (int j = 0; j < arr.Length; j++)
         {
            result[j] = conversion(arr[j]);
         }
      }
      return result;
   }

   public static T[,] ToNonJaggedArray<T>(this T[][] arr)
   {
      if (arr.Length == 0 || arr[0].Length == 0)
      {
         return new T[arr.Length, 0];
      }
      T[,] result = new T[arr.Length, arr[0].Length];
      for (int i = 0; i < arr.Length; i++)
      {
         T[] cur = arr[i];
         for (int j = 0; j < cur.Length; j++)
         {
            result[i, j] = cur[j];
         }
      }
      return result;
   }

   public static byte[] ToBytes<T>(this T[] arr) where T : struct
   {
      int sizeOfT = SizeOfType(arr, dotNetSizeForBoolean: true);
      byte[] clone = new byte[arr.Length * sizeOfT];
      arr.ToBytes(clone);
      return clone;
   }

   public static void ToBytes<T>(this T[] arr, byte[] buffer) where T : struct
   {
      int sizeOfT = SizeOfType(arr, dotNetSizeForBoolean: true);
      int noBytes = arr.Length * sizeOfT;
      if (buffer.Length < noBytes)
      {
         throw new ArgumentException("Buffer is too small");
      }
      Buffer.BlockCopy(arr, 0, buffer, 0, noBytes);
   }

   public static T[] FromByteArray<T>(this byte[] arr) where T : struct
   {
      int sizeOfT = SizeOfType(new T[0], dotNetSizeForBoolean: true);
      if (arr.Length % sizeOfT != 0)
      {
         throw new ArgumentException("Input array does not split evenly to allow deserialisation to type " + typeof(T).FullName);
      }
      T[] destination = new T[arr.Length / sizeOfT];
      Buffer.BlockCopy(arr, 0, destination, 0, arr.Length);
      return destination;
   }

   public static List<S> ToList<T, S>(this List<T> arr, Func<T, S> conversion)
   {
      List<S> result = new List<S>(arr.Count);
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

   public static string[][] Transpose(this string[][] bySubjectThenLine)
   {
      string[][] trans = JaggedArray<string>(bySubjectThenLine[0].Length, bySubjectThenLine.Length);
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

   [CLSCompliant(false)]
   public unsafe static IntPtr Duplicate(void* orig, int size, out GCHandle freeMeWhenDone, byte def)
   {
      byte[] arr = new byte[size];
      freeMeWhenDone = GCHandle.Alloc(arr, GCHandleType.Pinned);
      IntPtr result = freeMeWhenDone.AddrOfPinnedObject();
      if (size != 0)
      {
         Marshal.Copy(new IntPtr(orig), arr, 0, size);
      }
      return result;
   }

   [CLSCompliant(false)]
   public unsafe static IntPtr Duplicate(void* orig, int size, out GCHandle freeMeWhenDone, short def)
   {
      short[] arr = new short[size];
      freeMeWhenDone = GCHandle.Alloc(arr, GCHandleType.Pinned);
      IntPtr result = freeMeWhenDone.AddrOfPinnedObject();
      if (size != 0)
      {
         Marshal.Copy(new IntPtr(orig), arr, 0, size);
      }
      return result;
   }

   [CLSCompliant(false)]
   public unsafe static IntPtr Duplicate(void* orig, int size, out GCHandle freeMeWhenDone, int def)
   {
      int[] arr = new int[size];
      freeMeWhenDone = GCHandle.Alloc(arr, GCHandleType.Pinned);
      IntPtr result = freeMeWhenDone.AddrOfPinnedObject();
      if (size != 0)
      {
         Marshal.Copy(new IntPtr(orig), arr, 0, size);
      }
      return result;
   }

   [CLSCompliant(false)]
   public unsafe static IntPtr Duplicate(void* orig, int size, out GCHandle freeMeWhenDone, long def)
   {
      long[] arr = new long[size];
      freeMeWhenDone = GCHandle.Alloc(arr, GCHandleType.Pinned);
      IntPtr result = freeMeWhenDone.AddrOfPinnedObject();
      if (size != 0)
      {
         Marshal.Copy(new IntPtr(orig), arr, 0, size);
      }
      return result;
   }

   [CLSCompliant(false)]
   public unsafe static IntPtr Duplicate(void* orig, int size, out GCHandle freeMeWhenDone, float def)
   {
      float[] arr = new float[size];
      freeMeWhenDone = GCHandle.Alloc(arr, GCHandleType.Pinned);
      IntPtr result = freeMeWhenDone.AddrOfPinnedObject();
      if (size != 0)
      {
         Marshal.Copy(new IntPtr(orig), arr, 0, size);
      }
      return result;
   }

   [CLSCompliant(false)]
   public unsafe static IntPtr Duplicate(void* orig, int size, out GCHandle freeMeWhenDone, double def)
   {
      double[] arr = new double[size];
      freeMeWhenDone = GCHandle.Alloc(arr, GCHandleType.Pinned);
      IntPtr result = freeMeWhenDone.AddrOfPinnedObject();
      if (size != 0)
      {
         Marshal.Copy(new IntPtr(orig), arr, 0, size);
      }
      return result;
   }
}
