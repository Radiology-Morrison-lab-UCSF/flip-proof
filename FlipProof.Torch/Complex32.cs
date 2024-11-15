namespace FlipProof.Torch;

public readonly record struct Complex32(float Real, float Imaginary)
{
   internal Complex32((float Real, float Imaginary) realImag) : this(realImag.Real, realImag.Imaginary)
   { }
}
