using System.Diagnostics.CodeAnalysis;
using FlipProof.Base;
using FlipProof.Image;
using FlipProof.Image.IO;
using FlipProof.Torch;

namespace FlipProof.Image.Nifti;

public class NiftiReader(Stream unzippedStream) : NiftiReaderBase(new BinaryReader(unzippedStream))
{
	internal const int headerSizeMustBe = 348;

	public NiftiReader(string filename) : this(Gen.GetUnzippedStream(filename, returnedStreamMustBeSeekable: true))
	{
	}

   /// <summary>
   /// 
   /// </summary>
   /// <typeparam name="TVoxel">The anticipated nifti data type</typeparam>
   /// <typeparam name="TSpace"></typeparam>
   /// <param name="folder"></param>
   /// <returns></returns>
   public static Image<TSpace>[] ReadSeriesToVolume<TVoxel,TSpace>(FilePath folder) where TVoxel : struct, IComparable<TVoxel>, IComparable, IEquatable<TVoxel>
		where TSpace:ISpace
	{
		return (from a in ReadSeries<TVoxel>(folder)
			select a.ToImage<TSpace>(applyScalingFactors: true)).ToArray();
	}

	public static NiftiFile<T>[] ReadSeries<T>(FilePath folder) where T : struct, IComparable<T>, IComparable, IEquatable<T>
	{
      FilePath[] filenames = FlipProof.Image.IO.Gen.GetNiftiFilesLoc(folder).ToArray();
		NiftiFile<T>[] read = new NiftiFile<T>[filenames.Length];
      Iteration.Loop_Parallel(0, read.Length, delegate(int i)
		{
			if (TryRead(filenames[i], lookForZippedVariantIfNotFound: false, out NiftiFile<T>? readAndConverted, out string err))
			{
				read[i] = readAndConverted;
				return;
			}
			throw new Exception("Error opening file " + filenames[i] + ". Message: " + err);
		});
		return read;
	}

	public static ImageFloat<TSpace>[] ReadSeriesToFloat<TSpace>(FilePath folder)
		where TSpace:ISpace
	{
      FilePath[] filenames = FlipProof.Image.IO.Gen.GetNiftiFilesLoc(folder).ToArray();
		ImageFloat<TSpace>[] read = new ImageFloat<TSpace>[filenames.Length];
      Iteration.Loop_Parallel(0, read.Length, delegate(int i)
		{
			read[i] = ReadToFloat<TSpace>(filenames[i]);
		});
		return read;
	}

	public static ImageDouble<TSpace>[] ReadSeriesToDouble<TSpace>(FilePath folder, bool alsoLookForZippedFiles = false)
		where TSpace:ISpace
	{
		FilePath[] filenames = ((!alsoLookForZippedFiles) ? FlipProof.Image.IO.Gen.GetNiftiFilesLoc_UnzippedOnly(folder).ToArray() : FlipProof.Image.IO.Gen.GetNiftiFilesLoc(folder).ToArray());
		return ReadSeriesToDouble<TSpace>(filenames, lookForZippedVariantIfNotFound: false);
	}

	public static ImageDouble<TSpace>[] ReadSeriesToDouble<TSpace>(FilePath[] filenames, bool lookForZippedVariantIfNotFound = true)
		where TSpace:ISpace
	{
		ImageDouble<TSpace>[] read = new ImageDouble<TSpace>[filenames.Length];
		Iteration.Loop_Parallel(0, read.Length, delegate(int i)
		{
			read[i] = ReadToDouble<TSpace>(filenames[i], lookForZippedVariantIfNotFound, out _);
		});
		return read;
	}
	public static ImageDouble<TSpace4D> Read3DSeriesTo4DDouble<TSpace3D, TSpace4D>(FilePath[] filenames, bool lookForZippedVariantIfNotFound = true)
      where TSpace3D:ISpace
      where TSpace4D:ISpace<TSpace3D>

   {
		ImageDouble<TSpace3D>[] read = new ImageDouble<TSpace3D>[filenames.Length];
		Iteration.Loop_Parallel(0, read.Length, delegate(int i)
		{
			read[i] = ReadToDouble<TSpace3D>(filenames[i], lookForZippedVariantIfNotFound, out _);
		});

		return read.ConcatIImage<TSpace3D, TSpace4D>();
	}

	public static ImageFloat<TSpace> ReadToFloat<TSpace>(FilePath fileLoc, bool lookForZippedVariantIfNotFound = true)
		where TSpace :ISpace
	{
		NiftiFile_Base origNifti;
		return ReadToFloat<TSpace>(fileLoc, lookForZippedVariantIfNotFound, out origNifti);
	}

