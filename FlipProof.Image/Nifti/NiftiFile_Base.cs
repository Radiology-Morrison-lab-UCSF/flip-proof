using FlipProof.Base;
using FlipProof.Image.IO;
using FlipProof.Torch;
using System;
using System.IO;
using System.Numerics;
using static TorchSharp.torch;

namespace FlipProof.Image.Nifti;

public abstract class NiftiFile_Base(NiftiHeader head, Stream voxels) : IDisposable
{
   [CLSCompliant(false)]
   protected readonly Stream _voxels = voxels;

	public NiftiHeader Head { get; } = head;

	public bool Has4thDimension => Head.DataArrayDims[4] > 1;

	public long VoxelCount => _voxels.Length / SizeOfT;

	public abstract long SizeOfT { get; }

	public int VoxelsPerVolume => Head.DataArrayDims[1] * Head.DataArrayDims[2] * Head.DataArrayDims[3];

	public TimeSpan TR
	{
		get
		{
			switch (Head.UnitsTime)
			{
			case MeasurementUnits.Unknown:
				if (Head.PixDim[4] == 0f)
				{
					return TimeSpan.Zero;
				}
				throw new NotSupportedException("Units for time are unknown and so not convertable to seconds");
			case MeasurementUnits.Meter:
				throw new NotSupportedException("Units for time are not convertable to seconds");
			case MeasurementUnits.Milimeter:
				throw new NotSupportedException("Units for time are not convertable to seconds");
			case MeasurementUnits.MicroMeter:
				throw new NotSupportedException("Units for time are not convertable to seconds");
			case MeasurementUnits.Seconds:
				return TimeSpan.FromSeconds(Head.PixDim[4]);
			case MeasurementUnits.Miliseconds:
				return TimeSpan.FromMilliseconds(Head.PixDim[4]);
			case MeasurementUnits.Microseconds:
				return TimeSpan.FromMilliseconds(Head.PixDim[4] * 1000f);
			case MeasurementUnits.Hertz:
				throw new NotSupportedException("Units for time are not convertable to seconds");
			case MeasurementUnits.PartsPerMillion:
				throw new NotSupportedException("Units for time are not convertable to seconds");
			case MeasurementUnits.RadiansPerSecond:
				throw new NotSupportedException("Units for time are not convertable to seconds");
			default:
				throw new NotSupportedException("Units for time are not supported");
			}
		}
	}


	/// <summary>
	/// Creates a nifti file from an image. For boolean, convert to byte first
	/// </summary>
	/// <typeparam name="TVoxel"></typeparam>
	/// <typeparam name="TSpace"></typeparam>
	/// <param name="vols"></param>
	/// <param name="TR"></param>
	/// <returns></returns>
   public static NiftiFile<TVoxel> FromImage<TVoxel,TSpace>(Image<TVoxel, TSpace> vols, float TR = 1f)
		where TVoxel: struct, INumber<TVoxel>
		where TSpace : struct, ISpace
   {
      var head = NiftiHeader.Create(vols.Header, EnumMethods.Type2DataType(typeof(TVoxel), true));
      head.PixDim[4] = TR;

		// --- CAUTION ---
		// Nifti files are written i (fastest), j, k, volume (slowest)
		// Torch images are stored volume, k, j, i
		// ---------------
		Array4D<TVoxel> arr4D = vols.GetVoxelsAsArray4D();
		LargeMemoryStream stream = new(arr4D.NumberOfVoxels * CollectionCreation.SizeOfType<TVoxel>(true));
		arr4D.GetAllVoxels_XFastest(stream);
		arr4D.Dispose();// eager memory cleanup
      return new NiftiFile<TVoxel>(head, stream);
   }
   public abstract NiftiFile_Base To3DNiiBase(int fourthDim);

	public abstract Image<TSpace> ToImage<TSpace>(bool applyScalingFactors = true)
		where TSpace : struct, ISpace;


   public abstract float[] GetDataAsFloat();

	/// <summary>
	/// Returns a stream containing the voxels
	/// </summary>
	/// <returns></returns>
	internal Stream GetDataStream()
	{
		return _voxels;
	}

	public void ReplaceData(byte[] newData)
	{
		if (newData.Length != _voxels.Length)
		{
			throw new ArgumentException("Data size mismatch");
		}
		_voxels.Position = 0L;
		_voxels.Write(newData, 0, newData.Length);
	}

   public void Dispose()
   {
      ((IDisposable)_voxels).Dispose();
		GC.SuppressFinalize(this);
   }
}
