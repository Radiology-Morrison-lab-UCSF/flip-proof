#pragma expandtemplate typeToReplace=ImageDouble
#pragma expandtemplate ImageInt8 ImageUInt8 ImageInt16 ImageInt32 ImageInt64 ImageFloat ImageBool ImageComplex ImageComplex32

using FlipProof.Torch;
using static TorchSharp.torch;

namespace FlipProof.Image;

public static partial class ImageExtensionMethods
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageDouble<TSpace> Blank<TSpace>(this ImageDouble<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public static void CopyInto<TSpace>(this ImageDouble<TSpace> src, ImageDouble<TSpace> destination)
      where TSpace : struct, ISpace
   {
      src.Data.CopyInto(destination.Data, false);
   }

   public static ImageDouble<TSpace> DeepClone<TSpace>(this ImageDouble<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.DeepClone());
   }

   /// <summary>
   /// Concatenates images in the volumes dimension
   /// </summary>
   /// <typeparam name="TVoxel">Voxel data type</typeparam>
   /// <typeparam name="TSpaceIn">The Space being concatenated, which has fewer volumes than <typeparamref name="TSpaceResult"/>. It does not necessarily need to be 3D</typeparam>
   /// <typeparam name="TSelf">The image type</typeparam>
   /// <typeparam name="TTensor">The tensor type held in the image</typeparam>
   /// <typeparam name="TSpaceResult">The Space that results, which has more volumes</typeparam>
   /// <typeparam name="TReturnType">The resulting image type</typeparam>
   /// <param name="items"></param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public static ImageDouble<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageDouble<TSpaceIn>> items)
   where TSpaceIn : struct, ISpace
   where TSpaceResult : struct, ISpace<TSpaceIn>
   {
      if(items.Count == 0 )
      {
         throw new ArgumentException("No images provided");
      }
      var tensor = items.Count == 1 ? items[0].Data.DeepClone() : items[0].Data.Concat(3, items.Select(a => a.Data).Skip(1).ToArray());

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageDouble<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   [OrientationCheckedAtRuntime]
   public static ImageDouble<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageDouble<TSpaceIn> me, int index)
   where TSpaceIn : struct, ISpace<TSpaceResult>
   where TSpaceResult : struct, ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageDouble<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   [OrientationCheckedAtRuntime]
   public static ImageDouble<TSpace3d> SetVolume<TSpace4d, TSpace3d>(this ImageDouble<TSpace4d> me, int index, ImageDouble<TSpace3d> newData)
   where TSpace4d : struct, ISpace<TSpace3d>
   where TSpace3d : struct, ISpace
   {

      me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index] = newData.Data.Storage;
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageDouble<TSpace3d>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal static ImageDouble<TSpace> TrustedOperatorToNew<TSpace>(this ImageDouble<TSpace> im, Func<Tensor,Tensor> torchOperation)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }

   /// <summary>
   /// Applies an operation to each volume within this image, creating a new image in the process
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns>A new image the same shape and type as <paramref name="im"/></returns>
   public static ImageDouble<TSpace4D> VolumewiseOperation<TSpace4D,TSpace3D, TOtherImage>(this ImageDouble<TSpace4D> im, TOtherImage image, Func<ImageDouble<TSpace3D>, TOtherImage, ImageDouble<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      return im.DeepClone().VolumewiseOperationInPlace(image, function);
   }   
      
      
      /// <summary>
   /// Applies an operation to each volume within this image in place
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns><paramref name="im"/></returns>
   public static ImageDouble<TSpace4D> VolumewiseOperationInPlace<TSpace4D,TSpace3D, TOtherImage>(this ImageDouble<TSpace4D> im, TOtherImage image, Func<ImageDouble<TSpace3D>, TOtherImage, ImageDouble<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      for (int iVol = 0; iVol < im.Header.Size.VolumeCount; iVol++)
      {
         im.SetVolume(iVol,function(im.ExtractVolume<TSpace4D, TSpace3D>(iVol), image));
      }

      return im;
   }
}