	public static ImageFloat<TSpace> ReadToFloat<TSpace>(FilePath fileLoc, out NiftiFile_Base origNifti)
      where TSpace : ISpace
   {
      return ReadToFloat<TSpace>(fileLoc, lookForZippedVariantIfNotFound: false, out origNifti);
	}
   public static ImageBool<TSpace> ReadToBool<TSpace>(FilePath fileLoc, bool lookForZippedVariantIfNotFound, out NiftiFile_Base origNifti)
      where TSpace : ISpace
   {

      origNifti = Read(fileLoc, lookForZippedVariantIfNotFound);
      var im = origNifti.ToImage<TSpace>();

      ImageBool<TSpace> asB = im.ToBool();
      if (!object.ReferenceEquals(asB, im))
      {
         im.Dispose();
      }
      return asB;
   }

   public static ImageFloat<TSpace> ReadToFloat<TSpace>(FilePath fileLoc, bool lookForZippedVariantIfNotFound, out NiftiFile_Base origNifti)
      where TSpace : ISpace
   {

      origNifti = Read(fileLoc, lookForZippedVariantIfNotFound);
		var im = origNifti.ToImage<TSpace>();
		
		ImageFloat<TSpace> asF = im.ToFloat();
		if(!object.ReferenceEquals(asF, im))
		{
			im.Dispose();
		}
		return asF;
	}

   public static ImageDouble<TSpace> ReadToDouble<TSpace>(FilePath fileLoc, bool lookForZippedVariantIfNotFound, out NiftiFile_Base origNifti)
		where TSpace : ISpace
   {
      origNifti = Read(fileLoc, lookForZippedVariantIfNotFound);
      var im = origNifti.ToImage<TSpace>();

      ImageDouble<TSpace> asD = im.ToDouble();
      if (!object.ReferenceEquals(asD, im))
      {
         im.Dispose();
      }
      return asD;
   }
   private static void Checkfilename(ref FilePath fileLoc, bool changeFilenameToZippedVariantIfNotFound)
	{
		if (changeFilenameToZippedVariantIfNotFound)
		{
			ChangeFilenameIfZippedVariantNotFound(ref fileLoc);
		}
		if (!File.Exists(fileLoc))
		{
			throw new FileNotFoundException("Could not find file " + fileLoc + (changeFilenameToZippedVariantIfNotFound ? " or zipped/unzipped variant" : ""));
		}
	}

	private static void ChangeFilenameIfZippedVariantNotFound(ref FilePath fileLoc)
	{
		if (!File.Exists(fileLoc))
		{
			if (fileLoc.AbsolutePath.EndsWith(".gz"))
			{
				fileLoc = fileLoc.AbsolutePath.Substring(0, fileLoc.AbsolutePath.Length - 3);
			}
			else
			{
				fileLoc += ".gz";
			}
		}
	}
	

	public static Image<TSpace> ReadToVolume<TSpace>(FilePath fileLoc, bool lookForZippedVariantIfNotFound)
	where TSpace : ISpace
	{
      return ReadToVolume<TSpace>(fileLoc, lookForZippedVariantIfNotFound, out _);
   }


	public static Image<TSpace> ReadToVolume<TSpace>(FilePath fileLoc, bool lookForZippedVariantIfNotFound, out NiftiFile_Base? origNifti)
   where TSpace : ISpace
   {
      Checkfilename(ref fileLoc, lookForZippedVariantIfNotFound);
		using NiftiReader nr = new NiftiReader(fileLoc);
		if (!nr.TryRead(out var err, out origNifti))
		{
			throw new Exception(err);
		}
		return origNifti.ToImage<TSpace>();
	}
	
	public static NiftiFile<T> Read<T>(FilePath fileLoc, bool lookForZippedVariantIfNotFound) where T : struct, IComparable<T>, IComparable, IEquatable<T>
	{
		if (TryRead(fileLoc, lookForZippedVariantIfNotFound, out NiftiFile<T>? readAndConverted, out string err))
		{
			return readAndConverted;
		}
		throw new Exception(err);
	}

