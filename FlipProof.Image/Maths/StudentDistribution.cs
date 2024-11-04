using System;

namespace FlipProof.Image.Maths;

public class StudentDistribution : BetaDistribution
{
    public double stdDev = 1.0;

    public int degOfFreedom;

    public double mean;

    public StudentDistribution(int degOfFreedom_arg, double mean_Arg = 0.0, double stdDeviation = 1.0)
    {
        mean = mean_Arg;
        degOfFreedom = degOfFreedom_arg;
        stdDev = stdDeviation;
    }

    public double InvCDF(double p)
    {
        if (p <= 0.0 || p >= 1.0)
        {
            if (p == 1.0)
            {
                return double.PositiveInfinity;
            }
            if (p == 0.0)
            {
                return double.NegativeInfinity;
            }
            throw new ArgumentOutOfRangeException("Bad p:" + p);
        }
        double x = invbetai(2.0 * Math.Min(p, 1.0 - p), 0.5 * degOfFreedom, 0.5);
        x = stdDev * Math.Sqrt(degOfFreedom * (1.0 - x) / x);
        if (!(p >= 0.5))
        {
            return mean - x;
        }
        return mean + x;
    }

    public double invbetai(double p, double a, double b)
    {
        double a2 = a - 1.0;
        double b2 = b - 1.0;
        if (p <= 0.0)
        {
            return 0.0;
        }
        if (p >= 1.0)
        {
            return 1.0;
        }
        double x;
        if (a >= 1.0 && b >= 1.0)
        {
            double pp = p < 0.5 ? p : 1.0 - p;
            double t = Math.Sqrt(-2.0 * Math.Log(pp));
            x = (2.30753 + t * 0.27061) / (1.0 + t * (0.99229 + t * 0.04481)) - t;
            if (p < 0.5)
            {
                x = 0.0 - x;
            }
            double al = (x * x - 3.0) / 6.0;
            double h = 2.0 / (1.0 / (2.0 * a - 1.0) + 1.0 / (2.0 * b - 1.0));
            double w = x * Math.Sqrt(al + h) / h - (1.0 / (2.0 * b - 1.0) - 1.0 / (2.0 * a - 1.0)) * (al + 5.0 / 6.0 - 2.0 / (3.0 * h));
            x = a / (a + b * Math.Exp(2.0 * w));
        }
        else
        {
            double lna = Math.Log(a / (a + b));
            double lnb = Math.Log(b / (a + b));
            double t = Math.Exp(a * lna) / a;
            double u = Math.Exp(b * lnb) / b;
            double w = t + u;
            x = !(p < t / w) ? 1.0 - Math.Pow(b * w * (1.0 - p), 1.0 / b) : Math.Pow(a * w * p, 1.0 / a);
        }
        double afac = 0.0 - gammln(a) - gammln(b) + gammln(a + b);
        for (int i = 0; i < 10; i++)
        {
            if (x == 0.0 || x == 1.0)
            {
                return x;
            }
            double num = betai(a, b, x) - p;
            double t = Math.Exp(a2 * Math.Log(x) + b2 * Math.Log(1.0 - x) + afac);
            double u = num / t;
            x -= t = u / (1.0 - 0.5 * Math.Min(1.0, u * (a2 / x - b2 / (1.0 - x))));
            if (x <= 0.0)
            {
                x = 0.5 * (x + t);
            }
            if (x >= 1.0)
            {
                x = 0.5 * (x + t + 1.0);
            }
            if (Math.Abs(t) < 1E-08 * x && i > 0)
            {
                break;
            }
        }
        return x;
    }

    public double betai(double a, double b, double x)
    {
        if (x < 0.0 || x > 1.0)
        {
            throw new ArgumentException("Bad x in routine betai");
        }
        double bt = x != 0.0 && x != 1.0 ? Math.Exp(gammln(a + b) - gammln(a) - gammln(b) + a * Math.Log(x) + b * Math.Log(1.0 - x)) : 0.0;
        if (x < (a + 1.0) / (a + b + 2.0))
        {
            return bt * betacf(a, b, x) / a;
        }
        return 1.0 - bt * betacf(b, a, 1.0 - x) / b;
    }

    public double betacf(double a, double b, double x)
    {
        double qab = a + b;
        double qap = a + 1.0;
        double qam = a - 1.0;
        double c = 1.0;
        double d = 1.0 - qab * x / qap;
        if (Math.Abs(d) < 1.0000000031710769E-30)
        {
            d = 1.0000000031710769E-30;
        }
        d = 1.0 / d;
        double h = d;
        int i;
        for (i = 1; i <= 100; i++)
        {
            int m2 = 2 * i;
            double aa = i * (b - i) * x / ((qam + m2) * (a + m2));
            d = 1.0 + aa * d;
            if (Math.Abs(d) < 1.0000000031710769E-30)
            {
                d = 1.0000000031710769E-30;
            }
            c = 1.0 + aa / c;
            if (Math.Abs(c) < 1.0000000031710769E-30)
            {
                c = 1.0000000031710769E-30;
            }
            d = 1.0 / d;
            h *= d * c;
            aa = (0.0 - (a + i)) * (qab + i) * x / ((a + m2) * (qap + m2));
            d = 1.0 + aa * d;
            if (Math.Abs(d) < 1.0000000031710769E-30)
            {
                d = 1.0000000031710769E-30;
            }
            c = 1.0 + aa / c;
            if (Math.Abs(c) < 1.0000000031710769E-30)
            {
                c = 1.0000000031710769E-30;
            }
            d = 1.0 / d;
            double del = d * c;
            h *= del;
            if (Math.Abs(del - 1.0) < 3.000000106112566E-07)
            {
                break;
            }
        }
        if (i > 100)
        {
            throw new Exception("a or b too big, or MAXIT too small in betacf");
        }
        return h;
    }
}