#region TEMPLATE EXPANSION
public static partial class ImageExtensionMethods_ImageInt8
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageInt8<TSpace> Blank<TSpace>(this ImageInt8<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public static void CopyInto<TSpace>(this ImageInt8<TSpace> src, ImageInt8<TSpace> destination)
      where TSpace : struct, ISpace
   {
      src.Data.CopyInto(destination.Data, false);
   }

   public static ImageInt8<TSpace> DeepClone<TSpace>(this ImageInt8<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.DeepClone());
   }

   /// <summary>
   /// Concatenates images in the volumes dimension
   /// </summary>
   /// <typeparam name="TVoxel">Voxel data type</typeparam>
   /// <typeparam name="TSpaceIn">The Space being concatenated, which has fewer volumes than <typeparamref name="TSpaceResult"/>. It does not necessarily need to be 3D</typeparam>
   /// <typeparam name="TSelf">The image type</typeparam>
   /// <typeparam name="TTensor">The tensor type held in the image</typeparam>
   /// <typeparam name="TSpaceResult">The Space that results, which has more volumes</typeparam>
   /// <typeparam name="TReturnType">The resulting image type</typeparam>
   /// <param name="items"></param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public static ImageInt8<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageInt8<TSpaceIn>> items)
   where TSpaceIn : struct, ISpace
   where TSpaceResult : struct, ISpace<TSpaceIn>
   {
      if(items.Count == 0 )
      {
         throw new ArgumentException("No images provided");
      }
      var tensor = items.Count == 1 ? items[0].Data.DeepClone() : items[0].Data.Concat(3, items.Select(a => a.Data).Skip(1).ToArray());

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt8<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   [OrientationCheckedAtRuntime]
   public static ImageInt8<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageInt8<TSpaceIn> me, int index)
   where TSpaceIn : struct, ISpace<TSpaceResult>
   where TSpaceResult : struct, ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt8<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   [OrientationCheckedAtRuntime]
   public static ImageInt8<TSpace3d> SetVolume<TSpace4d, TSpace3d>(this ImageInt8<TSpace4d> me, int index, ImageInt8<TSpace3d> newData)
   where TSpace4d : struct, ISpace<TSpace3d>
   where TSpace3d : struct, ISpace
   {

      me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index] = newData.Data.Storage;
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt8<TSpace3d>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal static ImageInt8<TSpace> TrustedOperatorToNew<TSpace>(this ImageInt8<TSpace> im, Func<Tensor,Tensor> torchOperation)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }

   /// <summary>
   /// Applies an operation to each volume within this image, creating a new image in the process
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns>A new image the same shape and type as <paramref name="im"/></returns>
   public static ImageInt8<TSpace4D> VolumewiseOperation<TSpace4D,TSpace3D, TOtherImage>(this ImageInt8<TSpace4D> im, TOtherImage image, Func<ImageInt8<TSpace3D>, TOtherImage, ImageInt8<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      return im.DeepClone().VolumewiseOperationInPlace(image, function);
   }   
      
      
      /// <summary>
   /// Applies an operation to each volume within this image in place
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns><paramref name="im"/></returns>
   public static ImageInt8<TSpace4D> VolumewiseOperationInPlace<TSpace4D,TSpace3D, TOtherImage>(this ImageInt8<TSpace4D> im, TOtherImage image, Func<ImageInt8<TSpace3D>, TOtherImage, ImageInt8<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      for (int iVol = 0; iVol < im.Header.Size.VolumeCount; iVol++)
      {
         im.SetVolume(iVol,function(im.ExtractVolume<TSpace4D, TSpace3D>(iVol), image));
      }

      return im;
   }
}