	public static bool TryRead<T>(FilePath fileLoc, bool lookForZippedVariantIfNotFound, [NotNullWhen(true)] out NiftiFile<T>? readAndConverted, out string err) where T : struct, IComparable<T>, IComparable, IEquatable<T>
	{
		try
		{
			Checkfilename(ref fileLoc, lookForZippedVariantIfNotFound);
			using NiftiReader nr = new(fileLoc);
			if (!nr.TryRead(out err, out var read))
			{
				readAndConverted = null;
				return false;
			}
			if (read is NiftiFile<T> nf)
			{
				readAndConverted = nf;
				return true;

			}
			readAndConverted = null;
			err = "File is type " + read.GetType().ToString() + " not type of NiftiFile<" + typeof(T).Name + ">";
			return false;
		}
		catch (Exception e)
		{
			readAndConverted = null;
			err = e.ToString();
			return false;
		}
	}

	/// <summary>
	/// Reads the file, throwing if unsuccessful
	/// </summary>
	/// <param name="fileLoc">Path to the nifti</param>
	/// <param name="lookForZippedVariantIfNotFound"></param>
	/// <returns></returns>
	/// <exception cref="Exception">The file could not be read successfully</exception>
	public static NiftiFile_Base Read(FilePath fileLoc, bool lookForZippedVariantIfNotFound)
	{
		if(TryRead(fileLoc, lookForZippedVariantIfNotFound, out NiftiFile_Base? read, out string error))
		{
			return read;
		}
		throw new Exception(error);
	}
	public static bool TryRead(FilePath fileLoc, bool lookForZippedVariantIfNotFound, [NotNullWhen(true)] out NiftiFile_Base? read, out string err)
	{
		try
		{
			Checkfilename(ref fileLoc, lookForZippedVariantIfNotFound);
			using NiftiReader nr = new NiftiReader(fileLoc);
			return nr.TryRead(out err, out read);
		}
		catch (Exception e)
		{
			read = null;
			err = e.ToString();
			return false;
		}
	}

