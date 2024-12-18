#pragma expandtemplate typeToReplace=ImageDouble<TSpace>
#pragma expandtemplate ImageInt8<TSpace> ImageUInt8<TSpace> ImageInt16<TSpace> ImageInt32<TSpace> ImageInt64<TSpace> ImageFloat<TSpace> ImageBool<TSpace> ImageComplex<TSpace> ImageComplex32<TSpace>
#pragma expandtemplate typeToReplace=ImageDouble<TSpaceResult>
#pragma expandtemplate ImageInt8<TSpaceResult> ImageUInt8<TSpaceResult> ImageInt16<TSpaceResult> ImageInt32<TSpaceResult> ImageInt64<TSpaceResult> ImageFloat<TSpaceResult> ImageBool<TSpaceResult> ImageComplex<TSpaceResult> ImageComplex32<TSpaceResult>
#pragma expandtemplate typeToReplace=ImageDouble<TSpaceIn>
#pragma expandtemplate ImageInt8<TSpaceIn> ImageUInt8<TSpaceIn> ImageInt16<TSpaceIn> ImageInt32<TSpaceIn> ImageInt64<TSpaceIn> ImageFloat<TSpaceIn> ImageBool<TSpaceIn> ImageComplex<TSpaceIn> ImageComplex32<TSpaceIn>

using FlipProof.Torch;
using static TorchSharp.torch;

namespace FlipProof.Image;

public partial class ImageDouble<TSpace>
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public ImageDouble<TSpace> Blank() => UnsafeCreate(Data.Blank());

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public void CopyInto(ImageDouble<TSpace> destination) => Data.CopyInto(destination.Data, false);

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
   public static ImageDouble<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(IReadOnlyList<ImageDouble<TSpaceIn>> items)
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

   /// <summary>
   /// Pads or crops an image, returning an image in a different space
   /// </summary>
   /// <typeparam name="TSpaceResult">The new space describing the result</typeparam>
   /// <param name="x0">Voxels to insert before the image</param>
   /// <param name="x1">Voxels to insert after the image</param>
   /// <param name="y0">Voxels to insert before the image</param>
   /// <param name="y1">Voxels to insert after the image</param>
   /// <param name="z0">Voxels to insert before the image</param>
   /// <param name="z1">Voxels to insert after the image</param>
   /// <param name="vols0">Volumes to insert before the image</param>
   /// <param name="vols1">Volumes to insert after the image</param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public ImageDouble<TSpaceResult> Pad<TSpaceResult>(long x0, long x1, long y0, long y1, long z0, long z1, long vols0 = 0, long vols1=0)
      where TSpaceResult:struct, ISpace
   {
      ImageHeader head = Header.GetForPaddedImage(x0, x1, y0, y1, z0, z1, vols0, vols1);

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageDouble<TSpaceResult>(head, Data.PadSurround(x0, x1, y0, y1, z0, z1, vols0, vols1));
#pragma warning restore CS0618 // Type or member is obsolete
   }

   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal ImageDouble<TSpace> TrustedOperatorToNew(Func<Tensor,Tensor> torchOperation)
   {
      return UnsafeCreate(Data.CreateFromTrustedOperation(torchOperation));
   }

}

