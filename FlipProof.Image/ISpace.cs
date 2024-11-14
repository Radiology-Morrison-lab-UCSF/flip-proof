namespace FlipProof.Image;



/// <summary>
/// Describes the orientation and dimensionality of an image
/// </summary>
public interface ISpace
{
   static abstract ImageHeader? Orientation { get; internal set; }
   static abstract object LockObj { get; }

   static bool IsInitialised<T>() where T:ISpace => T.Orientation != null;

   /// <summary>
   /// Initialises this space. If the space is already initialised, and the orientation is the same, nothing is done. If it is already initialised and the orientation is a mismatch, an exception is thrown.
   /// </summary>
   /// <typeparam name="T">The space to initialise</typeparam>
   /// <param name="orientation">The desired orientation</param>
   /// <exception cref="ArgumentException">An empty orientation is provided or one with an unexpected number of dimensions</exception>
   /// <exception cref="OrientationException">This is already initialised and mismatches that provided</exception>
   internal static void Initialise<T>(ImageHeader orientation) where T: ISpace
   {
      if(orientation == null)
      {
         // Prevents not just nonsense but also the need for a IsInitialised field
         throw new ArgumentException("Orientation is empty");
      }


      if (IsInitialised<T>())
      {
         // No lock needed
         ThrowIfOrientationMismatch(orientation);
         return;
      }


      lock(T.LockObj)
      {
         if (IsInitialised<T>())
         {
            ThrowIfOrientationMismatch(orientation);
         }
         else
         {
            // Initialise
            T.Orientation = orientation;
         }
      }

      static void ThrowIfOrientationMismatch(ImageHeader orientation)
      {
         if (!Matches<T>(orientation))
         {
            throw new OrientationException($"{typeof(T)} is already intialised with a differing orientation to that requested");
         }
      }
   }

   public static bool Matches<S, T>()
      where S : ISpace
      where T : ISpace
   {

      return Matches<T>(S.Orientation);
   }
   static bool Matches<T>(ImageHeader? orientation) 
      where T:ISpace
   {
      
      return T.Orientation != null && T.Orientation.Equals(orientation);
   }

#if DEBUG
   /// <summary>
   /// For unit testing and debugging purposes only
   /// </summary>
   /// <remarks>Do not use for other purposes. Will allow images of bad orientations to mix</remarks>
   /// <typeparam name="T"></typeparam>
   internal static void Debug_Clear<T>() where T : ISpace
   {
      lock (T.LockObj)
      {
         T.Orientation = null;
      }
   }
#endif

}