public static partial class ImageExtensionMethods_ImageUInt8
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageUInt8<TSpace> Blank<TSpace>(this ImageUInt8<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public static void CopyInto<TSpace>(this ImageUInt8<TSpace> src, ImageUInt8<TSpace> destination)
      where TSpace : struct, ISpace
   {
      src.Data.CopyInto(destination.Data, false);
   }

   public static ImageUInt8<TSpace> DeepClone<TSpace>(this ImageUInt8<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.DeepClone());
   }

   /// <summary>
   /// Concatenates images in the volumes dimension
   /// </summary>
   /// <typeparam name="TVoxel">Voxel data type</typeparam>
   /// <typeparam name="TSpaceIn">The Space being concatenated, which has fewer volumes than <typeparamref name="TSpaceResult"/>. It does not necessarily need to be 3D</typeparam>
   /// <typeparam name="TSelf">The image type</typeparam>
   /// <typeparam name="TTensor">The tensor type held in the image</typeparam>
   /// <typeparam name="TSpaceResult">The Space that results, which has more volumes</typeparam>
   /// <typeparam name="TReturnType">The resulting image type</typeparam>
   /// <param name="items"></param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public static ImageUInt8<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageUInt8<TSpaceIn>> items)
   where TSpaceIn : struct, ISpace
   where TSpaceResult : struct, ISpace<TSpaceIn>
   {
      if(items.Count == 0 )
      {
         throw new ArgumentException("No images provided");
      }
      var tensor = items.Count == 1 ? items[0].Data.DeepClone() : items[0].Data.Concat(3, items.Select(a => a.Data).Skip(1).ToArray());

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageUInt8<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   [OrientationCheckedAtRuntime]
   public static ImageUInt8<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageUInt8<TSpaceIn> me, int index)
   where TSpaceIn : struct, ISpace<TSpaceResult>
   where TSpaceResult : struct, ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageUInt8<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   [OrientationCheckedAtRuntime]
   public static ImageUInt8<TSpace3d> SetVolume<TSpace4d, TSpace3d>(this ImageUInt8<TSpace4d> me, int index, ImageUInt8<TSpace3d> newData)
   where TSpace4d : struct, ISpace<TSpace3d>
   where TSpace3d : struct, ISpace
   {

      me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index] = newData.Data.Storage;
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageUInt8<TSpace3d>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal static ImageUInt8<TSpace> TrustedOperatorToNew<TSpace>(this ImageUInt8<TSpace> im, Func<Tensor,Tensor> torchOperation)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }

   /// <summary>
   /// Applies an operation to each volume within this image, creating a new image in the process
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns>A new image the same shape and type as <paramref name="im"/></returns>
   public static ImageUInt8<TSpace4D> VolumewiseOperation<TSpace4D,TSpace3D, TOtherImage>(this ImageUInt8<TSpace4D> im, TOtherImage image, Func<ImageUInt8<TSpace3D>, TOtherImage, ImageUInt8<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      return im.DeepClone().VolumewiseOperationInPlace(image, function);
   }   
      
      
      /// <summary>
   /// Applies an operation to each volume within this image in place
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns><paramref name="im"/></returns>
   public static ImageUInt8<TSpace4D> VolumewiseOperationInPlace<TSpace4D,TSpace3D, TOtherImage>(this ImageUInt8<TSpace4D> im, TOtherImage image, Func<ImageUInt8<TSpace3D>, TOtherImage, ImageUInt8<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      for (int iVol = 0; iVol < im.Header.Size.VolumeCount; iVol++)
      {
         im.SetVolume(iVol,function(im.ExtractVolume<TSpace4D, TSpace3D>(iVol), image));
      }

      return im;
   }
}

public static partial class ImageExtensionMethods_ImageInt16
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageInt16<TSpace> Blank<TSpace>(this ImageInt16<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public static void CopyInto<TSpace>(this ImageInt16<TSpace> src, ImageInt16<TSpace> destination)
      where TSpace : struct, ISpace
   {
      src.Data.CopyInto(destination.Data, false);
   }

   public static ImageInt16<TSpace> DeepClone<TSpace>(this ImageInt16<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.DeepClone());
   }

   /// <summary>
   /// Concatenates images in the volumes dimension
   /// </summary>
   /// <typeparam name="TVoxel">Voxel data type</typeparam>
   /// <typeparam name="TSpaceIn">The Space being concatenated, which has fewer volumes than <typeparamref name="TSpaceResult"/>. It does not necessarily need to be 3D</typeparam>
   /// <typeparam name="TSelf">The image type</typeparam>
   /// <typeparam name="TTensor">The tensor type held in the image</typeparam>
   /// <typeparam name="TSpaceResult">The Space that results, which has more volumes</typeparam>
   /// <typeparam name="TReturnType">The resulting image type</typeparam>
   /// <param name="items"></param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public static ImageInt16<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageInt16<TSpaceIn>> items)
   where TSpaceIn : struct, ISpace
   where TSpaceResult : struct, ISpace<TSpaceIn>
   {
      if(items.Count == 0 )
      {
         throw new ArgumentException("No images provided");
      }
      var tensor = items.Count == 1 ? items[0].Data.DeepClone() : items[0].Data.Concat(3, items.Select(a => a.Data).Skip(1).ToArray());

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt16<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   [OrientationCheckedAtRuntime]
   public static ImageInt16<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageInt16<TSpaceIn> me, int index)
   where TSpaceIn : struct, ISpace<TSpaceResult>
   where TSpaceResult : struct, ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt16<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   [OrientationCheckedAtRuntime]
   public static ImageInt16<TSpace3d> SetVolume<TSpace4d, TSpace3d>(this ImageInt16<TSpace4d> me, int index, ImageInt16<TSpace3d> newData)
   where TSpace4d : struct, ISpace<TSpace3d>
   where TSpace3d : struct, ISpace
   {

      me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index] = newData.Data.Storage;
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt16<TSpace3d>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal static ImageInt16<TSpace> TrustedOperatorToNew<TSpace>(this ImageInt16<TSpace> im, Func<Tensor,Tensor> torchOperation)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }

   /// <summary>
   /// Applies an operation to each volume within this image, creating a new image in the process
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns>A new image the same shape and type as <paramref name="im"/></returns>
   public static ImageInt16<TSpace4D> VolumewiseOperation<TSpace4D,TSpace3D, TOtherImage>(this ImageInt16<TSpace4D> im, TOtherImage image, Func<ImageInt16<TSpace3D>, TOtherImage, ImageInt16<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      return im.DeepClone().VolumewiseOperationInPlace(image, function);
   }   
      
      
      /// <summary>
   /// Applies an operation to each volume within this image in place
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns><paramref name="im"/></returns>
   public static ImageInt16<TSpace4D> VolumewiseOperationInPlace<TSpace4D,TSpace3D, TOtherImage>(this ImageInt16<TSpace4D> im, TOtherImage image, Func<ImageInt16<TSpace3D>, TOtherImage, ImageInt16<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      for (int iVol = 0; iVol < im.Header.Size.VolumeCount; iVol++)
      {
         im.SetVolume(iVol,function(im.ExtractVolume<TSpace4D, TSpace3D>(iVol), image));
      }

      return im;
   }
}

