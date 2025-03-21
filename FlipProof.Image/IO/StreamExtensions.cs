using System;
using System.Collections.Generic;
using System.IO;

namespace FlipProof.Image.IO;

public static class StreamExtensions
{
	public static byte[] PeekBytes(this Stream ms, int position, int count)
	{
		long prevPosition = ms.Position;
		ms.Position = position;
		byte[] result = ms.ReadBytes(count);
		ms.Position = prevPosition;
		return result;
	}

	public static char PeekChar(this Stream ms)
	{
		return ms.PeekChar((int)ms.Position);
	}

	public static char PeekChar(this Stream ms, int position)
	{
		return BitConverter.ToChar(ms.PeekBytes(position, 2), 0);
	}

	public static short PeekInt16(this Stream ms)
	{
		return ms.PeekInt16((int)ms.Position);
	}

	public static short PeekInt16(this Stream ms, int position)
	{
		return BitConverter.ToInt16(ms.PeekBytes(position, 2), 0);
	}

	public static int PeekInt32(this Stream ms)
	{
		return ms.PeekInt32((int)ms.Position);
	}

	public static int PeekInt32(this Stream ms, int position)
	{
		return BitConverter.ToInt32(ms.PeekBytes(position, 4), 0);
	}

	public static long PeekInt64(this Stream ms)
	{
		return ms.PeekInt64((int)ms.Position);
	}

	public static long PeekInt64(this Stream ms, int position)
	{
		return BitConverter.ToInt64(ms.PeekBytes(position, 8), 0);
	}
   [CLSCompliant(false)]
   public static ushort PeekUInt16(this Stream ms)
	{
		return ms.PeekUInt16((int)ms.Position);
	}
   [CLSCompliant(false)]
   public static ushort PeekUInt16(this Stream ms, int position)
	{
		return BitConverter.ToUInt16(ms.PeekBytes(position, 2), 0);
	}
   [CLSCompliant(false)]
   public static uint PeekUInt32(this Stream ms)
	{
		return ms.PeekUInt32((int)ms.Position);
	}
   [CLSCompliant(false)]
   public static uint PeekUInt32(this Stream ms, int position)
	{
		return BitConverter.ToUInt32(ms.PeekBytes(position, 4), 0);
	}
   [CLSCompliant(false)]
   public static ulong PeekUInt64(this Stream ms)
	{
		return ms.PeekUInt64((int)ms.Position);
	}
   [CLSCompliant(false)]
   public static ulong PeekUInt64(this Stream ms, int position)
	{
		return BitConverter.ToUInt64(ms.PeekBytes(position, 8), 0);
	}

	public static byte[] ReadBytes(this Stream ms, int count, bool ignoreError = false)
	{
		byte[] buffer = new byte[count];
		int result = ms.Read(buffer, 0, count);
		if (!ignoreError && result != count)
		{
			throw new Exception("End reached.");
		}
		return buffer;
	}

	public static char ReadChar(this Stream ms)
	{
		return BitConverter.ToChar(ms.ReadBytes(2), 0);
	}

	public static short ReadInt16(this Stream ms)
	{
		return BitConverter.ToInt16(ms.ReadBytes(2), 0);
	}

	public static int ReadInt32(this Stream ms)
	{
		return BitConverter.ToInt32(ms.ReadBytes(4), 0);
	}

	public static long ReadInt64(this Stream ms)
	{
		return BitConverter.ToInt64(ms.ReadBytes(8), 0);
	}

   [CLSCompliant(false)]
   public static ushort ReadUInt16(this Stream ms)
	{
		return BitConverter.ToUInt16(ms.ReadBytes(2), 0);
	}
   [CLSCompliant(false)]
   public static uint ReadUInt32(this Stream ms)
	{
		return BitConverter.ToUInt32(ms.ReadBytes(4), 0);
	}
   [CLSCompliant(false)]
   public static ulong ReadUInt64(this Stream ms)
	{
		return BitConverter.ToUInt64(ms.ReadBytes(8), 0);
	}

	public static LargeMemoryStream Concatenate(this IEnumerable<Stream> input)
	{
		LargeMemoryStream allData = new LargeMemoryStream(0L);
		input.Concatenate(allData);
		return allData;
	}

	public static void Concatenate<T>(this IEnumerable<Stream> input, T destination) where T : Stream
	{
		foreach (Stream item in input)
		{
			_ = new byte[item.Length];
			item.CopyTo(destination);
		}
	}

	public static void WriteByte(this Stream ms, int position, byte value)
	{
		long prevPosition = ms.Position;
		ms.Position = position;
		ms.WriteByte(value);
		ms.Position = prevPosition;
	}

