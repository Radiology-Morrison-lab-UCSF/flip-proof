using System.Numerics;
using System.Text;
using FlipProof.Base;
using FlipProof.Image;
using FlipProof.Image.IO;
using FlipProof.Image.Maths;
using FlipProof.Image.Matrices;
using FlipProof.Torch;
using TorchSharp;

namespace FlipProof.Image.Nifti;

public class NiftiFile<T> : NiftiFile_Base where T : struct, IComparable<T>, IComparable, IEquatable<T>
{
	private readonly long _sizeOfT;

	public override long SizeOfT => _sizeOfT;


   private NiftiFile()
	{
		_sizeOfT = GenMethods.SizeOfType(Array.Empty<T>(), dotNetSizeForBoolean: false);
	}

	public NiftiFile(NiftiHeader nh, T[] vox)
		: this(nh, new MemoryStream(vox.ToBytes()))
	{
	}

	public NiftiFile(NiftiHeader nh, Stream vox)
		: this()
	{
		voxels = vox;
		base.head = nh;
		if (base.head.DataArrayDims[0] >= base.head.DataArrayDims.Length)
		{
			throw new Exception("Header indicates more dimensions than can be supported by the nifti format");
		}
		long noVoxelsShouldBe = 1L;
		for (int i = 1; i <= base.head.DataArrayDims[0]; i++)
		{
			noVoxelsShouldBe *= base.head.DataArrayDims[i];
		}
		if (noVoxelsShouldBe != vox.Length / SizeOfT)
		{
			throw new ArgumentException("voxel data length does not match header information regarding number and type of voxels");
		}
	}


	public T[] GetAllVoxels()
	{
		voxels.Position = 0L;
		return voxels.ReadBytes((int)voxels.Length).FromByteArray<T>();
	}

	
	public override Image<TSpace> ToImage<TSpace>(bool applyScalingFactors = true)
   {
		bool scaleVoxels = applyScalingFactors && (base.head.sclSlope != 1f || base.head.scl_inter != 0f);
		T[] voxels = GetAllVoxels();


      if (scaleVoxels)
      {
			return voxels switch
			{
				bool[] b =>		ToFloatImage<bool, TSpace>(head, b, Convert.ToSingle),
				byte[] b =>		ToFloatImage<byte, TSpace>(head, b, Convert.ToSingle),
				UInt16[] b =>	ToFloatImage<UInt16, TSpace>(head, b, Convert.ToSingle),
				UInt32[] b =>	ToFloatImage<UInt32, TSpace>(head, b, Convert.ToSingle),
				UInt64[] b => ToDoubleImage<UInt64, TSpace>(head, b, Convert.ToDouble),
				sbyte[] b =>	ToFloatImage<sbyte, TSpace>(head, b, Convert.ToSingle),
				Int16[] b =>	ToFloatImage<Int16, TSpace>(head, b, Convert.ToSingle),
				Int32[] b =>	ToFloatImage<Int32, TSpace>(head, b, Convert.ToSingle),
				Int64[] b => ToDoubleImage<Int64, TSpace>(head, b, Convert.ToDouble),
				// TO DO: reinstate once complex available again
				//Complex[] b => ToComplexImage(head,b), // NB NiftiReader does not support double-pairs at the time of writing
            double[] b =>  ToDoubleImage<TSpace>(head, b, true),
				_ => throw new NotSupportedException($"Files of type {head.dataType} are not supported")
			};
      }

		ImageHeader imHead = ImageHeader.Create(head);
#pragma warning disable CS0618 // Type or member is obsolete
      return voxels switch
      {
         // TO DO: Support other types
         bool[] b => new ImageBool<TSpace>(imHead, b),
         byte[] b => new ImageFloat<TSpace>(imHead, b.CastArrayToFloat()),
         UInt16[] b => new ImageFloat<TSpace>(imHead, b.CastArrayToFloat()),
         UInt32[] b => new ImageFloat<TSpace>(imHead, b.CastArrayToFloat()),
         UInt64[] b => new ImageDouble<TSpace>(imHead, b.CastArrayToDouble()),
         sbyte[] b => new ImageInt8<TSpace>(imHead, b),
         Int16[] b => new ImageFloat<TSpace>(imHead, b.CastArrayToFloat()),
         Int32[] b => new ImageFloat<TSpace>(imHead, b.CastArrayToFloat()),
         Int64[] b => new ImageDouble<TSpace>(imHead, b.CastArrayToDouble()),
         // TO DO: Reinstate once complex available again
         //Complex[] b => ToImage(head, torch.tensor(b), Image3dComplex.Create), // NB NiftiReader does not support double-pairs at the time of writing
         float[] b => new ImageFloat<TSpace>(imHead, b),
         double[] b => new ImageDouble<TSpace>(imHead, b),
         _ => throw new NotSupportedException($"Files of type {head.dataType} are not supported")
      };
#pragma warning restore CS0618 // Type or member is obsolete


   }

