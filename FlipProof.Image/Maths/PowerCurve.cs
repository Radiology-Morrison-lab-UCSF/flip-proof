using System;
using System.Linq;
using FlipProof.Image.Maths;

namespace FlipProof.Image.Maths;

public class PowerCurve
{
    public double A;

    public double B;

    public PowerCurve(double exponent, double factor)
    {
        A = factor;
        B = exponent;
    }

    public static PowerCurve FitToData(double[] x, double[] y)
    {
        if (x.Length != y.Length)
        {
            throw new ArgumentException("arguments are not the same length");
        }
        double[] lnX = x.ToArray((double a) => Math.Log(a));
        double[] lnY = y.ToArray((double a) => Math.Log(a));
        double sumLnX = lnX.Sum();
        double sumLnY = lnY.Sum();
        double sumLnXLnY = 0.0;
        double sumLnXSq = 0.0;
        for (int i = 0; i < lnX.Length; i++)
        {
            double curLnX = lnX[i];
            sumLnXLnY += curLnX * lnY[i];
            sumLnXSq += curLnX * curLnX;
        }
        double j = x.Length;
        double num = j * sumLnXLnY - sumLnX * sumLnY;
        double bDen = j * sumLnXSq - sumLnX * sumLnX;
        double exponent = num / bDen;
        double factor = Math.Exp((sumLnY - exponent * sumLnX) / j);
        return new PowerCurve(exponent, factor);
    }

    public double CalcX(double y)
    {
        return Math.Pow(y / A, 1.0 / B);
    }

    public double CalcY(double x)
    {
        return A * Math.Pow(x, B);
    }

    public double CalcDerivativeY(double x)
    {
        return Math.Pow(x, (1.0 - B) / B) / (B * Math.Pow(A, 1.0 / B));
    }

    public double CalcDerivativeX(double slope)
    {
        return Math.Pow(slope * B * Math.Pow(A, 1.0 / B), B / (1.0 - B));
    }

    public override string ToString()
    {
        return $"y = {A} * x^{B}";
    }
}
