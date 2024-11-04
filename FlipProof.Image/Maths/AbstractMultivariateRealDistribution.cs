using System;

namespace FlipProof.Image.Maths;

public abstract class AbstractMultivariateRealDistribution : IMultivariateRealDistribution
{
    protected Random random;

    private readonly int dimension;

    protected AbstractMultivariateRealDistribution(Random rng, int n)
    {
        random = rng;
        dimension = n;
    }

    public int getDimension()
    {
        return dimension;
    }

    public abstract double[] sample();

    public double[][] sample(int sampleSize)
    {
        if (sampleSize <= 0)
        {
            throw new ArgumentOutOfRangeException("sampleSize", sampleSize, "Must be above 0");
        }
        double[][] returnVal = GenMethods.JaggedArray<double>(sampleSize, dimension);
        for (int i = 0; i < sampleSize; i++)
        {
            returnVal[i] = sample();
        }
        return returnVal;
    }

    public abstract double density(double[] x);
}
