using System.Numerics;
using System.Text;
using FlipProof.Base;
using FlipProof.Image;
using FlipProof.Image.IO;
using FlipProof.Image.Maths;
using FlipProof.Image.Matrices;
using FlipProof.Torch;
using TorchSharp;
using static Tensorboard.ApiDef.Types;

namespace FlipProof.Image.Nifti;

public class NiftiFile<T> : NiftiFile_Base where T : struct, IComparable<T>, IComparable, IEquatable<T>
{

   private readonly long _sizeOfT;

	public override long SizeOfT => _sizeOfT;


	public NiftiFile(NiftiHeader nh, T[] vox)
		: this(nh, new MemoryStream(vox.ToBytes()))
	{
	}

	public NiftiFile(NiftiHeader nh, Stream vox) :base(nh, vox)
		
	{
      _sizeOfT = CollectionCreation.SizeOfType(Array.Empty<T>(), dotNetSizeForBoolean: false);
		if (base.Head.DataArrayDims[0] >= base.Head.DataArrayDims.Length)
		{
			throw new Exception("Header indicates more dimensions than can be supported by the nifti format");
		}
		long noVoxelsShouldBe = 1L;
		for (int i = 1; i <= base.Head.DataArrayDims[0]; i++)
		{
			noVoxelsShouldBe *= base.Head.DataArrayDims[i];
		}
		if (noVoxelsShouldBe != vox.Length / SizeOfT)
		{
			throw new ArgumentException("voxel data length does not match header information regarding number and type of voxels");
		}
	}


	public T[] GetAllVoxels()
	{
		_voxels.Position = 0L;
		return _voxels.ReadBytes((int)_voxels.Length).FromByteArray<T>();
	}


	public override Image<TSpace> ToImage<TSpace>(bool applyScalingFactors = true)
	{
		bool scaleVoxels = applyScalingFactors && (base.Head.sclSlope != 1f || base.Head.scl_inter != 0f);
		T[] voxels = GetAllVoxels();

		// --- CAUTION ---
		// Nifti files are written i (fastest), j, k, volume (slowest)
		// Torch images are stored volume, k, j, i
		// ---------------

      ImageHeader imHead = ImageHeader.Create(Head);
#pragma warning disable CS0618 // Type or member is obsolete

      if (scaleVoxels)
		{
			return voxels switch
			{
				bool[] b => ToFloatImage<bool, TSpace>(Head, imHead, b, Convert.ToSingle),
				byte[] b => ToFloatImage<byte, TSpace>(Head, imHead, b, Convert.ToSingle),
				UInt16[] b => ToFloatImage<UInt16, TSpace>(Head, imHead, b, Convert.ToSingle),
				UInt32[] b => ToFloatImage<UInt32, TSpace>(Head, imHead, b, Convert.ToSingle),
				UInt64[] b => ToDoubleImage<UInt64, TSpace>(Head, imHead, b, Convert.ToDouble),
				sbyte[] b => ToFloatImage<sbyte, TSpace>(Head, imHead, b, Convert.ToSingle),
				Int16[] b => ToFloatImage<Int16, TSpace>(Head, imHead, b, Convert.ToSingle),
				Int32[] b => ToFloatImage<Int32, TSpace>(Head, imHead, b, Convert.ToSingle),
				Int64[] b => ToDoubleImage<Int64, TSpace>(Head, imHead, b, Convert.ToDouble),
				// TO DO: reinstate once complex available again
				//Complex[] b => ToComplexImage(head,b), // NB NiftiReader does not support double-pairs at the time of writing
				float[] b => ToFloatImage<float, TSpace>(Head, imHead, b, a=>a),
				double[] b => ToDoubleImage<double, TSpace>(Head, imHead, b, a=>a),
				_ => throw new NotSupportedException($"Files of type {Head.dataType} are not supported")
			};
		}

		return voxels switch
		{
			// TO DO: Support other types
			bool[] b => new ImageBool<TSpace>(imHead, To4D(imHead.Size, b)),
			byte[] b => ToFloatImage(imHead, b),
			UInt16[] b => ToFloatImage(imHead, b),
			UInt32[] b => ToFloatImage(imHead, b),
			UInt64[] b => ToDoubleImage(imHead, b),
			sbyte[] b => new ImageInt8<TSpace>(imHead, To4D(imHead.Size, b)),
			Int16[] b => ToFloatImage(imHead, b),
			Int32[] b => ToFloatImage(imHead, b),
			Int64[] b => ToDoubleImage(imHead, b),
			// TO DO: Reinstate once complex available again
			//Complex[] b => ToImage(head, torch.tensor(b), Image3dComplex.Create), // NB NiftiReader does not support double-pairs at the time of writing
			float[] b => ToFloatImage(imHead, b),
			double[] b => ToDoubleImage(imHead, b),
			_ => throw new NotSupportedException($"Files of type {Head.dataType} are not supported")
		};

      static ImageFloat<TSpace> ToFloatImage<S>(ImageHeader head, S[] arr) => new(head, To4D(head.Size, arr is float[] ff ? ff : arr.CastArrayToFloat()));
      static ImageDouble<TSpace> ToDoubleImage<S>(ImageHeader head, S[] arr) => new(head, To4D(head.Size, arr is double[] ff ? ff : arr.CastArrayToDouble()));
#pragma warning restore CS0618 // Type or member is obsolete

   }
   private static Array4D<S> To4D<S>(ImageSize imSize, S[] xyzVol) where S : struct
	{
		if (imSize.Any(a => a > int.MaxValue))
		{
			throw new NotSupportedException($"Images of dimension > {int.MaxValue} are not supported");
		}
		Array4D<S> a4D = new((int)imSize.X, (int)imSize.Y, (int)imSize.Z, (int)imSize.VolumeCount, xyzVol);
		return a4D;
	}