public static partial class ImageExtensionMethods_ImageInt32
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageInt32<TSpace> Blank<TSpace>(this ImageInt32<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public static void CopyInto<TSpace>(this ImageInt32<TSpace> src, ImageInt32<TSpace> destination)
      where TSpace : struct, ISpace
   {
      src.Data.CopyInto(destination.Data, false);
   }

   public static ImageInt32<TSpace> DeepClone<TSpace>(this ImageInt32<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.DeepClone());
   }

   /// <summary>
   /// Concatenates images in the volumes dimension
   /// </summary>
   /// <typeparam name="TVoxel">Voxel data type</typeparam>
   /// <typeparam name="TSpaceIn">The Space being concatenated, which has fewer volumes than <typeparamref name="TSpaceResult"/>. It does not necessarily need to be 3D</typeparam>
   /// <typeparam name="TSelf">The image type</typeparam>
   /// <typeparam name="TTensor">The tensor type held in the image</typeparam>
   /// <typeparam name="TSpaceResult">The Space that results, which has more volumes</typeparam>
   /// <typeparam name="TReturnType">The resulting image type</typeparam>
   /// <param name="items"></param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public static ImageInt32<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageInt32<TSpaceIn>> items)
   where TSpaceIn : struct, ISpace
   where TSpaceResult : struct, ISpace<TSpaceIn>
   {
      if(items.Count == 0 )
      {
         throw new ArgumentException("No images provided");
      }
      var tensor = items.Count == 1 ? items[0].Data.DeepClone() : items[0].Data.Concat(3, items.Select(a => a.Data).Skip(1).ToArray());

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt32<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   [OrientationCheckedAtRuntime]
   public static ImageInt32<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageInt32<TSpaceIn> me, int index)
   where TSpaceIn : struct, ISpace<TSpaceResult>
   where TSpaceResult : struct, ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt32<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   [OrientationCheckedAtRuntime]
   public static ImageInt32<TSpace3d> SetVolume<TSpace4d, TSpace3d>(this ImageInt32<TSpace4d> me, int index, ImageInt32<TSpace3d> newData)
   where TSpace4d : struct, ISpace<TSpace3d>
   where TSpace3d : struct, ISpace
   {

      me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index] = newData.Data.Storage;
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt32<TSpace3d>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal static ImageInt32<TSpace> TrustedOperatorToNew<TSpace>(this ImageInt32<TSpace> im, Func<Tensor,Tensor> torchOperation)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }

   /// <summary>
   /// Applies an operation to each volume within this image, creating a new image in the process
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns>A new image the same shape and type as <paramref name="im"/></returns>
   public static ImageInt32<TSpace4D> VolumewiseOperation<TSpace4D,TSpace3D, TOtherImage>(this ImageInt32<TSpace4D> im, TOtherImage image, Func<ImageInt32<TSpace3D>, TOtherImage, ImageInt32<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      return im.DeepClone().VolumewiseOperationInPlace(image, function);
   }   
      
      
      /// <summary>
   /// Applies an operation to each volume within this image in place
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns><paramref name="im"/></returns>
   public static ImageInt32<TSpace4D> VolumewiseOperationInPlace<TSpace4D,TSpace3D, TOtherImage>(this ImageInt32<TSpace4D> im, TOtherImage image, Func<ImageInt32<TSpace3D>, TOtherImage, ImageInt32<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      for (int iVol = 0; iVol < im.Header.Size.VolumeCount; iVol++)
      {
         im.SetVolume(iVol,function(im.ExtractVolume<TSpace4D, TSpace3D>(iVol), image));
      }

      return im;
   }
}

