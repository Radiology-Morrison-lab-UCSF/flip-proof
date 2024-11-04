using System.Collections;

namespace FlipProof.Base;

/// <summary>
/// Generally avoid using. Allows interface to be used with 1D arrays, useful for torch, for example
/// </summary>
/// <typeparam name="T"></typeparam>
public class Array1D<T> : IVoxelArray<T>, IReadOnlyList<T>, ICollection<T>, ICollection where T : struct
{
   readonly T[] _data;
   public T[] Data => _data;

   public Array1D(T[] data)
   {
      _data = data;
   }

   public IReadOnlyList<int> Shape => new Singleton<int>(_data.Length);

   public int Count => _data.Length;


   public T this[int index] => _data[index];

   public T[] GetAllVoxels_LastDimFastest() =>(T[])_data.Clone();

   public void SetAllVoxels_LastDimFastest(ReadOnlySpan<T> voxels) => voxels.CopyTo(_data);

   public IEnumerator<T> GetEnumerator()
   {
      return ((IEnumerable<T>)_data).GetEnumerator();
   }

   IEnumerator IEnumerable.GetEnumerator()
   {
      return _data.GetEnumerator();
   }

   #region ICollection
   void ICollection<T>.Add(T item) => throw new NotSupportedException();

   void ICollection<T>.Clear() => throw new NotSupportedException();

   bool ICollection<T>.Contains(T item) => _data.Contains(item);

   void ICollection<T>.CopyTo(T[] array, int arrayIndex) => _data.CopyTo(array, arrayIndex);
   void ICollection.CopyTo(Array array, int arrayIndex) => _data.CopyTo(array, arrayIndex);

   bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

   bool ICollection<T>.IsReadOnly => false;

   bool ICollection.IsSynchronized => false;

   object ICollection.SyncRoot => _data;

   #endregion

}
