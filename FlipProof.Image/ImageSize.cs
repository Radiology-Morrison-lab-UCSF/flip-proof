using System.Collections;

namespace FlipProof.Image;

public readonly record struct ImageSize : IEnumerable<long>
{
   public ImageSize(long volumeCount, long x, long y, long z)
   {
      X = x > 0 ? x : throw new ArgumentException("Dimensions must be 1 or greater", nameof(x));
      Y = y > 0 ? y : throw new ArgumentException("Dimensions must be 1 or greater", nameof(y));
      Z = z > 0 ? z : throw new ArgumentException("Dimensions must be 1 or greater", nameof(z));
      Volumes = volumeCount > 0 ? volumeCount : throw new ArgumentException("Dimensions must be 1 or greater", nameof(volumeCount));
   }
   public long X { get; }
   public long Y { get; }
   public long Z { get; }
   public long Volumes { get; }

   /// <summary>
   /// 4 if volumes is > 1, else 3
   /// </summary>
   public int NDims => Volumes == 1 ? 3 : 4;

   public long VoxelCount => X * Y * Z * Volumes;

   /// <summary>
   /// Yields X, Y, Z, Volumes
   /// </summary>
   /// <returns></returns>
   IEnumerator<long> IEnumerable<long>.GetEnumerator() => GetEnumerator();

   /// <summary>
   /// Yields X, Y, Z, Volumes
   /// </summary>
   /// <returns></returns>

   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

   private IEnumerator<long> GetEnumerator()
   {
      yield return X;
      yield return Y;
      yield return Z;
      yield return Volumes;
   }


}
