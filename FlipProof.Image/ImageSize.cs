using FlipProof.Base;
using System.Collections;

namespace FlipProof.Image;

public readonly record struct ImageSize : IEnumerable<long>
{
   public ImageSize(long x, long y, long z, long volumeCount)
   {
      X = x > 0 ? x : throw new ArgumentException("Dimensions must be 1 or greater", nameof(x));
      Y = y > 0 ? y : throw new ArgumentException("Dimensions must be 1 or greater", nameof(y));
      Z = z > 0 ? z : throw new ArgumentException("Dimensions must be 1 or greater", nameof(z));
      VolumeCount = volumeCount > 0 ? volumeCount : throw new ArgumentException("Dimensions must be 1 or greater", nameof(volumeCount));
   }
   public long X { get; }
   public long Y { get; }
   public long Z { get; }
   public long VolumeCount { get; }

   public XYZ<long> EachVolumeSize => new(X, Y, Z);

   /// <summary>
   /// 4 if volumes is > 1, else 3
   /// </summary>
   public int NDims => VolumeCount == 1 ? 3 : 4;

   public long VoxelCount => X * Y * Z * VolumeCount;
   public long VoxelCountPerVolume => X * Y * Z;

   /// <summary>
   /// Yields all voxel indices
   /// </summary>
   /// <returns></returns>
   public IEnumerable<XYZA<long>> GetAllVoxelIndices()
   {
      for (long a = 0; a < VolumeCount; a++)
      {
         for (long z = 0; z < Z; z++)
         {
            for (long y = 0; y < Y; y++)
            {
               for (long x = 0; x < X; x++)
               {
                  yield return new(x, y, z, a);
               }
            }
         }
      }
   }

   public bool InBounds(XYZ<int> k)
   {
      return k.X >= 0 && k.X < X &&
         k.Y >= 0 && k.Y < Y &&
         k.Z >= 0 && k.Z < Z;
   }

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
      yield return VolumeCount;
   }


   public static implicit operator XYZA<long>(ImageSize sz) => new(sz.X, sz.Y, sz.Z, sz.VolumeCount);
   public static implicit operator XYZ<long>(ImageSize sz) => new(sz.X, sz.Y, sz.Z);

}
