using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace FlipProof.Base;
/// <summary>
/// Describes a 3D array of numbers
/// </summary>
/// <typeparam name="T">Number type</typeparam>
public class Array3D<T> : IVoxelArray<T>, IDisposable where T : struct
{
   #region PARAMETERS
   // --- ADDITIONAL PARAMETERS TO CLONE METHOD(S) ----

   private readonly T[][][] data;
   private bool _isDisposed;
   public IReadOnlyList<IReadOnlyList<T[]>> Data => data;

   public int Size0 { get; }
   public int Size1 { get; }
   public int Size2 { get; }
   public long NumberOfVoxels => Size0 * Size1 * Size2;

   public IReadOnlyList<int> Shape { get; }

   // --- ADDITIONAL PARAMETERS TO CLONE METHOD(S) ----
   #endregion

   public Array3D(XYZ<int> size) : this(size.X, size.Y, size.Z)
   { }
   public Array3D(int size0, int size1, int size2)
   {
      if (size0 < 0 || size1 < 0 || size2 < 0)
      {
         throw new ArgumentException("Sizes must be greater than or equal to 0.");
      }

      Size0 = size0;
      Size1 = size1;
      Size2 = size2;
      Shape = ImmutableArray.Create<int>(size0, size1, size2);

      data = new T[size0][][];
      for (int i = 0; i < size0; i++)
      {
         T[][] di = new T[size1][];
         data[i] = di;
         for (int j = 0; j < size1; j++)
         {
            di[j] = new T[size2];
         }
      }
   }


   public static Array3D<T> FromRandom(Func<T> randomGenerator, XYZ<int> size) => FromRandom(randomGenerator, size.X, size.Y, size.Z);
   public static Array3D<T> FromRandom(Func<T> randomGenerator, int size0, int size1, int size2)
   {
      T[] array = new T[size0 * size1 * size2];

      for (int i = 0; i < array.Length; i++)
      {
         array[i] = randomGenerator();
      }

      return From1D_XFastest(array, size0, size1, size2);
   }

   /// <summary>
   /// Creates an new object given a value generator
   /// </summary>
   /// <param name="generator">Accepts i,j,k and returns the expected value</param>
   /// <param name="size0"></param>
   /// <param name="size1"></param>
   /// <param name="size2"></param>
   /// <returns></returns>
   public static Array3D<T> FromValueGenerator(Func<int, int, int, T> generator, int size0, int size1, int size2)
   {
      Array3D<T> array = new(size0, size1, size2);

      for (int i = 0; i < size0; i++)
      {
         for (int j = 0; j < size1; j++)
         {
            for (int k = 0; k < size2; k++)
            {
               array[i,j,k] = generator(i, j, k);
            }
         }
      }

      return array;
   }

   /// <summary>
   /// Generates a 3D array from another in which z changes fastest, then y, then x
   /// </summary>
   /// <param name="doubles"></param>
   /// <returns></returns>
   /// <exception cref="NotImplementedException"></exception>
   public static Array3D<T> From1D_ZFastest(T[] arr1D, int size0, int size1, int size2)
   {
      // ITK documentation:
      // For example a 3D image buffer should be accessed:  uint8_t* buffer = img->GetBufferAsUInt8();
      // buffer[c + numComponents*(x+xSize*(y+ySize*z))]

      Array3D<T> ar3D = new(size0, size1, size2);
      ar3D.SetAllVoxels_LastDimFastest(arr1D);
      return ar3D;


   }
   /// <summary>
   /// Generates a 3D array from another in which z changes fastest, then y, then x
   /// </summary>
   /// <remarks>This is the appropriate method for ITK data</remarks>
   /// <param name="doubles"></param>
   /// <returns></returns>
   /// <exception cref="NotImplementedException"></exception>
   public static Array3D<T> From1D_XFastest(T[] arr1D, int size0, int size1, int size2)
   {
      // ITK documentation:
      // For example a 3D image buffer should be accessed:  uint8_t* buffer = img->GetBufferAsUInt8();
      // buffer[c + numComponents*(x+xSize*(y+ySize*z))]

      Array3D<T> ar3D = new(size0, size1, size2);
      ar3D.SetAllVoxels_XFastest(arr1D);
      return ar3D;


   }

