using System;
using System.IO;

namespace FlipProof.Image.IO;

public class BinaryWriterWithInterface : BinaryWriter, IBinaryWriter, IDisposable
{
	public BinaryWriterWithInterface(Stream s)
		: base(s)
	{
	}
}
