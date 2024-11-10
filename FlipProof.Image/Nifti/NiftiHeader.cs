using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using FlipProof.Image;
using FlipProof.Image.Maths;
using FlipProof.Image.Matrices;

namespace FlipProof.Image.Nifti;

public class NiftiHeader : IEquatable<NiftiHeader>, IImageHeader
{
	/// <summary>
	/// The field char dim_info stores, in just one byte, the frequency encoding direction (1, 2 or 3), 
	/// the phase encoding direction (1, 2 or 3), and the direction in which the volume was sliced during 
	/// the acquisition (1, 2 or 3). For spiral sequences, frequency and phase encoding are both set as 0. 
	/// The reason to collapse all this information in just one byte was to save space. See also the fields 
	/// short slice_start, short slice_end, char slice_code and float slice_duration.
	/// </summary>
	public byte dimInfo;

	/// <summary>
	/// 1 indexed. 0 indicates not applicable
	/// </summary>
	public int FrequencyEncodingDimension => (((dimInfo)) & 0x03);
	/// <summary>
	/// 1 indexed. 0 indicates not applicable
	/// </summary>
	public int PhaseEncodingDimension => (((dimInfo) >> 2) & 0x03);
	/// <summary>
	/// 1 indexed. 0 indicates not applicable
	/// </summary>
	public int SliceEncodingDimension => (((dimInfo) >> 4) & 0x03);

	CoordinateSystem IImageHeader.CoordinateSystem => CoordinateSystem.RAS;
	EncodingDirection IImageHeader.PhaseEncodingDimension => (EncodingDirection)(PhaseEncodingDimension - 1);
	EncodingDirection IImageHeader.FrequencyEncodingDimension => (EncodingDirection)(FrequencyEncodingDimension - 1);
	EncodingDirection IImageHeader.SliceEncodingDimension => (EncodingDirection)(SliceEncodingDimension - 1);

	private short[] _dataArrayDims = new short[8] { 1, 1, 1, 1, 1, 1, 1, 1 };

	public NiftiIntent intent;

	public float intentParam1;

	public float intentParam2;

	public float intentParam3;

	public string intentName;

	public DataType dataType;

	public short bitPix;

	public byte sliceCode;

	public short sliceStart;

	public short sliceLast;

	public float sliceDuration;