#region TEMPLATE EXPANSION
public partial class ImageInt8<TSpace>
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public ImageInt8<TSpace> Blank() => UnsafeCreate(Data.Blank());

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public void CopyInto(ImageInt8<TSpace> destination) => Data.CopyInto(destination.Data, false);

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
   public static ImageInt8<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(IReadOnlyList<ImageInt8<TSpaceIn>> items)
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

   /// <summary>
   /// Pads or crops an image, returning an image in a different space
   /// </summary>
   /// <typeparam name="TSpaceResult">The new space describing the result</typeparam>
   /// <param name="x0">Voxels to insert before the image</param>
   /// <param name="x1">Voxels to insert after the image</param>
   /// <param name="y0">Voxels to insert before the image</param>
   /// <param name="y1">Voxels to insert after the image</param>
   /// <param name="z0">Voxels to insert before the image</param>
   /// <param name="z1">Voxels to insert after the image</param>
   /// <param name="vols0">Volumes to insert before the image</param>
   /// <param name="vols1">Volumes to insert after the image</param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public ImageInt8<TSpaceResult> Pad<TSpaceResult>(long x0, long x1, long y0, long y1, long z0, long z1, long vols0 = 0, long vols1=0)
      where TSpaceResult:struct, ISpace
   {
      ImageHeader head = Header.GetForPaddedImage(x0, x1, y0, y1, z0, z1, vols0, vols1);

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt8<TSpaceResult>(head, Data.PadSurround(x0, x1, y0, y1, z0, z1, vols0, vols1));
#pragma warning restore CS0618 // Type or member is obsolete
   }

   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal ImageInt8<TSpace> TrustedOperatorToNew(Func<Tensor,Tensor> torchOperation)
   {
      return UnsafeCreate(Data.CreateFromTrustedOperation(torchOperation));
   }

}

public partial class ImageUInt8<TSpace>
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public ImageUInt8<TSpace> Blank() => UnsafeCreate(Data.Blank());

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public void CopyInto(ImageUInt8<TSpace> destination) => Data.CopyInto(destination.Data, false);

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
   public static ImageUInt8<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(IReadOnlyList<ImageUInt8<TSpaceIn>> items)
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

   /// <summary>
   /// Pads or crops an image, returning an image in a different space
   /// </summary>
   /// <typeparam name="TSpaceResult">The new space describing the result</typeparam>
   /// <param name="x0">Voxels to insert before the image</param>
   /// <param name="x1">Voxels to insert after the image</param>
   /// <param name="y0">Voxels to insert before the image</param>
   /// <param name="y1">Voxels to insert after the image</param>
   /// <param name="z0">Voxels to insert before the image</param>
   /// <param name="z1">Voxels to insert after the image</param>
   /// <param name="vols0">Volumes to insert before the image</param>
   /// <param name="vols1">Volumes to insert after the image</param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public ImageUInt8<TSpaceResult> Pad<TSpaceResult>(long x0, long x1, long y0, long y1, long z0, long z1, long vols0 = 0, long vols1=0)
      where TSpaceResult:struct, ISpace
   {
      ImageHeader head = Header.GetForPaddedImage(x0, x1, y0, y1, z0, z1, vols0, vols1);

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageUInt8<TSpaceResult>(head, Data.PadSurround(x0, x1, y0, y1, z0, z1, vols0, vols1));
#pragma warning restore CS0618 // Type or member is obsolete
   }

   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal ImageUInt8<TSpace> TrustedOperatorToNew(Func<Tensor,Tensor> torchOperation)
   {
      return UnsafeCreate(Data.CreateFromTrustedOperation(torchOperation));
   }

}

public partial class ImageInt16<TSpace>
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public ImageInt16<TSpace> Blank() => UnsafeCreate(Data.Blank());

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public void CopyInto(ImageInt16<TSpace> destination) => Data.CopyInto(destination.Data, false);

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
   public static ImageInt16<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(IReadOnlyList<ImageInt16<TSpaceIn>> items)
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

   /// <summary>
   /// Pads or crops an image, returning an image in a different space
   /// </summary>
   /// <typeparam name="TSpaceResult">The new space describing the result</typeparam>
   /// <param name="x0">Voxels to insert before the image</param>
   /// <param name="x1">Voxels to insert after the image</param>
   /// <param name="y0">Voxels to insert before the image</param>
   /// <param name="y1">Voxels to insert after the image</param>
   /// <param name="z0">Voxels to insert before the image</param>
   /// <param name="z1">Voxels to insert after the image</param>
   /// <param name="vols0">Volumes to insert before the image</param>
   /// <param name="vols1">Volumes to insert after the image</param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public ImageInt16<TSpaceResult> Pad<TSpaceResult>(long x0, long x1, long y0, long y1, long z0, long z1, long vols0 = 0, long vols1=0)
      where TSpaceResult:struct, ISpace
   {
      ImageHeader head = Header.GetForPaddedImage(x0, x1, y0, y1, z0, z1, vols0, vols1);

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt16<TSpaceResult>(head, Data.PadSurround(x0, x1, y0, y1, z0, z1, vols0, vols1));
#pragma warning restore CS0618 // Type or member is obsolete
   }

   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal ImageInt16<TSpace> TrustedOperatorToNew(Func<Tensor,Tensor> torchOperation)
   {
      return UnsafeCreate(Data.CreateFromTrustedOperation(torchOperation));
   }

}

