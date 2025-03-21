#pragma expandtemplate typeToReplace=ImageDouble<TSpace>
#pragma expandtemplate ImageInt8<TSpace> ImageUInt8<TSpace> ImageInt16<TSpace> ImageInt32<TSpace> ImageInt64<TSpace> ImageFloat<TSpace> ImageBool<TSpace>
#pragma expandtemplate typeToReplace=ImageDouble
#pragma expandtemplate ImageInt8 ImageUInt8 ImageInt16 ImageInt32 ImageInt64 ImageFloat ImageBool
#pragma expandtemplate typeToReplace=DoubleTensor
#pragma expandtemplate Int8Tensor UInt8Tensor Int16Tensor Int32Tensor Int64Tensor FloatTensor BoolTensor
#pragma expandtemplate typeToReplace=double
#pragma expandtemplate sbyte byte Int16 Int32 Int64 float bool

using FlipProof.Base;
using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image;

public sealed partial class ImageDouble<TSpace>
{
   #region Constructors

#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageDouble<TSpace> UnsafeCreateStatic(DoubleTensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   /// <summary>
   /// Creates a new Image from an array of voxels. The header is explicitly checked against <typeparamref name="TSpace"/>: 
   /// use an operation with an existing image instead to use compile-time-checks where possible
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Order data strides volume(fastest), z, y, x (slowest)</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageDouble(ImageHeader header, double[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageDouble(DoubleTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageDouble(ImageHeader header, DoubleTensor voxels) : base(header, voxels)
   {
   }

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageDouble(ImageHeader header, Array3D<double> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageDouble(ImageHeader header, Array4D<double> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   internal override ImageDouble<TSpace> UnsafeCreate(DoubleTensor voxels) => ImageDouble<TSpace>.UnsafeCreateStatic(voxels);


   #endregion

   #region Operators
   public static ImageBool<TSpace> operator ==(ImageDouble<TSpace> im, double value) => ImageBool<TSpace>.UnsafeCreateStatic(im.Data.ValuewiseEquals(value));
   public static ImageBool<TSpace> operator !=(ImageDouble<TSpace> im, double value) => (im == value).Not();

   #endregion

   #region Index Operators and Get/Set

   /// <summary>
   /// Set values within a mask
   /// </summary>
   /// <param name="mask">Values are only set where mask is true</param>
   /// <returns>A new image with those values falling outside the mask set to the default value</returns>
   public ImageDouble<TSpace> this[ImageBool<TSpace> mask]
   {
      get => Masked(mask);
      set => Set(value, mask);
   }

   /// <summary>
   /// Returns a copy of this image. Voxels that are false in the mask are set to the default value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageDouble<TSpace> Masked(ImageBool<TSpace> mask) => UnsafeCreate(_data.Masked(mask.Data));

   /// <summary>
   /// Voxels that are true in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageDouble<TSpace> SetInsideMaskInPlace(ImageBool<TSpace> mask, double fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data, fillWith);
      return this;
   }

   /// <summary>
   /// Voxels that are false in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageDouble<TSpace> SetOutsideMaskInPlace(ImageBool<TSpace> mask, double fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data.Not(), fillWith);
      return this;
   }



   #endregion

   public override int GetHashCode() => Data.GetHashCode();
   public override bool Equals(object? obj) => obj is not null && object.ReferenceEquals(this, obj);  
}

#region TEMPLATE EXPANSION
public sealed partial class ImageInt8<TSpace>
{
   #region Constructors

#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageInt8<TSpace> UnsafeCreateStatic(Int8Tensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   /// <summary>
   /// Creates a new Image from an array of voxels. The header is explicitly checked against <typeparamref name="TSpace"/>: 
   /// use an operation with an existing image instead to use compile-time-checks where possible
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Order data strides volume(fastest), z, y, x (slowest)</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt8(ImageHeader header, sbyte[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt8(Int8Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt8(ImageHeader header, Int8Tensor voxels) : base(header, voxels)
   {
   }

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt8(ImageHeader header, Array3D<sbyte> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt8(ImageHeader header, Array4D<sbyte> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   internal override ImageInt8<TSpace> UnsafeCreate(Int8Tensor voxels) => ImageInt8<TSpace>.UnsafeCreateStatic(voxels);


   #endregion

   #region Operators
   public static ImageBool<TSpace> operator ==(ImageInt8<TSpace> im, sbyte value) => ImageBool<TSpace>.UnsafeCreateStatic(im.Data.ValuewiseEquals(value));
   public static ImageBool<TSpace> operator !=(ImageInt8<TSpace> im, sbyte value) => (im == value).Not();

   #endregion

   #region Index Operators and Get/Set

   /// <summary>
   /// Set values within a mask
   /// </summary>
   /// <param name="mask">Values are only set where mask is true</param>
   /// <returns>A new image with those values falling outside the mask set to the default value</returns>
   public ImageInt8<TSpace> this[ImageBool<TSpace> mask]
   {
      get => Masked(mask);
      set => Set(value, mask);
   }

   /// <summary>
   /// Returns a copy of this image. Voxels that are false in the mask are set to the default value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt8<TSpace> Masked(ImageBool<TSpace> mask) => UnsafeCreate(_data.Masked(mask.Data));

   /// <summary>
   /// Voxels that are true in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt8<TSpace> SetInsideMaskInPlace(ImageBool<TSpace> mask, sbyte fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data, fillWith);
      return this;
   }

   /// <summary>
   /// Voxels that are false in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt8<TSpace> SetOutsideMaskInPlace(ImageBool<TSpace> mask, sbyte fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data.Not(), fillWith);
      return this;
   }



   #endregion

   public override int GetHashCode() => Data.GetHashCode();
   public override bool Equals(object? obj) => obj is not null && object.ReferenceEquals(this, obj);  
}

public sealed partial class ImageUInt8<TSpace>
{
   #region Constructors

#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageUInt8<TSpace> UnsafeCreateStatic(UInt8Tensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   /// <summary>
   /// Creates a new Image from an array of voxels. The header is explicitly checked against <typeparamref name="TSpace"/>: 
   /// use an operation with an existing image instead to use compile-time-checks where possible
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Order data strides volume(fastest), z, y, x (slowest)</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageUInt8(ImageHeader header, byte[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageUInt8(UInt8Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageUInt8(ImageHeader header, UInt8Tensor voxels) : base(header, voxels)
   {
   }

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageUInt8(ImageHeader header, Array3D<byte> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageUInt8(ImageHeader header, Array4D<byte> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   internal override ImageUInt8<TSpace> UnsafeCreate(UInt8Tensor voxels) => ImageUInt8<TSpace>.UnsafeCreateStatic(voxels);


   #endregion

   #region Operators
   public static ImageBool<TSpace> operator ==(ImageUInt8<TSpace> im, byte value) => ImageBool<TSpace>.UnsafeCreateStatic(im.Data.ValuewiseEquals(value));
   public static ImageBool<TSpace> operator !=(ImageUInt8<TSpace> im, byte value) => (im == value).Not();

   #endregion

   #region Index Operators and Get/Set

   /// <summary>
   /// Set values within a mask
   /// </summary>
   /// <param name="mask">Values are only set where mask is true</param>
   /// <returns>A new image with those values falling outside the mask set to the default value</returns>
   public ImageUInt8<TSpace> this[ImageBool<TSpace> mask]
   {
      get => Masked(mask);
      set => Set(value, mask);
   }

   /// <summary>
   /// Returns a copy of this image. Voxels that are false in the mask are set to the default value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageUInt8<TSpace> Masked(ImageBool<TSpace> mask) => UnsafeCreate(_data.Masked(mask.Data));

   /// <summary>
   /// Voxels that are true in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageUInt8<TSpace> SetInsideMaskInPlace(ImageBool<TSpace> mask, byte fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data, fillWith);
      return this;
   }

   /// <summary>
   /// Voxels that are false in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageUInt8<TSpace> SetOutsideMaskInPlace(ImageBool<TSpace> mask, byte fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data.Not(), fillWith);
      return this;
   }



   #endregion

   public override int GetHashCode() => Data.GetHashCode();
   public override bool Equals(object? obj) => obj is not null && object.ReferenceEquals(this, obj);  
}

public sealed partial class ImageInt16<TSpace>
{
   #region Constructors

#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageInt16<TSpace> UnsafeCreateStatic(Int16Tensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   /// <summary>
   /// Creates a new Image from an array of voxels. The header is explicitly checked against <typeparamref name="TSpace"/>: 
   /// use an operation with an existing image instead to use compile-time-checks where possible
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Order data strides volume(fastest), z, y, x (slowest)</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt16(ImageHeader header, Int16[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt16(Int16Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt16(ImageHeader header, Int16Tensor voxels) : base(header, voxels)
   {
   }

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt16(ImageHeader header, Array3D<Int16> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt16(ImageHeader header, Array4D<Int16> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   internal override ImageInt16<TSpace> UnsafeCreate(Int16Tensor voxels) => ImageInt16<TSpace>.UnsafeCreateStatic(voxels);


   #endregion

   #region Operators
   public static ImageBool<TSpace> operator ==(ImageInt16<TSpace> im, Int16 value) => ImageBool<TSpace>.UnsafeCreateStatic(im.Data.ValuewiseEquals(value));
   public static ImageBool<TSpace> operator !=(ImageInt16<TSpace> im, Int16 value) => (im == value).Not();

   #endregion

   #region Index Operators and Get/Set

   /// <summary>
   /// Set values within a mask
   /// </summary>
   /// <param name="mask">Values are only set where mask is true</param>
   /// <returns>A new image with those values falling outside the mask set to the default value</returns>
   public ImageInt16<TSpace> this[ImageBool<TSpace> mask]
   {
      get => Masked(mask);
      set => Set(value, mask);
   }

   /// <summary>
   /// Returns a copy of this image. Voxels that are false in the mask are set to the default value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt16<TSpace> Masked(ImageBool<TSpace> mask) => UnsafeCreate(_data.Masked(mask.Data));

   /// <summary>
   /// Voxels that are true in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt16<TSpace> SetInsideMaskInPlace(ImageBool<TSpace> mask, Int16 fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data, fillWith);
      return this;
   }

   /// <summary>
   /// Voxels that are false in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt16<TSpace> SetOutsideMaskInPlace(ImageBool<TSpace> mask, Int16 fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data.Not(), fillWith);
      return this;
   }



   #endregion

   public override int GetHashCode() => Data.GetHashCode();
   public override bool Equals(object? obj) => obj is not null && object.ReferenceEquals(this, obj);  
}

public sealed partial class ImageInt32<TSpace>
{
   #region Constructors

#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageInt32<TSpace> UnsafeCreateStatic(Int32Tensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   /// <summary>
   /// Creates a new Image from an array of voxels. The header is explicitly checked against <typeparamref name="TSpace"/>: 
   /// use an operation with an existing image instead to use compile-time-checks where possible
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Order data strides volume(fastest), z, y, x (slowest)</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt32(ImageHeader header, Int32[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt32(Int32Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt32(ImageHeader header, Int32Tensor voxels) : base(header, voxels)
   {
   }

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt32(ImageHeader header, Array3D<Int32> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt32(ImageHeader header, Array4D<Int32> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   internal override ImageInt32<TSpace> UnsafeCreate(Int32Tensor voxels) => ImageInt32<TSpace>.UnsafeCreateStatic(voxels);


   #endregion

   #region Operators
   public static ImageBool<TSpace> operator ==(ImageInt32<TSpace> im, Int32 value) => ImageBool<TSpace>.UnsafeCreateStatic(im.Data.ValuewiseEquals(value));
   public static ImageBool<TSpace> operator !=(ImageInt32<TSpace> im, Int32 value) => (im == value).Not();

   #endregion

   #region Index Operators and Get/Set

   /// <summary>
   /// Set values within a mask
   /// </summary>
   /// <param name="mask">Values are only set where mask is true</param>
   /// <returns>A new image with those values falling outside the mask set to the default value</returns>
   public ImageInt32<TSpace> this[ImageBool<TSpace> mask]
   {
      get => Masked(mask);
      set => Set(value, mask);
   }

   /// <summary>
   /// Returns a copy of this image. Voxels that are false in the mask are set to the default value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt32<TSpace> Masked(ImageBool<TSpace> mask) => UnsafeCreate(_data.Masked(mask.Data));

   /// <summary>
   /// Voxels that are true in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt32<TSpace> SetInsideMaskInPlace(ImageBool<TSpace> mask, Int32 fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data, fillWith);
      return this;
   }

   /// <summary>
   /// Voxels that are false in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt32<TSpace> SetOutsideMaskInPlace(ImageBool<TSpace> mask, Int32 fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data.Not(), fillWith);
      return this;
   }



   #endregion

   public override int GetHashCode() => Data.GetHashCode();
   public override bool Equals(object? obj) => obj is not null && object.ReferenceEquals(this, obj);  
}

public sealed partial class ImageInt64<TSpace>
{
   #region Constructors

#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageInt64<TSpace> UnsafeCreateStatic(Int64Tensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   /// <summary>
   /// Creates a new Image from an array of voxels. The header is explicitly checked against <typeparamref name="TSpace"/>: 
   /// use an operation with an existing image instead to use compile-time-checks where possible
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Order data strides volume(fastest), z, y, x (slowest)</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt64(ImageHeader header, Int64[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt64(Int64Tensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageInt64(ImageHeader header, Int64Tensor voxels) : base(header, voxels)
   {
   }

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt64(ImageHeader header, Array3D<Int64> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageInt64(ImageHeader header, Array4D<Int64> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   internal override ImageInt64<TSpace> UnsafeCreate(Int64Tensor voxels) => ImageInt64<TSpace>.UnsafeCreateStatic(voxels);


   #endregion

   #region Operators
   public static ImageBool<TSpace> operator ==(ImageInt64<TSpace> im, Int64 value) => ImageBool<TSpace>.UnsafeCreateStatic(im.Data.ValuewiseEquals(value));
   public static ImageBool<TSpace> operator !=(ImageInt64<TSpace> im, Int64 value) => (im == value).Not();

   #endregion

   #region Index Operators and Get/Set

   /// <summary>
   /// Set values within a mask
   /// </summary>
   /// <param name="mask">Values are only set where mask is true</param>
   /// <returns>A new image with those values falling outside the mask set to the default value</returns>
   public ImageInt64<TSpace> this[ImageBool<TSpace> mask]
   {
      get => Masked(mask);
      set => Set(value, mask);
   }

   /// <summary>
   /// Returns a copy of this image. Voxels that are false in the mask are set to the default value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt64<TSpace> Masked(ImageBool<TSpace> mask) => UnsafeCreate(_data.Masked(mask.Data));

   /// <summary>
   /// Voxels that are true in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt64<TSpace> SetInsideMaskInPlace(ImageBool<TSpace> mask, Int64 fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data, fillWith);
      return this;
   }

   /// <summary>
   /// Voxels that are false in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageInt64<TSpace> SetOutsideMaskInPlace(ImageBool<TSpace> mask, Int64 fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data.Not(), fillWith);
      return this;
   }



   #endregion

   public override int GetHashCode() => Data.GetHashCode();
   public override bool Equals(object? obj) => obj is not null && object.ReferenceEquals(this, obj);  
}

public sealed partial class ImageFloat<TSpace>
{
   #region Constructors

#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageFloat<TSpace> UnsafeCreateStatic(FloatTensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   /// <summary>
   /// Creates a new Image from an array of voxels. The header is explicitly checked against <typeparamref name="TSpace"/>: 
   /// use an operation with an existing image instead to use compile-time-checks where possible
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Order data strides volume(fastest), z, y, x (slowest)</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageFloat(ImageHeader header, float[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageFloat(FloatTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageFloat(ImageHeader header, FloatTensor voxels) : base(header, voxels)
   {
   }

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageFloat(ImageHeader header, Array3D<float> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageFloat(ImageHeader header, Array4D<float> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   internal override ImageFloat<TSpace> UnsafeCreate(FloatTensor voxels) => ImageFloat<TSpace>.UnsafeCreateStatic(voxels);


   #endregion

   #region Operators
   public static ImageBool<TSpace> operator ==(ImageFloat<TSpace> im, float value) => ImageBool<TSpace>.UnsafeCreateStatic(im.Data.ValuewiseEquals(value));
   public static ImageBool<TSpace> operator !=(ImageFloat<TSpace> im, float value) => (im == value).Not();

   #endregion

   #region Index Operators and Get/Set

   /// <summary>
   /// Set values within a mask
   /// </summary>
   /// <param name="mask">Values are only set where mask is true</param>
   /// <returns>A new image with those values falling outside the mask set to the default value</returns>
   public ImageFloat<TSpace> this[ImageBool<TSpace> mask]
   {
      get => Masked(mask);
      set => Set(value, mask);
   }

   /// <summary>
   /// Returns a copy of this image. Voxels that are false in the mask are set to the default value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageFloat<TSpace> Masked(ImageBool<TSpace> mask) => UnsafeCreate(_data.Masked(mask.Data));

   /// <summary>
   /// Voxels that are true in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageFloat<TSpace> SetInsideMaskInPlace(ImageBool<TSpace> mask, float fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data, fillWith);
      return this;
   }

   /// <summary>
   /// Voxels that are false in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageFloat<TSpace> SetOutsideMaskInPlace(ImageBool<TSpace> mask, float fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data.Not(), fillWith);
      return this;
   }



   #endregion

   public override int GetHashCode() => Data.GetHashCode();
   public override bool Equals(object? obj) => obj is not null && object.ReferenceEquals(this, obj);  
}

public sealed partial class ImageBool<TSpace>
{
   #region Constructors

#pragma warning disable CS0618 // Type or member is obsolete
   internal static ImageBool<TSpace> UnsafeCreateStatic(BoolTensor voxels) => new(voxels, false);
#pragma warning restore CS0618 // Type or member is obsolete
   /// <summary>
   /// Creates a new Image from an array of voxels. The header is explicitly checked against <typeparamref name="TSpace"/>: 
   /// use an operation with an existing image instead to use compile-time-checks where possible
   /// </summary>
   /// <param name="header"></param>
   /// <param name="voxels">Order data strides volume(fastest), z, y, x (slowest)</param>
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageBool(ImageHeader header, bool[] voxels) : base(header, torch.tensor(voxels).view(header.Size.X, header.Size.Y, header.Size.Z, header.Size.VolumeCount))
   {
   }

   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageBool(BoolTensor voxels, bool verifyShape) : base(voxels, verifyShape)
   {
   }
   [Obsolete("Data are used directly. Do not feed in a tensor accessible outside this object")]
   internal ImageBool(ImageHeader header, BoolTensor voxels) : base(header, voxels)
   {
   }

   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageBool(ImageHeader header, Array3D<bool> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   [Obsolete("Header is checked at run time. Use an operation with an existing image instead to use compile-time-checks where possible")]
   public ImageBool(ImageHeader header, Array4D<bool> voxels) : base(header, voxels.ToTensor4D())
   {

   }
   internal override ImageBool<TSpace> UnsafeCreate(BoolTensor voxels) => ImageBool<TSpace>.UnsafeCreateStatic(voxels);


   #endregion

   #region Operators
   public static ImageBool<TSpace> operator ==(ImageBool<TSpace> im, bool value) => ImageBool<TSpace>.UnsafeCreateStatic(im.Data.ValuewiseEquals(value));
   public static ImageBool<TSpace> operator !=(ImageBool<TSpace> im, bool value) => (im == value).Not();

   #endregion

   #region Index Operators and Get/Set

   /// <summary>
   /// Set values within a mask
   /// </summary>
   /// <param name="mask">Values are only set where mask is true</param>
   /// <returns>A new image with those values falling outside the mask set to the default value</returns>
   public ImageBool<TSpace> this[ImageBool<TSpace> mask]
   {
      get => Masked(mask);
      set => Set(value, mask);
   }

   /// <summary>
   /// Returns a copy of this image. Voxels that are false in the mask are set to the default value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageBool<TSpace> Masked(ImageBool<TSpace> mask) => UnsafeCreate(_data.Masked(mask.Data));

   /// <summary>
   /// Voxels that are true in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageBool<TSpace> SetInsideMaskInPlace(ImageBool<TSpace> mask, bool fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data, fillWith);
      return this;
   }

   /// <summary>
   /// Voxels that are false in the mask are set to the provided value
   /// </summary>
   /// <param name="mask"></param>
   /// <returns></returns>
   public ImageBool<TSpace> SetOutsideMaskInPlace(ImageBool<TSpace> mask, bool fillWith = default)
   {
      Data.MaskedFillInPlace(mask.Data.Not(), fillWith);
      return this;
   }



   #endregion

   public override int GetHashCode() => Data.GetHashCode();
   public override bool Equals(object? obj) => obj is not null && object.ReferenceEquals(this, obj);  
}

#endregion TEMPLATE EXPANSION