   private static ImageFloat<TSpace> ToFloatImage<TVox,TSpace>(NiftiHeader head, ImageHeader imHead, TVox[] unscaledVoxels, Func<TVox, float> convert)
      where TSpace : struct, ISpace
   {
		float[] scaledVoxels = ScaleData(unscaledVoxels, convert, head.sclSlope, head.scl_inter);
      return new ImageFloat<TSpace>(imHead, To4D(imHead.Size, scaledVoxels));
   }
	private static ImageDouble<TSpace> ToDoubleImage<TVox,TSpace>(NiftiHeader head, ImageHeader imHead, TVox[] unscaledVoxels, Func<TVox, double> convert)
      where TSpace : struct, ISpace
   {
		double[] scaledVoxels = ScaleData(unscaledVoxels, convert, head.sclSlope, head.scl_inter);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageDouble<TSpace>(imHead, To4D(imHead.Size, scaledVoxels));
#pragma warning restore CS0618 // Type or member is obsolete
   }

	// TO DO reinstate once complex is possible
  // private static Image3dComplex ToComplexImage(NiftiHeader head, Complex[] unscaledVoxels)
  // {
		//Complex[] scaledVoxels = new Complex[unscaledVoxels.Length];
  //    for (int i = 0; i < unscaledVoxels.Length; i++)
  //    {
  //       scaledVoxels[i] = unscaledVoxels[i] * head.sclSlope + head.scl_inter;
  //    }