public static partial class ImageExtensionMethods_ImageInt64
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageInt64<TSpace> Blank<TSpace>(this ImageInt64<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public static void CopyInto<TSpace>(this ImageInt64<TSpace> src, ImageInt64<TSpace> destination)
      where TSpace : struct, ISpace
   {
      src.Data.CopyInto(destination.Data, false);
   }

   public static ImageInt64<TSpace> DeepClone<TSpace>(this ImageInt64<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.DeepClone());
   }

   /// <summary>
   /// Concatenates images in the volumes dimension
   /// </summary>
   /// <typeparam name="TVoxel">Voxel data type</typeparam>
   /// <typeparam name="TSpaceIn">The Space being concatenated, which has fewer volumes than <typeparamref name="TSpaceResult"/>. It does not necessarily need to be 3D</typeparam>
   /// <typeparam name="TSelf">The image type</typeparam>
   /// <typeparam name="TTensor">The tensor type held in the image</typeparam>
   /// <typeparam name="TSpaceResult">The Space that results, which has more volumes</typeparam>
   /// <typeparam name="TReturnType">The resulting image type</typeparam>
   /// <param name="items"></param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public static ImageInt64<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageInt64<TSpaceIn>> items)
   where TSpaceIn : struct, ISpace
   where TSpaceResult : struct, ISpace<TSpaceIn>
   {
      if(items.Count == 0 )
      {
         throw new ArgumentException("No images provided");
      }
      var tensor = items.Count == 1 ? items[0].Data.DeepClone() : items[0].Data.Concat(3, items.Select(a => a.Data).Skip(1).ToArray());

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   [OrientationCheckedAtRuntime]
   public static ImageInt64<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageInt64<TSpaceIn> me, int index)
   where TSpaceIn : struct, ISpace<TSpaceResult>
   where TSpaceResult : struct, ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   [OrientationCheckedAtRuntime]
   public static ImageInt64<TSpace3d> SetVolume<TSpace4d, TSpace3d>(this ImageInt64<TSpace4d> me, int index, ImageInt64<TSpace3d> newData)
   where TSpace4d : struct, ISpace<TSpace3d>
   where TSpace3d : struct, ISpace
   {

      me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index] = newData.Data.Storage;
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace3d>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal static ImageInt64<TSpace> TrustedOperatorToNew<TSpace>(this ImageInt64<TSpace> im, Func<Tensor,Tensor> torchOperation)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }

   /// <summary>
   /// Applies an operation to each volume within this image, creating a new image in the process
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns>A new image the same shape and type as <paramref name="im"/></returns>
   public static ImageInt64<TSpace4D> VolumewiseOperation<TSpace4D,TSpace3D, TOtherImage>(this ImageInt64<TSpace4D> im, TOtherImage image, Func<ImageInt64<TSpace3D>, TOtherImage, ImageInt64<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      return im.DeepClone().VolumewiseOperationInPlace(image, function);
   }   
      
      
      /// <summary>
   /// Applies an operation to each volume within this image in place
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns><paramref name="im"/></returns>
   public static ImageInt64<TSpace4D> VolumewiseOperationInPlace<TSpace4D,TSpace3D, TOtherImage>(this ImageInt64<TSpace4D> im, TOtherImage image, Func<ImageInt64<TSpace3D>, TOtherImage, ImageInt64<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      for (int iVol = 0; iVol < im.Header.Size.VolumeCount; iVol++)
      {
         im.SetVolume(iVol,function(im.ExtractVolume<TSpace4D, TSpace3D>(iVol), image));
      }

      return im;
   }
}

