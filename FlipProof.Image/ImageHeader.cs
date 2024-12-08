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
