namespace FlipProof.Image;

public static class IImageExtensionMethods
{

   /// <summary>
   /// Throws an argument exception if the two images are not in the same space
   /// </summary>
   /// <param name="image1"></param>
   /// <param name="image2"></param>
   /// <exception cref="ArgumentException"></exception>
   public static void ThrowIfNotSameSpaceAs(this IImage image1, IImage image2)
   {
      if (!IsSameSpaceAs(image1, image2))
      {
         throw new ArgumentException("Images are in different spaces");
      }
   }


   /// <summary>
   /// Returns true if images are in different spaces, within a degree of tolerance
   /// </summary>
   /// <param name="image1"></param>
   /// <param name="image2"></param>
   /// <returns></returns>
   public static bool IsSameSpaceAs(this IImage image1, IImage image2)
   {
      return image1.Header.IsSameSpaceAs(image2.Header);
   }

}