   public T this[XYZ<int> index]
   {
      get => data[index.X][index.Y][index.Z];
      set => data[index.X][index.Y][index.Z] = value;
   }
   public T this[int i, int j, int k]
   {
      get { return data[i][j][k]; }
      set { data[i][j][k] = value; }
   }
   
   /// <summary>
   /// Set all voxels within this box to the provided value
   /// </summary>
   /// <param name="box"></param>
   /// <returns></returns>
   public T this[Box<int> box]
   {
      set
      {
         box.ForEachPosition((x, y, z) => data[x][y][z] = value);
      }
   }

   /// <summary>
   /// Returns a span that can be read as [y][z]
   /// </summary>
   /// <param name="x"></param>
   /// <param name="y"></param>
   /// <returns></returns>
   public ReadOnlySpan<T[]> GetYZCol(int x) => data[x].AsSpan();
   public Span<T> GetZCol(int x, int y) => data[x][y].AsSpan();
   public Array2D<T> GetZSlice(int z)
   {
      Array2D<T> slice = new(Size0, Size1);

      for (int x = 0; x < Size0; x++)
      {
         T[][] cur = data[x];

         for (int y = 0; y < cur.Length; y++)
         {
            slice[x, y] = cur[y][z];
         }
      }

      return slice;
   }
   public Array2D<T> SetZSlice(int z, Array2D<T> slice)
   {

      for (int x = 0; x < Size0; x++)
      {
         T[][] cur = data[x];

         for (int y = 0; y < cur.Length; y++)
         {
            cur[y][z] = slice[x, y];
         }
      }

      return slice;
   }
   /// <summary>
   /// Crops this array to a smaller region
   /// </summary>
   /// <param name="i"></param>
   /// <param name="j"></param>
   /// <param name="k"></param>
   /// <returns></returns>
   /// <exception cref="ArgumentOutOfRangeException"></exception>
   public Array3D<T> this[Range i, Range j, Range k] => Crop(i, j, k);

   public Array3D<T> Crop(Range i, Range j, Range k)
   {
      (int iOffset, int iLength, int iTo) = SplitAndCheckBounds(i, Size0, nameof(i));
      (int jOffset, int jLength, int jTo) = SplitAndCheckBounds(j, Size1, nameof(j));
      (int kOffset, int kLength, int kTo) = SplitAndCheckBounds(k, Size2, nameof(k));

      Array3D<T> cropped = new(iLength, jLength, kLength);

      CopyIntoUnsafe(cropped, iOffset, iTo, jOffset, jTo, kOffset, kLength, 0, 0, 0);

      return cropped;

   }
      static (int iOffset, int iLength, int iTo) SplitAndCheckBounds(Range i, int dimSize, string paramName)
      {
         (int iOffset, int iLength) = i.GetOffsetAndLength(dimSize);
         int iTo = iOffset + iLength;
         if (iOffset < 0 || iTo > dimSize)
         {
            throw new ArgumentOutOfRangeException(paramName);
         }
         return (iOffset, iLength, iTo);
      }
   public Array3D<T> PadTo(int newSize0, int newSize1, int newSize2)
   {
      if(newSize0 < Size0)
      {
         throw new ArgumentException("Padding is negative", nameof(newSize0));
      }
      if(newSize1 < Size1)
      {
         throw new ArgumentException("Padding is negative", nameof(newSize1));
      }
      if(newSize2 < Size2)
      {
         throw new ArgumentException("Padding is negative", nameof(newSize2));
      }

      Array3D<T> padded = new(newSize0, newSize1, newSize2);

      CopyIntoUnsafe(padded, 0, Size0, 0, Size1, 0, Size2, 0, 0, 0);

      return padded;
   }

