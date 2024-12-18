using FlipProof.Base;
using System.Collections.Immutable;
using System.Numerics;

namespace FlipProof.Image;

public record ImageHeader(ImageSize Size,
                          Matrix4x4 Orientation,
                          CoordinateSystem CoordinateSystem,
                          EncodingDirection PhaseEncodingDimension,
                          EncodingDirection FrequencyEncodingDimension,
                          EncodingDirection SliceEncodingDimension) : IImageHeader
{

   /// <summary>
   /// The number of dimensions ths image has (3 or 4)
   /// </summary>
   public int NDims => Size.NDims;

   IImageHeader IImageHeader.Create(IImageHeader other) => Create(other);
   public static ImageHeader Create(IImageHeader other)
   {
      return new ImageHeader(other.Size, other.Orientation, other.CoordinateSystem, other.PhaseEncodingDimension, other.FrequencyEncodingDimension, other.SliceEncodingDimension);
   }

   /// <summary>
   /// Returns the header after a theoretical padding with voxels. The physical location of (0,0,0) 
   /// in the original matches the physical location of (<paramref name="x0"/>,<paramref name="y0"/>,<paramref name="z0"/>)
   /// in the padded header
   /// </summary>
   /// <param name="x0">Pad before image dim 0</param>
   /// <param name="x1">Pad after image dim 0</param>
   /// <param name="y0">Pad before image dim 1</param>
   /// <param name="y1">Pad after image dim 1</param>
   /// <param name="z0">Pad before image dim 2</param>
   /// <param name="z1">Pad after image dim 2</param>
   /// <param name="vols0">Volumes inserted at position 0</param>
   /// <param name="vols0">Volumes inserted at final positions</param>
   /// <returns>A new header representing a theoretically padded image aligned to the origina unpadded image</returns>
   public ImageHeader GetForPaddedImage(long x0, long x1, long y0, long y1, long z0, long z1, long vols0, long vols1) => this with
   {
      Orientation =  Orientation * new Matrix4x4(1, 0, 0, -x0, 0, 1, 0, -y0, 0, 0, 1, -z0, 0, 0, 0, 1),
      Size = new(Size.VolumeCount + vols0 + vols1, Size.X + x0 + x1, Size.Y + y0 + y1, Size.Z + z0 + z1),
   };

   /// <summary>
   /// Returns the voxel size in mm
   /// </summary>
   /// <returns></returns>
   public XYZ<float> GetVoxelSize()=> GetVoxelSizeFromMatrix(Orientation);

   internal static XYZ<float> GetVoxelSizeFromMatrix(Matrix4x4 orientation)
   {
      return new(Norm(orientation.M11, orientation.M21, orientation.M31),
                 Norm(orientation.M12, orientation.M22, orientation.M32),
                 Norm(orientation.M13, orientation.M23, orientation.M33));

      static float Norm(float x, float y, float z) => MathF.Sqrt(x * x + y * y + z * z);

   }

   public XYZ<float> VoxelToWorldCoordinate(XYZ<float> vox) => VoxelToWorldCoordinate(vox.X, vox.Y, vox.Z);
   
   public XYZ<float> VoxelToWorldCoordinate(float x, float y, float z)
   {
      return new XYZ<float>(
             Orientation.M11 * x + Orientation.M12 * y + Orientation.M13 * z + Orientation.M14,
             Orientation.M21 * x + Orientation.M22 * y + Orientation.M23 * z + Orientation.M24,
             Orientation.M31 * x + Orientation.M32 * y + Orientation.M33 * z + Orientation.M34
         );
   }

   /// <summary>
   /// Returns the same header but with the 4th dimension only 1 deep
   /// </summary>
   /// <returns></returns>
   public ImageHeader As3D() => this with { Size = new ImageSize(1, Size.X, Size.Y, Size.Z) };
}