public partial class ImageInt32<TSpace>
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public ImageInt32<TSpace> Blank() => UnsafeCreate(Data.Blank());

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public void CopyInto(ImageInt32<TSpace> destination) => Data.CopyInto(destination.Data, false);

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
   public static ImageInt32<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(IReadOnlyList<ImageInt32<TSpaceIn>> items)
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

   /// <summary>
   /// Pads or crops an image, returning an image in a different space
   /// </summary>
   /// <typeparam name="TSpaceResult">The new space describing the result</typeparam>
   /// <param name="x0">Voxels to insert before the image</param>
   /// <param name="x1">Voxels to insert after the image</param>
   /// <param name="y0">Voxels to insert before the image</param>
   /// <param name="y1">Voxels to insert after the image</param>
   /// <param name="z0">Voxels to insert before the image</param>
   /// <param name="z1">Voxels to insert after the image</param>
   /// <param name="vols0">Volumes to insert before the image</param>
   /// <param name="vols1">Volumes to insert after the image</param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public ImageInt32<TSpaceResult> Pad<TSpaceResult>(long x0, long x1, long y0, long y1, long z0, long z1, long vols0 = 0, long vols1=0)
      where TSpaceResult:struct, ISpace
   {
      ImageHeader head = Header.GetForPaddedImage(x0, x1, y0, y1, z0, z1, vols0, vols1);

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt32<TSpaceResult>(head, Data.PadSurround(x0, x1, y0, y1, z0, z1, vols0, vols1));
#pragma warning restore CS0618 // Type or member is obsolete
   }

   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal ImageInt32<TSpace> TrustedOperatorToNew(Func<Tensor,Tensor> torchOperation)
   {
      return UnsafeCreate(Data.CreateFromTrustedOperation(torchOperation));
   }

}

public partial class ImageInt64<TSpace>
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public ImageInt64<TSpace> Blank() => UnsafeCreate(Data.Blank());

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public void CopyInto(ImageInt64<TSpace> destination) => Data.CopyInto(destination.Data, false);

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
   public static ImageInt64<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(IReadOnlyList<ImageInt64<TSpaceIn>> items)
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

   /// <summary>
   /// Pads or crops an image, returning an image in a different space
   /// </summary>
   /// <typeparam name="TSpaceResult">The new space describing the result</typeparam>
   /// <param name="x0">Voxels to insert before the image</param>
   /// <param name="x1">Voxels to insert after the image</param>
   /// <param name="y0">Voxels to insert before the image</param>
   /// <param name="y1">Voxels to insert after the image</param>
   /// <param name="z0">Voxels to insert before the image</param>
   /// <param name="z1">Voxels to insert after the image</param>
   /// <param name="vols0">Volumes to insert before the image</param>
   /// <param name="vols1">Volumes to insert after the image</param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public ImageInt64<TSpaceResult> Pad<TSpaceResult>(long x0, long x1, long y0, long y1, long z0, long z1, long vols0 = 0, long vols1=0)
      where TSpaceResult:struct, ISpace
   {
      ImageHeader head = Header.GetForPaddedImage(x0, x1, y0, y1, z0, z1, vols0, vols1);

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpaceResult>(head, Data.PadSurround(x0, x1, y0, y1, z0, z1, vols0, vols1));
#pragma warning restore CS0618 // Type or member is obsolete
   }

   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal ImageInt64<TSpace> TrustedOperatorToNew(Func<Tensor,Tensor> torchOperation)
   {
      return UnsafeCreate(Data.CreateFromTrustedOperation(torchOperation));
   }

}

