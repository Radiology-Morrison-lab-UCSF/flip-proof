namespace FlipProof.Image.Nifti;

public enum SlicingOrder : byte
{
	Unknown,
	Sequential_Increasing,
	Sequential_Decreasing,
	Interleaved_Increasing_Start1st,
	Interleaved_Decreasing_StartLast,
	Interleaved_Increasing_Start2nd,
	Interleaved_Decreasing_Start2ndToLast
}
