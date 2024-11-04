namespace FlipProof.Image.Maths;

public class LogNaturalCurve
{
    public double factor;

    public double intercept;

    public LogNaturalCurve(double factor, double intercept)
    {
        this.factor = factor;
        this.intercept = intercept;
    }

    public double GetXAtY(double y)
    {
        return Math.Exp((y - intercept) / factor);
    }
}
