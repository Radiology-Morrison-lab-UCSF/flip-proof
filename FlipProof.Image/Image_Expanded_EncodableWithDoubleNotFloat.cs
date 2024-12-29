#pragma expandtemplate typeToReplace=ImageDouble<TSpace>
#pragma expandtemplate ImageInt32<TSpace> ImageInt64<TSpace>

// ------
// Methods for simple numeric types that cannot be losselessly encoded with a 32-bit float
// -----

namespace FlipProof.Image;

public partial class ImageDouble<TSpace>
{
   public ImageComplex<TSpace> FFT() => ImageComplex<TSpace>.UnsafeCreateStatic(Data.FFTN([0, 1, 2]));
}

#region TEMPLATE EXPANSION
public partial class ImageInt32<TSpace>
{
   public ImageComplex<TSpace> FFT() => ImageComplex<TSpace>.UnsafeCreateStatic(Data.FFTN([0, 1, 2]));
}

public partial class ImageInt64<TSpace>
{
   public ImageComplex<TSpace> FFT() => ImageComplex<TSpace>.UnsafeCreateStatic(Data.FFTN([0, 1, 2]));
}

#endregion TEMPLATE EXPANSION