		//return ToImage(head, torch.tensor(scaledVoxels), Image3dComplex.Create);
  // }
	

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="S">Voxel type after scaling</typeparam>
	/// <param name="head">The header</param>
	/// <param name="unscaledVoxels">Nifti scaling should not already have been applied</param>
	/// <returns></returns>
	/// <exception cref="OrientationException">The header orientation is voxel size only, which is deemed unsafe</exception>
	private static S[] ScaleData<U, S>(U[] unscaledVoxels, Func<U,S> converstion, float scaleSlope, float scaleOffset)
		where S:IMultiplyOperators<S,float,S>,IAdditionOperators<S,float,S>
	{


		// Scale data
		S[] scaledVoxels = new S[unscaledVoxels.Length];
		for (int i = 0; i < unscaledVoxels.Length; i++)
		{
			scaledVoxels[i] = converstion(unscaledVoxels[i]) * scaleSlope + scaleOffset;
		}
		return scaledVoxels;
	}
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="S">Voxel type after scaling</typeparam>
	/// <param name="head">The header</param>
	/// <param name="unscaledVoxels">Nifti scaling should not already have been applied</param>
	/// <returns></returns>
	/// <exception cref="OrientationException">The header orientation is voxel size only, which is deemed unsafe</exception>
	private static S[] ScaleData<U, S>(U[] unscaledVoxels, Func<U,S> converstion, double scaleSlope, double scaleOffset)
		where S:IMultiplyOperators<S,double,S>,IAdditionOperators<S,double,S>
	{


		// Scale data
		S[] scaledVoxels = new S[unscaledVoxels.Length];
		for (int i = 0; i < unscaledVoxels.Length; i++)
		{
			scaledVoxels[i] = converstion(unscaledVoxels[i]) * scaleSlope + scaleOffset;
		}
		return scaledVoxels;
	}

	public override NiftiFile_Base To3DNiiBase(int fourthDim)
	{
		return To3DNii(fourthDim);
	}

	public NiftiFile<T> To3DNii(int fourthDim)
	{
		T[] dataToUse = GetUnscaledVoxelsFrom3dVolume(fourthDim);
		NiftiHeader niftiHeader = base.Head.DeepClone();
		niftiHeader.PixDim[4] = 1f;
		niftiHeader.DataArrayDims[0] = 3;
		niftiHeader.DataArrayDims[4] = 1;
		return new NiftiFile<T>(niftiHeader, dataToUse);
	}

	private T[] GetUnscaledVoxelsFrom3dVolume(int fourthDim)
	{
		T[] dataToUse;
		if (base.Has4thDimension)
		{
			if (fourthDim >= base.Head.DataArrayDims[4])
			{
				throw new IndexOutOfRangeException($"Size of fourth dimension is {base.Head.DataArrayDims[4]} but index of {fourthDim} was specified");
			}
			int voxelsPerSlice = base.VoxelsPerVolume;
			dataToUse = new T[voxelsPerSlice];
			_voxels.Position = fourthDim * voxelsPerSlice * SizeOfT;
			using BinaryReader br = new(_voxels, Encoding.Default, leaveOpen: true);
			br.ReadDataToFillArray(dataToUse);
		}
		else
		{
			if (fourthDim != 0)
			{
				throw new IndexOutOfRangeException("Nifti is 3D and fourth dim specified is not zero");
			}
			dataToUse = GetAllVoxels();
		}
		return dataToUse;
	}


