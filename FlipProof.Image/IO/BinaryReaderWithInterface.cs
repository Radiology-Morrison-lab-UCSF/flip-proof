using System;
using System.IO;

namespace FlipProof.Image.IO;

public class BinaryReaderWithInterface : BinaryReader, IBinaryReader, IDisposable
{
	public BinaryReaderWithInterface(Stream s)
		: base(s)
	{
	}

	public char[] PeekChars(int no)
	{
		if (BaseStream.CanSeek && BaseStream.CanRead && BaseStream.Length - BaseStream.Position > no)
		{
			char[] result = ReadChars(3);
			BaseStream.Position -= 3L;
			return result;
		}
		return null;
	}

	public int GetBufferedUnreadBytes()
	{
		return 0;
	}

	public long GetPosition_Bits()
	{
		return BaseStream.Position * 8;
	}

	public void GoToPosition_Bits(long pos)
	{
		if (pos % 8 != 0L)
		{
			throw new ArgumentException("Must be whole bytes");
		}
		BaseStream.Position = pos / 8;
	}
}
