using FlipProof.Image;
using FlipProof.Torch;
using System.Numerics;
using TorchSharp;

namespace FlipProof.ImageTests;

public abstract class OperatorsTests(int seed) : ImageTestsBase(seed)
{
   DisposeScope? scope;


   [TestInitialize]
   public override void Initialise()
   {
      base.Initialise();
      Cleanup();
      scope = torch.NewDisposeScope();
   }

   [TestCleanup]
   public void Cleanup()
   {
      scope?.Dispose();
   }

   protected static void OperatorsSameTypeTestInt<TImage, TVoxel, TSpace, TTensor>(Func<TImage> getIm0, Func<ImageHeader, TImage> getIm1)
      where TImage : Image_Integer<TVoxel, TSpace, TImage, TTensor>
      where TSpace : ISpace
      where TVoxel : struct, INumber<TVoxel>, IBinaryInteger<TVoxel>
      where TTensor : IntegerTensor<TVoxel, TTensor>
   {
      TImage im0 = OperatorsSameTypeTest<TImage, TVoxel, TSpace, TTensor>(getIm0, getIm1);

      TImage im1 = getIm1(im0.Header);

      TTensor dat0 = im0.Data;
      TTensor dat1 = im1.Data;

      // divide
      // Torch does integr division to float
      AssertImagesMatch<ImageFloat<TSpace>, float, TSpace, FloatTensor>(dat0 / dat1, im0 / im1);
      AssertImagesMatch<ImageFloat<TSpace>, float, TSpace, FloatTensor>(dat1 / dat0, im1 / im0);

   }
   protected static TImage OperatorsSameTypeTest<TImage, TVoxel, TSpace, TTensor>(Func<TImage> getIm0, Func<ImageHeader, TImage> getIm1) 
      where TImage : Image_SimpleNumeric<TVoxel,TSpace, TImage, TTensor>
      where TSpace : ISpace
      where TVoxel : struct, INumber<TVoxel>
      where TTensor : SimpleNumericTensor<TVoxel,TTensor>
   {

      TImage im0 = getIm0();
      TImage im1 = getIm1(im0.Header);

      TTensor dat0 = im0.Data;
      TTensor dat1 = im1.Data;

      // add
      TTensor expected = dat0.Add(dat1);
      AssertImagesMatch<TImage,TVoxel, TSpace, TTensor>(expected, im0 + im1);
      AssertImagesMatch<TImage, TVoxel, TSpace, TTensor>(expected, im1 + im0);

      // subtract
      AssertImagesMatch<TImage, TVoxel, TSpace, TTensor>(dat0.Subtract(dat1), im0 - im1);
      AssertImagesMatch<TImage, TVoxel, TSpace, TTensor>(dat1.Subtract(dat0), im1 - im0);

      // multiply
      expected = dat0.Multiply(dat1);
      AssertImagesMatch<TImage, TVoxel, TSpace, TTensor>(expected, im0 * im1);
      AssertImagesMatch<TImage, TVoxel, TSpace, TTensor>(expected, im1 * im0);

      return im0;
   }

   protected static void AssertImagesMatch<TImage, TVoxel, TSpace, TTensor>(TTensor expected, TImage result)
   where TVoxel : struct, INumber<TVoxel>
   where TSpace : ISpace
   where TImage : Image_SimpleNumeric<TVoxel, TSpace, TImage, TTensor>
   where TTensor : SimpleNumericTensor<TVoxel, TTensor>
   {
      // NaN != NaN
      var res = result.GetVoxelTensor().Storage;
      Assert.IsTrue(torch.isnan(expected.Storage).equal(torch.isnan(res)).all().ToBoolean());

      Assert.IsTrue(torch.nan_to_num(expected.Storage).equal(torch.nan_to_num(result.GetVoxelTensor().Storage)).all().ToBoolean());
   }


   public void FFT_IFFT<TImage, TVoxel, TSpace, TTensor>(Func<TImage> getIm0)
      where TImage : Image_SimpleNumeric<TVoxel, TSpace, TImage, TTensor>
      where TSpace : ISpace
      where TVoxel : struct,INumber<TVoxel>
      where TTensor : SimpleNumericTensor<TVoxel, TTensor>
   {
      using DisposeScope scope = torch.NewDisposeScope();

      TImage orig = getIm0();

      ImageComplex32<TSpace> forward = orig.FFT();

      ImageFloat<TSpace> inverse = forward.IFFT();

      var denominator = orig.ToFloat().AbsInPlace().ReplaceInPlace(0f, 1f);

      ImageFloat<TSpace> differenceAsFraction = (orig.ToFloat() - inverse).AbsInPlace() / denominator;
      Assert.IsTrue(differenceAsFraction.GetMaxIntensity() < 0.01f);
   }
}
