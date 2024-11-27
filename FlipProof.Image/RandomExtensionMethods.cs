namespace FlipProof.Image;
/// <summary>
/// Extension methods for <see cref="Random"/>
/// </summary>
public static class RandomExtensionMethods
{
   [Obsolete("Header is checked at run time. Use DeepClone and FillWithRandom on an existing image instead to use compile-time-checks where possible")]
   public static ImageDouble<TSpace> GetRandomImage<TSpace>(this Random r, ImageHeader head)
      where TSpace : ISpace
   {
      double[] voxels = r.GetRandomDoubleVoxels(head);
      return new ImageDouble<TSpace>(head, voxels);
   }

   public static double[] GetRandomDoubleVoxels(this Random r, ImageHeader head)
   {
      double[] arr = new double[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = r.NextDouble();
      }
      return arr;
   }

   public static float[] GetRandomFloatVoxels(this Random r, ImageHeader head)
   {
      float[] arr = new float[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = r.NextSingle();
      }
      return arr;
   }

   public static Int64[] GetRandomInt64Voxels(this Random r, ImageHeader head)
   {
      Int64[] arr = new Int64[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = r.NextInt64();
      }
      return arr;
   }
   public static Int32[] GetRandomInt32Voxels(this Random r, ImageHeader head)
   {
      Int32[] arr = new Int32[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = r.Next();
      }
      return arr;
   }
   public static Int16[] GetRandomInt16Voxels(this Random r, ImageHeader head)
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

   [CLSCompliant(false)]
   public static sbyte[] GetRandomSByteVoxels(this Random r, ImageHeader head) => GetRandomInt8Voxels(r, head);

   [CLSCompliant(false)]
   public static sbyte[] GetRandomInt8Voxels(this Random r, ImageHeader head)
   {
      sbyte[] arr = new sbyte[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = (sbyte)r.Next(sbyte.MinValue, sbyte.MaxValue + 1);
      }
      return arr;
   }
   public static byte[] GetRandomUInt8Voxels(this Random r, ImageHeader head)
   {
      byte[] arr = new byte[head.Size.VoxelCount];
      r.NextBytes(arr);
      return arr;
   }


   [CLSCompliant(false)]
   public static bool[] GetRandomBoolVoxels(this Random r, ImageHeader head)
   {
      bool[] arr = new bool[head.Size.VoxelCount];
      for (int i = 0; i < arr.Length; i++)
      {
         arr[i] = r.Next(2) == 0;
      }
      return arr;
   }
}