public partial class ImageFloat<TSpace>
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public ImageFloat<TSpace> Blank() => UnsafeCreate(Data.Blank());

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public void CopyInto(ImageFloat<TSpace> destination) => Data.CopyInto(destination.Data, false);

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
   public static ImageFloat<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(IReadOnlyList<ImageFloat<TSpaceIn>> items)
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

   /// <summary>
   /// Pads or crops an image, returning an image in a different space
   /// </summary>
   /// <typeparam name="TSpaceResult">The new space describing the result</typeparam>
   /// <param name="x0">Voxels to insert before the image</param>
   /// <param name="x1">Voxels to insert after the image</param>
   /// <param name="y0">Voxels to insert before the image</param>
   /// <param name="y1">Voxels to insert after the image</param>
   /// <param name="z0">Voxels to insert before the image</param>
   /// <param name="z1">Voxels to insert after the image</param>
   /// <param name="vols0">Volumes to insert before the image</param>
   /// <param name="vols1">Volumes to insert after the image</param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public ImageFloat<TSpaceResult> Pad<TSpaceResult>(long x0, long x1, long y0, long y1, long z0, long z1, long vols0 = 0, long vols1=0)
      where TSpaceResult:struct, ISpace
   {
      ImageHeader head = Header.GetForPaddedImage(x0, x1, y0, y1, z0, z1, vols0, vols1);

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageFloat<TSpaceResult>(head, Data.PadSurround(x0, x1, y0, y1, z0, z1, vols0, vols1));
#pragma warning restore CS0618 // Type or member is obsolete
   }

   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal ImageFloat<TSpace> TrustedOperatorToNew(Func<Tensor,Tensor> torchOperation)
   {
      return UnsafeCreate(Data.CreateFromTrustedOperation(torchOperation));
   }

}

public partial class ImageBool<TSpace>
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public ImageBool<TSpace> Blank() => UnsafeCreate(Data.Blank());

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public void CopyInto(ImageBool<TSpace> destination) => Data.CopyInto(destination.Data, false);

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
   public static ImageBool<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(IReadOnlyList<ImageBool<TSpaceIn>> items)
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

   /// <summary>
   /// Pads or crops an image, returning an image in a different space
   /// </summary>
   /// <typeparam name="TSpaceResult">The new space describing the result</typeparam>
   /// <param name="x0">Voxels to insert before the image</param>
   /// <param name="x1">Voxels to insert after the image</param>
   /// <param name="y0">Voxels to insert before the image</param>
   /// <param name="y1">Voxels to insert after the image</param>
   /// <param name="z0">Voxels to insert before the image</param>
   /// <param name="z1">Voxels to insert after the image</param>
   /// <param name="vols0">Volumes to insert before the image</param>
   /// <param name="vols1">Volumes to insert after the image</param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public ImageBool<TSpaceResult> Pad<TSpaceResult>(long x0, long x1, long y0, long y1, long z0, long z1, long vols0 = 0, long vols1=0)
      where TSpaceResult:struct, ISpace
   {
      ImageHeader head = Header.GetForPaddedImage(x0, x1, y0, y1, z0, z1, vols0, vols1);

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageBool<TSpaceResult>(head, Data.PadSurround(x0, x1, y0, y1, z0, z1, vols0, vols1));
#pragma warning restore CS0618 // Type or member is obsolete
   }

   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal ImageBool<TSpace> TrustedOperatorToNew(Func<Tensor,Tensor> torchOperation)
   {
      return UnsafeCreate(Data.CreateFromTrustedOperation(torchOperation));
   }

}

