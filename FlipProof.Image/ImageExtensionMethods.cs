#pragma expandgeneric ImageInt8 ImageUInt8 ImageInt16 ImageInt32 ImageInt64 ImageFloat
#pragma expandGeneric typeToReplace=ImageDouble


using FlipProof.Torch;
using static TorchSharp.torch.utils;
using static TorchSharp.torch;
using System.Runtime.CompilerServices;
using TorchSharp.Modules;

namespace FlipProof.Image;
public static partial class ImageExtensionMethods
{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageDouble<TSpace> Blank<TSpace>(this ImageDouble<TSpace> im)
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }
   public static ImageDouble<TSpace> DeepClone<TSpace>(this ImageDouble<TSpace> im)
      where TSpace : ISpace
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
   public static ImageDouble<TSpaceResult> ConcatIImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageDouble<TSpaceIn>> items)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = items[0].Data.Stack(3, items.Select(a => a.Data).Skip(1).ToArray());
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageDouble<TSpaceResult>(tensor, false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   public static ImageDouble<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageDouble<TSpaceIn> me, int index)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index]);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageDouble<TSpaceResult>(tensor, true);
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
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }
}

#region GENERIC EXPANSION
public static partial class ImageExtensionMethods_ImageInt8{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageInt8<TSpace> Blank<TSpace>(this ImageInt8<TSpace> im)
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }
   public static ImageInt8<TSpace> DeepClone<TSpace>(this ImageInt8<TSpace> im)
      where TSpace : ISpace
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
   public static ImageInt8<TSpaceResult> ConcatIImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageInt8<TSpaceIn>> items)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = items[0].Data.Stack(3, items.Select(a => a.Data).Skip(1).ToArray());
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt8<TSpaceResult>(tensor, false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   public static ImageInt8<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageInt8<TSpaceIn> me, int index)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index]);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt8<TSpaceResult>(tensor, true);
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
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }
}

public static partial class ImageExtensionMethods_ImageUInt8{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageUInt8<TSpace> Blank<TSpace>(this ImageUInt8<TSpace> im)
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }
   public static ImageUInt8<TSpace> DeepClone<TSpace>(this ImageUInt8<TSpace> im)
      where TSpace : ISpace
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
   public static ImageUInt8<TSpaceResult> ConcatIImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageUInt8<TSpaceIn>> items)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = items[0].Data.Stack(3, items.Select(a => a.Data).Skip(1).ToArray());
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageUInt8<TSpaceResult>(tensor, false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   public static ImageUInt8<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageUInt8<TSpaceIn> me, int index)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index]);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageUInt8<TSpaceResult>(tensor, true);
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
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }
}

public static partial class ImageExtensionMethods_ImageInt16{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageInt16<TSpace> Blank<TSpace>(this ImageInt16<TSpace> im)
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }
   public static ImageInt16<TSpace> DeepClone<TSpace>(this ImageInt16<TSpace> im)
      where TSpace : ISpace
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
   public static ImageInt16<TSpaceResult> ConcatIImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageInt16<TSpaceIn>> items)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = items[0].Data.Stack(3, items.Select(a => a.Data).Skip(1).ToArray());
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt16<TSpaceResult>(tensor, false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   public static ImageInt16<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageInt16<TSpaceIn> me, int index)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index]);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt16<TSpaceResult>(tensor, true);
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
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }
}

public static partial class ImageExtensionMethods_ImageInt32{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageInt32<TSpace> Blank<TSpace>(this ImageInt32<TSpace> im)
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }
   public static ImageInt32<TSpace> DeepClone<TSpace>(this ImageInt32<TSpace> im)
      where TSpace : ISpace
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
   public static ImageInt32<TSpaceResult> ConcatIImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageInt32<TSpaceIn>> items)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = items[0].Data.Stack(3, items.Select(a => a.Data).Skip(1).ToArray());
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt32<TSpaceResult>(tensor, false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   public static ImageInt32<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageInt32<TSpaceIn> me, int index)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index]);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt32<TSpaceResult>(tensor, true);
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
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }
}

public static partial class ImageExtensionMethods_ImageInt64{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageInt64<TSpace> Blank<TSpace>(this ImageInt64<TSpace> im)
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }
   public static ImageInt64<TSpace> DeepClone<TSpace>(this ImageInt64<TSpace> im)
      where TSpace : ISpace
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
   public static ImageInt64<TSpaceResult> ConcatIImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageInt64<TSpaceIn>> items)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = items[0].Data.Stack(3, items.Select(a => a.Data).Skip(1).ToArray());
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpaceResult>(tensor, false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   public static ImageInt64<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageInt64<TSpaceIn> me, int index)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index]);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpaceResult>(tensor, true);
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
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }
}

public static partial class ImageExtensionMethods_ImageFloat{
   /// <summary>
   /// Creates a blank image with shape and orientation matching this
   /// </summary>
   /// <returns></returns>
   public static ImageFloat<TSpace> Blank<TSpace>(this ImageFloat<TSpace> im)
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.Blank());
   }
   public static ImageFloat<TSpace> DeepClone<TSpace>(this ImageFloat<TSpace> im)
      where TSpace : ISpace
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
   public static ImageFloat<TSpaceResult> ConcatIImage<TSpaceIn, TSpaceResult>(this IReadOnlyList<ImageFloat<TSpaceIn>> items)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = items[0].Data.Stack(3, items.Select(a => a.Data).Skip(1).ToArray());
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageFloat<TSpaceResult>(tensor, false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   public static ImageFloat<TSpaceResult> ExtractVolume<TSpaceIn, TSpaceResult>(this ImageFloat<TSpaceIn> me, int index)
   where TSpaceIn : ISpace<TSpaceResult>
   where TSpaceResult : ISpace
   {
      var tensor = me.Data.CreateFromTensor(me.Data.Storage[TensorIndex.Colon, TensorIndex.Colon, TensorIndex.Colon, index]);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageFloat<TSpaceResult>(tensor, true);
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
      where TSpace : ISpace
   {
      return im.UnsafeCreate(im.Data.CreateFromTrustedOperation(torchOperation));
   }
}

#endregion GENERIC EXPANSION