	private static ImageFloat<TSpace> ToFloatImage<TVox,TSpace>(NiftiHeader head, TVox[] unscaledVoxels, Func<TVox, float> convert)
      where TSpace : ISpace
   {
		float[] scaledVoxels = ScaleData(unscaledVoxels, convert, head.sclSlope, head.scl_inter);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageFloat<TSpace>(ImageHeader.Create(head), scaledVoxels);
#pragma warning restore CS0618 // Type or member is obsolete
   }
	private static ImageDouble<TSpace> ToDoubleImage<TVox,TSpace>(NiftiHeader head, TVox[] unscaledVoxels, Func<TVox, double> convert)
      where TSpace : ISpace
   {
		double[] scaledVoxels = ScaleData(unscaledVoxels, convert, head.sclSlope, head.scl_inter);
#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageDouble<TSpace>(ImageHeader.Create(head), scaledVoxels);
#pragma warning restore CS0618 // Type or member is obsolete
   }

   private static ImageDouble<TSpace> ToDoubleImage<TSpace>(NiftiHeader head, double[] unscaledVoxels, bool scaleVoxels)
		where TSpace : ISpace
   {
		double[] scaledVoxels = scaleVoxels ? ScaleVoxels(head, unscaledVoxels) : unscaledVoxels;

#pragma warning disable CS0618 // Type or member is obsolete
      return new ImageDouble<TSpace>(ImageHeader.Create(head), scaledVoxels);
#pragma warning restore CS0618 // Type or member is obsolete

      static double[] ScaleVoxels(NiftiHeader head, double[] unscaledVoxels)
      {
         double[] scaledVoxels = new double[unscaledVoxels.Length];
         for (int i = 0; i < unscaledVoxels.Length; i++)
         {
            scaledVoxels[i] = unscaledVoxels[i] * head.sclSlope + head.scl_inter;
         }

         return scaledVoxels;
      }
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
	
	private static TIm ToImage<TIm, TVoxel, TSpace>(NiftiHeader head, Tensor<TVoxel> scaledVoxels, Func<ImageHeader, Torch.Tensor<TVoxel>, TIm> createIm)
		where TSpace : ISpace
		where TVoxel : struct
		where TIm : Image<TVoxel, TSpace>
   {
      ImageHeader header = ImageHeader.Create(head);
      return createIm(header, scaledVoxels);

   }
	
	private static TIm ToImage<TIm, TVoxel, TSpace>(NiftiHeader head, TVoxel[] scaledVoxels, Func<ImageHeader, TVoxel[], TIm> createIm)
		where TSpace : ISpace
		where TVoxel : struct
		where TIm : Image<TVoxel, TSpace>
   {
      ImageHeader header = ImageHeader.Create(head);
      return createIm(header, scaledVoxels);

   }

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
		NiftiHeader niftiHeader = base.head.DeepClone();
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
			if (fourthDim >= base.head.DataArrayDims[4])
			{
				throw new IndexOutOfRangeException($"Size of fourth dimension is {base.head.DataArrayDims[4]} but index of {fourthDim} was specified");
			}
			int voxelsPerSlice = base.VoxelsPerVolume;
			dataToUse = new T[voxelsPerSlice];
			voxels.Position = fourthDim * voxelsPerSlice * SizeOfT;
			using BinaryReader br = new(voxels, Encoding.Default, leaveOpen: true);
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


	public XYZf Vox2WorldCoordinates(XYZf imageCoords)
	{
		if (base.head.sFormCode != CoordinateMapping_Nifti.Unknown)
      {
         return CalcWithSForm(imageCoords);
      }
      float b = base.head.quartern_b;
		float b_sq = b * b;
		float c = base.head.quartern_c;
		float c_sq = c * c;
		float d = base.head.quartern_d;
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
			[2, 0] = imageCoords.Z * base.head.PixDim[0]
		};
		DenseMatrix<float> pixDim = new(3, 1)
		{
			[0, 0] = base.head.PixDim[1],
			[1, 0] = base.head.PixDim[2],
			[2, 0] = base.head.PixDim[3]
		};

      DenseMatrix<float> result = new DenseMatrix<float>(3, 1)
		{
			[0, 0] = base.head.quartern_x,
			[1, 0] = base.head.quartern_y,
			[2, 0] = base.head.quartern_z
		} +  obj.MatMul(ijqk).MultiplyPointwise(pixDim);
		return new XYZf(result[0,0], result[1,0], result[2,0]);

      XYZf CalcWithSForm(XYZf imageCoords)
      {
         float[] srow_x = base.head.Srow_x;
         float[] srow_y = base.head.Srow_y;
         float[] srow_z = base.head.Srow_z;
         float i = imageCoords.X;
         float j = imageCoords.Y;
         float k = imageCoords.Z;
         float x = srow_x[0] * i + srow_x[1] * j + srow_x[2] * k + srow_x[3];
         float y_ = srow_y[0] * i + srow_y[1] * j + srow_y[2] * k + srow_y[3];
         float z_ = srow_z[0] * i + srow_z[1] * j + srow_z[2] * k + srow_z[3];
         return new XYZf(x, y_, z_);
      }
   }