	public XYZ<float> Vox2WorldCoordinates(XYZ<float> imageCoords)
	{
		if (base.Head.sFormCode != CoordinateMapping_Nifti.Unknown)
      {
         return CalcWithSForm(imageCoords);
      }
      float b = base.Head.quartern_b;
		float b_sq = b * b;
		float c = base.Head.quartern_c;
		float c_sq = c * c;
		float d = base.Head.quartern_d;
		float d_sq = d * d;
		float a_sq = 1f - b * b + c * c + d * d;
		float num = (float)Math.Sqrt(a_sq);
		float ab = num * b;
		float ac = num * c;
		float ad = num * d;
		float bc = b * c;
		float bd = b * d;
		float cd = c * d;
		DenseMatrix<float> obj = new(3, 3)
		{
			[0, 0] = a_sq + b_sq - c_sq - d_sq,
			[1, 0] = 2f * (bc + ad),
			[2, 0] = 2f * (bd + ac),
			[0, 1] = 2f * (bc - ad),
			[1, 1] = a_sq + c_sq - b_sq - d_sq,
			[2, 1] = 2f * (cd + ab),
			[0, 2] = 2f * (bd + ac),
			[1, 2] = 2f * (cd - ab),
			[2, 2] = a_sq + d_sq - b_sq - c_sq
		};
		DenseMatrix<float> ijqk = new(3, 1)
		{
			[0, 0] = imageCoords.X,
			[1, 0] = imageCoords.Y,
			[2, 0] = imageCoords.Z * base.Head.PixDim[0]
		};
		DenseMatrix<float> pixDim = new(3, 1)
		{
			[0, 0] = base.Head.PixDim[1],
			[1, 0] = base.Head.PixDim[2],
			[2, 0] = base.Head.PixDim[3]
		};

      DenseMatrix<float> result = new DenseMatrix<float>(3, 1)
		{
			[0, 0] = base.Head.quartern_x,
			[1, 0] = base.Head.quartern_y,
			[2, 0] = base.Head.quartern_z
		} +  obj.MatMul(ijqk).MultiplyPointwise(pixDim);
		return new XYZ<float>(result[0,0], result[1,0], result[2,0]);

      XYZ<float> CalcWithSForm(XYZ<float> imageCoords)
      {
         float[] srow_x = base.Head.Srow_x;
         float[] srow_y = base.Head.Srow_y;
         float[] srow_z = base.Head.Srow_z;
         float i = imageCoords.X;
         float j = imageCoords.Y;
         float k = imageCoords.Z;
         float x = srow_x[0] * i + srow_x[1] * j + srow_x[2] * k + srow_x[3];
         float y_ = srow_y[0] * i + srow_y[1] * j + srow_y[2] * k + srow_y[3];
         float z_ = srow_z[0] * i + srow_z[1] * j + srow_z[2] * k + srow_z[3];
         return new XYZ<float>(x, y_, z_);
      }
   }

	public XYZ<float> World2VoxCoordinates_ScannerSpace(XYZ<float> worldCoords)
	{
		if (base.Head.qFormCode > CoordinateMapping_Nifti.Unknown)
		{
			DenseMatrix<float> vox2WorldMatrix_ScannerSpace_Old = GetVox2WorldMatrix_ScannerSpace_Old();
			DenseMatrix<double> world_matrix = new(4, 1)
			{
				[0, 0] = worldCoords.X,
				[1, 0] = worldCoords.Y,
				[2, 0] = worldCoords.Z,
				[3, 0] = 1.0
			};
			DenseMatrix<double> result = (vox2WorldMatrix_ScannerSpace_Old.Inverse() * world_matrix);
			return new XYZ<float>((float)result[0,0], (float)result[1,0], (float)result[2,0]);
		}
		throw new NotImplementedException();
	}

	internal DecomposableNiftiTransform<double> GetVox2WorldMatrix_ScannerSpace()
	{
		return DecomposableNiftiTransform<double>.FromNiftiQuaternions(base.Head.quartern_b, base.Head.quartern_c, base.Head.quartern_d, base.Head.PixDim.Skip(1L).Take(3L).Select((Func<float, double>)((float entry) => entry))
			.ToArray(),
      [
         base.Head.quartern_x,
			base.Head.quartern_y,
			base.Head.quartern_z
		], base.Head.Qface);
	}

