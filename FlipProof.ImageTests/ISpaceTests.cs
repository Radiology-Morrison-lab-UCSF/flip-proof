using FlipProof.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.ImageTests;
[TestClass]
public  class ISpaceTests() : ImageTestsBase(55)
{

      class MySpace : ISpace { }
      class MySpace2 : ISpace<MySpace> { }
      class MySpace3 : ISpace<MySpace2> { }
      class MySpace4 : ISpace<MySpace2>, ISpace<MySpace3> { } // bad idea but for coding robustness
      class RecursiveSpace1 : ISpace<RecursiveSpace2> { }
      class RecursiveSpace2 : ISpace<RecursiveSpace3> { }
      class RecursiveSpace3 : ISpace<RecursiveSpace1> { }

   [TestInitialize]
   public override void Initialise()
   {
      base.Initialise();
#pragma warning disable CS0618 // Type or member is obsolete
      ISpace.Debug_ClearAll();
#pragma warning restore CS0618 // Type or member is obsolete
   }

   [TestMethod]
   public void CreateSecondWithCorrectHeader()
   {
      Image.ImageUInt8<TestSpace3D> im1 = GetRandom(out Torch.Tensor<byte> _);
#pragma warning disable CS0618 // Type or member is obsolete
      Image.ImageInt8<TestSpace3D> im2 = new(im1.Header with { }, new sbyte[(int)im1.Header.Size.VoxelCount]); // create with shallow clone of header
#pragma warning restore CS0618 // Type or member is obsolete

      Assert.ReferenceEquals(im1.Header, im2.Header);
   }

   [TestMethod]
   public void GetParentSpaces()
   {
      Assert.AreEqual(0, ISpace.GetAncestorSpaces(typeof(List<int>)).Count());
      Assert.AreEqual(0, ISpace.GetAncestorSpaces(typeof(MySpace)).Count());
      CollectionAssert.AreEquivalent(new Type[] { typeof(MySpace) }, ISpace.GetAncestorSpaces(typeof(MySpace2)).ToArray());
      CollectionAssert.AreEquivalent(new Type[] { typeof(MySpace), typeof(MySpace2) }, ISpace.GetAncestorSpaces(typeof(MySpace3)).ToArray());
      CollectionAssert.AreEquivalent(new Type[] { typeof(MySpace), typeof(MySpace2), typeof(MySpace3) }, ISpace.GetAncestorSpaces(typeof(MySpace4)).ToArray());
      CollectionAssert.AreEquivalent(new Type[] { typeof(RecursiveSpace2), typeof(RecursiveSpace3) }, ISpace.GetAncestorSpaces(typeof(RecursiveSpace1)).ToArray());
   }

   [TestMethod]
   public void IncompatibleSpace()
   {
      ISpace.Initialise<MySpace>(GetRandomHeader());
      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace>(GetRandomHeader()));
   }

   [TestMethod]
   public void CompatibleSpace()
   {
      var header = GetRandomHeader();
      ISpace.Initialise<MySpace>(header with { }); // send clone to avoid reference comparison
      ISpace.Initialise<MySpace>(header);
   }

   [TestMethod]
   public void IncompatibleDerivedSpace()
   {
      var header = GetRandomHeader();
      ISpace.Initialise<MySpace>(header);

      // All random
      Assert.ThrowsException<OrientationException>(()=> ISpace.Initialise<MySpace2>(GetRandomHeader()));

      // Orientation
      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace2>(header with { Orientation = GetRandomMatrix4x4() }));
      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace2>(header with { FrequencyEncodingDimension = header.FrequencyEncodingDimension == EncodingDirection.X ? EncodingDirection.Y : EncodingDirection.X }));
      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace2>(header with { PhaseEncodingDimension = header.PhaseEncodingDimension == EncodingDirection.X ? EncodingDirection.Y : EncodingDirection.X }));
      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace2>(header with { SliceEncodingDimension = header.SliceEncodingDimension == EncodingDirection.X ? EncodingDirection.Y : EncodingDirection.X }));
      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace2>(header with { CoordinateSystem = header.CoordinateSystem == CoordinateSystem.SRP ? CoordinateSystem.RAS : CoordinateSystem.SRP }));

      // Size changes are not allowed unless they are volume
      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace2>(header with { Size = new(header.Size.Volumes, header.Size.X + 1, header.Size.Y, header.Size.Z) }));
      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace2>(header with { Size = new(header.Size.Volumes, header.Size.X , header.Size.Y + 1, header.Size.Z) }));
      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace2>(header with { Size = new(header.Size.Volumes, header.Size.X, header.Size.Y, header.Size.Z + 1) }));
   }
   

   [TestMethod]
   public void IncompatibleDerivedSpace_IndirectDescendent()
   {
      var header = GetRandomHeader();
      ISpace.Initialise<MySpace>(header);

      Assert.ThrowsException<OrientationException>(()=> ISpace.Initialise<MySpace3>(GetRandomHeader()));

      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace3>(header with { Size = new(header.Size.Volumes, header.Size.X + 1, header.Size.Y, header.Size.Z) }));
      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace3>(header with { Size = new(header.Size.Volumes, header.Size.X, header.Size.Y + 1, header.Size.Z) }));
      Assert.ThrowsException<OrientationException>(() => ISpace.Initialise<MySpace3>(header with { Size = new(header.Size.Volumes, header.Size.X, header.Size.Y, header.Size.Z + 1) }));
   }
   
   [TestMethod]
   public void CompatibleDerivedSpace()
   {
      var header = GetRandomHeader();
      ISpace.Initialise<MySpace>(header with { Size = new(header.Size.Volumes+1, header.Size.X, header.Size.Y, header.Size.Z) });
      ISpace.Initialise<MySpace2>(header);
   }
   
   [TestMethod]
   public void CompatibleDerivedSpace_IndirectDescendent()
   {
      var header = GetRandomHeader();
      ISpace.Initialise<MySpace>(header with { Size = new(header.Size.Volumes + 1, header.Size.X, header.Size.Y, header.Size.Z) });
      ISpace.Initialise<MySpace3>(header);
   }
}
