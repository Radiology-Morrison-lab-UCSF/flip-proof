using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace FlipProof.Base;
/// <summary>
/// Describes a 2D array of numbers
/// </summary>
public class Array2D<T> : IVoxelArray<T> where T : struct
{
   #region PARAMETERS

   private readonly T[][] data;

   public IReadOnlyList<T[]> Data => data;

   public int Size0 { get; }
   public int Size1 { get; }
   public long NumberOfVoxels => Size0 * Size1;

   public IReadOnlyList<int> Shape { get; }

   public T this[int i, int j]
   {
      get { return data[i][j]; }
      set { data[i][j] = value; }
   }

   #endregion

   public Array2D(int size0, int size1)
   {
      if (size0 < 0 || size1 < 0)
      {
         throw new ArgumentException("Sizes must be greater than or equal to 0.");
      }

      Size0 = size0;
      Size1 = size1;
      Shape = ImmutableArray.Create<int>(size0, size1);

      data = CreateJagged(size0, size1);
   }

   private static T[][] CreateJagged(int size0, int size1)
   {
      T[][] data = new T[size0][];
      for (int i = 0; i < size0; i++)
      {
         data[i] = new T[size1];
      }
      return data;
   }

   /// <summary>
   /// Applies the provided function to each element, one at a time, and replaces the value stored. 
   /// Value is replaced upon calculation for that element, not after all elements have been calculated
   /// </summary>
   public void ApplyInPlace(Func<T, T> function)
   {
      foreach (T[] d in data)
      {
         d.ApplyInPlace(function);
      }
   }
   /// <summary>
   /// Applies the provided function to each element, one at a time, and replaces the value stored. 
   /// Value is replaced upon calculation for that element, not after all elements have been calculated
   /// </summary>
   public void ApplyInPlace(Func<int, int, T, T> function)
   {
      for (int x = 0; x < data.Length; x++)
      {
         T[] cur = data[x];
         for (int y = 0; y < cur.Length; y++)
         {
            cur[y] = function(x, y, cur[y]);
         }
      }
   }

   /// <summary>
   /// Sets voxel values from a 3D array from another in which x changes fastest, then y, then z
   /// </summary>
   public T[] GetAllVoxels_XFastest() => ConcatRowwise(data);

   /// <summary>
   /// Concatenates the array so that it is ordered arr[0,0], arr[1,0], arr[2,0] ... arr[0,1], arr[1,1], arr[2,1] etc
   /// </summary>
   /// <param name="jagged">All arrays must be the same length and cannot be empty</param>
   /// <returns></returns>
   public static T[] ConcatRowwise(T[][] jagged)
   {
      T[] concat = new T[jagged.Length * jagged[0].Length];
      int offset = 0;
      int noVox = jagged[0].Length;
      for (int y = 0; y < noVox; y++)
      {
         for (int x = 0; x < jagged.Length; x++)
         {
            concat[offset++] = jagged[x][y];
         }
      }

      return concat;
   }


   /// <summary>
   /// Unconcatenates the array internally oredered arr[0,0], arr[1,0], arr[2,0] ... arr[0,1], arr[1,1], arr[2,1] etc
   /// </summary>
   internal static T[][] UnconcatRowwise(ReadOnlySpan<T> flattened, int size0, int size1)
   {
      T[][] result = CreateJagged(size0, size1);
      UnconcatRowwise(flattened, result);
      return result;
   }

   /// <summary>
   /// Unconcatenates the array internally oredered arr[0,0], arr[1,0], arr[2,0] ... arr[0,1], arr[1,1], arr[2,1] etc
   /// </summary>
   /// <param name="jagged">All arrays must be the same length</param>
   /// <returns></returns>
   internal static void UnconcatRowwise(ReadOnlySpan<T> flattened, T[][] toFill)
   {
      int offset = 0;
      int size0 = toFill.Length;
      int size1 = toFill[0].Length;
      for (int k = 0; k < size1; k++)
      {
         for (int j = 0; j < size0; j++)
         {
            toFill[j][k] = flattened[offset++];
         }
      }
   }

   /// <summary>
   /// Sets voxel values from a 2D array from another in which x changes fastest, then y, then z
   /// </summary>
   public void SetAllVoxels_XFastest(ReadOnlySpan<T> arr2D)
   {
      if (arr2D.Length != NumberOfVoxels)
      {
         throw new ArgumentException($"Bad number of voxels: {arr2D.Length}. Expected: {NumberOfVoxels}", nameof(arr2D));
      }

      int offset = 0;
      for (int k = 0; k < Size1; k++)
      {
         for (int j = 0; j < data.Length; j++)
         {
            data[j][k] = arr2D[offset];

            offset++;
         }
      }
   }

   public T[] GetAllVoxels_LastDimFastest()
   {
      T[] result = new T[NumberOfVoxels];
      int offset = 0;
      for (int i = 0; i < data.Length; i++)
      {
         data.CopyTo(result, offset);
         offset += data.Length;
      }

      return result;
   }

   public void SetAllVoxels_LastDimFastest(ReadOnlySpan<T> arr2D)
   {
      if (arr2D.Length != NumberOfVoxels)
      {
         throw new ArgumentException($"Bad number of voxels: {arr2D.Length}. Expected: {NumberOfVoxels}", nameof(arr2D));
      }

      int offset = 0;
      for (int j = 0; j < Size1; j++)
      {
         var dest = data[j];
         for (int k = 0; k < dest.Length; k++)
            dest[k] = arr2D[offset++];
      }
   }
}
