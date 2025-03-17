using FlipProof.Base;
using FlipProof.Image.Matrices;
using System.Numerics;

namespace FlipProof.Image;


public interface IReadOnlyOrientation 
{
   XYZ<double> VoxelToWorldCoordinate(XYZ<double> xyz) => VoxelToWorldCoordinate(xyz.X, xyz.Y, xyz.Z);
   XYZ<double> VoxelToWorldCoordinate(double x, double y, double z);

   /// <summary>
   /// Voxel size (in mm by default)
   /// </summary>
   XYZ<double> VoxelSize { get; }
   /// <summary>
   /// Translation component of the orientation
   /// </summary>
   XYZ<double> Translation { get; }

   /// <summary>
   /// Returns a copy translated by the given offsets
   /// </summary>
   /// <param name="offset">In mm, not in voxels</param>
   /// <returns></returns>
   internal IReadOnlyOrientation GetTranslated(XYZ<double> offset) => GetTranslated(offset.X, offset.Y, offset.Z);
   /// <summary>
   /// Returns a copy translated by the given offsets
   /// </summary>
   /// <param name="offsetX">In mm, not in voxels</param>
   /// <param name="offsetY">In mm, not in voxels</param>
   /// <param name="offsetZ">In mm, not in voxels</param>
   /// <returns></returns>
   internal IReadOnlyOrientation GetTranslated(double offsetX, double offsetY, double offsetZ);

   /// <summary>
   /// Returns a copy translated by the given voxel amounts before the rotation, scaling, 
   /// and existing translations are applied.
   /// </summary>
   /// <param name="padBeforeX0">In voxels</param>
   /// <param name="padBeforeY0">In voxels</param>
   /// <param name="padBeforeZ0">In voxels</param>
   /// <returns></returns>
   internal IReadOnlyOrientation GetForPaddedImage(long padBeforeX0, long padBeforeY0, long padBeforeZ0)
   {
      return new OrientationMatrix(GetMatrix() * new Matrix4x4_Optimised<double>()
      {
         M11 = 1,
         M22 = 1,
         M33 = 1,
         M44 = 1,
         M14 = -padBeforeX0,
         M24 = -padBeforeY0,
         M34 = -padBeforeZ0,
      });
   }



   internal bool TryGetNiftiQuaternions(out double quartern_b, out double quartern_c, out double quartern_d, out double[] pixDims, out double[] translations, out double qFace);

   /// <summary>
   /// Returns a copy of the internal matrix used for orientation, if one exists
   /// </summary>
   /// <exception cref="NotSupportedException">Operation not supported</exception>
   internal Matrix4x4_Optimised<double> GetMatrix();

   /// <summary>
   /// Checks that two orientations return similar world coordinates for the same voxel
   /// </summary>
   /// <param name="imageSize">Size of the image this will be used with, in voxels. Furthest bounds of this are checked</param>
   /// <param name="tolerance">Defaults to 1/1000th of the smallest voxel size</param>
   /// <returns></returns>
   internal bool TolerantEquals(IReadOnlyOrientation other, ImageSize imageSize, double? toleranceOverride = null)
   {
      double tolerance = toleranceOverride ?? VoxelSize.Min()! * 0.001; //1000th of the smallest dim in voxel size 

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
