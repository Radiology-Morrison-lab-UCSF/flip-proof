using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Numerics;
using FlipProof.Base;
using FlipProof.Image.IO;

namespace FlipProof.Image.Nifti;

public static class NiftiWriter
{
	public static bool Write(NiftiFile_Base file, string loc, FileMode fm, [NotNullWhen(false)] out string? err)
	{
		try
		{
			Write(file, loc, fm);
			err = null;
			return true;
		}
		catch (Exception e)
		{
			err = e.ToString();
			return false;
		}
	}

	public static void Write(NiftiFile_Base file, string loc, FileMode fm)
	{
		if (loc.EndsWith(".gz"))
		{
			Write_Gz(file, loc, fm);
			return;
		}
		using FileStream fs = new FileStream(loc, fm);
		using BinaryWriter br = new BinaryWriter(fs);
		Write_Sub(file, fs, br);
	}

	public static void Write_Gz(NiftiFile_Base file, string loc, FileMode fm)
	{
		using MemoryStream ms = new MemoryStream();
		using BinaryWriter br = new BinaryWriter(ms);
		Write_Sub(file, ms, br);
		ms.Position = 0L;
		if (!Gen.Zip_gz(ms, loc, allowOverwrite: true, returnErrIfExistsAndNoOverwrite: true, out var err))
		{
			throw new Exception(err);
		}
	}

	private static void Write_Sub(NiftiFile_Base file, Stream fs, BinaryWriter br)
	{
		if (file is NiftiFile<bool>)
		{
			throw new NotSupportedException("Not suppored. Convert to byte format first");
		}
		br.Write(348);
		br.Write(new byte[28]);
		br.Write(16384);
		br.Write((short)0);
		br.Write('r');
		NiftiHeader nh = file.Head;
		br.Write(file.Head.dimInfo);
		for (int j = 0; j < 8; j++)
		{
			br.Write(file.Head.DataArrayDims[j]);
		}
		br.Write(nh.intentParam1);
		br.Write(nh.intentParam2);
		br.Write(nh.intentParam3);
		br.Write((short)nh.intent);
		br.Write((short)nh.dataType);
		br.Write(nh.bitPix);
		br.Write(nh.sliceStart);
		br.WriteFromArray(nh.PixDim, 8);
		br.Write(0f);
		br.Write(nh.sclSlope);
		br.Write(nh.scl_inter);
		br.Write(nh.sliceLast);
		br.Write(nh.sliceCode);
		br.Write((byte)nh.xyztUnits);
		br.Write(nh.calMax);
		br.Write(nh.calMin);
		br.Write(nh.sliceDuration);
		br.Write(nh.tOffset);
		int max = 0;
		int min = 0;
		br.Write(max);
		br.Write(min);
		br.Write(PadString(nh.Description, 80));
		string aux = nh.AuxFile_Trimmed;
		br.Write(PadString(string.IsNullOrWhiteSpace(aux) ? "" : Path.GetFileName(aux), 24));
		br.Write((short)nh.qFormCode);
		br.Write((short)nh.sFormCode);
		br.Write(nh.quartern_b);
		br.Write(nh.quartern_c);
		br.Write(nh.quartern_d);
		br.Write(nh.quartern_x);
		br.Write(nh.quartern_y);
		br.Write(nh.quartern_z);
		br.WriteFromArray(nh.Srow_x, 4);
		br.WriteFromArray(nh.Srow_y, 4);
		br.WriteFromArray(nh.Srow_z, 4);
		br.Write(PadString(nh.intentName, 16));
		br.Write(NiftiHeader.Magic_singlefile.ToArray());
		if (nh.HeaderExtras.Any())
		{
			for (int i = 0; i < nh.HeaderExtras.Count; i++)
			{
				br.Write((byte)1);
				br.Write((byte)0);
				br.Write((byte)0);
				br.Write((byte)0);
				KeyValuePair<HeaderExtraType, string> cur = nh.HeaderExtras[i];
				int esize = RoundUp(cur.Value.Length + 8, 16);
				int eCode = (int)cur.Key;
				br.Write(esize);
				br.Write(eCode);
				br.Write(PadString(cur.Value, esize - 8));
			}
		}
		else
		{
			byte[] paddingToOffset = new byte[4];
			br.Write(paddingToOffset);
		}
		long startOfImageData = fs.Position;
		float voxelOffset = startOfImageData;
		fs.Position = 108L;
		br.Write(voxelOffset);
		fs.Position = startOfImageData;
		br.Flush();
		Stream dataStream = file.GetDataStream();
		dataStream.Position = 0L;
		dataStream.CopyTo(fs);
	}

