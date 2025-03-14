﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using FlipProof.Image.Matrices;
using static FlipProof.Image.ImageHeader;
namespace FlipProof.Image;
public interface IImageHeader
{
   EncodingDirection PhaseEncodingDimension { get; }
   EncodingDirection FrequencyEncodingDimension { get; }
   EncodingDirection SliceEncodingDimension { get; }
   ImageSize Size { get; }
   /// <summary>
   /// Returns a readonly copy of the the orientation
   /// </summary>
   /// <remarks></remarks>
   IReadOnlyOrientation Orientation { get; }
   CoordinateSystem CoordinateSystem { get;}

   /// <summary>
   /// Returns true if the headers match within tolerances
   /// </summary>
   /// <remarks>Tolerance is defined as 1/10000th of the (smallest) voxel-size</remarks>
   /// <param name="other">The second header to compare</param>
   /// <returns>true if the headers match within tolerances</returns>
   public bool IsSameSpaceAs(IImageHeader other)
   {
      return this.Size.Equals(other.Size) &&
         Orientation.TolerantEquals(other.Orientation,Size);
   }

   ///// <summary>
   ///// Creates a new image header of the same type as this based on another
   ///// </summary>
   ///// <param name="other">Copies information from this</param>
   ///// <returns>A new imageheader of the same type as this</returns>
   //public IImageHeader Create(IImageHeader other);


   /// <summary>
   /// Attempts to return the dimension size in the slice encoding direction
   /// </summary>
   /// <returns></returns>
   public bool TryGetSize_SliceEncodingDirection(out long size)
   {
      size = SliceEncodingDimension switch
      {
         EncodingDirection.X => Size.X,
         EncodingDirection.Y => Size.Y,
         EncodingDirection.Z => Size.Z,
         _ => 0
      };

      return size != 0;
   }
}
