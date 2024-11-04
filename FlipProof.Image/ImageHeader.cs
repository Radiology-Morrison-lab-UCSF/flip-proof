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
}
