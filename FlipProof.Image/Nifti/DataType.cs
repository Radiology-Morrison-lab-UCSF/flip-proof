namespace FlipProof.Image.Nifti;

public enum DataType : short
{
	unknown = 0,
	boolean = 1,
	unsignedChar = 2,
	signedShort = 4,
	signedInt = 8,
	Float = 16,
	complex = 32,
	Double = 64,
	rgb = 128,
	All = 255,
	signedChar = 256,
	unsignedShort = 512,
	unsignedInt = 768,
	longlong = 1024,
	unsignedLongLong = 1280,
	longDouble = 1536,
	doublePair = 1792,
	longDoublePair = 2048,
	rgba = 2304
}
