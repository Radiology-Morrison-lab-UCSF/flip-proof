using System;

namespace FlipProof.Image.Maths;

public class BetaDistribution : GammaDistribution
{
    public static double factrl(int n)
    {
        double[] a = new double[171];
        if (true)
        {
            a[0] = 1.0;
            for (int i = 1; i < 171; i++)
            {
                a[i] = i * a[i - 1];
            }
        }
        if (n < 0 || n > 170)
        {
            throw new Exception("factrl out of range");
        }
        return a[n];
    }

    public static double factln(int n)
    {
        double[] a = new double[2000];
        if (true)
        {
            for (int i = 0; i < 2000; i++)
            {
                a[i] = gammln(i + 1);
            }
        }
        if (n < 0)
        {
            throw new Exception("negative arg in factln");
        }
        if (n < 2000)
        {
            return a[n];
        }
        return gammln(n + 1);
    }

    public static double bico(int n, int k)
    {
        if (n < 0 || k < 0 || k > n)
        {
            throw new Exception("bad args in bico");
        }
        if (n < 171)
        {
            return Math.Floor(0.5 + factrl(n) / (factrl(k) * factrl(n - k)));
        }
        return Math.Floor(0.5 + Math.Exp(factln(n) - factln(k) - factln(n - k)));
    }

    public static double beta(double z, double w)
    {
        return Math.Exp(gammln(z) + gammln(w) - gammln(z + w));
    }
}