public partial class ImageComplex<TSpace>
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public ImageComplex<TSpace> Blank() => UnsafeCreate(Data.Blank());

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public void CopyInto(ImageComplex<TSpace> destination) => Data.CopyInto(destination.Data, false);

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
   public static ImageComplex<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(IReadOnlyList<ImageComplex<TSpaceIn>> items)
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

   /// <summary>
   /// Pads or crops an image, returning an image in a different space
   /// </summary>
   /// <typeparam name="TSpaceResult">The new space describing the result</typeparam>
   /// <param name="x0">Voxels to insert before the image</param>
   /// <param name="x1">Voxels to insert after the image</param>
   /// <param name="y0">Voxels to insert before the image</param>
   /// <param name="y1">Voxels to insert after the image</param>
   /// <param name="z0">Voxels to insert before the image</param>
   /// <param name="z1">Voxels to insert after the image</param>
   /// <param name="vols0">Volumes to insert before the image</param>
   /// <param name="vols1">Volumes to insert after the image</param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public ImageComplex<TSpaceResult> Pad<TSpaceResult>(long x0, long x1, long y0, long y1, long z0, long z1, long vols0 = 0, long vols1=0)
      where TSpaceResult:struct, ISpace
   {
      ImageHeader head = Header.GetForPaddedImage(x0, x1, y0, y1, z0, z1, vols0, vols1);

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageComplex<TSpaceResult>(head, Data.PadSurround(x0, x1, y0, y1, z0, z1, vols0, vols1));
#pragma warning restore CS0618 // Type or member is obsolete
   }

   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal ImageComplex<TSpace> TrustedOperatorToNew(Func<Tensor,Tensor> torchOperation)
   {
      return UnsafeCreate(Data.CreateFromTrustedOperation(torchOperation));
   }

}

public partial class ImageComplex32<TSpace>
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public ImageComplex32<TSpace> Blank() => UnsafeCreate(Data.Blank());

   /// <summary>
   /// copies voxels into another image
   /// </summary>
   /// <returns></returns>
   public void CopyInto(ImageComplex32<TSpace> destination) => Data.CopyInto(destination.Data, false);

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
   public static ImageComplex32<TSpaceResult> ConcatImage<TSpaceIn, TSpaceResult>(IReadOnlyList<ImageComplex32<TSpaceIn>> items)
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

   /// <summary>
   /// Pads or crops an image, returning an image in a different space
   /// </summary>
   /// <typeparam name="TSpaceResult">The new space describing the result</typeparam>
   /// <param name="x0">Voxels to insert before the image</param>
   /// <param name="x1">Voxels to insert after the image</param>
   /// <param name="y0">Voxels to insert before the image</param>
   /// <param name="y1">Voxels to insert after the image</param>
   /// <param name="z0">Voxels to insert before the image</param>
   /// <param name="z1">Voxels to insert after the image</param>
   /// <param name="vols0">Volumes to insert before the image</param>
   /// <param name="vols1">Volumes to insert after the image</param>
   /// <returns></returns>
   [OrientationCheckedAtRuntime]
   public ImageComplex32<TSpaceResult> Pad<TSpaceResult>(long x0, long x1, long y0, long y1, long z0, long z1, long vols0 = 0, long vols1=0)
      where TSpaceResult:struct, ISpace
   {
      ImageHeader head = Header.GetForPaddedImage(x0, x1, y0, y1, z0, z1, vols0, vols1);

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageComplex32<TSpaceResult>(head, Data.PadSurround(x0, x1, y0, y1, z0, z1, vols0, vols1));
#pragma warning restore CS0618 // Type or member is obsolete
   }

   /// <summary>
   /// Creates a new image presumed to be the same size and shape without checking from a torch operation
   /// </summary>
   /// <typeparam name="TSpace">Image space</typeparam>
   /// <param name="im">Source image</param>
   /// <param name="torchOperation">An operation that does not alter the source and returns a new tensor</param>
   /// <returns></returns>
   internal ImageComplex32<TSpace> TrustedOperatorToNew(Func<Tensor,Tensor> torchOperation)
   {
      return UnsafeCreate(Data.CreateFromTrustedOperation(torchOperation));
   }

}

#endregion TEMPLATE EXPANSION
