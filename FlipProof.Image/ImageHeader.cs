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



   public XYZf VoxelToWorldCoordinate(XYZf vox) => VoxelToWorldCoordinate(vox.X, vox.Y, vox.Z);
   
   public XYZf VoxelToWorldCoordinate(float x, float y, float z)
   {
      return new XYZf(
             Orientation.M11 * x + Orientation.M12 * y + Orientation.M13 * z + Orientation.M14,
             Orientation.M21 * x + Orientation.M22 * y + Orientation.M23 * z + Orientation.M24,
             Orientation.M31 * x + Orientation.M32 * y + Orientation.M33 * z + Orientation.M34
         );
   }
}
