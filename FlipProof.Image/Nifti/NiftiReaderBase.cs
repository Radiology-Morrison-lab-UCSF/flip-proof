using System;
using System.Collections;
using System.IO;
using System.Linq;
using FlipProof.Image.IO;

namespace FlipProof.Image.Nifti;

public abstract class NiftiReaderBase : IDisposable
{
	protected Stream s;

	protected BinaryReader br;

	protected bool[] ReadIntoArray_Bool(long count)
	{
		bool[] arr = new bool[count];
		ReadIntoArray_Bool(arr, count);
		return arr;
	}

	protected void ReadIntoArray_Bool(bool[] arr, long boolean_count)
	{
		int byteCount = (int)Math.Ceiling((double)boolean_count / 8.0);
		BitArray ba = new BitArray((from b in br.ReadBytes(byteCount)
			select (byte)((ulong)((((long)b * 2149582850L) & 0x884422110L) * 4311810305L) >> 32)).ToArray());
		for (int i = 0; i < boolean_count; i++)
		{
			arr[i] = ba[i];
		}
	}

	protected Stream ReadIntoStream(long count)
	{
		ReadIntoArrayCheck(count);
		if (count > 2147483591)
		{
			LargeMemoryStream lms = new LargeMemoryStream(count);
			long remaining = count;
			byte[] buffer = new byte[lms.BlockSize];
			while (remaining > 0)
			{
				int toRead = (int)Math.Min(remaining, lms.BlockSize);
				br.Read(buffer, 0, toRead);
				lms.Write(buffer, 0L, toRead);
				remaining -= toRead;
			}
			return lms;
		}
		return new MemoryStream(br.ReadBytes((int)count));
	}

	protected byte[] ReadIntoArray_Byte(long count)
	{
		byte[] arr = new byte[count];
		ReadIntoArray_Byte(arr, count);
		return arr;
	}

	protected void ReadIntoArray_Byte(byte[] arr, long count)
	{
		ReadIntoArrayCheck(count);
		for (int i = 0; i < count; i++)
		{
			arr[i] = br.ReadByte();
		}
	}

	protected sbyte[] ReadIntoArray_SByte(long count)
	{
		sbyte[] arr = new sbyte[count];
		ReadIntoArray_SByte(arr, count);
		return arr;
	}

	protected void ReadIntoArray_SByte(sbyte[] arr, long count)
	{
		ReadIntoArrayCheck(count);
		for (int i = 0; i < count; i++)
		{
			arr[i] = br.ReadSByte();
		}
	}

	protected ushort[] ReadIntoArray_UInt16(long count)
	{
		ushort[] arr = new ushort[count];
		ReadIntoArray_UInt16(arr, count);
		return arr;
	}

	protected void ReadIntoArray_UInt16(ushort[] arr, long count)
	{
		ReadIntoArrayCheck(count * 2);
		for (int i = 0; i < count; i++)
		{
			arr[i] = br.ReadUInt16();
		}
	}

	protected short[] ReadIntoArray_Int16(long count)
	{
		short[] arr = new short[count];
		ReadIntoArray_Int16(arr, count);
		return arr;
	}

	protected void ReadIntoArray_Int16(short[] arr, long count)
	{
		ReadIntoArrayCheck(count * 2);
		for (int i = 0; i < count; i++)
		{
			arr[i] = br.ReadInt16();
		}
	}

	protected uint[] ReadIntoArray_UInt32(long count)
	{
		uint[] arr = new uint[count];
		ReadIntoArray_UInt32(arr, count);
		return arr;
	}

	protected void ReadIntoArray_UInt32(uint[] arr, long count)
	{
		ReadIntoArrayCheck(count * 4);
		for (int i = 0; i < count; i++)
		{
			arr[i] = br.ReadUInt32();
		}
	}

	protected int[] ReadIntoArray_Int32(long count)
	{
		int[] arr = new int[count];
		ReadIntoArray_Int32(arr, count);
		return arr;
	}

	protected void ReadIntoArray_Int32(int[] arr, long count)
	{
		ReadIntoArrayCheck(count * 4);
		for (int i = 0; i < count; i++)
		{
			arr[i] = br.ReadInt32();
		}
	}

	protected ulong[] ReadIntoArray_UInt64(long count)
	{
		ulong[] arr = new ulong[count];
		ReadIntoArray_UInt64(arr, count);
		return arr;
	}

	protected void ReadIntoArray_UInt64(ulong[] arr, long count)
	{
		ReadIntoArrayCheck(count * 8);
		for (int i = 0; i < count; i++)
		{
			arr[i] = br.ReadUInt64();
		}
	}

	protected long[] ReadIntoArray_Int64(long count)
	{
		long[] arr = new long[count];
		ReadIntoArray_Int64(arr, count);
		return arr;
	}

	protected void ReadIntoArray_Int64(long[] arr, long count)
	{
		ReadIntoArrayCheck(count * 8);
		for (int i = 0; i < count; i++)
		{
			arr[i] = br.ReadInt64();
		}
	}

	protected float[] ReadIntoArray_Float(int count)
	{
		ReadIntoArrayCheck(count * 4);
		float[] arr = new float[count];
		br.ReadDataToFillArray_f(arr, count);
		return arr;
	}

	protected double[] ReadIntoArray_Double(long count)
	{
		ReadIntoArrayCheck(count * 8);
		double[] arr = new double[count];
		br.ReadDataToFillArray_d(arr);
		return arr;
	}

	private void ReadIntoArrayCheck(long byteCount)
	{
		if (br.BaseStream.Length - br.BaseStream.Position < byteCount)
		{
			throw new Exception("Cannot read beyond end of file");
		}
	}

	public void Dispose()
	{
		if (s != null)
		{
			s.Dispose();
			s = null;
		}
		if (br != null)
		{
			br.Dispose();
			br = null;
		}
	}
}
