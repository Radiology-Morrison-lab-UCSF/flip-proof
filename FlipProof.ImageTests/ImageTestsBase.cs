using FlipProof.Image;
using FlipProof.Torch;
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
      ISpace.Debug_Clear<TestSpace3D>();
   }


   protected ImageDouble<TestSpace3D> GetRandom(out Tensor<double> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageDouble<TestSpace3D> GetRandom(ImageHeader head, out Tensor<double> voxels)
   {
      var data = GetRandomDoubles(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageDouble<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.GetVoxelTensor();
      return im;
   }
   protected ImageDouble<TestSpace3D> GetRandom(ImageHeader head, out double[] data)
   {
      data = GetRandomDoubles(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageDouble<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   protected ImageFloat<TestSpace3D> GetRandom(out Tensor<float> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageFloat<TestSpace3D> GetRandom(ImageHeader head, out Tensor<float> voxels)
   {
      var data = GetRandomFloats(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageFloat<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.GetVoxelTensor();
      return im;
   }
   
   protected ImageFloat<TestSpace3D> GetRandom(ImageHeader head, out float[] data)
   {
      data = GetRandomFloats(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageFloat<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   protected ImageInt64<TestSpace3D> GetRandom(out Tensor<Int64> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageInt64<TestSpace3D> GetRandom(ImageHeader head, out Tensor<Int64> voxels)
   {
      var data = GetRandomInt64s(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageInt64<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.GetVoxelTensor();
      return im;
   }
   
   protected ImageInt64<TestSpace3D> GetRandom(ImageHeader head, out Int64[] data)
   {
      data = GetRandomInt64s(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt64<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   protected ImageInt32<TestSpace3D> GetRandom(out Tensor<Int32> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageInt32<TestSpace3D> GetRandom(ImageHeader head, out Tensor<Int32> voxels)
   {
      var data = GetRandomInt32s(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageInt32<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.GetVoxelTensor();
      return im;
   }

   protected ImageInt32<TestSpace3D> GetRandom(ImageHeader head, out Int32[] data)
   {
      data = GetRandomInt32s(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt32<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   protected ImageInt16<TestSpace3D> GetRandom(out Tensor<Int16> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageInt16<TestSpace3D> GetRandom(ImageHeader head, out Tensor<Int16> voxels)
   {
      var data = GetRandomInt16s(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageInt16<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.GetVoxelTensor();
      return im;
   }

   protected ImageInt16<TestSpace3D> GetRandom(ImageHeader head, out Int16[] data)
   {
      data = GetRandomInt16s(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt16<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   protected ImageInt8<TestSpace3D> GetRandom(out Tensor<sbyte> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageInt8<TestSpace3D> GetRandom(ImageHeader head, out Tensor<sbyte> voxels)
   {
      var data = GetRandomInt8s(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageInt8<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.GetVoxelTensor();
      return im;
   }

   protected ImageInt8<TestSpace3D> GetRandom(ImageHeader head, out sbyte[] data)
   {
      data = GetRandomInt8s(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageInt8<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }
   protected ImageUInt8<TestSpace3D> GetRandom(out Tensor<byte> voxels) => GetRandom(GetRandomHeader(), out voxels);
   protected ImageUInt8<TestSpace3D> GetRandom(ImageHeader head, out Tensor<byte> voxels)
   {
      var data = GetRandomUInt8s(head);
#pragma warning disable CS0618 // Type or member is obsolete
      var im = new ImageUInt8<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
      voxels = im.GetVoxelTensor();
      return im;
   }
   protected ImageUInt8<TestSpace3D> GetRandom(ImageHeader head, out byte[] data)
   {
      data = GetRandomUInt8s(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageUInt8<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   
   protected ImageBool<TestSpace3D> GetRandom(ImageHeader head, out bool[] data)
   {
      data = GetRandomBools(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageBool<TestSpace3D>(head, data);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   protected double[] GetRandomDoubles(ImageHeader head)
   {
      double[] arr = new double[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = r.NextDouble();
      }
      return arr;
   }
   
   protected float[] GetRandomFloats(ImageHeader head)
   {
      float[] arr = new float[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = r.NextSingle();
      }
      return arr;
   }
   
   protected Int64[] GetRandomInt64s(ImageHeader head)
   {
      Int64[] arr = new Int64[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = r.NextInt64();
      }
      return arr;
   }   
   protected Int32[] GetRandomInt32s(ImageHeader head)
   {
      Int32[] arr = new Int32[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = r.Next();
      }
      return arr;
   }
   protected Int16[] GetRandomInt16s(ImageHeader head)
   {
      Int16[] arr = new Int16[head.Size.VoxelCount];
      byte[] buffer = new byte[2];
      for (int i = 0; i < arr.Length; i++)
      {
         r.NextBytes(buffer);
         arr[i] = BitConverter.ToInt16(buffer);
      }
      return arr;
   }   
   
   protected sbyte[] GetRandomInt8s(ImageHeader head)
   {
      sbyte[] arr = new sbyte[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = (sbyte)r.Next(sbyte.MinValue, sbyte.MaxValue + 1);
      }
      return arr;
   }
   protected byte[] GetRandomUInt8s(ImageHeader head)
   {
      byte[] arr = new byte[head.Size.VoxelCount];
      r.NextBytes(arr);
      return arr;
   }
      
   protected sbyte[] GetRandomSBytes(ImageHeader head)
   {
      sbyte[] arr = new sbyte[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] =(sbyte)r.Next(sbyte.MinValue, sbyte.MaxValue+1);
      }
      return arr;
   }
   
   protected bool[] GetRandomBools(ImageHeader head)
   {
      bool[] arr = new bool[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = r.Next(2) == 0;
      }
      return arr;
   }

   protected ImageHeader GetRandomHeader()
   {

      return new ImageHeader(new ImageSize(1, (uint)r.NextInt64(100), (uint)r.NextInt64(100), (uint)r.NextInt64(100)),
                             GetRandomMatrix4x4(r),
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

   protected static Matrix4x4 GetRandomMatrix4x4(Random r)
   {
      return new Matrix4x4(r.NextSingle(), r.NextSingle(), r.NextSingle(), r.NextSingle(),
         r.NextSingle(), r.NextSingle(), r.NextSingle(), r.NextSingle(),
         r.NextSingle(), r.NextSingle(), r.NextSingle(), r.NextSingle(),
         r.NextSingle(), r.NextSingle(), r.NextSingle(), r.NextSingle());
   }
}
