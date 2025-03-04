using FlipProof.Base;
using FlipProof.Image.Matrices;
using System.Collections.Immutable;
using System.Numerics;
using static TorchSharp.torch;

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
      Orientation =  Orientation * new Matrix4x4(1, 0, 0, -x0, 0, 1, 0, -y0, 0, 0, 1, -z0, 0, 0, 0, 1),
      Size = new(Size.X + x0 + x1, Size.Y + y0 + y1, Size.Z + z0 + z1, Size.VolumeCount + vols0 + vols1),
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
      return VoxelToWorldCoordinate(x, y, z, Orientation);
   }

   private static XYZ<float> VoxelToWorldCoordinate(float x, float y, float z, Matrix4x4 orientation)
   {
      return new XYZ<float>(
             orientation.M11 * x + orientation.M12 * y + orientation.M13 * z + orientation.M14,
             orientation.M21 * x + orientation.M22 * y + orientation.M23 * z + orientation.M24,
             orientation.M31 * x + orientation.M32 * y + orientation.M33 * z + orientation.M34
         );
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
         DistWithinTolerance();

      bool DistWithinTolerance()
      {
         var voxelSize = this.GetVoxelSize();
         double tolerance = 0.001 * voxelSize.Min(); //1000th of the smallest dim in voxel size 

         var this000 = this.VoxelToWorldCoordinate(0, 0, 0);
         var other000 = VoxelToWorldCoordinate(0, 0, 0, other.Orientation);
         if (this000.DistanceTo(other000) > tolerance)
         {
            return false;
         }


         // Check end of image bounds
         var thisEdge = this.VoxelToWorldCoordinate(Size.X, Size.Y, Size.Z);
         var otherEdge = VoxelToWorldCoordinate(Size.X, Size.Y, Size.Z, other.Orientation);
         if (thisEdge.DistanceTo(otherEdge) > tolerance)
         {
            return false;
         }

         if(Size.X == Size.Y || Size.Y == Size.Z || Size.Z == Size.X)
         {
            // Do 1,3,5 in case of rotation
            var this135 = this.VoxelToWorldCoordinate(1, 3, 5);
            var other135 = VoxelToWorldCoordinate(1, 3, 5, other.Orientation);
            if (this135.DistanceTo(other135) > tolerance)
            {
               return false;
            }
         }

         return true;
      }
   }
}