	public bool TryRead(out string msg, [NotNullWhen(true)] out NiftiFile_Base? nf)
	{
		List<string> warnings = new();
		nf = null;
		int headerSize = br.ReadInt32();
		if (headerSize != 348)
		{
			if (headerSize == 1543569408)
			{
				msg = "big endian file format not supported";
			}
			else
			{
				msg = "Header size was not " + 348 + " bytes. It was " + headerSize;
			}
			return false;
		}
		br.BaseStream.Seek(39L, SeekOrigin.Begin);
		NiftiHeader nh = new();
		nh.dimInfo = br.ReadByte();
		for (int i = 0; i < 8; i++)
		{
			nh.DataArrayDims[i] = br.ReadInt16();
		}
		nh.intentParam1 = br.ReadSingle();
		nh.intentParam2 = br.ReadSingle();
		nh.intentParam3 = br.ReadSingle();
		nh.intent = (NiftiIntent)br.ReadInt16();
		nh.dataType = (DataType)br.ReadInt16();
		nh.bitPix = br.ReadInt16();
		nh.sliceStart = br.ReadInt16();
		br.ReadDataToFillArray_f(nh.PixDim, 8);
		if (float.IsNaN(nh.PixDim[4]))
		{
			Console.WriteLine("Warning - file pixel dimension 4 is NaN. Replaced with 1");
			nh.PixDim[4] = 1f;
		}
		nh._voxOffset = br.ReadSingle();
		nh.sclSlope = br.ReadSingle();
		nh.scl_inter = br.ReadSingle();
		if (nh.sclSlope == 0f)
		{
			nh.sclSlope = 1f;
			nh.scl_inter = 0f;
		}
		nh.sliceLast = br.ReadInt16();
		nh.sliceCode = br.ReadByte();
		nh.xyztUnits = (MeasurementUnits)br.ReadByte();
		nh.calMax = br.ReadSingle();
		nh.calMin = br.ReadSingle();
		nh.sliceDuration = br.ReadSingle();
		nh.tOffset = br.ReadSingle();
		br.BaseStream.Seek(8L, SeekOrigin.Current);
		nh.Description = new string(br.ReadChars(80));
		nh.AuxFile = new string(br.ReadChars(24));
		nh.qFormCode = (CoordinateMapping_Nifti)br.ReadInt16();
		nh.sFormCode = (CoordinateMapping_Nifti)br.ReadInt16();
		nh.quartern_b = br.ReadSingle();
		nh.quartern_c = br.ReadSingle();
		nh.quartern_d = br.ReadSingle();
		nh.quartern_x = br.ReadSingle();
		nh.quartern_y = br.ReadSingle();
		nh.quartern_z = br.ReadSingle();
		br.ReadDataToFillArray_f(nh.Srow_x, 4);
		br.ReadDataToFillArray_f(nh.Srow_y, 4);
		br.ReadDataToFillArray_f(nh.Srow_z, 4);
		nh.intentName = new string(br.ReadChars(16));
		nh.magicString = br.ReadBytes(4);
		while (br.ReadByte() != 0)
		{
			br.ReadBytes(3);
			int eSize = br.ReadInt32();
			int eCode = br.ReadInt32();
			char[] extraBit = br.ReadChars(eSize - 8);
			nh.HeaderExtras.Add(new KeyValuePair<HeaderExtraType, string>((HeaderExtraType)eCode, new string(extraBit)));
		}
		if (!nh.Verify(warnings, out string? verificationErr))
		{
			msg = verificationErr;
			return false;
		}
		Stream.Seek(nh.VoxOffset, SeekOrigin.Begin);
		long noVoxelsTotal = FlipProof.Image.Maths.Gen.Product(nh.DataArrayDims.Skip(1).TakeWhile((short a) => a != 0));
		DataType dataType = nh.dataType;
      nf = dataType switch
      {
         DataType.boolean => new NiftiFile<bool>(nh, ReadIntoArray_Bool(noVoxelsTotal)),
         DataType.unsignedChar => new NiftiFile<byte>(nh, ReadIntoStream(noVoxelsTotal)),
         DataType.signedShort => new NiftiFile<short>(nh, ReadIntoStream(noVoxelsTotal * 2)),
         DataType.signedInt => new NiftiFile<int>(nh, ReadIntoStream(noVoxelsTotal * 4)),
         DataType.Float => new NiftiFile<float>(nh, ReadIntoStream(noVoxelsTotal * 4)),
         DataType.Double => new NiftiFile<double>(nh, ReadIntoStream(noVoxelsTotal * 8)),
         DataType.signedChar => new NiftiFile<sbyte>(nh, ReadIntoStream(noVoxelsTotal)),
         DataType.unsignedShort => new NiftiFile<ushort>(nh, ReadIntoStream(noVoxelsTotal * 2)),
         DataType.unsignedInt => new NiftiFile<uint>(nh, ReadIntoStream(noVoxelsTotal * 4)),
         DataType.unsignedLongLong => new NiftiFile<ulong>(nh, ReadIntoStream(noVoxelsTotal * 8)),
         DataType.longlong => new NiftiFile<long>(nh, ReadIntoStream(noVoxelsTotal * 8)),
         _ => throw new NotSupportedException("Unsupported data type " + nh.dataType),
      };
      if (Stream.Position != Stream.Length)
		{
			msg = $"File length longer than stated in header. Expected {noVoxelsTotal} voxels, but found {(float)Stream.Length - nh._voxOffset - (float)noVoxelsTotal}";
		}
		else
		{
			msg = "OK";
		}
		if (warnings.Any())
		{
			msg += "\r\nWarnings:";
			foreach (string curWarn in warnings)
			{
				msg = msg + "\r\n" + curWarn;
			}
		}
		return true;

   }

	internal static NiftiFile<T>[] OpenSeries<T>(string folder) where T : struct, IComparable<T>, IComparable, IEquatable<T>
	{
		return OpenSeries<T>((from a in Directory.EnumerateFiles(folder)
			orderby a
			select a).ToArray(), lookForZippedVariantIfNotFound: false);
	}

	internal static NiftiFile<T>[] OpenSeries<T>(string[] volLocations, bool lookForZippedVariantIfNotFound) where T : struct, IComparable<T>, IComparable, IEquatable<T>
	{
		NiftiFile<T>[] series = new NiftiFile<T>[volLocations.Length];
		Parallel.For(0, volLocations.Length, delegate(int i)
		{
			if (!TryRead(volLocations[i], lookForZippedVariantIfNotFound, out NiftiFile<T>? readAndConverted, out string err))
			{
				throw new Exception(volLocations[i] + " " + err);
			}
			series[i] = readAndConverted;
		});
		return series;
	}

	public static void StripComments(string locFrom, string locTo)
	{
		if (TryRead(locFrom, lookForZippedVariantIfNotFound: true, out var nf, out var err))
		{
			nf.Head.HeaderExtras.Clear();
			nf.Head.AuxFile = "";
			nf.Head.Description = "";
			if (NiftiWriter.Write(nf, locTo, FileMode.Create, out err))
			{
				return;
			}
		}
		throw new Exception(err);
	}
}