   public void CopyInto(Array3D<T> dest, Range iSrc, Range jSrc, Range kSrc, int iDest, int jDest, int kDest)
   {
      (int iOffset, int iLength, int iTo) = SplitAndCheckBounds(iSrc, Size0, nameof(iSrc));
      (int jOffset, int jLength, int jTo) = SplitAndCheckBounds(jSrc, Size1, nameof(jSrc));
      (int kOffset, int kLength, int kTo) = SplitAndCheckBounds(kSrc, Size2, nameof(kSrc));

      if (!(dest.InBounds_I(new Range(iDest, iDest + iSrc.GetOffsetAndLength(Size0).Length)) &&
         dest.InBounds_J(new Range(jDest, jDest + jSrc.GetOffsetAndLength(Size1).Length)) && 
         dest.InBounds_K(new Range(kDest, kDest + kSrc.GetOffsetAndLength(Size1).Length))))
      {
         throw new ArgumentOutOfRangeException();
      }

      CopyIntoUnsafe(dest, iOffset, iTo, jOffset, jTo, kOffset, kLength, iDest, jDest, kDest);
   }
   /// <summary>
   /// No bounds checking. Beward of argument irregularity
   /// </summary>
   /// <param name="dest"></param>
   /// <param name="iSrcOffset"></param>
   /// <param name="iSrcTo"></param>
   /// <param name="jSrcOffset"></param>
   /// <param name="jSrcTo"></param>
   /// <param name="kSrcOffset"></param>
   /// <param name="kSrcLength"></param>
   private void CopyIntoUnsafe(Array3D<T> dest, int iSrcOffset, int iSrcTo, int jSrcOffset, int jSrcTo, int kSrcOffset, int kSrcLength, int iDest, int jDest, int kDest)
   {
      for (int iSrc = iSrcOffset; iSrc < iSrcTo; iSrc++, iDest++)
      {
         T[][] srcI = data[iSrc];
         T[][] destI = dest.data[iDest];
         Copy2D(srcI, destI, jDest);
      }

      void Copy2D(T[][] srcI, T[][] destI, int jDest)
      {
         for (int jSrc = jSrcOffset; jSrc < jSrcTo; jSrc++, jDest++)
         {
            ReadOnlySpan<T> srcJ = new(srcI[jSrc], kSrcOffset, kSrcLength);
            Span<T> destJ = destI[jDest].AsSpan(kDest);
            srcJ.CopyTo(destJ);
         }
      }
   }

   public bool InBounds(XYZ<int> k) => k.X < Size0 && k.Y < Size1 && k.Z < Size2 && k.X >= 0 && k.Y >= 0 && k.Z >= 0;
   public bool InBounds_I(Range r) => InBounds(r, Size0);
   public bool InBounds_J(Range r) => InBounds(r, Size1);
   public bool InBounds_K(Range r) => InBounds(r, Size2);

   private static bool InBounds(Range r, int dimSize)
   {
      (int offset, int length) = r.GetOffsetAndLength(dimSize);
      return offset >= 0 && offset + length <= dimSize;
   }

   /// <summary>
   /// Zeros the 2D area with this index at the first dimension
   /// </summary>
   /// <param name="i"></param>
   /// <exception cref="IndexOutOfRangeException"></exception>
   public void ZeroDim0(int i)
   {
      if (i < 0 || i >= Size0)
      {
         throw new IndexOutOfRangeException("Index is out of range.");
      }

      T[][] src = data[i];
      for (int j = 0; j < Size1; j++)
      {
         Array.Clear(src[i]);
      }
   }

   /// <summary>
   /// Zeros the 2D area with this index at the second dimension
   /// </summary>
   /// <param name="i"></param>
   /// <exception cref="IndexOutOfRangeException"></exception>
   public void ZeroDim1(int j)
   {
      if (j < 0 || j >= Size1)
      {
         throw new IndexOutOfRangeException("Index is out of range.");
      }

      for (int i = 0; i < data.Length; i++)
      {
         T[] src0 = data[i][j];
         Array.Clear(src0);
      }
   }

