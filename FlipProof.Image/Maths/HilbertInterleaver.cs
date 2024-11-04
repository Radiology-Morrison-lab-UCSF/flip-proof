using System;
using System.Collections.Concurrent;
using System.Numerics;

namespace FlipProof.Image.Maths;

internal class HilbertInterleaver
{
    private struct Indices
    {
        public readonly int iFromUintVector;

        public readonly short iToByteVector;

        public readonly byte iFromUintBit;

        public readonly byte iToByteBit;

        public Indices(int fromUintVector, byte fromUintBit, short toByteVector, byte toByteBit)
        {
            iFromUintVector = fromUintVector;
            iFromUintBit = fromUintBit;
            iToByteVector = toByteVector;
            iToByteBit = toByteBit;
        }

        public override string ToString()
        {
            return $"Byte.Bit from {iFromUintVector}.{iFromUintBit} to {iToByteVector}.{iToByteBit}";
        }
    }

    private static readonly Lazy<ConcurrentDictionary<int, HilbertInterleaver>> _cache = new Lazy<ConcurrentDictionary<int, HilbertInterleaver>>(() => new ConcurrentDictionary<int, HilbertInterleaver>());

    private readonly int BitDepth;

    private readonly int Dimensions;

    private readonly int Bits;

    private readonly int BytesNeeded;

    private Indices[] PrecomputedIndices;

    private static ConcurrentDictionary<int, HilbertInterleaver> Cache => _cache.Value;

    public static HilbertInterleaver Instance(int dimensions, int bitDepth)
    {
        return Cache.GetOrAdd(MakeHashCode(dimensions, bitDepth), (key) => new HilbertInterleaver(dimensions, bitDepth));
    }

    public HilbertInterleaver(int dimensions, int bitDepth)
    {
        Dimensions = dimensions;
        BitDepth = bitDepth;
        Bits = Dimensions * BitDepth;
        BytesNeeded = Bits >> 3;
        PrecomputedIndices = new Indices[Bits];
        int iBit = 0;
        for (byte iSourceBit = 0; iSourceBit < BitDepth; iSourceBit++)
        {
            for (int iSourceInt = Dimensions - 1; iSourceInt >= 0; iSourceInt--)
            {
                short iTargetByte = (short)(iBit >> 3);
                byte iTargetBit = (byte)(iBit % 8);
                PrecomputedIndices[iBit] = new Indices(iSourceInt, iSourceBit, iTargetByte, iTargetBit);
                iBit++;
            }
        }
        Comparison<Indices> sortByTargetByteAndBit = delegate (Indices a, Indices b)
        {
            if (a.iToByteVector < b.iToByteVector)
            {
                return -1;
            }
            if (a.iToByteVector > b.iToByteVector)
            {
                return 1;
            }
            if (a.iToByteBit < b.iToByteBit)
            {
                return -1;
            }
            return a.iToByteBit > b.iToByteBit ? 1 : 0;
        };
        Array.Sort(PrecomputedIndices, sortByTargetByteAndBit);
    }

    public byte[] Interleave(uint[] vector)
    {
        byte[] byteVector = new byte[BytesNeeded + 1];
        int iIndex = 0;
        for (int iByte = 0; iByte < BytesNeeded; iByte++)
        {
            uint bits = 0u;
            Indices idx2 = PrecomputedIndices[iIndex];
            Indices idx3 = PrecomputedIndices[iIndex + 1];
            Indices idx4 = PrecomputedIndices[iIndex + 2];
            Indices idx5 = PrecomputedIndices[iIndex + 3];
            Indices idx6 = PrecomputedIndices[iIndex + 4];
            Indices idx7 = PrecomputedIndices[iIndex + 5];
            Indices idx8 = PrecomputedIndices[iIndex + 6];
            Indices idx9 = PrecomputedIndices[iIndex + 7];
            bits = vector[idx2.iFromUintVector] >> idx2.iFromUintBit & 1u | (vector[idx3.iFromUintVector] >> idx3.iFromUintBit & 1) << 1 | (vector[idx4.iFromUintVector] >> idx4.iFromUintBit & 1) << 2 | (vector[idx5.iFromUintVector] >> idx5.iFromUintBit & 1) << 3 | (vector[idx6.iFromUintVector] >> idx6.iFromUintBit & 1) << 4 | (vector[idx7.iFromUintVector] >> idx7.iFromUintBit & 1) << 5 | (vector[idx8.iFromUintVector] >> idx8.iFromUintBit & 1) << 6 | (vector[idx9.iFromUintVector] >> idx9.iFromUintBit & 1) << 7;
            byteVector[iByte] = (byte)bits;
            iIndex += 8;
        }
        for (; iIndex < PrecomputedIndices.Length; iIndex++)
        {
            Indices idx = PrecomputedIndices[iIndex];
            byte bit = (byte)((vector[idx.iFromUintVector] >> idx.iFromUintBit & 1) << idx.iToByteBit);
            byteVector[idx.iToByteVector] |= bit;
        }
        return byteVector;
    }

    public byte[] Interleave_Unordered(uint[] vector)
    {
        byte[] byteVector = new byte[BytesNeeded + 1];
        Indices[] precomputedIndices = PrecomputedIndices;
        for (int i = 0; i < precomputedIndices.Length; i++)
        {
            Indices idx = precomputedIndices[i];
            byte bit = (byte)((vector[idx.iFromUintVector] >> idx.iFromUintBit & 1) << idx.iToByteBit);
            byteVector[idx.iToByteVector] |= bit;
        }
        return byteVector;
    }

    public BigInteger Untranspose(uint[] transposedIndex)
    {
        return new BigInteger(Interleave(transposedIndex));
    }

    private static int MakeHashCode(int dimensions, int bitDepth)
    {
        return bitDepth * dimensions << 6;
    }

    public override int GetHashCode()
    {
        return MakeHashCode(Dimensions, BitDepth);
    }

    public override bool Equals(object obj)
    {
        if (obj != null)
        {
            return GetHashCode() == obj.GetHashCode();
        }
        return false;
    }

    public override string ToString()
    {
        return $"Interleaver for {Dimensions} dimensions of {BitDepth} bits each";
    }
}
