using FlipProof.Base;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Image;

/// <summary>
/// Describes the orientation and dimensionality of an image. Attempts to register a 4D image will result in a runtime exception
/// </summary>
public interface ISpace3D : ISpace
{

}

/// <summary>
/// Describes the orientation and dimensionality of an image
/// </summary>
/// <typeparam name="TMatches">Any other space that this must align to. May differ by number of volumes only.</typeparam>
public interface ISpace<TMatches> : ISpace
   where TMatches:ISpace
{
}

/// <summary>
/// Describes the orientation and dimensionality of an image
/// </summary>
public interface ISpace
{
   private static object LockObj { get; } = new object();
   private static ConcurrentDictionary<Type, ImageHeader> Orientations { get; } = [];


   /// <summary>
   /// Returns the known orientation for <typeparamref name="T"/>
   /// </summary>
   /// <typeparam name="T">The space of interest</typeparam>
   /// <returns>The known header information for that space</returns>
   public static ImageHeader? GetOrientation<T>() where T : struct, ISpace
   {
      lock (LockObj)
      {

         if (Orientations.TryGetValue(typeof(T), out ImageHeader? orientation))
         {
            return orientation;
         }
         return null;
      }
   }

   /// <summary>
   /// Initialises this space. If the space is already initialised, and the orientation is the same, nothing is done. If it is already initialised and the orientation is a mismatch, an exception is thrown.
   /// </summary>
   /// <typeparam name="T">The space to initialise</typeparam>
   /// <param name="orientation">The desired orientation</param>
   /// <exception cref="ArgumentException">An empty orientation is provided or one with an unexpected number of dimensions</exception>
   /// <exception cref="OrientationException">This is already initialised and mismatches that provided</exception>
   /// <remarks>ISpace must be a value type to avoid complicated scenarios where they derive from other space objects, and allows more efficient checks internally</remarks>
   public static void Initialise<T>(ImageHeader orientation) where T: struct, ISpace
   {
      if(orientation == null)
      {
         // Prevents not just nonsense but also the need for a IsInitialised field
         throw new ArgumentException("Orientation is empty");
      }

      Type t = typeof(T);

      lock (LockObj)
      {
         CheckIfSpace3DIsLegal();
         CheckAlignsWithParentSpace();
         CheckAlignsWithChildSpaces();

         if (Orientations.TryAdd(t, orientation))
         {
            // initialised successfully
         }
         else
         {
            // Already was initialised
            ThrowIfOrientationMismatch(orientation);
         }
      }

      static void ThrowIfOrientationMismatch(ImageHeader orientation)
      {
         if (!Matches<T>(orientation))
         {
            throw new OrientationException($"{typeof(T)} is already intialised with a differing orientation to that requested");
         }
      }

      static void ThrowIfOrientationMismatchesIgnoringVolumeCount(Type mustMatch, ImageHeader orientation)
      {
         if(Orientations.TryGetValue(mustMatch, out ImageHeader? otherOrientation))
         {
            if(!otherOrientation.As3D().Equals(orientation.As3D()))
            {
               throw new OrientationException($"The orientation of {typeof(T)} does not match the orientation of {mustMatch}");
            }
         }
      }

      void CheckAlignsWithParentSpace()
      {
         foreach (var parent in GetAncestorSpaces(t))
         {
            ThrowIfOrientationMismatchesIgnoringVolumeCount(parent, orientation);
         }
      }
      
      void CheckAlignsWithChildSpaces()
      {
         foreach (var child in GetKnownDescedentSpaces(t))
         {
            ThrowIfOrientationMismatchesIgnoringVolumeCount(child, orientation);
         }
      }

      void CheckIfSpace3DIsLegal()
      {
         if(orientation.Size.VolumeCount > 1 && default(T) is ISpace3D)
         {
            throw new OrientationException($"{t.FullName} implements {nameof(ISpace3D)} but contains more than one fourth dimension");
         }
      }
   }

   public static bool Matches<S, T>()
      where S : struct, ISpace
      where T : struct, ISpace
   {
      return Matches<T>(GetOrientation<S>());
   }
   static bool Matches<T>(ImageHeader? orientation) 
      where T: struct, ISpace
   {
      var existing = ISpace.GetOrientation<T>();
      return existing != null && existing.Equals(orientation);
   }

   /// <summary>
   /// Returns the ISpace(s) that the provided type is labelled as matching due to ISpace&lt;T&gt; 
   /// Traverses ancestors as necessary
   /// </summary>
   /// <param name="t">The type to investiage</param>
   /// <param name="parentSpace">The type it must match</param>
   /// <returns></returns>
   internal static IEnumerable<Type> GetAncestorSpaces(Type t)
   {
      Queue<Type> toTraverse = new();
      toTraverse.Enqueue(t);

      HashSet<Type> all = [];
      while(toTraverse.Count != 0)
      {
         Type cur = toTraverse.Dequeue();
         if(!all.Add(cur))
         {
            continue;
         }

         GetDirectParents(cur).Foreach(toTraverse.Enqueue);
      }

      all.Remove(t);

      return all;


      static IEnumerable<Type> GetDirectParents(Type t)
      {
         return from interfac in t.GetInterfaces()
                where interfac.IsGenericType && interfac.GetGenericTypeDefinition() == typeof(ISpace<>)
                select interfac.GenericTypeArguments.Single();
      }
   }

   /// <summary>
   /// Returns the intialised spaces that derive from the provided type
   /// </summary>
   /// <param name="t"></param>
   /// <returns></returns>
   static IEnumerable<Type> GetKnownDescedentSpaces(Type t) => Orientations.Keys.Where(a => GetAncestorSpaces(a).Contains(t));

#if DEBUG
   /// <summary>
   /// For unit testing and debugging purposes only
   /// </summary>
   /// <remarks>Do not use for other purposes. Will allow images of bad orientations to mix</remarks>
   /// <typeparam name="T"></typeparam>
   [Obsolete("Unsafe and not available in release")]
   public static void Debug_Clear<T>() where T : ISpace
   {
      lock (LockObj)
      {
         Orientations.TryRemove(typeof(T), out _);
      }
   }
   
   /// <summary>
   /// For unit testing and debugging purposes only
   /// </summary>
   /// <remarks>Do not use for other purposes. Will allow images of bad orientations to mix</remarks>
   /// <typeparam name="T"></typeparam>
   [Obsolete("Unsafe and not available in release")]
   public static void Debug_ClearAll()
   {
      lock (LockObj)
      {
         Orientations.Clear();
      }
   }
#endif

}