   /// <summary>
   /// Zeros the 2D area with this index at the third dimension
   /// </summary>
   /// <param name="i"></param>
   /// <exception cref="IndexOutOfRangeException"></exception>
   public void ZeroDim2(int k)
   {
      if (k < 0 || k >= Size2)
      {
         throw new IndexOutOfRangeException("Index is out of range.");
      }

      for (int i = 0; i < data.Length; i++)
      {
         T[][] src0 = data[i];
         for (int j = 0; j < src0.Length; j++)
         {
            src0[j][k] = default;
         }
      }
   }

   /// <summary>
   /// Applies the provided function to each element, one at a time, and replaces the value stored. 
   /// Value is replaced upon calculation for that element, not after all elements have been calculated
   /// </summary>
   public void ApplyInPlace(Func<T, T> function)
   {
      foreach (T[][] d in data)
      {
         foreach (T[] t in d)
         {
            t.ApplyInPlace(function);
         }
      }
   }

   /// <summary>
   /// Applies the provided function to each element, one at a time, and replaces the value stored. 
   /// Value is replaced upon calculation for that element, not after all elements have been calculated.
   /// Only applied within the mask
   /// </summary>
   public void ApplyInPlace(Func<T, T> function, Array3D<bool> mask)
   {
      ApplyZipFunctionInPlace(mask,(val,mas)=>mas ? function(val) : val);
   }

   public IEnumerable<XYZ<int>> GetAllVoxelIndices()
   {
      for (int i = 0; i < Size0; i++)
      {
         for (int j = 0; j < Size1; j++)
         {
            for (int k = 0; k < Size2; k++)
            {
               yield return new XYZ<int>(i, j, k);
            }
         }
      }
   }

   /// <summary>
   /// Gets voxel indices connected with a full face that are larger in one dimension than this (i.e. voxel.X+1, voxel.Y+1, or voxel.Z+1)
   /// </summary>
   /// <param name="voxel"></param>
   /// <returns></returns>
   public IEnumerable<XYZ<int>> GetConnectedVoxels_3Wise(XYZ<int> voxel)
   {
      if (voxel.X + 1 < Size0)
      {
         yield return voxel with { X = voxel.X + 1 };
      }
      if (voxel.Y + 1 < Size1)
      {
         yield return voxel with { Y = voxel.Y + 1 };
      }
      if (voxel.Z + 1 < Size2)
      {
         yield return voxel with { Z = voxel.Z + 1 };
      }
   }
   /// <summary>
   /// Gets voxel indices connected with a full face (i.e. no diagonals)
   /// </summary>
   /// <param name="voxel"></param>
   /// <returns></returns>
   internal IEnumerable<XYZ<int>> GetConnectedVoxels_6Wise(XYZ<int> voxel)
   {
      if (voxel.X > 0)
      {
         yield return voxel with { X = voxel.X - 1 };
      }
      if (voxel.Y > 0)
      {
         yield return voxel with { Y = voxel.Y - 1 };
      }
      if (voxel.Z > 0)
      {
         yield return voxel with { Z = voxel.Z - 1 };
      }
      if (voxel.X + 1 < Size0)
      {
         yield return voxel with { X = voxel.X + 1 };
      }
      if (voxel.Y + 1 < Size1)
      {
         yield return voxel with { Y = voxel.Y + 1 };
      }
      if (voxel.Z + 1 < Size2)
      {
         yield return voxel with { Z = voxel.Z + 1 };
      }
   }

   public IEnumerable<T> GetVoxelsInMask(Array3D<bool> mask) => ZipValues(mask).Where(a => a.Item2).Select(a => a.Item1);
   public IEnumerable<T> GetVoxelsOutsideMask(Array3D<bool> mask) => ZipValues(mask).Where(a => !a.Item2).Select(a => a.Item1);

   public XYZ<int>[] GetIndicesSortedByValueDescending()
   {
      XYZ<int>[] allCoords = GetIndicesSortedByValueAscending();
      Array.Reverse(allCoords);

      return allCoords;
   }