public static partial class ImageExtensionMethods_ImageFloat
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageFloat<TSpace> Blank<TSpace>(this ImageFloat<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public static void CopyInto<TSpace>(this ImageFloat<TSpace> src, ImageFloat<TSpace> destination)
      where TSpace : struct, ISpace
   {
      src.Data.CopyInto(destination.Data, false);
   }

   public static ImageFloat<TSpace> DeepClone<TSpace>(this ImageFloat<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.DeepClone());
   }

   /// <summary>
   /// Concatenates images in the volumes dimension
   /// </summary>
   /// <typeparam name="TVoxel">Voxel data type</typeparam>
   /// <typeparam name="TSpaceIn">The Space being concatenated, which has fewer volumes than <typeparamref name="TSpaceResult"/>. It does not necessarily need to be 3D</typeparam>
   /// <typeparam name="TSelf">The image type</typeparam>
   /// <typeparam name="TTensor">The tensor type held in the image</typeparam>
   /// <typeparam name="TSpaceResult">The Space that results, which has more volumes</typeparam>
   /// <typeparam name="TReturnType">The resulting image type</typeparam>
   /// <param name="items"></param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public static ImageFloat<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageFloat<TSpaceIn>> items)
   where TSpaceIn : struct, ISpace
   where TSpaceResult : struct, ISpace<TSpaceIn>
   {
      if(items.Count == 0 )
      {
         throw new ArgumentException("No images provided");
      }
      var tensor = items.Count == 1 ? items[0].Data.DeepClone() : items[0].Data.Concat(3, items.Select(a => a.Data).Skip(1).ToArray());

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageFloat<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   [OrientationCheckedAtRuntime]
   public static ImageFloat<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageFloat<TSpaceIn> me, int index)
   where TSpaceIn : struct, ISpace<TSpaceResult>
   where TSpaceResult : struct, ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageFloat<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   [OrientationCheckedAtRuntime]
   public static ImageFloat<TSpace3d> SetVolume<TSpace4d, TSpace3d>(this ImageFloat<TSpace4d> me, int index, ImageFloat<TSpace3d> newData)
   where TSpace4d : struct, ISpace<TSpace3d>
   where TSpace3d : struct, ISpace
   {

      me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index] = newData.Data.Storage;
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageFloat<TSpace3d>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal static ImageFloat<TSpace> TrustedOperatorToNew<TSpace>(this ImageFloat<TSpace> im, Func<Tensor,Tensor> torchOperation)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }

   /// <summary>
   /// Applies an operation to each volume within this image, creating a new image in the process
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns>A new image the same shape and type as <paramref name="im"/></returns>
   public static ImageFloat<TSpace4D> VolumewiseOperation<TSpace4D,TSpace3D, TOtherImage>(this ImageFloat<TSpace4D> im, TOtherImage image, Func<ImageFloat<TSpace3D>, TOtherImage, ImageFloat<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      return im.DeepClone().VolumewiseOperationInPlace(image, function);
   }   
      
      
      /// <summary>
   /// Applies an operation to each volume within this image in place
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns><paramref name="im"/></returns>
   public static ImageFloat<TSpace4D> VolumewiseOperationInPlace<TSpace4D,TSpace3D, TOtherImage>(this ImageFloat<TSpace4D> im, TOtherImage image, Func<ImageFloat<TSpace3D>, TOtherImage, ImageFloat<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      for (int iVol = 0; iVol < im.Header.Size.VolumeCount; iVol++)
      {
         im.SetVolume(iVol,function(im.ExtractVolume<TSpace4D, TSpace3D>(iVol), image));
      }

      return im;
   }
}

public static partial class ImageExtensionMethods_ImageBool
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageBool<TSpace> Blank<TSpace>(this ImageBool<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public static void CopyInto<TSpace>(this ImageBool<TSpace> src, ImageBool<TSpace> destination)
      where TSpace : struct, ISpace
   {
      src.Data.CopyInto(destination.Data, false);
   }

   public static ImageBool<TSpace> DeepClone<TSpace>(this ImageBool<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.DeepClone());
   }

   /// <summary>
   /// Concatenates images in the volumes dimension
   /// </summary>
   /// <typeparam name="TVoxel">Voxel data type</typeparam>
   /// <typeparam name="TSpaceIn">The Space being concatenated, which has fewer volumes than <typeparamref name="TSpaceResult"/>. It does not necessarily need to be 3D</typeparam>
   /// <typeparam name="TSelf">The image type</typeparam>
   /// <typeparam name="TTensor">The tensor type held in the image</typeparam>
   /// <typeparam name="TSpaceResult">The Space that results, which has more volumes</typeparam>
   /// <typeparam name="TReturnType">The resulting image type</typeparam>
   /// <param name="items"></param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public static ImageBool<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageBool<TSpaceIn>> items)
   where TSpaceIn : struct, ISpace
   where TSpaceResult : struct, ISpace<TSpaceIn>
   {
      if(items.Count == 0 )
      {
         throw new ArgumentException("No images provided");
      }
      var tensor = items.Count == 1 ? items[0].Data.DeepClone() : items[0].Data.Concat(3, items.Select(a => a.Data).Skip(1).ToArray());

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageBool<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   [OrientationCheckedAtRuntime]
   public static ImageBool<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageBool<TSpaceIn> me, int index)
   where TSpaceIn : struct, ISpace<TSpaceResult>
   where TSpaceResult : struct, ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageBool<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   [OrientationCheckedAtRuntime]
   public static ImageBool<TSpace3d> SetVolume<TSpace4d, TSpace3d>(this ImageBool<TSpace4d> me, int index, ImageBool<TSpace3d> newData)
   where TSpace4d : struct, ISpace<TSpace3d>
   where TSpace3d : struct, ISpace
   {

      me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index] = newData.Data.Storage;
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageBool<TSpace3d>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal static ImageBool<TSpace> TrustedOperatorToNew<TSpace>(this ImageBool<TSpace> im, Func<Tensor,Tensor> torchOperation)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }

   /// <summary>
   /// Applies an operation to each volume within this image, creating a new image in the process
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns>A new image the same shape and type as <paramref name="im"/></returns>
   public static ImageBool<TSpace4D> VolumewiseOperation<TSpace4D,TSpace3D, TOtherImage>(this ImageBool<TSpace4D> im, TOtherImage image, Func<ImageBool<TSpace3D>, TOtherImage, ImageBool<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      return im.DeepClone().VolumewiseOperationInPlace(image, function);
   }   
      
      
      /// <summary>
   /// Applies an operation to each volume within this image in place
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns><paramref name="im"/></returns>
   public static ImageBool<TSpace4D> VolumewiseOperationInPlace<TSpace4D,TSpace3D, TOtherImage>(this ImageBool<TSpace4D> im, TOtherImage image, Func<ImageBool<TSpace3D>, TOtherImage, ImageBool<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      for (int iVol = 0; iVol < im.Header.Size.VolumeCount; iVol++)
      {
         im.SetVolume(iVol,function(im.ExtractVolume<TSpace4D, TSpace3D>(iVol), image));
      }

      return im;
   }
}

