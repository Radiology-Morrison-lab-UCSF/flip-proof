using System;

namespace FlipProof.Image.IO;

internal interface IBinaryWriter : IDisposable
{

	void Write(sbyte allOfMe);

	void Write(byte allOfMe);

	void Write(byte[] allOfMe);

	void Write(char allOfMe);

	void Write(char[] allOfMe);

	void Write(short allOfMe);

	void Write(ushort allOfMe);

	void Write(int allOfMe);

	void Write(uint allOfMe);

	void Write(float f);

	void Write(double d);

	void Write(long allOfMe);

	void Write(ulong allOfMe);
}
