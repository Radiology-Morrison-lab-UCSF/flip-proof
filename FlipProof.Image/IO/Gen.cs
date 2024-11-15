using FlipProof.Base;
using FlipProof.Image.Matrices;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;


namespace FlipProof.Image.IO;

public static class Gen
{
	public static byte[] ConvertEndian(byte[] i)
	{
		if (i.Length % 2 != 0)
		{
			throw new Exception("byte array length must multiply of 2");
		}
		Array.Reverse((Array)i);
		return i;
	}

	public static Stream GetUnzippedStream(string filename, bool returnedStreamMustBeSeekable)
	{
		Stream s;
		if (filename.EndsWith(".gz"))
      {
         FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 192000);
         s = GetUnzippedStream(fs, returnedStreamMustBeSeekable);
      }
      else
		{
			s = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 192000);
		}
		return s;
	}

   internal static Stream GetUnzippedStream(Stream zippedStream, bool returnedStreamMustBeSeekable)
   {
      Stream s;
      GZipStream gzip = new GZipStream(zippedStream, CompressionMode.Decompress);
      if (returnedStreamMustBeSeekable)
      {
         using (gzip)
         {
            s = new LargeMemoryStream(zippedStream.Length);
            gzip.CopyTo(s);
            s.Seek(0L, SeekOrigin.Begin);
         }
         zippedStream.Dispose();
      }
      else
      {
         s = gzip;
      }

      return s;
   }

   public static float ConvertEndian(float f)
	{
		return BitConverter.ToSingle(BitConverter.GetBytes(ConvertEndian(BitConverter.ToUInt32(BitConverter.GetBytes(f), 0))).ToArray(), 0);
	}

	public static string AddToEndOfFileName(string path, string addBeforeSuffix)
	{
		if (path.Length == 0)
		{
			return addBeforeSuffix;
		}
		string filename = Path.GetFileName(path);
		string dir = path.Substring(0, path.Length - filename.Length);
		if (dir.Length != 0)
		{
			dir = AddDirSeparatorChar(dir);
		}
		int indexOfFullstop = ((filename.Count((char a) => a == '.') <= 1 || !filename.EndsWith(".gz")) ? filename.IndexOfLast((char a, int i) => a == '.') : RemoveGzFromFileName(filename).IndexOfLast((char a, int i) => a == '.'));
		if (indexOfFullstop < 0)
		{
			return path + addBeforeSuffix;
		}
		return dir + filename.Substring(0, indexOfFullstop) + addBeforeSuffix + filename.Substring(indexOfFullstop);
	}

	public static int ConvertEndian(int i)
	{
		return BitConverter.ToInt32(ConvertEndian(BitConverter.GetBytes(i)), 0);
	}

   [CLSCompliant(false)]
   public static uint ConvertEndian(uint i)
	{
		return BitConverter.ToUInt32(ConvertEndian(BitConverter.GetBytes(i)), 0);
	}

	public static short ConvertEndian(short i)
	{
		return BitConverter.ToInt16(ConvertEndian(BitConverter.GetBytes(i)), 0);
	}

   [CLSCompliant(false)]
   public static ushort ConvertEndian(ushort i)
	{
		return BitConverter.ToUInt16(ConvertEndian(BitConverter.GetBytes(i)), 0);
	}

   [CLSCompliant(false)]
   public static void ConvertEndian_16bit(ref ushort[] arr)
	{
		byte[] bytes = new byte[arr.Length * 2];
		Buffer.BlockCopy(arr, 0, bytes, 0, bytes.Length);
		ConvertEndian_16bit(ref bytes, 0, bytes.Length);
		Buffer.BlockCopy(bytes, 0, arr, 0, bytes.Length);
	}

	public unsafe static void ConvertEndian_16bit(ref byte[] writeTo, int convertFromIndex, int noBytesToConvert)
	{
		if (convertFromIndex + noBytesToConvert * 2 > writeTo.Length)
		{
			throw new IndexOutOfRangeException();
		}
		fixed (byte* src = writeTo)
		{
			byte* firstByte = src;
			byte* secondByte = src;
			firstByte += convertFromIndex;
			secondByte += convertFromIndex + 1;
			for (int i = 0; i < noBytesToConvert; i++)
			{
				byte tmp = *firstByte;
				*firstByte = *secondByte;
				*secondByte = tmp;
				secondByte += 2;
				firstByte += 2;
			}
		}
	}

	public static void ConvertEndian_32bit_Untested(ref int[] arr)
	{
		byte[] bytes = new byte[arr.Length * 4];
		Buffer.BlockCopy(arr, 0, bytes, 0, bytes.Length);
		ConvertEndian_32bit_Untested(ref bytes, 0, bytes.Length);
		Buffer.BlockCopy(bytes, 0, arr, 0, bytes.Length);
	}

   [CLSCompliant(false)]
   public static void ConvertEndian_32bit_Untested(ref uint[] arr)
	{
		byte[] bytes = new byte[arr.Length * 4];
		Buffer.BlockCopy(arr, 0, bytes, 0, bytes.Length);
		ConvertEndian_32bit_Untested(ref bytes, 0, bytes.Length);
		Buffer.BlockCopy(bytes, 0, arr, 0, bytes.Length);
	}

	public static void ConvertEndian_32bit_Untested(ref float[] arr)
	{
		byte[] bytes = new byte[arr.Length * 4];
		Buffer.BlockCopy(arr, 0, bytes, 0, bytes.Length);
		ConvertEndian_32bit_Untested(ref bytes, 0, bytes.Length);
		Buffer.BlockCopy(bytes, 0, arr, 0, bytes.Length);
	}

	public unsafe static void ConvertEndian_32bit_Untested(ref byte[] writeTo, int convertFromIndex, int noBytesToConvert)
	{
		if (convertFromIndex + noBytesToConvert * 3 > writeTo.Length)
		{
			throw new IndexOutOfRangeException();
		}
		fixed (byte* src = writeTo)
		{
			byte* firstByte = src;
			byte* secondByte = src;
			byte* thirdByte = src;
			byte* fourthByte = src;
			firstByte += convertFromIndex;
			secondByte += convertFromIndex + 1;
			thirdByte += convertFromIndex + 2;
			fourthByte += convertFromIndex + 3;
			for (int i = 0; i < noBytesToConvert; i++)
			{
				byte tmp = *firstByte;
				*firstByte = *fourthByte;
				*fourthByte = tmp;
				tmp = *secondByte;
				*secondByte = *thirdByte;
				*thirdByte = tmp;
				secondByte += 4;
				firstByte += 4;
				thirdByte += 4;
				fourthByte += 4;
			}
		}
	}

	public static void CopyDir(string sourceDirName, string destDirName, OverwriteAction onFileOverwrite, bool copySubDirs = true, string[]? ignoreSuffix = null)
	{
		ignoreSuffix ??= [];
		ignoreSuffix = ignoreSuffix.ToArray((string a) => (a[0] != '.') ? ("." + a) : a);
		DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
		if (!directoryInfo.Exists)
		{
			throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
		}
		DirectoryInfo[] dirs = directoryInfo.GetDirectories();
		if (!Directory.Exists(destDirName))
		{
			Directory.CreateDirectory(destDirName);
		}
		foreach (FileInfo item in from a in directoryInfo.GetFiles()
			where !ignoreSuffix.Contains(a.Extension)
			select a)
		{
			string sourcePath = item.FullName;
			string destPath = Path.Combine(destDirName, sourcePath);
			CopyFile(sourcePath, destPath, onFileOverwrite);
		}
		if (copySubDirs)
		{
			DirectoryInfo[] array = dirs;
			foreach (DirectoryInfo subdir in array)
			{
				string temppath = Path.Combine(destDirName, subdir.Name);
				CopyDir(subdir.FullName, temppath, onFileOverwrite, copySubDirs, ignoreSuffix);
			}
		}
	}

	public static void CopyFile(string sourcePath, string destPath, OverwriteAction onFileOverwrite)
	{
		if (File.Exists(destPath))
		{
			switch (onFileOverwrite)
			{
			case OverwriteAction.Overwrite:
				File.Copy(sourcePath, destPath, overwrite: true);
				break;
			case OverwriteAction.Crash:
				throw new IOException("File already exists, overwrite disallowed");
			default:
				throw new NotSupportedException();
			case OverwriteAction.Ignore:
				break;
			}
		}
		else
		{
			File.Copy(sourcePath, destPath, onFileOverwrite == OverwriteAction.Overwrite);
		}
	}

	public static void CopyFileIfNotFound(string source, string dest, bool crashIfSourceNotFound = true)
	{
		if (!File.Exists(dest) && (crashIfSourceNotFound || File.Exists(source)))
		{
			File.Copy(source, dest);
		}
	}

   [CLSCompliant(false)]
   public static byte[] GetBytes_BE(ushort i)
	{
		return GenMethods.ReverseInPlace_2(BitConverter.GetBytes(i));
	}

	public static byte[] GetBytes_BE(short i)
	{
		return GenMethods.ReverseInPlace_2(BitConverter.GetBytes(i));
	}

   [CLSCompliant(false)]
   public static byte[] GetBytes_BE(uint i)
	{
		return GenMethods.ReverseInPlace_4(BitConverter.GetBytes(i));
	}

	public static byte[] GetBytes_BE(int i)
	{
		return GenMethods.ReverseInPlace_4(BitConverter.GetBytes(i));
	}

   [CLSCompliant(false)]
   public static byte[] GetBytes_BE(ulong i)
	{
		return GenMethods.ReverseInPlace_8(BitConverter.GetBytes(i));
	}

	public static byte[] GetBytes_BE(long i)
	{
		return GenMethods.ReverseInPlace_8(BitConverter.GetBytes(i));
	}

	public static byte[] GetBytes_LE(string str)
	{
		byte[] bytes = new byte[str.Length * 2];
		Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}

	public static string GetString(byte[] bytes)
	{
		char[] chars = new char[bytes.Length / 2];
		Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}

	public static string GetPathForCurrentOS(string p, bool isDir)
	{
		p = p.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
		if (isDir)
		{
			return AddDirSeparatorChar(p);
		}
		return p;
	}

	public static string AddDirSeparatorChar(string p)
	{
		p = p.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
		if (p.Last() != Path.DirectorySeparatorChar)
		{
			string text = p;
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			p = text + directorySeparatorChar;
		}
		return p;
	}

	public static string RemoveDirSeparatorChar(string p)
	{
		if (p.Last() == Path.DirectorySeparatorChar)
		{
			return p[..^1];
		}
		return p;
	}

	public static DirectoryInfo? GetParentDirectory(string directoryPath)
	{
		return Directory.GetParent(RemoveDirSeparatorChar(directoryPath));
	}

	public static string[] GetImageLocs(string dir, string suffix = "dcm")
	{
		return (from a in Directory.GetFiles(dir)
			where a.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) && !Path.GetFileName(a).StartsWith(".")
			select a).ToArray();
	}

	public static void MoveIfExists(string from, string to)
	{
		if (File.Exists(from))
		{
			File.Move(from, to);
		}
	}

	public unsafe static float[] ParseFloats_AnyDelim(string s, int length)
	{
		float[] floats = new float[length];
		int index = 0;
		int startpos = 0;
		int lM1 = length - 1;
		fixed (char* pStringBuffer = s)
		{
			fixed (float* pFloatBuffer = floats)
			{
				int sLength = s.Length;
				char cur = ' ';
				for (int i = 0; i < sLength; i++)
				{
					if (index >= lM1)
					{
						break;
					}
					cur = pStringBuffer[i];
					if ((cur < '0' || cur > '9') && cur != '-' && cur != '.' && cur != 'E' && i - startpos > 0)
					{
						pFloatBuffer[index++] = ParseFloat(pStringBuffer + startpos, i - startpos);
						startpos = i + 1;
					}
				}
				if (index < length)
				{
					pFloatBuffer[index++] = ParseFloat(pStringBuffer + startpos, s.Length - startpos);
				}
			}
		}
		return floats;
	}

   [CLSCompliant(false)]
   public unsafe static float ParseFloat(char* input, int len)
	{
		int pos = 0;
		int part = 0;
		bool neg = false;
		double* ret = stackalloc double[1];
		for (; pos < len; pos++)
		{
			char cur4 = input[pos];
			if ((cur4 >= '0' && cur4 <= '9') || cur4 == '-' || cur4 == '.')
			{
				break;
			}
		}
		if (input[pos] == '-')
		{
			neg = true;
			pos++;
		}
		for (; pos < len; pos++)
		{
			char cur3 = input[pos];
			if (cur3 > '9' || cur3 < '0')
			{
				break;
			}
			part = part * 10 + (cur3 - 48);
		}
		*ret = (neg ? (part * -1) : part);
		if (pos < len && input[pos] == '.')
		{
			pos++;
			int posInitial = pos;
			part = 0;
			for (; pos < len; pos++)
			{
				char cur2 = input[pos];
				if (cur2 > '9' || cur2 < '0')
				{
					break;
				}
				part = part * 10 + (cur2 - 48);
			}
			int mul = pos - posInitial;
			if (neg)
			{
				*ret -= (double)part * RootLookup(mul);
			}
			else
			{
				*ret += (double)part * RootLookup(mul);
			}
		}
		if (pos < len && (input[pos] == 'e' || input[pos] == 'E'))
		{
			pos++;
			neg = input[pos] == '-';
			pos++;
			part = 0;
			for (; pos < len; pos++)
			{
				char cur = input[pos];
				if (cur > '9' || cur < '0')
				{
					break;
				}
				part = part * 10 + (cur - 48);
			}
			if (neg)
			{
				*ret *= RootLookup(part);
			}
			else
			{
				*ret *= PowLookup_Single(part);
			}
		}
		return (float)(*ret);
	}

	private static double RootLookup(int no)
	{
		return no switch
		{
			0 => 1.0, 
			1 => 0.1, 
			2 => 0.01, 
			3 => 0.001, 
			4 => 0.0001, 
			5 => 1E-05, 
			6 => 1E-06, 
			7 => 1E-07, 
			8 => 1E-08, 
			9 => 1E-09, 
			10 => 1E-10, 
			11 => 1E-11, 
			12 => 1E-12, 
			13 => 1E-13, 
			14 => 1E-14, 
			15 => 1E-15, 
			16 => 1E-16, 
			17 => 1E-17, 
			18 => 1E-18, 
			19 => 1E-19, 
			20 => 1E-20, 
			21 => 1E-21, 
			22 => 1E-22, 
			23 => 1E-23, 
			24 => 1E-24, 
			25 => 1E-25, 
			26 => 1E-26, 
			27 => 1E-27, 
			28 => 1E-28, 
			29 => 1E-29, 
			30 => 1E-30, 
			31 => 1E-31, 
			32 => 1E-32, 
			33 => 1E-33, 
			34 => 1E-34, 
			35 => 1E-35, 
			36 => 1E-36, 
			37 => 1E-37, 
			38 => 1E-38, 
			39 => 1E-39, 
			40 => 1E-40, 
			41 => 1E-41, 
			42 => 1E-42, 
			43 => 1E-43, 
			44 => 1E-44, 
			45 => 1E-45, 
			46 => 1E-46, 
			47 => 1E-47, 
			48 => 1E-48, 
			49 => 1E-49, 
			50 => 1E-50, 
			51 => 1E-51, 
			52 => 1E-52, 
			53 => 1E-53, 
			54 => 1E-54, 
			55 => 1E-55, 
			56 => 1E-56, 
			57 => 1E-57, 
			58 => 1E-58, 
			59 => 1E-59, 
			60 => 1E-60, 
			61 => 1E-61, 
			62 => 1E-62, 
			63 => 1E-63, 
			64 => 1E-64, 
			_ => 1.0 / Math.Pow(10.0, no), 
		};
	}

	private static double PowLookup_Single(int no)
	{
		return no switch
		{
			0 => 1.0, 
			1 => 10.0, 
			2 => 100.0, 
			3 => 1000.0, 
			4 => 10000.0, 
			5 => 100000.0, 
			6 => 1000000.0, 
			7 => 10000000.0, 
			8 => 100000000.0, 
			9 => 1000000000.0, 
			10 => 10000000000.0, 
			11 => 100000000000.0, 
			12 => 1000000000000.0, 
			13 => 10000000000000.0, 
			14 => 100000000000000.0, 
			15 => 1000000000000000.0, 
			16 => 10000000000000000.0, 
			17 => 1E+17, 
			18 => 1E+18, 
			19 => 1E+19, 
			20 => 1E+20, 
			21 => 1E+21, 
			22 => 1E+22, 
			23 => 1E+23, 
			24 => 1E+24, 
			25 => 1E+25, 
			26 => 1E+26, 
			27 => 1E+27, 
			28 => 1E+28, 
			29 => 1E+29, 
			30 => 1E+30, 
			31 => 1E+31, 
			32 => 1E+32, 
			33 => 1E+33, 
			34 => 1E+34, 
			35 => 1E+35, 
			36 => 1E+36, 
			37 => 1E+37, 
			38 => 1E+38, 
			_ => Math.Pow(10.0, no), 
		};
	}

	public static void UnZip_gz(string loc_data_nii, bool deletezip)
	{
		if (!UnZip_gz(loc_data_nii, deletezip, out var err))
		{
			throw new Exception(err);
		}
	}

	public static bool UnZip_gz(string loc_unzipTo, bool deleteZip, [NotNullWhen(false)] out string? err)
	{
		string loc_zip = loc_unzipTo + ".gz";
		bool allowOverwrite = true;
		bool returnErrIfExistsAndNoOverwrite = false;
		return UnZip_gz(loc_unzipTo, deleteZip, loc_zip, allowOverwrite, returnErrIfExistsAndNoOverwrite, out err);
	}

	public static void UnZip_gz(string loc_unzipTo, bool deleteZip, string loc_zip, bool allowOverwrite, bool returnErrIfExistsAndNoOverwrite)
	{
		if (!UnZip_gz(loc_unzipTo, deleteZip, loc_zip, allowOverwrite, returnErrIfExistsAndNoOverwrite, out var err))
		{
			throw new Exception(err);
		}
	}

	public static bool UnZip_gz(string loc_unzipTo, bool deleteZip, string loc_zip, bool allowOverwrite, bool returnErrIfExistsAndNoOverwrite, [NotNullWhen(false)] out string? err)
	{
		if (allowOverwrite || !File.Exists(loc_unzipTo))
		{
			if (!File.Exists(loc_zip))
			{
				err = loc_unzipTo + " does not exist in location specified";
				return false;
			}
			using (FileStream compressed = new FileStream(loc_zip, FileMode.Open, FileAccess.Read))
			{
				using GZipStream uncompressed = new GZipStream(compressed, CompressionMode.Decompress);
				using FileStream stream = new FileStream(loc_unzipTo, FileMode.Create);
				uncompressed.CopyTo(stream);
				uncompressed.Flush();
				uncompressed.Close();
			}
			if (deleteZip)
			{
				File.Delete(loc_zip);
			}
		}
		else if (returnErrIfExistsAndNoOverwrite)
		{
			err = "Unzipped file already exists";
			return false;
		}
		err = null;
		return true;
	}

	public static MemoryStream UnZip_gz(Stream compressed, bool rewindResult)
	{
		MemoryStream stream = new MemoryStream();
		using (GZipStream uncompressed = new GZipStream(compressed, CompressionMode.Decompress))
		{
			uncompressed.CopyTo(stream);
			uncompressed.Flush();
			uncompressed.Close();
		}
		if (rewindResult)
		{
			stream.Position = 0L;
		}
		return stream;
	}

	public static void Zip_gz(string loc_zipFrom, bool deleteOriginal)
	{
		if (!Zip_gz(loc_zipFrom, deleteOriginal, out var err))
		{
			throw new Exception(err);
		}
	}

	public static bool Zip_gz(string loc_zipFrom, bool deleteOriginal, [NotNullWhen(false)] out string? err)
	{
		string loc_zip = loc_zipFrom + ".gz";
		bool allowOverwrite = true;
		bool returnErrIfExistsAndNoOverwrite = false;
		return Zip_gz(loc_zipFrom, deleteOriginal, loc_zip, allowOverwrite, returnErrIfExistsAndNoOverwrite, out err);
	}

	public static void Zip_gz(string loc_zipFrom, bool deleteOriginal, string loc_zip, bool allowOverwrite, bool returnErrIfExistsAndNoOverwrite)
	{
		if (!Zip_gz(loc_zipFrom, deleteOriginal, loc_zip, allowOverwrite, returnErrIfExistsAndNoOverwrite, out var err))
		{
			throw new Exception(err);
		}
	}

	public static bool Zip_gz(string loc_zipFrom, bool deleteOriginal, string loc_zip, bool allowOverwrite, bool returnErrIfExistsAndNoOverwrite, [NotNullWhen(false)] out string? err)
	{
		if (allowOverwrite || !File.Exists(loc_zip))
		{
			if (!File.Exists(loc_zipFrom))
			{
				err = loc_zipFrom + " does not exist in location specified";
				return false;
			}
			using (FileStream stream = new FileStream(loc_zipFrom, FileMode.Open, FileAccess.Read))
			{
				using FileStream writeTo = new FileStream(loc_zip, FileMode.Create);
				using GZipStream compressed = new GZipStream(writeTo, CompressionMode.Compress);
				stream.CopyTo(compressed);
				compressed.Flush();
				compressed.Close();
			}
			if (deleteOriginal)
			{
				File.Delete(loc_zipFrom);
			}
		}
		else if (returnErrIfExistsAndNoOverwrite)
		{
			err = "Unzipped file already exists";
			return false;
		}
		err = null;
		return true;
	}

	public static bool Zip_gz(Stream stream, string loc_zip, bool allowOverwrite, bool returnErrIfExistsAndNoOverwrite, [NotNullWhen(false)] out string? err)
	{
		if (allowOverwrite || !File.Exists(loc_zip))
		{
			using FileStream writeTo = new FileStream(loc_zip, FileMode.Create, FileAccess.Write, FileShare.Read, 192000);
			using GZipStream compressed = new GZipStream(writeTo, CompressionMode.Compress);
			stream.CopyTo(compressed);
			compressed.Flush();
			compressed.Close();
		}
		else if (returnErrIfExistsAndNoOverwrite)
		{
			err = "Zipped file already exists";
			return false;
		}
		err = null;
		return true;
	}

	public static void DeleteDir(string outputFolder)
	{
		if (Directory.Exists(outputFolder))
		{
			Directory.Delete(outputFolder, recursive: true);
		}
	}

	public static void DeleteDirContents(string outputFolder)
	{
		if (!Directory.Exists(outputFolder))
		{
			return;
		}
		string[] files = Directory.GetFiles(outputFolder);
		for (int j = 0; j < files.Length; j++)
		{
			File.Delete(files[j]);
		}
		string[] dirs = Directory.GetDirectories(outputFolder);
		for (int i = 0; i < dirs.Length; i++)
		{
			if (Directory.Exists(dirs[i]))
			{
				if (Directory.GetFiles(dirs[i]).Length != 0 || Directory.GetDirectories(dirs[i]).Length != 0)
				{
					DeleteDirContents(dirs[i]);
				}
				Directory.Delete(dirs[i], recursive: true);
			}
		}
	}

	public static void CreateDirIfNonExistent(string loc)
	{
		if (!Directory.Exists(loc))
		{
			Directory.CreateDirectory(loc);
		}
	}

	public static void ClearOrCreateDir(string loc)
	{
		if (Directory.Exists(loc))
		{
			DeleteDirContents(loc);
		}
		else
		{
			Directory.CreateDirectory(loc);
		}
	}

	public static void CompareFiles_PrintToConsole(string fileloc1, string fileloc2, bool stopWhenLengthsMismatch, int maxEndAt = int.MaxValue)
	{
		foreach (int cur in CompareFiles(fileloc1, fileloc2, stopWhenLengthsMismatch))
		{
			if (cur > maxEndAt)
			{
				break;
			}
			Console.WriteLine(cur);
		}
	}

	public static List<int> CompareFiles(string fileloc1, string fileloc2, bool stopWhenLengthsMismatch)
	{
		List<int> mismatches = new List<int>();
		byte[] b0 = File.ReadAllBytes(fileloc1);
		byte[] b1 = File.ReadAllBytes(fileloc2);
		for (int i = 0; i < b0.Length; i++)
		{
			if (b1.Length <= i)
			{
				mismatches.Add(i);
				if (stopWhenLengthsMismatch)
				{
					break;
				}
			}
			else if (b0[i] != b1[i])
			{
				mismatches.Add(i);
			}
		}
		return mismatches;
	}


	public static void ReadDataToFillArray<T>(this UnbufferedStreamReader sr, T[] pointLocXYZ) where T : struct
	{
		if (pointLocXYZ is float[] f)
		{
			sr.ReadDataToFillArray_f(f);
			return;
		}
		if (pointLocXYZ is double[] d)
		{
			sr.ReadDataToFillArray_d(d);
			return;
		}
		if (pointLocXYZ is byte[] b)
		{
			sr.ReadDataToFillArray(b);
			return;
		}
		throw new NotSupportedException();
	}

	public static void ReadDataToFillArray<T>(this BinaryReader br, T[] pointLocXYZ) where T : struct
	{
		if (pointLocXYZ is float[] f)
		{
			br.ReadDataToFillArray_f(f);
			return;
		}
		if (pointLocXYZ is double[] d)
		{
			br.ReadDataToFillArray_d(d);
			return;
		}
		if (pointLocXYZ is int[] i)
		{
			br.ReadDataToFillArray_Int32(i);
			return;
		}
		throw new NotSupportedException();
	}

	public static void ReadDataToFillArray(this UnbufferedStreamReader sr, byte[] fillMe)
	{
		byte[] read;
		for (int pointsInCurCoordRead = 0; pointsInCurCoordRead < fillMe.Length; pointsInCurCoordRead += read.Length)
		{
			read = (from a in sr.ReadLine().Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
				select byte.Parse(a)).ToArray();
			Array.Copy(read, 0, fillMe, pointsInCurCoordRead, read.Length);
		}
	}

	public static void ReadDataToFillArray_f(this UnbufferedStreamReader sr, float[] fillMe)
	{
		float[] read;
		for (int pointsInCurCoordRead = 0; pointsInCurCoordRead < fillMe.Length; pointsInCurCoordRead += read.Length)
		{
			read = (from a in sr.ReadLine().Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
				select float.Parse(a)).ToArray();
			Array.Copy(read, 0, fillMe, pointsInCurCoordRead, read.Length);
		}
	}

	internal static void ReadDataToFillArray_f(this IBinaryReader br, float[] fillMe, int count = -1)
	{
		if (count == -1)
		{
			count = fillMe.Length;
		}
		byte[] asBytes = br.ReadBytes(count * 4);
		Buffer.BlockCopy(asBytes, 0, fillMe, 0, asBytes.Length);
	}

	public static void ReadDataToFillArray_f(this BinaryReader br, float[] fillMe, int count = -1)
	{
		if (count == -1)
		{
			count = fillMe.Length;
		}
		byte[] asBytes = br.ReadBytes(count * 4);
		Buffer.BlockCopy(asBytes, 0, fillMe, 0, asBytes.Length);
	}

	public static void ReadDataToFillArray_Int16(this BinaryReader br, short[] fillMe, int count = -1)
	{
		if (count == -1)
		{
			count = fillMe.Length;
		}
		byte[] asBytes = br.ReadBytes(count * 2);
		Buffer.BlockCopy(asBytes, 0, fillMe, 0, asBytes.Length);
	}

	public static void ReadDataToFillArray_Int32(this BinaryReader br, int[] fillMe, int count = -1)
	{
		if (count == -1)
		{
			count = fillMe.Length;
		}
		byte[] asBytes = br.ReadBytes(count * 4);
		Buffer.BlockCopy(asBytes, 0, fillMe, 0, asBytes.Length);
	}

	public static void ReadDataToFillArray_Int64(this BinaryReader br, long[] fillMe, int count = -1)
	{
		if (count == -1)
		{
			count = fillMe.Length;
		}
		byte[] asBytes = br.ReadBytes(count * 8);
		Buffer.BlockCopy(asBytes, 0, fillMe, 0, asBytes.Length);
	}

   [CLSCompliant(false)]
   public static void ReadDataToFillArray_UInt64(this BinaryReader br, ulong[] fillMe, int count = -1)
	{
		if (count == -1)
		{
			count = fillMe.Length;
		}
		byte[] asBytes = br.ReadBytes(count * 8);
		Buffer.BlockCopy(asBytes, 0, fillMe, 0, asBytes.Length);
	}

	public static T[] ReadDataToArray<T>(this BinaryReader br, int count, bool netSizeForBoolean)
	{
		T[] arr = new T[count];
		br.ReadDataToFillArray(arr, netSizeForBoolean);
		return arr;
	}

	public static void ReadDataToFillArray<T>(this BinaryReader br, T[] fillMe, bool netSizeForBoolean, int count = -1)
	{
		int sizeOfT = CollectionCreation.SizeOfType(fillMe, netSizeForBoolean);
		if (count == -1)
		{
			count = fillMe.Length;
		}
		ulong countOfBytes = (ulong)count * (ulong)sizeOfT;
		if (countOfBytes > int.MaxValue)
		{
			ulong collectedBytes = 0uL;
			int collectedT = 0;
			ulong largestPossibleChunkSize = (ulong)Math.Floor(2147483647.0 / (double)sizeOfT) * (ulong)sizeOfT;
			while (collectedBytes < countOfBytes)
			{
				int chunkSize = (int)Math.Min(countOfBytes - collectedBytes, largestPossibleChunkSize);
				byte[] asBytes = br.ReadBytes(chunkSize);
				Buffer.BlockCopy(asBytes, 0, fillMe, collectedT, asBytes.Length);
				collectedBytes += (ulong)chunkSize;
				collectedT = (int)(collectedBytes / (ulong)sizeOfT);
			}
		}
		else
		{
			byte[] asBytes2 = br.ReadBytes((int)countOfBytes);
			Buffer.BlockCopy(asBytes2, 0, fillMe, 0, asBytes2.Length);
		}
	}

	public static void ReadDataToFillArray_d(this UnbufferedStreamReader sr, double[] fillMe)
	{
		double[] read;
		for (int pointsInCurCoordRead = 0; pointsInCurCoordRead < fillMe.Length; pointsInCurCoordRead += read.Length)
		{
			read = (from a in sr.ReadLine().Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
				select double.Parse(a)).ToArray();
			Array.Copy(read, 0, fillMe, pointsInCurCoordRead, read.Length);
		}
	}

	public static void ReadDataToFillArray_d(this BinaryReader br, double[] fillMe, int count = -1)
	{
		if (count == -1)
		{
			count = fillMe.Length;
		}
		byte[] asBytes = br.ReadBytes(count * 8);
		Buffer.BlockCopy(asBytes, 0, fillMe, 0, asBytes.Length);
	}

	public static void ReadDataToFillArray(this BinaryReader br, XYZf[] fillMe)
	{
		float[] pointLocXYZ = new float[fillMe.Length * 3];
		br.ReadDataToFillArray_f(pointLocXYZ);
		for (int i = 0; i < fillMe.Length; i++)
		{
			fillMe[i] = new XYZf(pointLocXYZ[i * 3], pointLocXYZ[i * 3 + 1], pointLocXYZ[i * 3 + 2]);
		}
	}


	public static bool File_Exists_GzChecked(string fileLoc)
	{
		if (File.Exists(fileLoc))
		{
			return true;
		}
		if (Path.GetExtension(fileLoc).ToLowerInvariant() == ".gz")
		{
			return File.Exists(fileLoc.Substring(0, fileLoc.Length - 3));
		}
		if (Environment.OSVersion.Platform == PlatformID.Unix)
		{
			if (!File.Exists(fileLoc + ".gz") && !File.Exists(fileLoc + ".Gz") && !File.Exists(fileLoc + ".gZ"))
			{
				return File.Exists(fileLoc + ".GZ");
			}
			return true;
		}
		return File.Exists(fileLoc + ".gz");
	}

	public static string RemoveGzFromFileName(string fileLoc)
	{
		if (fileLoc.EndsWith(".gz", StringComparison.InvariantCultureIgnoreCase))
		{
			fileLoc = fileLoc.Substring(0, fileLoc.Length - 3);
		}
		return fileLoc;
	}

	public static bool ChangeFilenameGzToExistingName(ref string fileLoc)
	{
		if (File.Exists(fileLoc))
		{
			return true;
		}
		if (Path.GetExtension(fileLoc).ToLowerInvariant() == ".gz")
		{
			string newName = fileLoc.Substring(0, fileLoc.Length - 3);
			if (File.Exists(newName))
			{
				fileLoc = newName;
				return true;
			}
		}
		else
		{
			string newName2 = fileLoc + ".gz";
			if (File.Exists(newName2))
			{
				fileLoc = newName2;
				return true;
			}
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				newName2 = fileLoc + ".Gz";
				if (File.Exists(newName2))
				{
					fileLoc = newName2;
					return true;
				}
				newName2 = fileLoc + ".gZ";
				if (File.Exists(newName2))
				{
					fileLoc = newName2;
					return true;
				}
				newName2 = fileLoc + ".GZ";
				if (File.Exists(newName2))
				{
					fileLoc = newName2;
					return true;
				}
			}
		}
		return false;
	}

	public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true, string? excludeSubDirsNamed = null)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
		DirectoryInfo[] dirs = directoryInfo.GetDirectories();
		if (!directoryInfo.Exists)
		{
			throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
		}
		if (!Directory.Exists(destDirName))
		{
			Directory.CreateDirectory(destDirName);
		}
		FileInfo[] files = directoryInfo.GetFiles();
		foreach (FileInfo file in files)
		{
			string temppath2 = Path.Combine(destDirName, file.Name);
			file.CopyTo(temppath2, overwrite: false);
		}
		if (!copySubDirs)
		{
			return;
		}
		DirectoryInfo[] array = dirs;
		foreach (DirectoryInfo subdir in array)
		{
			if (!subdir.Name.Equals(excludeSubDirsNamed))
			{
				string temppath = Path.Combine(destDirName, subdir.Name);
				DirectoryCopy(subdir.FullName, temppath, copySubDirs, excludeSubDirsNamed);
			}
		}
	}

	public static IOrderedEnumerable<string> GetNiftiFilesLoc(string dir, string? filenameContains = null)
	{
		return GetFilesWithExtension_NotCaseSensitive(dir, [".nii", ".nii.gz"], filenameContains);
	}

	public static IOrderedEnumerable<string> GetNiftiFilesLoc_UnzippedOnly(string dir, string? filenameContains = null)
	{
		return GetFilesWithExtension_NotCaseSensitive(dir, [".nii"], filenameContains);
	}

	public static IOrderedEnumerable<string> GetFilesWithExtension_NotCaseSensitive(string dir, string[] extensions, string? filenameContains = null, string? filenameStartsWith = null)
	{
		filenameContains = ((filenameContains == null) ? null : filenameContains.ToLowerInvariant());
		extensions = (from a in extensions
			select a.ToLowerInvariant() into a
			select (!a[0].Equals('.')) ? ("." + a) : a).ToArray();
		return from a in Directory.GetFiles(dir)
			where filenameContains == null || Path.GetFileNameWithoutExtension(a).ToLowerInvariant().Contains(filenameContains)
			where filenameStartsWith == null || Path.GetFileNameWithoutExtension(a).ToLowerInvariant().StartsWith(filenameStartsWith)
			where extensions.Any((string ext) => a.ToLowerInvariant().EndsWith(ext))
			orderby a
			select a;
	}



	public static string GetSessionCodeFromPath<TEnum>(string curPath, out TEnum parsed) where TEnum : struct
	{
		List<string> candidateFolderNames = curPath.Split(Path.DirectorySeparatorChar, '\\', '/').ToList();
		Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
		parsed = default(TEnum);
		for (int i = candidateFolderNames.Count - 1; i >= 0; i--)
		{
			string cur = candidateFolderNames[i];
			bool matchesPattern = false;
			if (cur.Count((char a) => a == '-') == 2 && cur.Count((char a) => a == '_') == 1)
			{
				int indexOfUnderscore = cur.IndexOf('_');
				if (Enum.TryParse<TEnum>(cur.Substring(0, indexOfUnderscore), out parsed) && cur.Length == 12 + indexOfUnderscore && cur.Substring(indexOfUnderscore + 1, 4).All((char a) => a >= '0' && a <= '9') && cur[indexOfUnderscore + 5] == '-' && cur[indexOfUnderscore + 9] == '-' && cur.Substring(indexOfUnderscore + 10, 2).All((char a) => a >= '0' && a <= '9'))
				{
					matchesPattern = true;
				}
			}
			if (!matchesPattern)
			{
				candidateFolderNames.RemoveAt(i);
			}
		}
		if (candidateFolderNames.Count == 1)
		{
			return candidateFolderNames[0];
		}
		if (candidateFolderNames.Count == 0)
		{
			throw new Exception("No session directory found in path" + curPath);
		}
		throw new Exception("More than one session directory found in path " + curPath);
	}

	public static void ConcatTextFiles(IEnumerable<string> locsSaved, string locTo, int gapLinesBetweenTextFiles)
	{
		using StreamWriter sw = new StreamWriter(locTo, append: false);
		foreach (string cur in locsSaved)
		{
			sw.Write(File.ReadAllText(cur));
			for (int i = 0; i < gapLinesBetweenTextFiles; i++)
			{
				sw.WriteLine();
			}
		}
	}

	public static void WriteFromArray<T>(this BinaryWriter br, T[] data) where T : struct
	{
		br.WriteFromArray(data, data.Length);
	}

	public static void WriteFromArray<T>(this BinaryWriter br, T[] data, int length)
	{
		int sizeOfType = CollectionCreation.SizeOfType(data, dotNetSizeForBoolean: true);
		byte[] clone = new byte[length * sizeOfType];
		Buffer.BlockCopy(data, 0, clone, 0, length * sizeOfType);
		br.Write(clone);
	}

	public static void ThrowExceptionIfNotFound(string loc, string? message = null)
	{
		if (File_Exists_GzChecked(loc))
		{
			if (message == null)
			{
				throw new Exception("Could not find required file: " + loc);
			}
			throw new Exception(message + "File Location: " + loc);
		}
	}

	public static void RotateTabDelimFile(string fileLoc, bool flipHorizontally, out string locSavedTo)
	{
		string[] lines = File.ReadAllLines(fileLoc);
		string[][] splitLines = new string[lines.Length][];
		for (int i = 0; i < lines.Length; i++)
		{
			splitLines[i] = lines[i].Split(new char[1] { '\t' });
		}
		int noRows_result = splitLines.Max((string[] a) => a.Length);
		int noCols_result = splitLines.Length;
		StringBuilder sb = new StringBuilder();
		for (int y = 0; y < noRows_result; y++)
		{
			int x_orig = y;
			for (int x = 0; x < noCols_result; x++)
			{
				int y_orig = (flipHorizontally ? x : (splitLines.Length - x - 1));
				if (y_orig < splitLines.Length && x_orig < splitLines[y_orig].Length)
				{
					sb.Append(splitLines[y_orig][x_orig]);
				}
				sb.Append("\t");
			}
			sb.AppendLine();
		}
		string? directoryName = Path.GetDirectoryName(fileLoc);
		char directorySeparatorChar = Path.DirectorySeparatorChar;
		locSavedTo = directoryName + directorySeparatorChar + Path.GetFileNameWithoutExtension(fileLoc) + "_transposed.txt";
		File.WriteAllText(locSavedTo, sb.ToString());
	}

	public static void WriteAllLines(string path, IEnumerable<string> vals, Encoding encoding, bool append = false)
	{
		using StreamWriter sw = new StreamWriter(path, append, encoding, 2097152);
		foreach (string line in vals)
		{
			sw.WriteLine(line);
		}
	}

	public static void WriteAllLines(string path, IEnumerable<int> vals, Encoding encoding, bool append = false)
	{
		using StreamWriter sw = new StreamWriter(path, append, encoding, 2097152);
		foreach (int line in vals)
		{
			sw.WriteLine(line);
		}
	}
}
