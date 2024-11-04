namespace FlipProof.Base;

public interface IVoxelArray<T>
{
   /// <summary>
   /// The size of each dimension
   /// </summary>
   IReadOnlyList<int> Shape { get; }
   /// <summary>
   /// Number of dimensions
   /// </summary>
   int NoDims => Shape.Count;
   /// <summary>
   /// Returns all voxels, ordered by dim N-1, N-2, N-3...0
   /// </summary>
   /// <returns></returns>
   T[] GetAllVoxels_LastDimFastest();
   /// <summary>
   /// Sets all voxels from an array ordered by dim N-1, N-2, N-3...0
   /// </summary>
   void SetAllVoxels_LastDimFastest(ReadOnlySpan<T> voxels);
}
