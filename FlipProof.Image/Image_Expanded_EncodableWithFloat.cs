#pragma expandtemplate typeToReplace=ImageFloat<TSpace>
#pragma expandtemplate ImageInt8<TSpace> ImageUInt8<TSpace> ImageInt16<TSpace>
// ------
// Methods for simple numeric types that can be losselessly encoded with a 32-bit float
// -----
namespace FlipProof.Image;

public partial class ImageFloat<TSpace>
{
   public ImageComplex32<TSpace> FFT() => ImageComplex32<TSpace>.UnsafeCreateStatic(Data.FFTN([0, 1, 2]));
}

#region TEMPLATE EXPANSION
public partial class ImageInt8<TSpace>
{
   public ImageComplex32<TSpace> FFT() => ImageComplex32<TSpace>.UnsafeCreateStatic(Data.FFTN([0, 1, 2]));
}

public partial class ImageUInt8<TSpace>
{
   public ImageComplex32<TSpace> FFT() => ImageComplex32<TSpace>.UnsafeCreateStatic(Data.FFTN([0, 1, 2]));
}

public partial class ImageInt16<TSpace>
{
   public ImageComplex32<TSpace> FFT() => ImageComplex32<TSpace>.UnsafeCreateStatic(Data.FFTN([0, 1, 2]));
}

#endregion TEMPLATE EXPANSION