public static partial class ImageExtensionMethods_ImageComplex
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageComplex<TSpace> Blank<TSpace>(this ImageComplex<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public static void CopyInto<TSpace>(this ImageComplex<TSpace> src, ImageComplex<TSpace> destination)
      where TSpace : struct, ISpace
   {
      src.Data.CopyInto(destination.Data, false);
   }

   public static ImageComplex<TSpace> DeepClone<TSpace>(this ImageComplex<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.DeepClone());
   }

   /// <summary>
   /// Concatenates images in the volumes dimension
   /// </summary>
   /// <typeparam name="TVoxel">Voxel data type</typeparam>
   /// <typeparam name="TSpaceIn">The Space being concatenated, which has fewer volumes than <typeparamref name="TSpaceResult"/>. It does not necessarily need to be 3D</typeparam>
   /// <typeparam name="TSelf">The image type</typeparam>
   /// <typeparam name="TTensor">The tensor type held in the image</typeparam>
   /// <typeparam name="TSpaceResult">The Space that results, which has more volumes</typeparam>
   /// <typeparam name="TReturnType">The resulting image type</typeparam>
   /// <param name="items"></param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public static ImageComplex<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageComplex<TSpaceIn>> items)
   where TSpaceIn : struct, ISpace
   where TSpaceResult : struct, ISpace<TSpaceIn>
   {
      if(items.Count == 0 )
      {
         throw new ArgumentException("No images provided");
      }
      var tensor = items.Count == 1 ? items[0].Data.DeepClone() : items[0].Data.Concat(3, items.Select(a => a.Data).Skip(1).ToArray());

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageComplex<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   [OrientationCheckedAtRuntime]
   public static ImageComplex<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageComplex<TSpaceIn> me, int index)
   where TSpaceIn : struct, ISpace<TSpaceResult>
   where TSpaceResult : struct, ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageComplex<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   [OrientationCheckedAtRuntime]
   public static ImageComplex<TSpace3d> SetVolume<TSpace4d, TSpace3d>(this ImageComplex<TSpace4d> me, int index, ImageComplex<TSpace3d> newData)
   where TSpace4d : struct, ISpace<TSpace3d>
   where TSpace3d : struct, ISpace
   {

      me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index] = newData.Data.Storage;
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageComplex<TSpace3d>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal static ImageComplex<TSpace> TrustedOperatorToNew<TSpace>(this ImageComplex<TSpace> im, Func<Tensor,Tensor> torchOperation)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }

   /// <summary>
   /// Applies an operation to each volume within this image, creating a new image in the process
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns>A new image the same shape and type as <paramref name="im"/></returns>
   public static ImageComplex<TSpace4D> VolumewiseOperation<TSpace4D,TSpace3D, TOtherImage>(this ImageComplex<TSpace4D> im, TOtherImage image, Func<ImageComplex<TSpace3D>, TOtherImage, ImageComplex<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      return im.DeepClone().VolumewiseOperationInPlace(image, function);
   }   
      
      
      /// <summary>
   /// Applies an operation to each volume within this image in place
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns><paramref name="im"/></returns>
   public static ImageComplex<TSpace4D> VolumewiseOperationInPlace<TSpace4D,TSpace3D, TOtherImage>(this ImageComplex<TSpace4D> im, TOtherImage image, Func<ImageComplex<TSpace3D>, TOtherImage, ImageComplex<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      for (int iVol = 0; iVol < im.Header.Size.VolumeCount; iVol++)
      {
         im.SetVolume(iVol,function(im.ExtractVolume<TSpace4D, TSpace3D>(iVol), image));
      }

      return im;
   }
}

