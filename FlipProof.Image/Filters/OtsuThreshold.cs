using FlipProof.Base;
using FlipProof.Torch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TorchSharp;
using TorchSharp.Modules;

namespace FlipProof.Image.Filters;

public class OtsuThreshold
{
   /// <summary>
   /// Calculates the the threshold that minimised intra-class variance and returns the result as a mask
   /// </summary>
   /// <param name="im"></param>
   /// <param name="inverse">When false, returns voxels above the threshold. When true, returns those below it</param>
   /// <returns>A mask of voxels exceeding the threshold</returns>
   public static ImageBool<TSpace> Apply<TVoxel, TSpace, TSelf, TTensor>(Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor> im, bool inverse=false)
      where TVoxel : struct, INumber<TVoxel>
      where TSpace : ISpace
      where TSelf : Image_SimpleNumeric<TVoxel, TSpace, TSelf, TTensor>
      where TTensor : SimpleNumericTensor<TVoxel, TTensor>
   {
      var asD = im.ToDouble();
      if(inverse)
      {
         return asD <= CalculateThreshold(asD.Data);
      }
      return asD > CalculateThreshold(asD.Data);
   }

   public static double CalculateThreshold(Tensor<double> image, int noBins = 1024)
   {
      if (noBins < 2)
      {
         throw new ArgumentException("At least two bins required", nameof(noBins));
      }
      (torch.Tensor hist, torch.Tensor binEdges) = torch.histogram(image.Storage, bins: noBins);

      binEdges = binEdges[1..];// first is the bottom of the first bin. Keep only the tops

      using torch.Tensor cumHist = hist.cumsum(0);
      using torch.Tensor count = cumHist[^1];


      using torch.Tensor maxVariance = double.PositiveInfinity;
      using torch.Tensor threshold = 0d;

      foreach (var i in Enumerable.Range(0, noBins))
      {
         using torch.Tensor weightB = cumHist[i] / count;
         using torch.Tensor weightA = 1d - weightB;

         if (weightB.eq(0).logical_or(weightA.eq(0)).ToBoolean())
         {
            // all in one class
            continue;
         }

         
         using torch.Tensor varianceA = CalculateVariance(count - cumHist[i], hist[(i+1)..], binEdges[(i+1)..]);
         using torch.Tensor varianceB = CalculateVariance(cumHist[i], hist[..(i+1)], binEdges[..(i+1)]);


         using torch.Tensor var_between = weightA * varianceA + weightB * varianceB;

         if (var_between.less(maxVariance).ToBoolean())
         {
            maxVariance.set_(var_between);
            threshold.set_(binEdges[i]);
         }
      }
      return threshold.ToDouble();


   }
   static torch.Tensor CalculateVariance(torch.Tensor count, torch.Tensor frequency, torch.Tensor value)
   {
      // variance = mean ( (x - u)^2)
      using var mean = (frequency * value).sum() / count;
      using var diffFromMean = (value - mean);
      using var sqDiffFromMean = diffFromMean.square();
      return (sqDiffFromMean * frequency).sum() / count;
   }

}
