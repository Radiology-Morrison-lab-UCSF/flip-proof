using FlipProof.Base;
using System.Numerics;

namespace FlipProof.Image;


public interface IReadOnlyOrientation 
{
   XYZ<double> VoxelToWorldCoordinate(XYZ<double> xyz) => VoxelToWorldCoordinate(xyz.X, xyz.Y, xyz.Z);
   XYZ<double> VoxelToWorldCoordinate(double x, double y, double z);

   XYZ<double> VoxelSize { get; }

   /// <summary>
   /// Returns a copy translated by the given offsets
   /// </summary>
   /// <param name="offsetX"></param>
   /// <param name="offsetY"></param>
   /// <param name="offsetZ"></param>
   /// <returns></returns>
   IReadOnlyOrientation GetTranslated(double offsetX, double offsetY, double offsetZ);

   bool TryGetNiftiQuaternions(out double quartern_b, out double quartern_c, out double quartern_d, out double[] pixDims, out double[] translations, out double qFace);

   /// <summary>
   /// Returns a copy of the internal matrix used for orientation, if one exists
   /// </summary>
   /// <exception cref="NotSupportedException">Operation not supported</exception>
   Matrix4x4 GetMatrix();

   /// <summary>
   /// Checks that two orientations return similar world coordinates for the same voxel
   /// </summary>
   /// <param name="imageSize">Size of the image this will be used with, in voxels. Furthest bounds of this are checked</param>
   /// <param name="tolerance">Defaults to 1/1000th of the smallest voxel size</param>
   /// <returns></returns>
   public bool Equals(IReadOnlyOrientation other, ImageSize imageSize, double? toleranceOverride = null)
   {
      double tolerance = toleranceOverride ??  VoxelSize.Min()! * 0.001; //1000th of the smallest dim in voxel size 

      var this000 = this.VoxelToWorldCoordinate(0,0,0);
      var other000 = other.VoxelToWorldCoordinate(0,0,0);
      if (this000.DistanceTo(other000) > tolerance)
      {
         return false;
      }


      // Check end of image bounds
      var thisEdge = this.VoxelToWorldCoordinate(imageSize.X, imageSize.Y, imageSize.Z);
      var otherEdge = other.VoxelToWorldCoordinate(imageSize.X, imageSize.Y, imageSize.Z);
      if (thisEdge.DistanceTo(otherEdge) > tolerance)
      {
         return false;
      }

      if (imageSize.X == imageSize.Y || imageSize.Y == imageSize.Z || imageSize.Z == imageSize.X)
      {
         // Do 1,3,5 in case of rotation
         var this135 = this.VoxelToWorldCoordinate(1, 3, 5);
         var other135 = other.VoxelToWorldCoordinate(1, 3, 5);
         if (this135.DistanceTo(other135) > tolerance)
         {
            return false;
         }
      }

      return true;

   }

}