	public static void WriteBytes(this Stream ms, byte[] value)
	{
		ms.Write(value, 0, value.Length);
	}

	public static void WriteBytes(this Stream ms, int position, byte[] value)
	{
		long prevPosition = ms.Position;
		ms.Position = position;
		ms.Write(value, 0, value.Length);
		ms.Position = prevPosition;
	}

	public static void WriteInt16(this Stream ms, short value)
	{
		ms.Write(BitConverter.GetBytes(value), 0, 2);
	}

	public static void WriteInt16(this Stream ms, int position, short value)
	{
		ms.WriteBytes(position, BitConverter.GetBytes(value));
	}

	public static void WriteInt32(this Stream ms, int value)
	{
		ms.Write(BitConverter.GetBytes(value), 0, 4);
	}

	public static void WriteInt32(this Stream ms, int position, int value)
	{
		ms.WriteBytes(position, BitConverter.GetBytes(value));
	}

	public static void WriteInt64(this Stream ms, long value)
	{
		ms.Write(BitConverter.GetBytes(value), 0, 8);
	}

	public static void WriteInt64(this Stream ms, int position, long value)
	{
		ms.WriteBytes(position, BitConverter.GetBytes(value));
	}
   [CLSCompliant(false)]
   public static void WriteUInt16(this Stream ms, ushort value)
	{
		ms.Write(BitConverter.GetBytes(value), 0, 2);
	}
   [CLSCompliant(false)]
   public static void WriteUInt16(this Stream ms, int position, ushort value)
	{
		ms.WriteBytes(position, BitConverter.GetBytes(value));
	}
   [CLSCompliant(false)]
   public static void WriteUInt32(this Stream ms, uint value)
	{
		ms.Write(BitConverter.GetBytes(value), 0, 4);
	}
   [CLSCompliant(false)]
   public static void WriteUInt32(this Stream ms, int position, uint value)
	{
		ms.WriteBytes(position, BitConverter.GetBytes(value));
	}
   [CLSCompliant(false)]
   public static void WriteUInt64(this Stream ms, ulong value)
	{
		ms.Write(BitConverter.GetBytes(value), 0, 8);
	}
   [CLSCompliant(false)]
   public static void WriteUInt64(this Stream ms, int position, ulong value)
	{
		ms.WriteBytes(position, BitConverter.GetBytes(value));
	}

	public static void WriteInt16_BE(this Stream ms, short value)
	{
		ms.Write(Gen.GetBytes_BE(value), 0, 2);
	}

	public static void WriteInt16_BE(this Stream ms, int position, short value)
	{
		ms.WriteBytes(position, Gen.GetBytes_BE(value));
	}

	public static void WriteInt32_BE(this Stream ms, int value)
	{
		ms.Write(Gen.GetBytes_BE(value), 0, 4);
	}

	public static void WriteInt32_BE(this Stream ms, int position, int value)
	{
		ms.WriteBytes(position, Gen.GetBytes_BE(value));
	}

	public static void WriteInt64_BE(this Stream ms, long value)
	{
		ms.Write(Gen.GetBytes_BE(value), 0, 8);
	}

	public static void WriteInt64_BE(this Stream ms, int position, long value)
	{
		ms.WriteBytes(position, Gen.GetBytes_BE(value));
	}
   [CLSCompliant(false)]
   public static void WriteUInt16_BE(this Stream ms, ushort value)
	{
		ms.Write(Gen.GetBytes_BE(value), 0, 2);
	}
   [CLSCompliant(false)]
   public static void WriteUInt16_BE(this Stream ms, int position, ushort value)
	{
		ms.WriteBytes(position, Gen.GetBytes_BE(value));
	}
   [CLSCompliant(false)]
   public static void WriteUInt32_BE(this Stream ms, uint value)
	{
		ms.Write(Gen.GetBytes_BE(value), 0, 4);
	}
   [CLSCompliant(false)]
   public static void WriteUInt32_BE(this Stream ms, int position, uint value)
	{
		ms.WriteBytes(position, Gen.GetBytes_BE(value));
	}
   [CLSCompliant(false)]
   public static void WriteUInt64_BE(this Stream ms, ulong value)
	{
		ms.Write(Gen.GetBytes_BE(value), 0, 8);
	}
   [CLSCompliant(false)]
   public static void WriteUInt64_BE(this Stream ms, int position, ulong value)
	{
		ms.WriteBytes(position, Gen.GetBytes_BE(value));
	}
}
