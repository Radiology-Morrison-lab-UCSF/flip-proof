

using FlipProof.Base;

namespace FlipProof.Image.Maths;

public class HermiteInterpolator
{
    private float oneMinusTensionDiv2;

    private float a0;

    private float a1;

    private float a2;

    private float a3;

    public HermiteInterpolator(float mu, float tension)
    {
        CalcAVals(mu, tension, out oneMinusTensionDiv2, out a0, out a1, out a2, out a3);
    }

    public static float Interpolate1D(float y0, float y1, float y3, float y4, float mu, float tension, float bias)
    {
        float mu2 = mu * mu;
        float mu3 = mu2 * mu;
        float m0 = (y1 - y0) * (1f + bias) * (1f - tension) / 2f;
        m0 += (y3 - y1) * (1f - bias) * (1f - tension) / 2f;
        float m1 = (y3 - y1) * (1f + bias) * (1f - tension) / 2f;
        m1 += (y4 - y3) * (1f - bias) * (1f - tension) / 2f;
        float num = 2f * mu3 - 3f * mu2 + 1f;
        float a1 = mu3 - 2f * mu2 + mu;
        float a2 = mu3 - mu2;
        float a3 = -2f * mu3 + 3f * mu2;
        return num * y1 + a1 * m0 + a2 * m1 + a3 * y3;
    }

    public static float Interpolate1D(float y0, float y1, float y3, float y4, float mu, float tension)
    {
        CalcAVals(mu, tension, out var oneMinusTensionDiv2, out var a0, out var a1, out var a2, out var a3);
        float m0 = (y1 - y0) * oneMinusTensionDiv2;
        m0 += (y3 - y1) * oneMinusTensionDiv2;
        float m1 = (y3 - y1) * oneMinusTensionDiv2;
        m1 += (y4 - y3) * oneMinusTensionDiv2;
        return a0 * y1 + a1 * m0 + a2 * m1 + a3 * y3;
    }

    public static XYZf Interpolate3D(XYZf p0, XYZf p1, XYZf p3, XYZf p4, float mu, float tension)
    {
        CalcAVals(mu, tension, out var oneMinusTensionDiv2, out var a0, out var a1, out var a2, out var a3);
        return Interpolate_Sub(p0, p1, p3, p4, oneMinusTensionDiv2, a0, a1, a2, a3);
    }

    public XYZf Interpolate3D(XYZf p0, XYZf p1, XYZf p3, XYZf p4)
    {
        return Interpolate_Sub(p0, p1, p3, p4, oneMinusTensionDiv2, a0, a1, a2, a3);
    }

    private static XYZf Interpolate_Sub(XYZf p0, XYZf p1, XYZf p3, XYZf p4, float oneMinusTensionDiv2, float a0, float a1, float a2, float a3)
    {
        XYZf p3MinusP1 = p3 - p1;
        XYZf m0 = (p1 - p0) * oneMinusTensionDiv2 + p3MinusP1 * oneMinusTensionDiv2;
        XYZf m1 = p3MinusP1 * oneMinusTensionDiv2 + (p4 - p3) * oneMinusTensionDiv2;
        return p1 * a0 + m0 * a1 + m1 * a2 + p3 * a3;
    }

    private static void CalcAVals(float mu, float tension, out float oneMinusTensionDiv2, out float a0, out float a1, out float a2, out float a3)
    {
        oneMinusTensionDiv2 = (1f - tension) / 2f;
        float mu2 = mu * mu;
        float mu3 = mu2 * mu;
        float _3mu2 = 3f * mu2;
        float _2mu3 = 2f * mu3;
        a0 = _2mu3 - _3mu2 + 1f;
        a1 = mu3 - 2f * mu2 + mu;
        a2 = mu3 - mu2;
        a3 = 0f - _2mu3 + _3mu2;
    }
}