   public XYZ<int>[] GetIndicesSortedByValueAscending()
   {
      XYZ<int>[] allCoords = new XYZ<int>[Size0 * Size1 * Size2];
      T[] values = new T[Size0 * Size1 * Size2];
      int index = 0;
      for (int i = 0; i < Size0; i++)
      {
         for (int j = 0; j < Size1; j++)
         {
            for (int k = 0; k < Size2; k++)
            {
               allCoords[index] = new XYZ<int> { X = i, Y = j, Z = k };
               values[index++] = this[i, j, k];
            }
         }
      }

      Array.Sort(values, allCoords);
      return allCoords;
   }

   public bool SizesEqual<S>(Array3D<S> other) where S : struct => Size0 == other.Size0 && Size1 == other.Size1 && Size2 == other.Size2;

   public Array3D<T> Clone()
   {
      Array3D<T> clone = new(Size0, Size1, Size2);
      CloneInto(clone);
      return clone;
   }
   public void CloneInto(Array3D<T> array3D)
   {
      ThrowIfSizesNotEqual(array3D);

      for (int i = 0; i < Size0; i++)
      {
         T[][] srcI = data[i];
         T[][] destI = array3D.data[i];

         for (int j = 0; j < srcI.Length; j++)
         {
            T[] srcJ = srcI[j];
            T[] destJ = destI[j];
            Array.Copy(srcJ, destJ, srcJ.Length);
         }
      }
   }

   public static void ThrowIfSizesNotEqual(IReadOnlyList<Array3D<T>> array3D)
   {
      if (array3D.Count < 2)
      {
         return;
      }
      var arr0 = array3D[0];
      for (int i = 1; i < array3D.Count; i++)
      {
         arr0.ThrowIfSizesNotEqual(array3D[i]);
      }
   }
   public void ThrowIfSizesNotEqual<S>(Array3D<S> array3D) where S : struct
   {
      if (!SizesEqual(array3D)) throw new ArgumentException("Array is wrong size");
   }

   public void ApplyFunctionInPlaceByZSlice(Func<T, T> function, int zSlice)
   {
      for (int i = 0; i < Size0; i++)
      {
         var curX = data[i];
         for (int j = 0; j < Size1; j++)
         {
            var curY = curX[j];

            curY[zSlice] = function.Invoke(curY[zSlice]);
         }
      }
   }

   public void ApplyFunctionInPlaceByZSlice(Func<int, T, T> function)
   {
      for (int x = 0; x < Size0; x++)
      {
         var curX = data[x];
         for (int y = 0; y < Size1; y++)
         {
            var curY = curX[y];

            for (int z = 0; z < curY.Length; z++)
            {
               curY[z] = function.Invoke(z, curY[z]);
            }
         }
      }
   }

   public void ApplyFunctionInPlaceByXSlice(Func<int, T, T> function)
   {
      for (int x = 0; x < Size0; x++)
      {
         var curX = data[x];
         for (int y = 0; y < Size1; y++)
         {
            var curY = curX[y];

            for (int z = 0; z < curY.Length; z++)
            {
               curY[z] = function.Invoke(x, curY[z]);
            }
         }
      }
   }

