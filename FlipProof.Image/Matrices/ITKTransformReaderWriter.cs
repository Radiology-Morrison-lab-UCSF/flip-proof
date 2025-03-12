using FlipProof.Base;

namespace FlipProof.Image.Matrices;

internal class ITKTransformReaderWriter
{
   public static Matrix4x4_Optimised<float> FromITKAffineTransform_f(string loc, out XYZ<float> fixedParameters)
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
         M11 = br.ReadSingle(),
         M12 = br.ReadSingle(),
         M13 = br.ReadSingle(),
         M21 = br.ReadSingle(),
         M22 = br.ReadSingle(),
         M23 = br.ReadSingle(),
         M31 = br.ReadSingle(),
         M32 = br.ReadSingle(),
         M33 = br.ReadSingle(),
         M44 = 1f
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
      mat.M14 = offset[0];
      mat.M24 = offset[1];
      mat.M34 = offset[2];
      fixedParameters = new XYZ<float>(fixedX, fixedY, fixedZ);
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
         M11 = br.ReadDouble(),
         M12 = br.ReadDouble(),
         M13 = br.ReadDouble(),
         M21 = br.ReadDouble(),
         M22 = br.ReadDouble(),
         M23 = br.ReadDouble(),
         M31 = br.ReadDouble(),
         M32 = br.ReadDouble(),
         M33 = br.ReadDouble(),
         M44 = 1.0
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
      mat.M14 = offset[0];
      mat.M24 = offset[1];
      mat.M34 = offset[2];
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
      br.Write(mat.M11);
      br.Write(mat.M12);
      br.Write(mat.M13);
      br.Write(mat.M21);
      br.Write(mat.M22);
      br.Write(mat.M23);
      br.Write(mat.M31);
      br.Write(mat.M32);
      br.Write(mat.M33);
      double[] offset = [mat.M14, mat.M24, mat.M34];
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
