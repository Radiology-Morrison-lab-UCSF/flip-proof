using FlipProof.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.ImageTests;
public class TestSpace4D : ISpace
{
   public static object LockObj { get; } = new object();

   public static int NDims => 4;

   static ImageHeader? ISpace.Orientation { get; set; }

}

public class TestSpace3D : ISpace
{
   public static object LockObj { get; } = new object();

   public static int NDims => 3;

   static ImageHeader? ISpace.Orientation { get; set; }
}
