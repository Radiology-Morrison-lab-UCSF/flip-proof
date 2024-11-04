using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FlipProof.Image.IO;

public class UnbufferedStreamReader : TextReader, IDisposable
{
	private const int newLine = 10;

	private const byte newLineB = 10;

	private const byte carriageReturn = 13;

	private Stream s;

	public bool CanRead
	{
		get
		{
			if (s.CanRead)
			{
				return s.Position < s.Length;
			}
			return false;
		}
	}

	public UnbufferedStreamReader(string path)
	{
		s = new FileStream(path, FileMode.Open);
	}

	public UnbufferedStreamReader(Stream stream)
	{
		s = stream;
	}

	public override string ReadLine()
	{
		if (s.Position >= s.Length)
		{
			throw new InvalidOperationException("Stream is at end");
		}
		List<byte> bytes = new List<byte>();
		bool lastWasCarriageReturn = false;
		int current;
		while ((current = s.ReadByte()) != -1 && current != 10)
		{
			byte b = (byte)current;
			bytes.Add(b);
			lastWasCarriageReturn = b == 13;
		}
		if (lastWasCarriageReturn)
		{
			bytes.RemoveAt(bytes.Count - 1);
		}
		return Encoding.ASCII.GetString(bytes.ToArray());
	}

	public override int Read()
	{
		return s.ReadByte();
	}

	public override void Close()
	{
		s.Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (s != null)
		{
			s.Dispose();
		}
	}

	public override int Peek()
	{
		throw new NotImplementedException();
	}

	public override int Read(char[] buffer, int index, int count)
	{
		throw new NotImplementedException();
	}

	public override int ReadBlock(char[] buffer, int index, int count)
	{
		throw new NotImplementedException();
	}

	public override string ReadToEnd()
	{
		throw new NotImplementedException();
	}
}
