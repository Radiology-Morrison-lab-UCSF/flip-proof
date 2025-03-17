using FlipProof.Image;
using FlipProof.Image.Matrices;
using FlipProof.Torch;
using SkiaSharp;
using System;
using System.Numerics;
using TorchSharp;
using static System.Formats.Asn1.AsnWriter;
using static TorchSharp.torch;

namespace FlipProof.ImageTests;

public abstract class ImageTestsBase(int seed)
{
  readonly Random r = new(seed);


   [TestInitialize]
   public virtual void Initialise()
   {
#if DEBUG
#pragma warning disable CS0618 // Type or member is obsolete
      ISpace.Debug_Clear<TestSpace3D>();
#pragma warning restore CS0618 // Type or member is obsolete
#else
      throw new Exception("Tests require clearing the space, which is not availabe in Release mode");
#endif
   }


   protected ImageDouble<TestSpace3D> GetRandom(long minImSizeEachDim, out Tensor<double> voxels) => GetRandom(GetRandomHeader(minImSizeEachDim), out voxels);
   protected ImageDouble<TestSpace3D> GetRandom(out Tensor<double> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageDouble<TestSpace3D> GetRandom(ImageHeader head, out Tensor<double> voxels)
   {
      var data = r.GetRandomDoubleVoxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageDouble<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.Data;
      return im;
   }
   protected ImageDouble<TestSpace3D> GetRandom(ImageHeader head, out double[] data)
   {
      data = r.GetRandomDoubleVoxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageDouble<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   protected ImageFloat<TestSpace3D> GetRandom(out Tensor<float> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageFloat<TestSpace3D> GetRandom(ImageHeader head, out Tensor<float> voxels)
   {
      var data = r.GetRandomFloatVoxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageFloat<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.Data;
      return im;
   }
   
   protected ImageFloat<TestSpace3D> GetRandom(ImageHeader head, out float[] data)
   {
      data = r.GetRandomFloatVoxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageFloat<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   protected ImageInt64<TestSpace3D> GetRandom(out Tensor<Int64> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageInt64<TestSpace3D> GetRandom(ImageHeader head, out Tensor<Int64> voxels)
   {
      var data = r.GetRandomInt64Voxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageInt64<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.Data;
      return im;
   }
   
   protected ImageInt64<TestSpace3D> GetRandom(ImageHeader head, out Int64[] data)
   {
      data = r.GetRandomInt64Voxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   protected ImageInt32<TestSpace3D> GetRandom(out Tensor<Int32> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageInt32<TestSpace3D> GetRandom(ImageHeader head, out Tensor<Int32> voxels)
   {
      var data = r.GetRandomInt32Voxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageInt32<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.Data;
      return im;
   }

   protected ImageInt32<TestSpace3D> GetRandom(ImageHeader head, out Int32[] data)
   {
      data = r.GetRandomInt32Voxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt32<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   protected ImageInt16<TestSpace3D> GetRandom(out Tensor<Int16> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageInt16<TestSpace3D> GetRandom(ImageHeader head, out Tensor<Int16> voxels)
   {
      var data = r.GetRandomInt16Voxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageInt16<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.Data;
      return im;
   }

   protected ImageInt16<TestSpace3D> GetRandom(ImageHeader head, out Int16[] data)
   {
      data = r.GetRandomInt16Voxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt16<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   protected ImageInt8<TestSpace3D> GetRandom(out Tensor<sbyte> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageInt8<TestSpace3D> GetRandom(ImageHeader head, out Tensor<sbyte> voxels)
   {
      var data = r.GetRandomInt8Voxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageInt8<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.Data;
      return im;
   }

   protected ImageInt8<TestSpace3D> GetRandom(ImageHeader head, out sbyte[] data)
   {
      data = r.GetRandomInt8Voxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt8<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   protected ImageUInt8<TestSpace3D> GetRandom(out Tensor<byte> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageUInt8<TestSpace3D> GetRandom(ImageHeader head, out Tensor<byte> voxels)
   {
      var data = r.GetRandomUInt8Voxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageUInt8<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.Data;
      return im;
   }
   protected ImageUInt8<TestSpace3D> GetRandom(ImageHeader head, out byte[] data)
   {
      data = r.GetRandomUInt8Voxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageUInt8<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   
   protected ImageBool<TestSpace3D> GetRandom(ImageHeader head, out bool[] data)
   {
      data = r.GetRandomBoolVoxels(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageBool<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }



   protected ImageHeader GetRandomHeader(long minImSizeEachDim=2)
   {

      return new ImageHeader(new ImageSize((uint)r.NextInt64(minImSizeEachDim, 100), (uint)r.NextInt64(minImSizeEachDim, 100), (uint)r.NextInt64(minImSizeEachDim, 100), 1),
                             new OrientationMatrix(GetRandomMatrix4x4()),
                             CoordinateSystem.RAS,
                             NextEncodingDir(),
                             NextEncodingDir(),
                             NextEncodingDir());

      EncodingDirection NextEncodingDir() => r.Next(3) switch
      {
         0 => EncodingDirection.X,
         1 => EncodingDirection.Y,
         2 => EncodingDirection.Z,
         _ => throw new NotSupportedException()
      };

   }

   protected Matrix4x4_Optimised<double> GetRandomMatrix4x4() => GetRandomMatrix4x4(r);
   internal static Matrix4x4_Optimised<double> GetRandomMatrix4x4(Random r)
   {
      return new(r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble(),
         r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble(),
         r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble(),
         0,0,0, 1);
   }
}
