using FlipProof.Base;
using FlipProof.Image.Matrices;
using System.Collections.Immutable;
using System.Numerics;
using static TorchSharp.torch;

namespace FlipProof.Image;

public record ImageHeader(ImageSize Size,
                          IReadOnlyOrientation Orientation,
                          CoordinateSystem CoordinateSystem,
                          EncodingDirection PhaseEncodingDimension,
                          EncodingDirection FrequencyEncodingDimension,
                          EncodingDirection SliceEncodingDimension) : IImageHeader
{

   /// <summary>
   /// The number of dimensions ths image has (3 or 4)
   /// </summary>
   public int NDims => Size.NDims;
   /// <summary>
   /// Bounds of the image in voxel space
   /// </summary>
   public Box4D<long> VoxelBounds => new(new(), Size);

   //IImageHeader IImageHeader.Create(IImageHeader other) => Create(other);
   public static ImageHeader Create(IImageHeader other)
   {
      return new ImageHeader(other.Size, other.Orientation, other.CoordinateSystem, other.PhaseEncodingDimension, other.FrequencyEncodingDimension, other.SliceEncodingDimension);
   }

   /// <summary>
   /// Returns the header after a theoretical padding with voxels. The physical location of (0,0,0) 
   /// in the original matches the physical location of (<paramref name="x0"/>,<paramref name="y0"/>,<paramref name="z0"/>)
   /// in the padded header
   /// </summary>
   /// <param name="newBounds">In voxel space</param>
   public ImageHeader GetForPaddedImage(Box4D<long> newBounds)
   {
      VoxelBounds.CalcPadding(newBounds, out long xB4, out long xAfter, out long yB4, out long yAfter, out long zB4, out long zAfter, out long volB4, out long volAfter);

      return GetForPaddedImage(xB4, xAfter, yB4, yAfter, zB4,zAfter, volB4, volAfter);
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
      Orientation =  Orientation.GetTranslated(-x0, -y0, -z0),
      Size = new(Size.X + x0 + x1, Size.Y + y0 + y1, Size.Z + z0 + z1, Size.VolumeCount + vols0 + vols1),
   };

   /// <summary>
   /// Returns the voxel size in mm
   /// </summary>
   /// <returns></returns>
   public XYZ<double> GetVoxelSize() => Orientation.VoxelSize;



   public XYZ<double> VoxelToWorldCoordinate(XYZ<float> vox) => VoxelToWorldCoordinate(vox.X, vox.Y, vox.Z);
   
   public XYZ<double> VoxelToWorldCoordinate(float x, float y, float z)
   {
      return Orientation.VoxelToWorldCoordinate(x, y, z);
   }



   /// <summary>
   /// Returns the same header but with the 4th dimension only 1 deep
   /// </summary>
   /// <returns></returns>
   public ImageHeader As3D() => this with { Size = new ImageSize(Size.X, Size.Y, Size.Z, 1) };


   /// <summary>
   /// Compares two headers, tolerantly wrt to orientation
   /// </summary>
   /// <param name="other"></param>
   /// <returns></returns>
   public bool TolerantEquals(IImageHeader other, bool checkEncodingDirections)
   {
      return CoordinateSystem == other.CoordinateSystem &&
         ((!checkEncodingDirections) || PhaseEncodingDimension == other.PhaseEncodingDimension) &&
         ((!checkEncodingDirections) || FrequencyEncodingDimension == other.FrequencyEncodingDimension) &&
         ((!checkEncodingDirections) || SliceEncodingDimension == other.SliceEncodingDimension) &&
         Size.Equals(other.Size) && 
         Orientation.Equals(other.Orientation);


   }
}