   /// <summary>
   /// Applies a function to values, and writes out to a new array
   /// </summary>
   /// <typeparam name="S"></typeparam>
   /// <param name="function"></param>
   /// <param name="writeTo"></param>
   /// <exception cref="NotImplementedException"></exception>
   public Array3D<S> ApplyFunction<S>(Func<T, S> function) where S : struct
   {
      Array3D<S> array = new Array3D<S>(Size0, Size1, Size2);
      ApplyFunction(function, array);
      return array;
   }
   /// <summary>
   /// Applies a function to values, and writes out to the destination provicded
   /// </summary>
   /// <typeparam name="S"></typeparam>
   /// <param name="function"></param>
   /// <param name="writeTo"></param>
   /// <exception cref="NotImplementedException"></exception>
   public void ApplyFunction<S>(Func<T, S> function, Array3D<S> writeTo) where S : struct
   {
      ThrowIfSizesNotEqual(writeTo);

      for (int i = 0; i < Size0; i++)
      {
         T[][] srcI = data[i];
         S[][] destI = writeTo.data[i];

         for (int j = 0; j < srcI.Length; j++)
         {
            T[] srcJ = srcI[j];
            S[] destJ = destI[j];

            for (int k = 0; k < srcJ.Length; k++)
            {
               destJ[k] = function(srcJ[k]);
            }
         }
      }
   }
   /// <summary>
   /// Zips values from two 3D arrays and plugs them into a function, which saves to a third array
   /// </summary>
   /// <typeparam name="U">Seconday input voxel type</typeparam>
   /// <typeparam name="S">Output voxel type</typeparam>
   /// <param name="other">Uses values from this array, as well as 'this'</param>
   /// <param name="function">Transforms the inputs into a reulst</param>
   /// <exception cref="ArgumentException">Arrays are different sizes</exception>
   public void ApplyZipFunctionInPlace<U>(Array3D<U> other, Func<T, U, T> function)
      where U : struct
   {
      ApplyZipFunction(other, function, this);

   }
   
   /// <summary>
   /// Zips values from two 3D arrays and plugs them into a function, which saves to a third array
   /// </summary>
   /// <typeparam name="U">Seconday input voxel type</typeparam>
   /// <typeparam name="S">Output voxel type</typeparam>
   /// <param name="other">Uses values from this array, as well as 'this'</param>
   /// <param name="function">Transforms the inputs into a reulst</param>
   /// <exception cref="ArgumentException">Arrays are different sizes</exception>
   public Array3D<S> ApplyZipFunction<S, U>(Array3D<U> other, Func<T, U, S> function)
      where U : struct
      where S : struct
   {
      Array3D<S> writeTo = new(Size0, Size1, Size2);

      ApplyZipFunction(other, function, writeTo);

      return writeTo;
   }

   /// <summary>
   /// Zips values from two 3D arrays and plugs them into a function, which saves to a third array
   /// </summary>
   /// <typeparam name="U">Seconday input voxel type</typeparam>
   /// <typeparam name="S">Output voxel type</typeparam>
   /// <param name="other">Uses values from this array, as well as 'this'</param>
   /// <param name="function">Transforms the inputs into a reulst</param>
   /// <param name="writeTo">Save to this array</param>
   /// <exception cref="ArgumentException">Arrays are different sizes</exception>
   public void ApplyZipFunction<S, U>(Array3D<U> other, Func<T, U, S> function, Array3D<S> writeTo)
      where U : struct
      where S : struct
   {
      ThrowIfSizesNotEqual(other);
      ThrowIfSizesNotEqual(writeTo);

      for (int i = 0; i < Size0; i++)
      {
         T[][] src0I = data[i];
         U[][] src1I = other.data[i];
         S[][] destI = writeTo.data[i];

         for (int j = 0; j < src0I.Length; j++)
         {
            T[] src0J = src0I[j];
            U[] src1J = src1I[j];
            S[] destJ = destI[j];

            for (int k = 0; k < src0J.Length; k++)
            {
               destJ[k] = function(src0J[k], src1J[k]);
            }
         }
      }
   }


   public IEnumerable<(T, S)> ZipValues<S>(Array3D<S> other) where S : struct
   {
      ThrowIfSizesNotEqual(other);

      for (int i = 0; i < Size0; i++)
      {
         T[][] thisI = data[i];
         S[][] otherI = other.data[i];

         for (int j = 0; j < thisI.Length; j++)
         {
            T[] thisJ = thisI[j];
            S[] ptherJ = otherI[j];

            for (int k = 0; k < thisJ.Length; k++)
            {
               yield return (thisJ[k], ptherJ[k]);
            }
         }
      }
   }

