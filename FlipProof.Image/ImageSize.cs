using System.Collections;

namespace FlipProof.Image;

public readonly record struct ImageSize : IEnumerable<uint>
{
   public ImageSize(uint volumeCount, uint x, uint y, uint z)
   {
      X = x > 0 ? x : throw new ArgumentException("Dimensions must be 1 or greater", nameof(x));
      Y = y > 0 ? y : throw new ArgumentException("Dimensions must be 1 or greater", nameof(y));
      Z = z > 0 ? z : throw new ArgumentException("Dimensions must be 1 or greater", nameof(z));
      Volumes = volumeCount > 0 ? volumeCount : throw new ArgumentException("Dimensions must be 1 or greater", nameof(volumeCount));
   }
   public uint X { get; }
   public uint Y { get; }
   public uint Z { get; }
   public uint Volumes { get; }

   /// <summary>
   /// 4 if volumes is > 1, else 3
   /// </summary>
   public int NDims => Volumes == 1 ? 3 : 4;

   public uint VoxelCount => X * Y * Z * Volumes;

   /// <summary>
   /// Yields X, Y, Z, Volumes
   /// </summary>
   /// <returns></returns>
   IEnumerator<uint> IEnumerable<uint>.GetEnumerator() => GetEnumerator();

   /// <summary>
   /// Yields X, Y, Z, Volumes
   /// </summary>
   /// <returns></returns>

   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

   private IEnumerator<uint> GetEnumerator()
   {
      yield return X;
      yield return Y;
      yield return Z;
      yield return Volumes;
   }


}