	private DenseMatrix<float> GetVox2WorldMatrix_ScannerSpace_Old()
	{
		double b = base.Head.quartern_b;
		double b_sq = b * b;
		double c = base.Head.quartern_c;
		double c_sq = c * c;
		double d = base.Head.quartern_d;
		double d_sq = d * d;
		double a_sq = 1.0 - b_sq - c_sq - d_sq;
		double num = ((a_sq <= 0.0) ? 0.0 : Math.Sqrt(a_sq));
		double ab = num * b;
		double ac = num * c;
		double ad = num * d;
		double bc = b * c;
		double bd = b * d;
		double cd = c * d;
		DenseMatrix<float> denseMatrix = new(3, 4);
		double pixSize_x = base.Head.PixDim[1];
		double pixSize_y = base.Head.PixDim[2];
		double pixSize_z = base.Head.PixDim[3];
		denseMatrix[0, 0] = (float)((a_sq + b_sq - c_sq - d_sq) * pixSize_x);
		denseMatrix[1, 0] = (float)(2.0 * (bc + ad) * pixSize_y);
		denseMatrix[2, 0] = (float)(2.0 * (bd - ac) * pixSize_z);
		denseMatrix[0, 1] = (float)(2.0 * (bc - ad) * pixSize_x);
		denseMatrix[1, 1] = (float)((a_sq + c_sq - b_sq - d_sq) * pixSize_y);
		denseMatrix[2, 1] = (float)(2.0 * (cd + ab) * pixSize_z);
		denseMatrix[0, 2] = (float)(2.0 * (bd + ac) * pixSize_x);
		denseMatrix[1, 2] = (float)(2.0 * (cd - ab) * pixSize_y);
		denseMatrix[2, 2] = (float)((a_sq + d_sq - b_sq - c_sq) * pixSize_z);
		denseMatrix[0, 3] = (float)((double)base.Head.quartern_x * pixSize_x);
		denseMatrix[1, 3] = (float)((double)base.Head.quartern_y * pixSize_y);
		denseMatrix[2, 3] = (float)((double)base.Head.quartern_z * pixSize_z);
		return denseMatrix;
	}

	public DenseMatrix<float> GetVox2StdSpaceMatrix_old()
	{
		return new DenseMatrix<float>(4, 4)
		{
			[0, 0] = base.Head.Srow_x[0],
			[0, 1] = base.Head.Srow_x[1],
			[0, 2] = base.Head.Srow_x[2],
			[0, 3] = base.Head.Srow_x[3],
			[1, 0] = base.Head.Srow_y[0],
			[1, 1] = base.Head.Srow_y[1],
			[1, 2] = base.Head.Srow_y[2],
			[1, 3] = base.Head.Srow_y[3],
			[2, 0] = base.Head.Srow_z[0],
			[2, 1] = base.Head.Srow_z[1],
			[2, 2] = base.Head.Srow_z[2],
			[2, 3] = base.Head.Srow_z[3],
			[3, 3] = 1f
		};
	}

	public Matrix4x4_Optimised<float> GetVox2StdSpaceMatrix()
	{
		return new Matrix4x4_Optimised<float>
		{
			M11 = base.Head.Srow_x[0],
			M12 = base.Head.Srow_x[1],
			M13 = base.Head.Srow_x[2],
			M14 = base.Head.Srow_x[3],
			M21 = base.Head.Srow_y[0],
			M22 = base.Head.Srow_y[1],
			M23 = base.Head.Srow_y[2],
			M24 = base.Head.Srow_y[3],
			M31 = base.Head.Srow_z[0],
			M32 = base.Head.Srow_z[1],
			M33 = base.Head.Srow_z[2],
			M34 = base.Head.Srow_z[3],
			M44 = 1f
		};
	}

	public static void ConcatenateTimeSeriesTo4D(string[] volLocations, string saveTo, FileMode fm, bool lookForZippedVariantIfNotFound)
	{
		NiftiWriter.Write(ConcatenateTimeSeriesTo4D(volLocations, lookForZippedVariantIfNotFound), saveTo, fm);
	}

	public static NiftiFile<T> ConcatenateTimeSeriesTo4D(string[] volLocations, bool lookForZippedVariantIfNotFound)
	{
		return ConcatenateTimeSeriesTo4D(NiftiReader.OpenSeries<T>(volLocations, lookForZippedVariantIfNotFound));
	}

	public static NiftiFile<T> ConcatenateTimeSeriesTo4D(IEnumerable<NiftiFile<T>> vols)
	{
		ConcatenateTimeSeries_Checks(vols);
		if (vols.Any((NiftiFile<T> a) => a.Has4thDimension))
		{
			throw new ArgumentException("One or more volumes is already 4D: ");
		}
		typeof(T).Type2DataType(crashIfUnknown: true);
		NiftiHeader niftiHeader = vols.First().Head.DeepClone();
		niftiHeader.DataArrayDims[0] = 4;
		LargeMemoryStream allData = vols.Select((NiftiFile<T> a) => a._voxels).Concatenate();
		return new NiftiFile<T>(niftiHeader, allData);
	}