   /// <summary>
   /// Returns all voxel values
   /// </summary>
   /// <returns></returns>
   public IEnumerable<T> GetValues() => data.SelectMany(a => a).SelectMany(a => a);


   /// <summary>
   /// Returns all voxel values as an array in which z changes fastest, then y, then x
   /// </summary>
   /// <returns></returns>
   public T[] GetAllVoxels_LastDimFastest()
   {
      T[] values = new T[NumberOfVoxels];

      int offset = 0;
      for (int i = 0; i < data.Length; i++)
      {
         T[][] curI = data[i];
         for (int y = 0; y < curI.Length; y++)
         {
            T[] cur = curI[y];

            Array.Copy(cur, 0, values, offset, Size2);
            offset += Size2;
         }
      }

      return values;
   }


   /// <summary>
   /// Sets voxel values from a 3D array from another in which z changes fastest, then y, then x
   /// </summary>
   /// <remarks>This is the appropriate method for ITK data</remarks>
   public void SetAllVoxels_LastDimFastest(ReadOnlySpan<T> arr3D)
   {
      if (arr3D.Length != NumberOfVoxels)
      {
         throw new ArgumentException($"Bad number of voxels: {arr3D.Length}. Expected: {NumberOfVoxels}", nameof(arr3D));
      }
      int offset = 0;
      for (int i = 0; i < Size0; i++)
      {
         T[][] thisI = data[i];

         for (int j = 0; j < thisI.Length; j++)
         {
            T[] thisJ = thisI[j];

            arr3D.Slice(offset, Size2).CopyTo(thisJ);
            offset += Size2;
         }
      }
   }
   /// <summary>
   /// Sets voxel values from a 3D array from another in which z changes fastest, then y, then x
   /// </summary>
   /// <remarks>This is the appropriate method for ITK data</remarks>
   public void SetAllVoxels_ZFastest(ReadOnlySpan<T> arr3D)
   {
      if (arr3D.Length != NumberOfVoxels)
      {
         throw new ArgumentException($"Bad number of voxels: {arr3D.Length}. Expected: {NumberOfVoxels}", nameof(arr3D));
      }
      int offset = 0;
      for (int i = 0; i < Size0; i++)
      {
         T[][] thisI = data[i];

         for (int j = 0; j < thisI.Length; j++)
         {
            T[] thisJ = thisI[j];

            arr3D.Slice(offset, Size2).CopyTo(thisJ);
            offset += Size2;
         }
      }
   }

   /// <summary>
   /// Sets voxel values from a 3D array from another in which x changes fastest, then y, then z
   /// </summary>
   public T[] GetAllVoxels_XFastest()
   {
      T[] arr = new T[NumberOfVoxels];

      int offset = 0;
      for (int l = 0; l < Size2; l++)
      {
         for (int k = 0; k < Size1; k++)
         {
            for (int j = 0; j < data.Length; j++)
            {
               arr[offset] = data[j][k][l];

               offset++;
            }
         }
      }

      return arr;
   }

   public void SetAllVoxels_XFastest(T[] arr3D) => SetAllVoxels_XFastest(arr3D.AsSpan());
   /// <summary>
   /// Sets voxel values from a 3D array from another in which x changes fastest, then y, then z
   /// </summary>
   public void SetAllVoxels_XFastest(ReadOnlySpan<T> arr3D)
   {
      if (arr3D.Length != NumberOfVoxels)
      {
         throw new ArgumentException($"Bad number of voxels: {arr3D.Length}. Expected: {NumberOfVoxels}", nameof(arr3D));
      }

      int offset = 0;
      for (int l = 0; l < Size2; l++)
      {
         for (int k = 0; k < Size1; k++)
         {
            for (int j = 0; j < data.Length; j++)
            {
               data[j][k][l] = arr3D[offset];

               offset++;
            }
         }
      }
   }

