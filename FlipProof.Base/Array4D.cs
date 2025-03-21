using System.Collections.Immutable;
using System.Numerics;

namespace FlipProof.Base;
public class Array4D<T> : IVoxelArray<T>, IDisposable
   where T : struct
{
   #region PARAMETERS
   // --- ADDITIONAL PARAMETERS TO CLONE METHOD(S) ----

   private readonly Array3D<T>[] _data;
   private bool _isDisposed;

   public IReadOnlyList<Array3D<T>> Data => _data;

   public int Size0 { get; }
   public int Size1 { get; }
   public int Size2 { get; }
   public int Size3 { get; }

   public IReadOnlyList<int> Shape { get; }

   public long NumberOfVoxels => Size0 * Size1 * Size2 * Size3;

   // --- ADDITIONAL PARAMETERS TO CLONE METHOD(S) ----
   #endregion


   /// <summary>
   /// Creates a 4D array from a stack of 3D arrays
   /// </summary>
   /// <param name="data"></param>
   /// <param name="linked">Uses the arrays provided directly. If false, they are copied</param>
   /// <exception cref="ArgumentException"></exception>
   public Array4D(Array3D<T>[] data, bool linked = false)
   {
      if (data.Length == 0)
      {
         throw new ArgumentException("Array4D cannot be empty");
      }
      Array3D<T>.ThrowIfSizesNotEqual(data);


      Size0 = data[0].Size0;
      Size1 = data[0].Size1;
      Size2 = data[0].Size2;
      Size3 = data.Length;
      Shape = ImmutableArray.Create([Size0, Size1, Size2, Size3]);

      if (Size0 < 0 || Size1 < 0 || Size2 < 0)
      {
         throw new ArgumentException("Sizes must be greater than or equal to 0.");
      }


      _data = new Array3D<T>[Size3];
      if (linked)
      {
         for (var i = 0; i < data.Length; i++)
         {
            _data[i] = data[i];
         }
      }
      else
      {
         InitialiseAsBlank();
         for (var i = 0; i < data.Length; i++)
         {
            data[i].CloneInto(_data[i]);
         }
      }
   }

   private void InitialiseAsBlank()
   {
      for (int i = 0; i < _data.Length; i++)
      {
         _data[i] = new Array3D<T>(Size0, Size1, Size2);
      }
   }

   /// <summary>
   /// Creates a new 4D array
   /// </summary>
   /// <param name="size0">x</param>
   /// <param name="size1">y</param>
   /// <param name="size2">z</param>
   /// <param name="size3">number of 3d volumes</param>
   /// <exception cref="ArgumentException"></exception>
   public Array4D(int size0, int size1, int size2, int size3)
   {
      if (size0 < 0 || size1 < 0 || size2 < 0 || size3 < 0)
      {
         throw new ArgumentException("Sizes must be greater than or equal to 0.");
      }

      Size0 = size0;
      Size1 = size1;
      Size2 = size2;
      Size3 = size3;
      Shape = ImmutableArray.Create([Size0, Size1, Size2, Size3]);

      _data = new Array3D<T>[size3];
      InitialiseAsBlank();
   }
   /// <summary>
   /// Creates a new 4D array
   /// </summary>
   /// <param name="size0">x</param>
   /// <param name="size1">y</param>
   /// <param name="size2">z</param>
   /// <param name="size3">number of 3d volumes</param>
   /// <param name="voxels">Ordered i (fastest),j,k,volume (slowest)</param>
   /// <exception cref="ArgumentException"></exception>
   public Array4D(int size0, int size1, int size2, int size3, T[] voxels) : this(size0, size1, size2, size3)
   {
      if(voxels.Length != NumberOfVoxels)
      {
         throw new ArgumentException($"Expected {voxels.Length} voxels but got {NumberOfVoxels}");
      }
      int voxelsPerVol = Size0 * Size1 * Size2;
      int offset = 0;
      for (int i = 0; i < _data.Length; i++)
      {
         _data[i].SetAllVoxels_XFastest(voxels.AsSpan(offset, voxelsPerVol));
         offset += voxelsPerVol;
      }
   }

   public static Array4D<T> FromValueGenerator(int size0, int size1, int size2, int size3, Func<T> valueGenerator)
   {
      Array4D<T> arr = new(size0, size1, size2, size3);
      arr.ApplyInPlace(a => valueGenerator());
      return arr;
   }

   /// <summary>
   /// Creates a new blank <see cref="Array3D{T}"/> based on the size of this array
   /// </summary>
   /// <typeparam name="S">Voxel type</typeparam>
   public Array4D<S> CreateNewEmpty<S>() where S : struct => new(Size0, Size1, Size2, Size3);

   public Array3D<T> this[int iVolume] => _data[iVolume];
   public T this[XYZA<int> index] => _data[index.A][index.X, index.Y, index.Z];
   public T this[int x, int y, int z, int vol]
   {
      get => _data[vol][x, y, z];
      set => _data[vol][x, y, z] = value;
   }

   /// <summary>
   /// Applies the provided function to each element, one at a time, and replaces the value stored. 
   /// Value is replaced upon calculation for that element, not after all elements have been calculated
   /// </summary>
   /// <param name="function"></param>
   public void ApplyInPlace(Func<T, T> function) => _data.Foreach(a => a.ApplyInPlace(function));

   /// <summary>
   /// Clones this object and applies a function to that clone
   /// </summary>
   /// <param name="function"></param>
   /// <returns></returns>
   public Array4D<T> ApplyIntoNew(Func<T, T> function)
   {
      var clone = Clone();
      clone.ApplyInPlace(function);
      return clone;
   }

   /// <summary>
   /// Clones this object and applies a function to that clone
   /// </summary>
   /// <param name="function"></param>
   /// <returns></returns>
   public Array4D<S> ApplyIntoNew<S>(Func<T, S> function) where S : struct, INumber<S>
   {
      var clone = new Array4D<S>(Size0, Size1, Size2, Size3);

      for (int i = 0; i < Size3; i++)
      {
         var srcIm = _data[i];
         var destIm = clone[i];
         srcIm.ApplyFunction(function, destIm);
      }

      return clone;
   }
   /// <summary>
   /// Clones this object and applies a function to that clone
   /// </summary>
   /// <param name="threadsafeFunction">One thread per image is used</param>
   /// <returns></returns>
   public Array4D<S> ApplyIntoNewInParallel<S>(Func<T, S> threadsafeFunction) where S : struct, INumber<S>
   {
      var clone = new Array4D<S>(Size0, Size1, Size2, Size3);

      Parallel.For(0, Size3, i =>
      {
         var srcIm = _data[i];
         var destIm = clone[i];
         srcIm.ApplyFunction(threadsafeFunction, destIm);
      });

      return clone;
   }

   public Array4D<T> Clone()
   {
      Array4D<T> other = new Array4D<T>(Size0, Size1, Size2, Size3);
      for (int i = 0; i < Size3; i++)
      {
         this[i].CloneInto(other[i]);
      }

      return other;
   }

   /// <summary>
   /// Flattens to a single array. Use with caution as large series will be too large to fit in an array
   /// </summary>
   /// <returns></returns>
   public T[] GetValuesAsArray()
   {
      T[] values = new T[Size0 * Size1 * Size2 * Size3];

      int offset = 0;
      for (int i = 0; i < Size3; i++)
      {
         foreach (var item in Data[i].GetValues())
         {
            values[offset++] = item;
         }
      }

      return values;
   }

   public IEnumerable<T> GetValues()
   {
      for (int i = 0; i < Size3; i++)
      {
         foreach (var item in Data[i].GetValues())
         {
            yield return item;
         }
      }
   }

   public Array4D<int> GetVoxelRanks()
   {
      T[] values = GetValuesAsArray();
      Array.Sort(values);

      var ranks = ApplyIntoNew(val => Array.BinarySearch(values, val));

      return ranks;
   }

   /// <summary>
   /// Scales voxels from 0 to 1, inclusive, based on how large their value is relative to others
   /// </summary>
   /// <returns></returns>
   public Array4D<float> GetVoxelRanksF()
   {
      T[] values = GetValuesAsArray();
      Array.Sort(values);

      double initialScale = 1 / (values.Length - 1d);// theoretical max is length -1 as we're 1 indexed
      Array4D<float> ranks = ApplyIntoNewInParallel(val => Convert.ToSingle(Array.BinarySearch(values, val) * initialScale));

      // Due to ties with extreme voxels, the range is not necessarily 0 to 1 from the binary search
      float min = ranks.GetValues().Min();
      float max = ranks.GetValues().Max();

      float scale = 1 / (max - min);
      ranks.ApplyInPlace(a => (a - min) * scale);


      return ranks;
   }

   /// <summary>
   /// Writes voxels to the stream, ordered x,y,z,vols
   /// </summary>
   /// <param name="s"></param>
   public void GetAllVoxels_XFastest(Stream s)
   {
      foreach (var item in _data)
      {
         s.Write(item.GetAllVoxels_XFastest().ToBytes());
      }
   }


   public T[] GetAllVoxels_LastDimFastest()
   {
      T[][] voxelsByImage = _data.ToArray(a => a.GetAllVoxels_LastDimFastest());

      T[] oneD = new T[NumberOfVoxels];

      int offset = 0;
      long voxPer3D = Size0 * Size1 * Size2;
      for (int iVox = 0; iVox < voxPer3D; iVox++)
      {
         for (int im = 0; im < voxelsByImage.Length; im++)
         {
            oneD[offset++] = voxelsByImage[im][iVox];
         }
      }

      // eager clean up as memory pressure might be high
      for (int i = 0;i < voxelsByImage.Length;i++)
      {
         voxelsByImage[i] = null!;
      }

      return oneD;
   }

   public void SetAllVoxels_XFastest(ReadOnlySpan<T> voxels)
   {
      T[][] voxelsByImage = Array2D<T>.UnconcatRowwise(voxels, Size0, Size1 * Size2 * Size3);

      for (int i = 0; i < _data.Length; i++)
      {
         _data[i].SetAllVoxels_XFastest(voxelsByImage[i]);
      }
   }
   public void SetAllVoxels_LastDimFastest(ReadOnlySpan<T> voxels)
   {
      if(voxels.Length != NumberOfVoxels)
      {
         throw new ArgumentException($"Expected {NumberOfVoxels} voxels");
      }
      int offset = 0;
      long voxPer3D = Size0 * Size1 * Size2;
      T[][] voxelsByImage = _data.ToArray(a=> new T[voxPer3D]);
      for (int iVox = 0; iVox < voxPer3D; iVox++)
      {
         for (int iIm = 0; iIm < _data.Length; iIm++)
         {
            voxelsByImage[iIm][iVox] = voxels[offset++];
         }
      }


      for (int i = 0; i < _data.Length; i++)
      {
         _data[i].SetAllVoxels_LastDimFastest(voxelsByImage[i]);
         voxelsByImage[i] = null!; // eagerly helping GC reduce pressure
      }
   }

   protected virtual void Dispose(bool disposing)
   {
      if (!_isDisposed)
      {
         if (disposing)
         {
            // dispose managed state (managed objects)
         }

         // TODO: free unmanaged resources (unmanaged objects) and override finalizer
         
         // set large fields to null
         for (int i = 0; i < _data.Length; i++)
         {
            _data[i] = null!;
         }
         _isDisposed = true;
      }
   }

   // // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
   // ~Array4D()
   // {
   //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
   //     Dispose(disposing: false);
   // }

   public void Dispose()
   {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
   }
}
