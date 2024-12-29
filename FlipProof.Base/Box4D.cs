using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace FlipProof.Base;

/// <summary>
/// 4D box
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly record struct Box4D<T> where T : INumber<T>
{
   /// <summary>
   /// Corner with smallest x, y, z, and volume
   /// </summary>
   public XYZA<T> Origin { get; }
   public XYZA<T> Size { get; }

   /// <summary>
   ///  Corner with largest x, y, z and volume
   /// </summary>
   public readonly XYZA<T> FarCorner => Origin + Size;

   public Box4D(XYZA<T> origin, XYZA<T> size)
   {
      HandleNegativeSize(origin.X, size.X, out T xLeft, out T xSize);
      HandleNegativeSize(origin.Y, size.Y, out T yLeft, out T ySize);
      HandleNegativeSize(origin.Z, size.Z, out T zLeft, out T zSize);
      HandleNegativeSize(origin.A, size.A, out T aLeft, out T aSize);

      Origin = new(xLeft, yLeft, zLeft, aLeft);
      Size = new(xSize, ySize, zSize, aSize);


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

   /// <summary>
   /// Calculates padding (or cropping, if negative) needed to convert this box into the provided box
   /// </summary>
   /// <param name="toMake">The result after padding</param>
   /// <param name="xB4">Padding to add before</param>
   /// <param name="xAfter">Padding to add after</param>
   /// <param name="yB4">Padding to add before</param>
   /// <param name="yAfter">Padding to add after</param>
   /// <param name="zB4">Padding to add before</param>
   /// <param name="zAfter">Padding to add after</param>
   /// <param name="aB4">Padding to add before</param>
   /// <param name="aAfter">Padding to add after</param>
   public void CalcPadding(Box4D<T> toMake, out T xB4, out T xAfter, out T yB4, out T yAfter, out T zB4, out T zAfter, out T aB4, out T aAfter)
   {
      (xB4, xAfter) = CalcPad(this.Origin.X, this.Size.X, toMake.Origin.X, toMake.Size.X);
      (yB4, yAfter) = CalcPad(this.Origin.Y, this.Size.Y, toMake.Origin.Y, toMake.Size.Y);
      (zB4, zAfter) = CalcPad(this.Origin.Z, this.Size.Z, toMake.Origin.Z, toMake.Size.Z);
      (aB4, aAfter) = CalcPad(this.Origin.A, this.Size.A, toMake.Origin.A, toMake.Size.A);

      static (T before, T after) CalcPad(T origStart, T origSize, T newStart, T newSize)
      {
         T before = origStart - newStart;
         T after = newSize - origSize - before;
         return new(before, after);
      }
   }

   /// <summary>
   /// Pads or crops this box
   /// </summary>
   /// <param name="xB4">Added before this box</param>
   /// <param name="xAfter">Added after this box</param>
   /// <param name="yB4">Added before this box</param>
   /// <param name="yAfter">Added after this box</param>
   /// <param name="zB4">Added before this box</param>
   /// <param name="zAfter">Added after this box</param>
   /// <param name="aB4">Added before this box</param>
   /// <param name="aAfter">Added after this box</param>
   /// <returns>A new box of the new size</returns>
   public Box4D<T> Pad(T xB4, T xAfter, T yB4, T yAfter, T zB4, T zAfter, T aB4, T aAfter)
   {
      XYZA<T> origin = new(Origin.X - xB4, Origin.Y - yB4, Origin.Z - zB4, Origin.A - aB4);
      XYZA<T> size = new(Size.X + xB4 + xAfter, Size.Y + yB4 + yAfter, Size.Z + zB4 + zAfter, Size.A + aB4 + aAfter);
      return new(origin, size);
   }

   public bool IsWithin(T xOrig, T yOrig, T zOrig, T aOrig)
   {
      var right = FarCorner;
      return xOrig >= Origin.X && xOrig < right.X &&
      yOrig >= Origin.Y && yOrig < right.Y &&
      zOrig >= Origin.Z && zOrig < right.Z &&
      aOrig >= Origin.A && aOrig < right.A;
   }

   public static explicit operator Box<T>(Box4D<T> box) => new(box.Origin, box.Size);
}