   /// <summary>
   /// this[i+1,j,k] becomes this[i,j,k]. this[0,j,k] becomes the default value for <see cref="T"/>
   /// </summary>
   public void ShiftIPlus()
   {
      for (int i = data.Length - 2; i >= 0; i--)
      {
         CopyJagged(data[i], data[i + 1]);
      }

      ZeroJagged(data[0]);
   }

   private static void CopyJagged(T[][] src, T[][] dest)
   {
      for (int j = 0; j < src.Length; j++)
      {
         var curJ = src[j];
         var curJDest = dest[j];

         curJ.CopyTo(curJDest, 0);
      }
   }   private static void CopyJaggedRev(T[][] src, T[][] dest)
   {
        for (int j = src.Length - 1; j >= 0; j--)
        {
         var curJ = src[j];
         var curJDest = dest[j];

         curJ.CopyTo(curJDest, 0);
      }
   }

   /// <summary>
   /// this[i,j,k] becomes this[i+1,j,k]. this[^1,j,k] becomes the default value for <see cref="T"/>
   /// </summary>
   public void ShiftIMinus()
   {
      for (int i = 0; i < data.Length - 1; i++)
      {
         CopyJagged(data[i+1], data[i]);
      }

      ZeroJagged(data[^1]);
   }


   private static void ZeroJagged(T[][] wipe)
   {
      for (int i = 0; i < wipe.Length; i++)
      {
         ZeroArray(wipe[i]);
      }
   }

   private static void ZeroArray(T[] cur)
   {
      for (int k = 0; k < cur.Length; k++)
      {
         cur[k] = default;
      }
   }


   /// <summary>
   /// this[i,j+1,k] becomes this[i,j,k]. this[0,j,k] becomes the default value for <see cref="T"/>
   /// </summary>
   public void ShiftJPlus()
   {
      for (int i = 0; i < data.Length; i++)
      {
         T[][] cur = data[i];

         for (int j = cur.Length - 2; j >= 0; j--)
         {
            cur[j].CopyTo(cur[j+1], 0);
         }
         ZeroArray(cur[0]);
      }
   }

   /// <summary>
   /// this[i,j,k] becomes this[i,j+1,k]. this[^1,j,k] becomes the default value for <see cref="T"/>
   /// </summary>
   public void ShiftJMinus()
   {
      for (int i = 0; i < data.Length; i++)
      {
         T[][] cur = data[i];

         for (int j = 0; j < cur.Length - 1; j++)
         {
            cur[j+1].CopyTo(cur[j], 0);
         }
         ZeroArray(cur[^1]);
      }
   }


   /// <summary>
   /// this[i,j,k+1] becomes this[i,j,k]. this[i,j,0] becomes the default value for <see cref="T"/>
   /// </summary>
   public void ShiftKPlus()
   {
      IterateIJ(Shift);

      static void Shift(T[] curJ)
      {
         for (int k = curJ.Length - 2; k >= 0; k--)
         {
            curJ[k + 1] = curJ[k];
         }
         curJ[0] = default;
      }
   }

   private void IterateIJ(Action<T[]> action)
   {
      for (int i = 0; i < data.Length; i++)
      {
         var curI = data[i];

         for (int j = 0; j < curI.Length; j++)
         {
            var curJ = curI[j];
            action(curJ);
         }
      }
   }

   /// <summary>
   /// this[i,j,k] becomes this[i,j,k+1]. this[i,j,^1] becomes the default value for <see cref="T"/>
   /// </summary>
   public void ShiftKMinus()
   {
      IterateIJ(Shift);

      static void Shift(T[] curJ)
      {
         for (int k = 0; k < curJ.Length - 1; k++)
         {
            curJ[k] = curJ[k + 1];
         }
         curJ[^1] = default;
      }
   }

   public void Dispose()
   {
      if(_isDisposed)
      {
         return;
      }
      _isDisposed = true;

      // dismantle data to potentially encourage GC
      for (int i = 0; i < data.Length; i++)
      {
         var curD = data[i];
         for(int j = 0; j < curD.Length; j++)
         {
            curD[j] = null!;
         }
         data[i] = null!;
      }
   }
}