	public XYZf World2VoxCoordinates_ScannerSpace(XYZf worldCoords)
	{
		if (base.head.qFormCode > CoordinateMapping_Nifti.Unknown)
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
			return new XYZf((float)result[0,0], (float)result[1,0], (float)result[2,0]);
		}
		throw new NotImplementedException();
	}

	public DecomposableTransform<double> GetVox2WorldMatrix_ScannerSpace()
	{
		return DecomposableTransform<double>.FromNiftiQuaternions(base.head.quartern_b, base.head.quartern_c, base.head.quartern_d, base.head.PixDim.Skip(1L).Take(3L).Select((Func<float, double>)((float entry) => entry))
			.ToArray(),
      [
         base.head.quartern_x,
			base.head.quartern_y,
			base.head.quartern_z
		], base.head.Qface);
	}

	private DenseMatrix<float> GetVox2WorldMatrix_ScannerSpace_Old()
	{
		double b = base.head.quartern_b;
		double b_sq = b * b;
		double c = base.head.quartern_c;
		double c_sq = c * c;
		double d = base.head.quartern_d;
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
		double pixSize_x = base.head.PixDim[1];
		double pixSize_y = base.head.PixDim[2];
		double pixSize_z = base.head.PixDim[3];
		denseMatrix[0, 0] = (float)((a_sq + b_sq - c_sq - d_sq) * pixSize_x);
		denseMatrix[1, 0] = (float)(2.0 * (bc + ad) * pixSize_y);
		denseMatrix[2, 0] = (float)(2.0 * (bd - ac) * pixSize_z);
		denseMatrix[0, 1] = (float)(2.0 * (bc - ad) * pixSize_x);
		denseMatrix[1, 1] = (float)((a_sq + c_sq - b_sq - d_sq) * pixSize_y);
		denseMatrix[2, 1] = (float)(2.0 * (cd + ab) * pixSize_z);
		denseMatrix[0, 2] = (float)(2.0 * (bd + ac) * pixSize_x);
		denseMatrix[1, 2] = (float)(2.0 * (cd - ab) * pixSize_y);
		denseMatrix[2, 2] = (float)((a_sq + d_sq - b_sq - c_sq) * pixSize_z);
		denseMatrix[0, 3] = (float)((double)base.head.quartern_x * pixSize_x);
		denseMatrix[1, 3] = (float)((double)base.head.quartern_y * pixSize_y);
		denseMatrix[2, 3] = (float)((double)base.head.quartern_z * pixSize_z);
		return denseMatrix;
	}

	public DenseMatrix<float> GetVox2StdSpaceMatrix_old()
	{
		return new DenseMatrix<float>(4, 4)
		{
			[0, 0] = base.head.Srow_x[0],
			[0, 1] = base.head.Srow_x[1],
			[0, 2] = base.head.Srow_x[2],
			[0, 3] = base.head.Srow_x[3],
			[1, 0] = base.head.Srow_y[0],
			[1, 1] = base.head.Srow_y[1],
			[1, 2] = base.head.Srow_y[2],
			[1, 3] = base.head.Srow_y[3],
			[2, 0] = base.head.Srow_z[0],
			[2, 1] = base.head.Srow_z[1],
			[2, 2] = base.head.Srow_z[2],
			[2, 3] = base.head.Srow_z[3],
			[3, 3] = 1f
		};
	}

	public Matrix4x4_Optimised<float> GetVox2StdSpaceMatrix()
	{
		return new Matrix4x4_Optimised<float>
		{
			_0_0 = base.head.Srow_x[0],
			_0_1 = base.head.Srow_x[1],
			_0_2 = base.head.Srow_x[2],
			_0_3 = base.head.Srow_x[3],
			_1_0 = base.head.Srow_y[0],
			_1_1 = base.head.Srow_y[1],
			_1_2 = base.head.Srow_y[2],
			_1_3 = base.head.Srow_y[3],
			_2_0 = base.head.Srow_z[0],
			_2_1 = base.head.Srow_z[1],
			_2_2 = base.head.Srow_z[2],
			_2_3 = base.head.Srow_z[3],
			_3_3 = 1f
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
		NiftiHeader niftiHeader = vols.First().head.DeepClone();
		niftiHeader.DataArrayDims[0] = 4;
		LargeMemoryStream allData = vols.Select((NiftiFile<T> a) => a.voxels).Concatenate();
		return new NiftiFile<T>(niftiHeader, allData);
	}

	private static void ConcatenateTimeSeries_Checks(IEnumerable<NiftiFile<T>> vols, bool allowMisalignedImages = false)
	{
		if (vols.Select((NiftiFile<T> a) => a.head.bitPix).Distinct().Count() > 1)
		{
			throw new ArgumentException("Bit depths do not match");
		}
		if (vols.Select((NiftiFile<T> a) => a.head.DataArrayDims).Distinct(new ArrayComparer_Int16()).Count() > 1)
		{
			throw new ArgumentException("Data dimensions do not match");
		}
		if (!allowMisalignedImages && (vols.Select((NiftiFile<T> a) => a.head.qFormCode).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.head.quartern_b).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.head.quartern_c).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.head.quartern_d).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.head.quartern_x).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.head.quartern_y).Distinct().Count() > 1 || vols.Select((NiftiFile<T> a) => a.head.quartern_z).Distinct().Count() > 1))
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
		NiftiHeader niftiHeader = vols.First().head.DeepClone();
		niftiHeader.DataArrayDims[4] = (short)vols.Sum((NiftiFile<T> a) => a.head.DataArrayDims[4]);
		LargeMemoryStream allData = vols.Select((NiftiFile<T> a) => a.voxels).Concatenate();
		return new NiftiFile<T>(niftiHeader, allData);
	}

	public void DeleteVolumes_AllButOne(int vol)
	{
		if (vol >= base.head.DataArrayDims[4])
		{
			throw new ArgumentException("Nifti file does not have that many dimensions");
		}
		if (vol < 0)
		{
			throw new ArgumentException("volume must be positive number");
		}
		int bytesPerVolume = base.VoxelsPerVolume * GenMethods.SizeOfType(Array.Empty<T>(), dotNetSizeForBoolean: true);
		byte[] volData = new byte[bytesPerVolume];
		voxels.Position = vol * bytesPerVolume;
		voxels.Read(volData, 0, volData.Length);
		voxels.SetLength(bytesPerVolume);
		voxels.Position = 0L;
		voxels.Write(volData, 0, volData.Length);
		base.head.DataArrayDims[4] = 1;
	}

	public override float[] GetDataAsFloat()
	{
		voxels.Position = 0L;
      using BinaryReader br = new(voxels, Encoding.Default, leaveOpen: true);
		float[] asF = new float[base.VoxelCount];
		br.ReadDataToFillArray_f(asF);
		return asF;
	}


}