	private static void ConcatenateTimeSeries_Checks(IEnumerable<NiftiFile<T>> vols, bool allowMisalignedImages = false)
	{
		if (vols.Select((NiftiFile<T> a) => a.Head.bitPix).Distinct().Count() > 1)
		{
			throw new ArgumentException("Bit depths do not match");
		}
		if (vols.Select((NiftiFile<T> a) => a.Head.DataArrayDims).Distinct(new ArrayComparer_Int16()).Count() > 1)
		{
			throw new ArgumentException("Data dimensions do not match");
		}
		if (!allowMisalignedImages && (vols.Select((NiftiFile<T> a) => a.Head.qFormCode).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.Head.quartern_b).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.Head.quartern_c).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.Head.quartern_d).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.Head.quartern_x).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.Head.quartern_y).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.Head.quartern_z).Distinct().Count() > 1))
		{
			throw new ArgumentException("Vox2World matrices do not match");
		}
		if (!vols.Any())
		{
			throw new ArgumentException("No volumes provided");
		}
	}

	public static void ConcatenateTimeSeries_4DImages(string[] volLocations, string saveTo, FileMode fm, bool allowMisalignedImages, bool lookForZippedVariantIfNotFound)
	{
		NiftiWriter.Write(ConcatenateTimeSeries_4DImages(volLocations, allowMisalignedImages, lookForZippedVariantIfNotFound), saveTo, fm);
	}

	public static NiftiFile<T> ConcatenateTimeSeries_4DImages(string[] volLocations, bool allowMisalignedImages, bool lookForZippedVariantIfNotFound)
	{
		return ConcatenateTimeSeries_4DImages(NiftiReader.OpenSeries<T>(volLocations, lookForZippedVariantIfNotFound), allowMisalignedImages);
	}

	public static NiftiFile<T> ConcatenateTimeSeries_4DImages(IEnumerable<NiftiFile<T>> vols, bool allowMisalignedImages = false)
	{
		ConcatenateTimeSeries_Checks(vols, allowMisalignedImages);
		if (vols.Any((NiftiFile<T> a) => !a.Has4thDimension))
		{
			throw new ArgumentException("This method is for 4d images only");
		}
		typeof(T).Type2DataType(crashIfUnknown: true);
		NiftiHeader niftiHeader = vols.First().Head.DeepClone();
		niftiHeader.DataArrayDims[4] = (short)vols.Sum((NiftiFile<T> a) => a.Head.DataArrayDims[4]);
		LargeMemoryStream allData = vols.Select((NiftiFile<T> a) => a._voxels).Concatenate();
		return new NiftiFile<T>(niftiHeader, allData);
	}

	public void DeleteVolumes_AllButOne(int vol)
	{
		if (vol >= base.Head.DataArrayDims[4])
		{
			throw new ArgumentException("Nifti file does not have that many dimensions");
		}
		if (vol < 0)
		{
			throw new ArgumentException("volume must be positive number");
		}
		int bytesPerVolume = base.VoxelsPerVolume * CollectionCreation.SizeOfType(Array.Empty<T>(), dotNetSizeForBoolean: true);
		byte[] volData = new byte[bytesPerVolume];
		_voxels.Position = vol * bytesPerVolume;
		_voxels.Read(volData, 0, volData.Length);
		_voxels.SetLength(bytesPerVolume);
		_voxels.Position = 0L;
		_voxels.Write(volData, 0, volData.Length);
		base.Head.DataArrayDims[4] = 1;
	}

	public override float[] GetDataAsFloat()
	{
		_voxels.Position = 0L;
      using BinaryReader br = new(_voxels, Encoding.Default, leaveOpen: true);
		float[] asF = new float[base.VoxelCount];
		br.ReadDataToFillArray_f(asF);
		return asF;
	}


}
