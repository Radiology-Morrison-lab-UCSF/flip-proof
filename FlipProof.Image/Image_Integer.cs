using FlipProof.Torch;
using System.Numerics;
using static TorchSharp.torch;

namespace FlipProof.Image;

/// <summary>
/// Holds integer type voxels
/// </summary>
/// <typeparam name="TVoxel"></typeparam>
/// <typeparam name="TSpace"></typeparam>
public abstract class Image_Integer<TVoxel, TSpace, TSelf, TTensor> : Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor>
   where TVoxel : struct, IBinaryInteger<TVoxel>
   where TSpace : ISpace
   where TTensor: IntegerTensor<TVoxel, TTensor>
   where TSelf : Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor>
{


   /// <summary>
   /// Creates a new image, explicitly stating the orientation. Appropriate 
   /// for use in I/O operations or when first stating the orientation of 
   /// this type. Provided voxels are cloned, not used directly.
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Voxel data - a copy of the provided object is made</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal Image_Integer(ImageHeader header, Tensor voxels) : base(header, voxels)
   {
   }

   /// <summary>
   /// Creates a new image, explicitly stating the orientation. Appropriate 
   /// for use in I/O operations or when first stating the orientation of 
   /// this type. Provided voxels are cloned, not used directly.
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Voxel data - a copy of the provided object is made</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal Image_Integer(ImageHeader header, TTensor voxels) : base(header, voxels)
   {
   }


   /// <summary>
   /// Creates a new Image from an operation known to not affect the shape or data-order of the tensor
   /// </summary>
   /// <param name="voxels">Voxel data. Data are used directly. Do not feed in a tensor accessible outside this object</param>
   /// <param name="verifyShape">Verify that the voxel array shape should be verified against the header. True if the operation is not trusted</param>
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal Image_Integer(TTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }


   public static ImageFloat<TSpace> operator +(Image_Integer<TVoxel, TSpace, TSelf, TTensor> left, ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data + right.Data);
   public static ImageFloat<TSpace> operator +(ImageFloat<TSpace> left, Image_Integer<TVoxel, TSpace, TSelf, TTensor> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data + right.Data);
   public static ImageFloat<TSpace> operator -(Image_Integer<TVoxel, TSpace, TSelf, TTensor> left, ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data - right.Data);
   public static ImageFloat<TSpace> operator -(ImageFloat<TSpace> left, Image_Integer<TVoxel, TSpace, TSelf, TTensor> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data - right.Data);
   public static ImageFloat<TSpace> operator *(Image_Integer<TVoxel, TSpace, TSelf, TTensor> left, ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data * right.Data);
   public static ImageFloat<TSpace> operator *(ImageFloat<TSpace> left, Image_Integer<TVoxel, TSpace, TSelf, TTensor> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data * right.Data);
   public static ImageFloat<TSpace> operator /(Image_Integer<TVoxel, TSpace, TSelf, TTensor> left, ImageFloat<TSpace> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);
   public static ImageFloat<TSpace> operator /(ImageFloat<TSpace> left, Image_Integer<TVoxel, TSpace, TSelf, TTensor> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);

   // Int / Int in torch returns float
   public static ImageFloat<TSpace> operator /(Image_Integer<TVoxel, TSpace, TSelf, TTensor> left, Image_Integer<TVoxel, TSpace, TSelf, TTensor> right) => ImageFloat<TSpace>.UnsafeCreateStatic(left.Data / right.Data);

}