	private float[] pixDim = new float[8] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };

	public float voxOffset = 352f;

	public float sclSlope = 1f;

	public float scl_inter;

	public MeasurementUnits xyztUnits;

	public float calMax;

	public float calMin;

	public float tOffset;

	private string _description = "";

	private string _auxFile = "";

	public CoordinateMapping_Nifti qFormCode;

	public CoordinateMapping_Nifti sFormCode;

	public float quartern_b;

	public float quartern_c;

	public float quartern_d;

	public float quartern_x;

	public float quartern_y;

	public float quartern_z;

	private float[] srow_x = new float[4];

	private float[] srow_y = new float[4];

	private float[] srow_z = new float[4];

	private static readonly byte[] magic_singlefile = new byte[4] { 110, 43, 49, 0 };

	private static readonly byte[] magic_filePair = new byte[4] { 110, 105, 49, 0 };

	internal byte[] magicString;

	private List<KeyValuePair<HeaderExtraType, string>> headerExtras = new List<KeyValuePair<HeaderExtraType, string>>();

	public short[] DataArrayDims => _dataArrayDims;

	private int NoDimensions => _dataArrayDims[0];

	public string IntentName_Trimmed => intentName.TrimEnd(new char[1]);

	public float[] PixDim => pixDim;

	public int VoxOffset => (int)voxOffset;

	public MeasurementUnits UnitsXYZ => xyztUnits & MeasurementUnits.MicroMeter;

	public MeasurementUnits UnitsTime => xyztUnits & (MeasurementUnits)252;

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			if (value.Length > 80)
			{
				_description = value.Remove(80);
			}
			else
			{
				_description = value.PadRight(80);
			}
		}
	}

	public string Description_Trimmed => _description.TrimEnd(new char[1]);

	public string AuxFile
	{
		get
		{
			return _auxFile;
		}
		set
		{
			if (value.Length > 24)
			{
				_auxFile = value.Remove(24);
			}
			else
			{
				_auxFile = value.PadRight(24);
			}
		}
	}

	public string AuxFile_Trimmed => _auxFile.TrimEnd(new char[1]);

	public int Qface => (int)pixDim[0];

	public float[] Srow_x => srow_x;

	public float[] Srow_y => srow_y;

	public float[] Srow_z => srow_z;

	public static IEnumerable<byte> Magic_singlefile => magic_singlefile.AsEnumerable();

	public static IEnumerable<byte> Magic_FilePair => magic_filePair.AsEnumerable();

	public List<KeyValuePair<HeaderExtraType, string>> HeaderExtras => headerExtras;

	ImageSize IImageHeader.Size
	{
		get
		{
			uint noVols = NoDimensions < 4 ? 1 : (uint)_dataArrayDims[4];

			return new ImageSize(noVols, (uint)_dataArrayDims[1], (uint)_dataArrayDims[2], (uint)_dataArrayDims[3]);
		}
	}
	/// <summary>
	/// Returns the patient-scanner matrix, if available, else the standard space matrix. An exception is thrown if using the analyse orientation (qform and sform both zero).
	/// </summary>
	System.Numerics.Matrix4x4 IImageHeader.Orientation
	{
		get
		{
         // Images only have one coordinate system
         // Nifti offers two coordinate systems in each file
         // These are:
         // qform_code = 0
         //		There is only voxel size. There is no orientation information
         // qform_code > 0
         //		The normal way. The nifti documentation states "The (x,y,z) coordinates
         //		are given by the pixdim[] scales, a rotation matrix, and a shift.
         // sform_code > 0
         //		The (x,y,z) coordinates are given by a general affine transformation
         //
         // The docs state we may have two coordinate systems (1 only, 2 only, or 3 only, or 2 + 3)
         // It also states:
         // Method 1 is provided only for backwards compatibility.  The intention
         // is that Method 2(qform_code > 0) represents the nominal voxel locations
         // as reported by the scanner, or as rotated to some fiducial orientation and
         // location.Method 3, if present(sform_code > 0), is to be used to give
         // the location of the voxels in some standard space.
         //
         // So priority here is:
         // Method 2
         // Method 3 
         // Method 1

			if (qFormCode > 0)
			{
				return GetVox2WorldMatrix_ScannerSpace();
         }
			else if (sFormCode > 0)
			{
				return GetVox2StdSpaceMatrix();
			}
			else
			{
				throw new OrientationException("Nifti file does not contain orientation information (sform and qform codes both zero). While valid, this is considered unsafe and is disallowed by this framework");
			}
		}
	}

   private Matrix4x4 GetVox2StdSpaceMatrix()
   {
      return new Matrix4x4()
      {
         M11 = Srow_x[0],
         M12 = Srow_x[1],
         M13 = Srow_x[2],
         M14 = Srow_x[3],
         M21 = Srow_y[0],
         M22 = Srow_y[1],
         M23 = Srow_y[2],
         M24 = Srow_y[3],
         M31 = Srow_z[0],
         M32 = Srow_z[1],
         M33 = Srow_z[2],
         M34 = Srow_z[3],
         M44 = 1f
      };
   }



   public NiftiHeader()
		: this(setMagicString: false)
	{
	}

	public static NiftiHeader FromVolume<TSpace,TVoxel>(Image<TVoxel, TSpace> v0) 
		where TVoxel : struct, INumber<TVoxel>
		where TSpace: ISpace
	{
		float calMax = Convert.ToSingle(v0.GetMaxIntensity());
		float calMin = Convert.ToSingle(v0.GetMinIntensity());
		if (float.IsNaN(calMax))
		{
			calMax = 0f;
		}
		if (float.IsNaN(calMin))
		{
			calMin = 0f;
		}

		NiftiHeader header = Create(v0.Header);
		header.calMin = calMin;
		header.calMax = calMax;
		header.bitPix = typeof(TVoxel).Type2DataType(true).BitsPerPixel();
		
		return header;

	}

	public NiftiHeader(bool setMagicString)
	{
		magicString = magic_singlefile.ToArray();
	}

   public Matrix4x4 GetVox2WorldMatrix_ScannerSpace()
	{
      DenseMatrix<double> d = GetVox2WorldDecomposableMatrix_ScannerSpace().FastCalcMat;
      return new Matrix4x4()
      {
         M11 = (float)d[0, 0],
         M21 = (float)d[1, 0],
         M31 = (float)d[2, 0],
         M41 = 0f,

         M12 = (float)d[0, 1],
         M22 = (float)d[1, 1],
         M32 = (float)d[2, 1],
         M42 = 0f,

         M13 = (float)d[0, 2],
         M23 = (float)d[1, 2],
         M33 = (float)d[2, 2],
         M43 = 0f,

         M14 = (float)d[0, 3],
         M24 = (float)d[1, 3],
         M34 = (float)d[2, 3],
         M44 = 1f,

      };
   }
   public DecomposableTransform<double> GetVox2WorldDecomposableMatrix_ScannerSpace()
   {
      return DecomposableTransform<double>.FromNiftiQuaternions(quartern_b, quartern_c, quartern_d, PixDim.Skip(1L).Take(3L).Select(Convert.ToDouble).ToArray(),
      [
         quartern_x,
         quartern_y,
         quartern_z
      ], Qface);

   }
   public void RemoveFlatDimensions(int startAtIndex = 3)
	{
		for (int i = startAtIndex; i < _dataArrayDims.Length; i++)
		{
			if (_dataArrayDims[i] == 1)
			{
				for (int j = i; j < _dataArrayDims.Length - 1; j++)
				{
					_dataArrayDims[j] = _dataArrayDims[j + 1];
					pixDim[j] = pixDim[j + 1];
				}
				_dataArrayDims[_dataArrayDims.Length - 1] = 1;
				pixDim[pixDim.Length - 1] = 1f;
			}
		}
	}

	private void SetQuarternsFromVox2WorldMatrix_ScannerSpace(DecomposableTransform<double> trans)
	{
		trans.ToNiftiQuaternions(out var b, out var c, out var d, out var pixDims, out var _, out var qFac);
		PixDim[0] = (float)qFac;
		PixDim[1] = (float)pixDims[0];
		PixDim[2] = (float)pixDims[1];
		PixDim[3] = (float)pixDims[2];
		quartern_b = (float)b;
		quartern_c = (float)c;
		quartern_d = (float)d;
		double[] translation = trans.GetTranslation();
		quartern_x = (float)translation[0];
		quartern_y = (float)translation[1];
		quartern_z = (float)translation[2];
	}

	public bool Verify(IList<string> warnings, out string? errMsg)
	{
		if (bitPix != dataType.BitsPerPixel())
		{
			errMsg = "Bits per pixel does not match data type";
		}
		else if (!magicString.SequenceEqual(magic_filePair) && !magicString.SequenceEqual(magic_singlefile))
		{
			errMsg = "Magic string is not valid";
		}
		else
		{
			errMsg = null;
		}
		if (pixDim[0] != 1f && pixDim[0] != -1f)
		{
			warnings.Add("pixel dimensions entry [0] was " + pixDim[0] + ". It should always be 1 or -1. Value of 1 assumed");
			pixDim[0] = 1f;
		}
		return errMsg == null;
	}

	public void StripOrientation()
	{
		quartern_b = 0f;
		quartern_c = 0f;
		quartern_d = 1f;
		quartern_x = 0f;
		quartern_y = 0f;
		quartern_z = 0f;
		srow_x[0] = 0f;
		srow_x[1] = 0f;
		srow_x[2] = 0f;
		srow_x[3] = 0f;
		srow_y[0] = 0f;
		srow_y[1] = 0f;
		srow_y[2] = 0f;
		srow_y[3] = 0f;
		srow_z[0] = 0f;
		srow_z[1] = 0f;
		srow_z[2] = 0f;
		srow_z[3] = 0f;
	}

	public void CopyOrientationEtc(NiftiHeader other)
	{
		if (!other.pixDim.Take(other.NoDimensions + 1).SequenceEqual(pixDim.Take(NoDimensions + 1)))
		{
			throw new ArgumentException("Image dimensions mismatch");
		}
		if (!other.DataArrayDims.Take(other.NoDimensions + 1).SequenceEqual(DataArrayDims.Take(NoDimensions + 1)))
		{
			throw new ArgumentException("Image dimensions mismatch");
		}
		quartern_b = other.quartern_b;
		quartern_c = other.quartern_c;
		quartern_d = other.quartern_d;
		quartern_x = other.quartern_x;
		quartern_y = other.quartern_y;
		quartern_z = other.quartern_z;
		srow_x[0] = other.srow_x[0];
		srow_x[1] = other.srow_x[1];
		srow_x[2] = other.srow_x[2];
		srow_x[3] = other.srow_x[3];
		srow_y[0] = other.srow_y[0];
		srow_y[1] = other.srow_y[1];
		srow_y[2] = other.srow_y[2];
		srow_y[3] = other.srow_y[3];
		srow_z[0] = other.srow_z[0];
		srow_z[1] = other.srow_z[1];
		srow_z[2] = other.srow_z[2];
		srow_z[3] = other.srow_z[3];
		qFormCode = other.qFormCode;
		sFormCode = other.sFormCode;
		xyztUnits = other.xyztUnits;
		pixDim = other.pixDim.ToArray();
	}

	public NiftiHeader DeepClone()
	{
		NiftiHeader clone = (NiftiHeader)MemberwiseClone();
		clone._dataArrayDims = new short[_dataArrayDims.Length];
		Array.Copy(_dataArrayDims, clone._dataArrayDims, _dataArrayDims.Length);
		clone.headerExtras = new List<KeyValuePair<HeaderExtraType, string>>();
		clone.headerExtras.AddRange(headerExtras);
		clone.magicString = new byte[magicString.Length];
		magicString.CopyTo(clone.magicString, 0);
		clone.pixDim = pixDim.Duplicate();
		clone.srow_x = srow_x.Duplicate();
		clone.srow_y = srow_y.Duplicate();
		clone.srow_z = srow_z.Duplicate();
		return clone;
	}

	public bool Equals(NiftiHeader other)
	{
		return Equals(other, strict: true, checkIntensityParams: true, ignoreStringPadding: false);
	}

	public bool Equals(NiftiHeader other, bool strict, bool checkIntensityParams, bool ignoreStringPadding)
	{
		if (dimInfo != other.dimInfo || !_dataArrayDims.SequenceEqual_Fast(other._dataArrayDims))
		{
			return false;
		}
		if (intent != other.intent || intentParam1 != other.intentParam1 || intentParam2 != other.intentParam3 || intentParam2 != other.intentParam3 || !CompareString(intentName, other.intentName))
		{
			return false;
		}
		bool sizeAndTypeOK = dataType == other.dataType && bitPix == other.bitPix && sliceCode == other.sliceCode && sliceStart == other.sliceStart && sliceLast == other.sliceLast && sliceDuration == other.sliceDuration && voxOffset == other.voxOffset;
		if (strict)
		{
			sizeAndTypeOK = sizeAndTypeOK && pixDim.SequenceEqual(other.pixDim);
		}
		else
		{
			int noDims = NoDimensions;
			sizeAndTypeOK = sizeAndTypeOK && pixDim.Take(noDims + 1).SequenceEqual(other.pixDim.Take(noDims + 1));
		}
		if (!sizeAndTypeOK)
		{
			return false;
		}
		if ((checkIntensityParams && (sclSlope != other.sclSlope || scl_inter != other.scl_inter || calMax != other.calMax || calMin != other.calMin)) || xyztUnits != other.xyztUnits || tOffset != other.tOffset)
		{
			return false;
		}
		if (strict && (!CompareString(_description, other._description) || !CompareString(_auxFile, other._auxFile)))
		{
			return false;
		}
		if (qFormCode != other.qFormCode || sFormCode != other.sFormCode || quartern_b != other.quartern_b || quartern_c != other.quartern_c || quartern_d != other.quartern_d || quartern_x != other.quartern_x || quartern_y != other.quartern_y || quartern_z != other.quartern_z || !srow_x.SequenceEqual(other.srow_x) || !srow_y.SequenceEqual(other.srow_y) || !srow_z.SequenceEqual(other.srow_z))
		{
			return false;
		}
		bool rest = magicString.SequenceEqual(other.magicString);
		if (strict)
		{
			if (headerExtras.Count == other.headerExtras.Count)
			{
				for (int i = 0; i < headerExtras.Count; i++)
				{
					rest = rest && headerExtras[i].Key == other.headerExtras[i].Key && headerExtras[i].Value == other.headerExtras[i].Value;
				}
			}
			else
			{
				rest = false;
			}
		}
		return rest;
		bool CompareString(string a, string b)
		{
			a ??= "";
			b ??= "";
			if (ignoreStringPadding)
			{
				a = a.TrimEnd(new char[1]);
				b = b.TrimEnd(new char[1]);
			}
			return a.Equals(b);
		}
	}

	IImageHeader IImageHeader.Create(IImageHeader other)
	{
		return Create(other);
	}
	/// <summary>
	/// Creates a nifti header from a generic image header
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	/// <exception cref="NotSupportedException"></exception>
   public static NiftiHeader Create(IImageHeader other)
	{
		if (other.CoordinateSystem != CoordinateSystem.RAS)
		{
			throw new NotSupportedException("Only RAS is supported");
		}

		CoordinateMapping_Nifti qCode;
		CoordinateMapping_Nifti sCode;

      float qa, qb, qc, qd;
		float translationX, translationY, translationZ;
		if (Matrix4x4.Decompose(other.Orientation, out var scale, out var rotationQuaternions, out var translation))
		{
			qa = rotationQuaternions.W;
			qb = rotationQuaternions.X;
			qc = rotationQuaternions.Y;
			qd = rotationQuaternions.Z;
			translationX = translation.X;
			translationY = translation.Y;
			translationZ = translation.Z;
			qCode = CoordinateMapping_Nifti.ScannerAnat;
         sCode = CoordinateMapping_Nifti.Unknown;
      }
		else
		{
         qCode = CoordinateMapping_Nifti.Unknown;
         sCode = CoordinateMapping_Nifti.ScannerAnat;

         qa = qb = qc = qd = 0;
         translationX = translationY = translationZ = 0;

      }
		float[]	pixelDims = new float[8] { 
												other.Size.NDims, 
												other.Orientation.GetVoxelSizeX(), 
												other.Orientation.GetVoxelSizeY(),
												other.Orientation.GetVoxelSizeZ(), 
												1, 1, 1, 1, };

		// first slice is inclusice, 1 indexed
		// last slice is inclusive, 1-indexed
		uint lastSlice = other.SliceEncodingDimension switch
		{
			EncodingDirection.UnknownOrNA => other.Size.X,
			EncodingDirection.X => other.Size.X,
			EncodingDirection.Y => other.Size.Y,
			EncodingDirection.Z => other.Size.Z,
			_ => throw new Exception("Unexpected Encoding direction")
		};

		var header = new NiftiHeader(true)
		{
			magicString = magic_singlefile.ToArray(),
         bitPix = 8,
			calMax = 1,
			calMin = 0,
			dataType = DataType.signedChar,
			qFormCode = qCode,
			sFormCode = sCode,
			sliceCode = 0, // not supported
			xyztUnits = MeasurementUnits.Milimeter | MeasurementUnits.Seconds,
			pixDim = pixelDims,
			dimInfo = GetDimInfo(other),
			_auxFile = string.Empty,
			_description = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"), // Date stamp the file in the description
			headerExtras = new(),
			intent = NiftiIntent.None,
			intentName = string.Empty,
			intentParam1 = default,
			intentParam2 = default,
			intentParam3 = default,
			quartern_b = (float)qb,
			quartern_c = (float)qc,
			quartern_d = (float)qd,
			quartern_x = translationX,
			quartern_y = translationY,
			quartern_z = translationZ,
			sclSlope = 1f,
			scl_inter = 0f,
			srow_x = [other.Orientation.M11, other.Orientation.M12, other.Orientation.M13, other.Orientation.M14],
			srow_y = [other.Orientation.M21, other.Orientation.M22, other.Orientation.M23, other.Orientation.M24],
			srow_z = [other.Orientation.M31, other.Orientation.M32, other.Orientation.M33, other.Orientation.M34],
			sliceDuration = 0, // not supported
			sliceStart = 1, // inclusive, 1-indexed
			sliceLast = (short)lastSlice, // inclusive, 1-indexed
         tOffset = 0, // not supported
			_dataArrayDims = GetDataArrayDimensions(other),
			voxOffset = 0,// set upon write
		};


		if(!header.Verify([], out string? error))
		{
			throw new Exception(error);
		}
		return header;
	}

	

	private static short[] GetDataArrayDimensions(IImageHeader other)
	{
		return other.Size.Select(Convert.ToInt16).Concat_Start((short)other.Size.NDims).ToArray();
	}

   private static byte GetDimInfo(IImageHeader head)
	{ 
		if((int)head.PhaseEncodingDimension < -1 || (int)head.PhaseEncodingDimension > 2 ||
        (int)head.FrequencyEncodingDimension < -1 || (int)head.FrequencyEncodingDimension > 2 ||
        (int)head.SliceEncodingDimension < -1 || (int)head.SliceEncodingDimension > 2)
      {
			throw new NotSupportedException("Nifit images require phase, freq, and slice directions to be the first dimensions of an image, or unset");
		}
		return GetDimInfo((int)head.FrequencyEncodingDimension + 1,
								(int)head.PhaseEncodingDimension + 1,
								(int)head.SliceEncodingDimension + 1);
	}
   /// <summary>
   /// Compresses freq, phase, and slice dimensions into a single byte.
   /// </summary>
   /// <param name="freqDimension">1 indexed or 0 to indicate not applicable</param>
   /// <param name="phaseDimension">1 indexed or 0 to indicate not applicable</param>
   /// <param name="sliceDimension">1 indexed or 0 to indicate not applicable</param>
   /// <returns></returns>
   private static byte GetDimInfo(int freqDimension, int phaseDimension, int sliceDimension) =>
      (byte)(((((byte)(freqDimension)) & 0x03)) |
      ((((byte)(phaseDimension)) & 0x03) << 2) |
      ((((byte)(sliceDimension)) & 0x03) << 4));

}