namespace FlipProof.Image.Maths;

public interface IMultivariateRealDistribution
{
    double density(double[] x);

    int getDimension();

    double[] sample();

    double[][] sample(int sampleSize);
}
