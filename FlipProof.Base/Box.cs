using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace FlipProof.Base;

/// <summary>
/// 3D box
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct Box<T> where T : INumber<T>
{
   /// <summary>
   /// Corner with smallest x, y and z
   /// </summary>
   public XYZ<T> Origin { get; }
   public XYZ<T> Size { get; }

   /// <summary>
   ///  Corner with largest x, y and z
   /// </summary>
   public readonly XYZ<T> FarCorner => Origin + Size;

   public Box(XYZ<T> origin, XYZ<T> size)
   {
      HandleNegativeSize(origin.X, size.X, out T xLeft, out T xSize);
      HandleNegativeSize(origin.Y, size.Y, out T yLeft, out T ySize);
      HandleNegativeSize(origin.Z, size.Z, out T zLeft, out T zSize);

      Origin = new(xLeft, yLeft, zLeft);
      Size = new(xSize, ySize, zSize);


      static void HandleNegativeSize(T origin, T size, out T left, out T xSize)
      {
         if (size < T.Zero)
         {
            left = origin + size;
            xSize = -size;
         }
         else
         {
            left = origin;
            xSize = size;
         }
      }
   }

   public bool IsWithin(T xOrig, T yOrig, T zOrig)
   {
      var right = FarCorner;
      return xOrig >= Origin.X && xOrig < right.X &&
      yOrig >= Origin.Y && yOrig < right.Y &&
      zOrig >= Origin.Z && zOrig < right.Z;
   }
}