	private static char[] PadString(string s, int length)
	{
		string st;
		if (s == null)
		{
			st = new string('\0', length);
		}
		else if (s.Length < length)
		{
			string pad = new string('\0', length - s.Length);
			st = s + pad;
		}
		else
		{
			st = s;
		}
		return st.ToArray();
	}

	private static char[] UnpadString(string s)
	{
		char zero = '\0';
		return s.SkipWhile((char a) => a == zero).TakeWhile((char a) => a != zero).ToArray();
	}

	public static int RoundUp(int numToRound, int multiple)
	{
		if (multiple == 0)
		{
			return numToRound;
		}
		int remainder = numToRound % multiple;
		if (remainder == 0)
		{
			return numToRound;
		}
		return numToRound + multiple - remainder;
	}
}
public static class NiftiWriter<T> where T : struct, INumber<T>
{

	public static void Write<TSpace>(Image<T, TSpace> file, FilePath loc, FileMode fm)
	where TSpace : struct, ISpace
	{
		using var nii = NiftiFile<T>.FromImage(file);
      NiftiWriter.Write(nii, loc, fm);
	}

	private static bool IsNaNOrOutsideIntRange(object val)
	{
		if (typeof(T) == typeof(float))
		{
			float asf = (float)val;
			if (!float.IsNaN(asf))
			{
				return asf > 2.1474836E+09f;
			}
			return true;
		}
		if (typeof(T) == typeof(double))
		{
			double asD = (double)val;
			if (!double.IsNaN(asD))
			{
				return asD > 2147483647.0;
			}
			return true;
		}
		if (typeof(T) == typeof(long))
		{
			return (long)val > int.MaxValue;
		}
		if (typeof(T) == typeof(uint))
		{
			return (uint)val > int.MaxValue;
		}
		if (typeof(T) == typeof(ulong))
		{
			return (ulong)val > int.MaxValue;
		}
		return false;
	}

	private static void WriteFromArray(BinaryWriter br, bool[] data, int noBooleans)
	{
		if (noBooleans % 8 != 0)
		{
			throw new Exception("Number must be divible by 8 or filestream can't work properly - it always steps in bytes");
		}
		byte[] byteArray = new byte[noBooleans / 8];
		int i = 0;
		int j = 0;
		while (i < noBooleans)
		{
			byte b = 0;
			if (data[i])
			{
				b = (byte)(b | 0x80u);
			}
			if (data[i + 1])
			{
				b = (byte)(b | 0x40u);
			}
			if (data[i + 2])
			{
				b = (byte)(b | 0x20u);
			}
			if (data[i + 3])
			{
				b = (byte)(b | 0x10u);
			}
			if (data[i + 4])
			{
				b = (byte)(b | 8u);
			}
			if (data[i + 5])
			{
				b = (byte)(b | 4u);
			}
			if (data[i + 6])
			{
				b = (byte)(b | 2u);
			}
			if (data[i + 7])
			{
				b = (byte)(b | 1u);
			}
			byteArray[j] = b;
			i += 8;
			j++;
		}
		br.Write(byteArray);
	}

	private static void WriteFromArray(BinaryWriter br, T[] data, int length)
	{
		if (data is bool[] b)
		{
			WriteFromArray(br, b, length);
		}
		else if (data is byte[] by && data.Length == length)
		{
			br.Write(by);
		}
		else
		{
			br.WriteFromArray(data, length);
		}
	}
}
