using System;
using System.Collections.Generic;
using System.IO;

namespace FlipProof.Image.IO;

public class LargeMemoryStream : Stream
{
	private long _length;

	private long _position;

	private byte[][] blocks;

	public const long MaximumLegalBlockSize = 2147483591L;

	private int blockLength;

	private double blockLength_d;

	public override bool CanRead => true;

	public override bool CanSeek => true;

	public override bool CanWrite => true;

	public override long Length => _length;

	public override long Position
	{
		get
		{
			return _position;
		}
		set
		{
			if (_position > _length)
			{
				throw new IndexOutOfRangeException("Position is past the end of the stream");
			}
			_position = value;
		}
	}

	public int NoInternalBlocks
	{
		get
		{
			if (!IsDisposed)
			{
				return blocks.Length;
			}
			throw new ObjectDisposedException("LargeMemoryStream");
		}
	}

	public int BlockSize => blockLength;

	public bool IsDisposed => blocks == null;

	public LargeMemoryStream(long size, int blockSize = -1)
	{
		_length = size;
		if ((long)blockSize > 2147483591L)
		{
			throw new ArgumentOutOfRangeException("blockSize max is " + 2147483591L);
		}
		if (blockSize < 0)
		{
			blockSize = (int)((_length <= 2147483648u) ? size : 1073741824);
		}
		blockLength = blockSize;
		blockLength_d = blockLength;
		blocks = new byte[0][];
		SetLength(size);
	}

	public (int arr, int index) GetInternalIndex(long ind)
	{
		long index;
		return (arr: (int)Math.DivRem(ind, blockLength, out index), index: (int)index);
	}

	public override void Flush()
	{
	}

	public override int ReadByte()
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException("LargeMemoryStream");
		}
		if (_position >= _length)
		{
			return -1;
		}
		var (arr, index) = GetInternalIndex(_position);
		_position++;
		return blocks[arr][index];
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return (int)Read(buffer, offset, count);
	}

	public long Read(byte[] buffer, long offset, long count)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException("LargeMemoryStream");
		}
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset + count > buffer.Length)
		{
			throw new ArgumentException();
		}
		if (offset < 0 || count < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		count = Math.Min(_length - _position, count);
		long remaining = count;
		long bufferOffset = offset;
		var (arr, index) = GetInternalIndex(_position);
		while (remaining > 0)
		{
			long take = Math.Min(blockLength - index, remaining);
			Array.Copy(blocks[arr], index, buffer, bufferOffset, take);
			remaining -= take;
			bufferOffset += take;
			index = 0;
			arr++;
		}
		_position += count;
		return count;
	}

	public IEnumerable<byte> ReadAsIEnumerable(long count)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException("LargeMemoryStream");
		}
		if (_position >= _length)
		{
			yield break;
		}
		(int, int) internalIndex = GetInternalIndex(_position);
		int iChunk = internalIndex.Item1;
		int index = internalIndex.Item2;
		long stopAt = Math.Min(_length, _position + count);
		for (; iChunk < blocks.Length; iChunk++)
		{
			byte[] curChunk = blocks[iChunk];
			for (int i = index; i < curChunk.Length; i++)
			{
				yield return curChunk[i];
				_position++;
				if (_position == stopAt)
				{
					yield break;
				}
			}
			index = 0;
		}
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException("LargeMemoryStream");
		}
		switch (origin)
		{
			case SeekOrigin.Begin:
				_position = offset;
				break;
			case SeekOrigin.Current:
				_position += offset;
				if (_position > _length)
				{
					throw new IndexOutOfRangeException("offset");
				}
				break;
			case SeekOrigin.End:
				_position = _length - 1 + offset;
				if (_position > _length)
				{
					throw new IndexOutOfRangeException("offset");
				}
				break;
			default:
				throw new NotSupportedException(origin.ToString());
		}
		return _position;
	}

	public override void SetLength(long value)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException("LargeMemoryStream");
		}
		int origNoChunks = blocks.Length;
		long originalSpaceAvail = (long)blockLength * (long)origNoChunks;
		_length = value;
		if (originalSpaceAvail >= value)
		{
			_position = Math.Min(_position, _length);
			return;
		}
		int additionalChunksReq = (int)Math.Ceiling((double)(value - originalSpaceAvail) / blockLength_d);
		Array.Resize(ref blocks, origNoChunks + additionalChunksReq);
		for (int i = origNoChunks; i < blocks.Length; i++)
		{
			blocks[i] = new byte[blockLength];
		}
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		Write(buffer, offset, count);
	}

	public void Write(byte[] buffer, long offset, long count)
	{
		ObjectDisposedException.ThrowIf(IsDisposed, this);
      ArgumentNullException.ThrowIfNull(buffer);
      if (offset + count > buffer.Length)
		{
			throw new ArgumentException();
		}
		if (offset < 0 || count < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		long requiredLength = _position + count;
		if (requiredLength > _length)
		{
			SetLength(requiredLength);
		}
		long remaining = count;
		long bufferOffset = offset;
		var (arr, index) = GetInternalIndex(_position);
		while (remaining > 0)
		{
			long take = Math.Min(blockLength - index, remaining);
			Array.Copy(buffer, bufferOffset, blocks[arr], index, take);
			remaining -= take;
			bufferOffset += take;
			arr++;
			index = 0;
		}
		_position += count;
	}

	protected override void Dispose(bool disposing)
	{
		_position = 0L;
		_length = 0L;
		if (blocks != null)
		{
			for (int i = 0; i < blocks.Length; i++)
			{
				blocks[i] = null;
			}
			blocks = null;
		}
		base.Dispose(disposing);
	}
}