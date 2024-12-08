using FlipProof.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;


namespace FlipProof.Image.Maths;

public class HilbertCurve
{
    private XYZ<int>[] nodes_unscaled;

    private readonly Dictionary<XYZ<int>, float> nodeProportionLookup = new Dictionary<XYZ<int>, float>();

    private float scaleFactor;

    public readonly int sideSize;

    public float Length => (nodes_unscaled.Length - 1) / scaleFactor;

    private HilbertCurve(int sideSize, XYZ<int>[] nodesUnscaled, float scale)
    {
        this.sideSize = sideSize;
        nodes_unscaled = nodesUnscaled;
        scaleFactor = scale;
        float totalLength = nodesUnscaled.Length - 1;
        for (int i = 0; i < nodesUnscaled.Length; i++)
        {
            float proportionOfLength = i / totalLength;
            nodeProportionLookup.Add(nodesUnscaled[i], proportionOfLength);
        }
    }

    public float GetProportionOfLength(float x, float y=0, float z = 0)
    {
        XYZ<float> coord =new XYZ<float>(x,y,z);
        return GetProportionOfLength(coord);
    }

    public float GetProportionOfLength(XYZ<float> xyz)
    {
        XYZ<int> rounded = (XYZ<int>)(xyz * scaleFactor);
        try
        {
            return nodeProportionLookup[rounded];
        }
        catch (KeyNotFoundException)
        {
            throw new Exception("Coordinate outside bounds of this curve");
        }
    }

    public static int XY2d(int n, int x, int y)
    {
        int d = 0;
        for (int s = n / 2; s > 0; s /= 2)
        {
            int rx = (x & s) > 0 ? 1 : 0;
            int ry = (y & s) > 0 ? 1 : 0;
            d += s * s * (3 * rx ^ ry);
            rot(s, ref x, ref y, rx, ry);
        }
        return d;
    }

    public static void D2xy(int n, int d, out int x, out int y)
    {
        int t = d;
        x = 0;
        y = 0;
        for (int s = 1; s < n; s *= 2)
        {
            int rx = 1 & t / 2;
            int ry = 1 & (t ^ rx);
            rot(s, ref x, ref y, rx, ry);
            x += s * rx;
            y += s * ry;
            t /= 4;
        }
    }

    private static void rot(int n, ref int x, ref int y, int rx, int ry)
    {
        if (ry == 0)
        {
            if (rx == 1)
            {
                x = n - 1 - x;
                y = n - 1 - y;
            }
            int t = x;
            x = y;
            y = t;
        }
    }

    public static HilbertCurve Make3D(int cubeSizeLength)
    {
        List<IList<int>> allCoords = new List<IList<int>>(cubeSizeLength * cubeSizeLength * cubeSizeLength);
        for (int x = 0; x < cubeSizeLength; x++)
        {
            for (int y = 0; y < cubeSizeLength; y++)
            {
                for (int z = 0; z < cubeSizeLength; z++)
                {
                    allCoords.Add(new int[3] { x, y, z });
                }
            }
        }
        return MakeND_Sub(cubeSizeLength, allCoords);
    }

    public static HilbertCurve Make2D(int sqSizeLength)
    {
        List<IList<int>> allCoords = new List<IList<int>>(sqSizeLength * sqSizeLength);
        for (int x = 0; x < sqSizeLength; x++)
        {
            for (int y = 0; y < sqSizeLength; y++)
            {
                allCoords.Add(new int[2] { x, y });
            }
        }
        return MakeND_Sub(sqSizeLength, allCoords);
    }

    private static HilbertCurve MakeND_Sub(int sqSizeLength, List<IList<int>> allCoords)
    {
        BigInteger[] keys = (from a in HilbertPoint.Transform(allCoords)
                             select a.HilbertIndex).ToArray();
        XYZ<int>[] ordered;
        if (allCoords[0].Count == 3)
        {
            ordered = allCoords.Select((a) => new XYZ<int>(a[0], a[1], a[2])).ToArray();
        }
        else
        {
            if (allCoords[0].Count != 2)
            {
                throw new NotImplementedException();
            }
            ordered = allCoords.Select((a) => new XYZ<int>(a[0], a[1], 0)).ToArray();
        }
        Array.Sort(keys, ordered);
        return new HilbertCurve(sqSizeLength, ordered, 1f);
    }
}