public static partial class ImageExtensionMethods_ImageComplex32
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageComplex32<TSpace> Blank<TSpace>(this ImageComplex32<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public static void CopyInto<TSpace>(this ImageComplex32<TSpace> src, ImageComplex32<TSpace> destination)
      where TSpace : struct, ISpace
   {
      src.Data.CopyInto(destination.Data, false);
   }

   public static ImageComplex32<TSpace> DeepClone<TSpace>(this ImageComplex32<TSpace> im)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.DeepClone());
   }

   /// <summary>
   /// Concatenates images in the volumes dimension
   /// </summary>
   /// <typeparam name="TVoxel">Voxel data type</typeparam>
   /// <typeparam name="TSpaceIn">The Space being concatenated, which has fewer volumes than <typeparamref name="TSpaceResult"/>. It does not necessarily need to be 3D</typeparam>
   /// <typeparam name="TSelf">The image type</typeparam>
   /// <typeparam name="TTensor">The tensor type held in the image</typeparam>
   /// <typeparam name="TSpaceResult">The Space that results, which has more volumes</typeparam>
   /// <typeparam name="TReturnType">The resulting image type</typeparam>
   /// <param name="items"></param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public static ImageComplex32<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageComplex32<TSpaceIn>> items)
   where TSpaceIn : struct, ISpace
   where TSpaceResult : struct, ISpace<TSpaceIn>
   {
      if(items.Count == 0 )
      {
         throw new ArgumentException("No images provided");
      }
      var tensor = items.Count == 1 ? items[0].Data.DeepClone() : items[0].Data.Concat(3, items.Select(a => a.Data).Skip(1).ToArray());

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageComplex32<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   [OrientationCheckedAtRuntime]
   public static ImageComplex32<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageComplex32<TSpaceIn> me, int index)
   where TSpaceIn : struct, ISpace<TSpaceResult>
   where TSpaceResult : struct, ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageComplex32<TSpaceResult>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   [OrientationCheckedAtRuntime]
   public static ImageComplex32<TSpace3d> SetVolume<TSpace4d, TSpace3d>(this ImageComplex32<TSpace4d> me, int index, ImageComplex32<TSpace3d> newData)
   where TSpace4d : struct, ISpace<TSpace3d>
   where TSpace3d : struct, ISpace
   {

      me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index] = newData.Data.Storage;
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index].unsqueeze_(3));

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageComplex32<TSpace3d>(tensor, true);
#pragma warning restore CS0618 // Type or member is obsolete
   }


   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal static ImageComplex32<TSpace> TrustedOperatorToNew<TSpace>(this ImageComplex32<TSpace> im, Func<Tensor,Tensor> torchOperation)
      where TSpace : struct, ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }

   /// <summary>
   /// Applies an operation to each volume within this image, creating a new image in the process
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns>A new image the same shape and type as <paramref name="im"/></returns>
   public static ImageComplex32<TSpace4D> VolumewiseOperation<TSpace4D,TSpace3D, TOtherImage>(this ImageComplex32<TSpace4D> im, TOtherImage image, Func<ImageComplex32<TSpace3D>, TOtherImage, ImageComplex32<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      return im.DeepClone().VolumewiseOperationInPlace(image, function);
   }   
      
      
      /// <summary>
   /// Applies an operation to each volume within this image in place
   /// </summary>
   /// <typeparam name="TSpace4D">4D image space</typeparam>
   /// <typeparam name="TSpace3D">3D image space</typeparam>
   /// <typeparam name="TOtherImage">Image type</typeparam>
   /// <param name="im">Image to alter and first input to the function</param>
   /// <param name="image">Second input to the function</param>
   /// <param name="function">The operation to apply</param>
   /// <returns><paramref name="im"/></returns>
   public static ImageComplex32<TSpace4D> VolumewiseOperationInPlace<TSpace4D,TSpace3D, TOtherImage>(this ImageComplex32<TSpace4D> im, TOtherImage image, Func<ImageComplex32<TSpace3D>, TOtherImage, ImageComplex32<TSpace3D>> function)
      where TSpace4D : struct, ISpace<TSpace3D>
      where TSpace3D : struct, ISpace3D
      where TOtherImage : Image<TSpace3D>
   {
      for (int iVol = 0; iVol < im.Header.Size.VolumeCount; iVol++)
      {
         im.SetVolume(iVol,function(im.ExtractVolume<TSpace4D, TSpace3D>(iVol), image));
      }

      return im;
   }
}

#endregion TEMPLATE EXPANSION
