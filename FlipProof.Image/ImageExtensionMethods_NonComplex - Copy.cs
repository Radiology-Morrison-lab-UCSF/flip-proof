#pragma expandtemplate typeToReplace=ImageDouble
#pragma expandtemplate ImageInt8 ImageUInt8 ImageInt16 ImageInt32 ImageInt64 ImageFloat ImageBool

using FlipProof.Torch;

namespace FlipProof.Image;

public static partial class ImageExtensionMethods
{
   /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMax<TSpace>(this ImageDouble<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMax(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
      /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMin<TSpace>(this ImageDouble<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMin(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }

}

#region TEMPLATE EXPANSION
public static partial class ImageExtensionMethods_ImageInt8
{
   /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMax<TSpace>(this ImageInt8<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMax(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
      /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMin<TSpace>(this ImageInt8<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMin(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }

}

public static partial class ImageExtensionMethods_ImageUInt8
{
   /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMax<TSpace>(this ImageUInt8<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMax(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
      /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMin<TSpace>(this ImageUInt8<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMin(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }

}

public static partial class ImageExtensionMethods_ImageInt16
{
   /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMax<TSpace>(this ImageInt16<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMax(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
      /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMin<TSpace>(this ImageInt16<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMin(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }

}

public static partial class ImageExtensionMethods_ImageInt32
{
   /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMax<TSpace>(this ImageInt32<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMax(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
      /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMin<TSpace>(this ImageInt32<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMin(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }

}

public static partial class ImageExtensionMethods_ImageInt64
{
   /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMax<TSpace>(this ImageInt64<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMax(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
      /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMin<TSpace>(this ImageInt64<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMin(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }

}

public static partial class ImageExtensionMethods_ImageFloat
{
   /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMax<TSpace>(this ImageFloat<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMax(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
      /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMin<TSpace>(this ImageFloat<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMin(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }

}

public static partial class ImageExtensionMethods_ImageBool
{
   /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMax<TSpace>(this ImageBool<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMax(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }
      /// <summary>
   /// Returns an image where each voxel indicates the provided volume with the largest value
   /// </summary>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="ims">Images</param>
   /// <returns></returns>
   public static ImageInt64<TSpace> ArgMin<TSpace>(this ImageBool<TSpace>[] ims)
      where TSpace : struct, ISpace
   {
      if(ims.Length < 2)
      {
         throw new ArgumentException("Expected at least two volumes",nameof(ims));
      }
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TSpace>(ims.Select(static a => a.Data).ToList().ArgMin(), false);
#pragma warning restore CS0618 // Type or member is obsolete
   }

}

#endregion TEMPLATE EXPANSION
