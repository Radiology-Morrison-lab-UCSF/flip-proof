using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

public abstract class ImageComplexType<TVoxel,TSpace, TSelf, TTensor> : Image<TVoxel, TSpace, TSelf, TTensor>
   where TSpace : struct, ISpace
   where TVoxel : struct
   where TTensor : ComplexNumericTensor<TVoxel, TTensor>
   where TSelf : ImageComplexType<TVoxel, TSpace, TSelf, TTensor>
{
   #region Constructors

   /// <summary>
   /// Creates a new image, explicitly stating the orientation. Appropriate 
   /// for use in I/O operations or when first stating the orientation of 
   /// this type. Provided voxels are cloned, not used directly.
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Voxel data - a copy of the provided object is made</param>

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   internal ImageComplexType(ImageHeader header, torch.Tensor voxels) : base(header, voxels)
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
   internal ImageComplexType(ImageHeader header, TTensor voxels) : base(header, voxels)
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageComplexType(TTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }

   #endregion



   #region Operations / Conversions

   public TSelf ToConjugate() => TrustedOperatorToNew(a => a.ToConjugate());

   /// <summary>
   /// Applies a tensor operator to create a new object from the resulting Tensor
   /// </summary>
   /// <remarks>Operators are only applied to voxels. As the header will remain unchanged - do not use operations such as rotate or the header will be incorrect</remarks>
   /// <param name="operation">The operation to apply to the image data</param>
   /// <returns></returns>
   internal TSelf TrustedOperatorToNew(Func<TTensor, TTensor> operation) => UnsafeCreate(operation(Data));


   #endregion
}
