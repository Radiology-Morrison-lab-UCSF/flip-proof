using FlipProof.Base;

namespace FlipProof.Image.Matrices;

internal class ITKTransformReaderWriter
{
   public static Matrix4x4_Optimised<float> FromITKAffineTransform_f(string loc, out XYZf fixedParameters)
   {
      using BinaryReader br = new(new FileStream(loc, FileMode.Open, FileAccess.Read, FileShare.Read));
      for (char read = '\0'; read != "AffineTransform_float_3_3"[0]; read = (char)br.ReadByte())
      {
      }
      if (!(from a in br.ReadBytes("AffineTransform_float_3_3".Length - 1)
            select (char)a).ToArray().SequenceEqual("AffineTransform_float_3_3".Substring(1)))
      {
         throw new IOException("File was not AffineTransform_double_3_3");
      }
      br.ReadByte();
      Matrix4x4_Optimised<float> mat = new Matrix4x4_Optimised<float>
      {
         _0_0 = br.ReadSingle(),
         _0_1 = br.ReadSingle(),
         _0_2 = br.ReadSingle(),
         _1_0 = br.ReadSingle(),
         _1_1 = br.ReadSingle(),
         _1_2 = br.ReadSingle(),
         _2_0 = br.ReadSingle(),
         _2_1 = br.ReadSingle(),
         _2_2 = br.ReadSingle(),
         _3_3 = 1f
      };
      float k = br.ReadSingle();
      float l = br.ReadSingle();
      float m = br.ReadSingle();
      float[] m_Translation = [k, l, m];
      for (char read = '\0'; read != 'f'; read = (char)br.ReadByte())
      {
      }
      if (!(from a in br.ReadBytes("fixed".Length - 1)
            select (char)a).ToArray().SequenceEqual("ixed"))
      {
         throw new IOException("Could not find 'fixed' params in file");
      }
      br.ReadByte();
      float fixedX = br.ReadSingle();
      float fixedY = br.ReadSingle();
      float fixedZ = br.ReadSingle();
      float[] m_Centre = [fixedX, fixedY, fixedZ];
      float[] offset = new float[3];
      for (int i = 0; i < 3; i++)
      {
         offset[i] = m_Translation[i] + m_Centre[i];
         for (int j = 0; j < 3; j++)
         {
            offset[i] -= mat.At(i, j) * m_Centre[j];
         }
      }
      mat._0_3 = offset[0];
      mat._1_3 = offset[1];
      mat._2_3 = offset[2];
      fixedParameters = new XYZf(fixedX, fixedY, fixedZ);
      return mat;
   }

   public static Matrix4x4_Optimised<double> ReadITKAffineTransform_d(string loc, out XYZ<double> fixedParameters)
   {
      // Weird code here for "AffineTransform_double_3_3" due to code going through
      // code decompilation
      using BinaryReader br = new(new FileStream(loc, FileMode.Open, FileAccess.Read, FileShare.Read));
      for (char read = '\0'; read != "AffineTransform_double_3_3"[0]; read = (char)br.ReadByte())
      {
      }
      if (!(from a in br.ReadBytes("AffineTransform_double_3_3".Length - 1)
            select (char)a).ToArray().SequenceEqual("AffineTransform_double_3_3".Substring(1)))
      {
         throw new IOException("File was not AffineTransform_double_3_3");
      }
      br.ReadByte();
      Matrix4x4_Optimised<double> mat = new()
      {
         _0_0 = br.ReadDouble(),
         _0_1 = br.ReadDouble(),
         _0_2 = br.ReadDouble(),
         _1_0 = br.ReadDouble(),
         _1_1 = br.ReadDouble(),
         _1_2 = br.ReadDouble(),
         _2_0 = br.ReadDouble(),
         _2_1 = br.ReadDouble(),
         _2_2 = br.ReadDouble(),
         _3_3 = 1.0
      };
      double k = br.ReadDouble();
      double l = br.ReadDouble();
      double m = br.ReadDouble();
      double[] m_Translation = [k, l, m];
      for (char read = '\0'; read != 'f'; read = (char)br.ReadByte())
      {
      }
      if (!(from a in br.ReadBytes("fixed".Length - 1)
            select (char)a).ToArray().SequenceEqual("ixed"))
      {
         throw new IOException("Could not find 'fixed' params in file");
      }
      br.ReadByte();
      double fixedX = br.ReadDouble();
      double fixedY = br.ReadDouble();
      double fixedZ = br.ReadDouble();
      double[] m_Centre = [fixedX, fixedY, fixedZ];
      double[] offset = new double[3];
      for (int i = 0; i < 3; i++)
      {
         offset[i] = m_Translation[i] + m_Centre[i];
         for (int j = 0; j < 3; j++)
         {
            offset[i] -= mat.At(i, j) * m_Centre[j];
         }
      }
      mat._0_3 = offset[0];
      mat._1_3 = offset[1];
      mat._2_3 = offset[2];
      fixedParameters = new XYZ<double>(fixedX, fixedY, fixedZ);
      return mat;
   }

   public static Matrix4x4_Optimised<double> ReadITKAffineTransform(string loc, Matrix4x4_Optimised<double> mat, XYZ<double> fixedParameters)
   {
      using BinaryWriter br = new(new FileStream(loc, FileMode.OpenOrCreate));
      byte[] start =
      [
         0, 0, 0, 0, 12, 0, 0, 0, 1, 0,
         0, 0, 0, 0, 0, 0, 27, 0, 0, 0,
         65, 102, 102, 105, 110, 101, 84, 114, 97, 110,
         115, 102, 111, 114, 109, 95, 100, 111, 117, 98,
         108, 101, 95, 51, 95, 51, 0
      ];
      br.Write(start);
      br.Write(mat._0_0);
      br.Write(mat._0_1);
      br.Write(mat._0_2);
      br.Write(mat._1_0);
      br.Write(mat._1_1);
      br.Write(mat._1_2);
      br.Write(mat._2_0);
      br.Write(mat._2_1);
      br.Write(mat._2_2);
      double[] offset = [mat._0_3, mat._1_3, mat._2_3];
      for (int i = 0; i < 3; i++)
      {
         double translation_i = offset[i] - fixedParameters[i];
         for (int j = 0; j < 3; j++)
         {
            translation_i += mat.At(i, j) * fixedParameters[j];
         }
         br.Write(translation_i);
      }
      br.Write(new byte[20]
      {
         0, 0, 0, 0, 3, 0, 0, 0, 1, 0,
         0, 0, 0, 0, 0, 0, 6, 0, 0, 0
      });
      br.Write("fixed\0".Select((char a) => (byte)a).ToArray());
      br.Write(fixedParameters.X);
      br.Write(fixedParameters.Y);
      br.Write(fixedParameters.Z);
      return mat;
   }
}
