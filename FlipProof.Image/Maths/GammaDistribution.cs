using System;

namespace FlipProof.Image.Maths;

public class GammaDistribution
{
    public static double gammln(double xx)
    {
        double[] cof = new double[14]
        {
            57.15623566586292, -59.59796035547549, 14.136097974741746, -0.4919138160976202, 3.399464998481189E-05, 4.652362892704858E-05, -9.837447530487956E-05, 0.0001580887032249125, -0.00021026444172410488, 0.00021743961811521265,
            -0.0001643181065367639, 8.441822398385275E-05, -2.6190838401581408E-05, 3.6899182659531625E-06
        };
        if (xx <= 0.0)
        {
            throw new ArgumentException("bad arg in gammln");
        }
        double x;
        double y = x = xx;
        double tmp = x + 5.2421875;
        tmp = (x + 0.5) * Math.Log(tmp) - tmp;
        double ser = 0.9999999999999971;
        for (int i = 0; i < 14; i++)
        {
            ser += cof[i] / (y += 1.0);
        }
        return tmp + Math.Log(2.5066282746310007 * ser / x);
    }
}
