using System;
using System.IO;

namespace FlipProof.Image.IO;

internal interface IBinaryReader : IDisposable
{
	Stream BaseStream { get; }

	sbyte ReadSByte();

	byte ReadByte();

	byte[] ReadBytes(int count);

	int PeekChar();

	/// <summary>
	/// Returns null if peek is not possible
	/// </summary>
	/// <param name="no"></param>
	/// <returns></returns>
	char[]? PeekChars(int no);

	char ReadChar();

	char[] ReadChars(int length);

	short ReadInt16();

	ushort ReadUInt16();

	int ReadInt32();

	uint ReadUInt32();

	float ReadSingle();

	double ReadDouble();

	long ReadInt64();

	ulong ReadUInt64();

	int GetBufferedUnreadBytes();

	long GetPosition_Bits();

	void GoToPosition_Bits(long pos);
}
