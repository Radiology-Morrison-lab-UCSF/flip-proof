using FlipProof.Base.Geometry;
using System.Runtime.InteropServices;

namespace FlipProof.Base;

public static class CollectionCreation
{
   public static T[] ArrayOf<T>(T val, int count)
   {
      T[] array = new T[count];
      array.FillArray(val);
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
      if (arr is long[] l)
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
   public static S[] ToArrayMultithread<T, S>(this T[] arr, Func<T, S> conversion)
   {
      S[] result = new S[arr.Length];
      Iteration.Loop_Parallel(0, result.Length, delegate (int i)
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
         Iteration.Loop(0, arr.Length, parallel: true, delegate (int i)
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

   public static int SizeOfType<T>(bool dotNetSizeForBoolean) => SizeOfType(Array.Empty<T>(), dotNetSizeForBoolean);
   public static int SizeOfType<T>(this T[] arr, bool dotNetSizeForBoolean)
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
}
