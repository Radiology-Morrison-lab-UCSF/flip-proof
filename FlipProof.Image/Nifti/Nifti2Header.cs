using System;
using System.IO;
using System.Linq;

namespace FlipProof.Image.Nifti;

public class Nifti2Header
{
	private int sizeof_hdr = 540;

	public char[] magic;

	public DataType datatype;

	public ushort bitpix;

	public ulong[] dim;

	public double intent_p1;

	public double intent_p2;

	public double intent_p3;

	public double[] pixdim;

	public ulong vox_offset;

	public double scl_slope;

	public double scl_inter;

	public double cal_max;

	public double cal_min;

	public double slice_duration;

	public double toffset;

	public ulong slice_start;

	public ulong slice_end;

	public char[] descrip;

	public char[] aux_file;

	public int qform_code;

	public int sform_code;

	public double quatern_b;

	public double quatern_c;

	public double quatern_d;

	public double qoffset_x;

	public double qoffset_y;

	public double qoffset_z;

	public double[] srow_x;

	public double[] srow_y;

	public double[] srow_z;

	public int slice_code;

	public int xyzt_units;

	public int intent_code;

	public char[] intent_name;

	public byte dim_info;

	public char[] unused_str;

	public static Nifti2Header Read(BinaryReader br)
	{
		Nifti2Header head = new Nifti2Header();
		head.sizeof_hdr = br.ReadInt32();
		if (head.sizeof_hdr != 540)
		{
			if (head.sizeof_hdr == 348)
			{
				throw new Exception("Nifti-1, not a Nifti-2 file");
			}
			throw new Exception("Not a Nifti-2 file");
		}
		head.magic = (from a in br.ReadBytes(8)
			select (char)a).ToArray();
		head.datatype = (DataType)br.ReadUInt16();
		head.bitpix = br.ReadUInt16();
		head.dim = new ulong[8]
		{
			br.ReadUInt64(),
			br.ReadUInt64(),
			br.ReadUInt64(),
			br.ReadUInt64(),
			br.ReadUInt64(),
			br.ReadUInt64(),
			br.ReadUInt64(),
			br.ReadUInt64()
		};
		head.intent_p1 = br.ReadDouble();
		head.intent_p2 = br.ReadDouble();
		head.intent_p3 = br.ReadDouble();
		head.pixdim = ReadDoubles(br, 8);
		head.vox_offset = br.ReadUInt64();
		head.scl_slope = br.ReadDouble();
		head.scl_inter = br.ReadDouble();
		head.cal_max = br.ReadDouble();
		head.cal_min = br.ReadDouble();
		head.slice_duration = br.ReadDouble();
		head.toffset = br.ReadDouble();
		head.slice_start = br.ReadUInt64();
		head.slice_end = br.ReadUInt64();
		head.descrip = ReadCharsAsByte(br, 80);
		head.aux_file = ReadCharsAsByte(br, 24);
		head.qform_code = br.ReadInt32();
		head.sform_code = br.ReadInt32();
		head.quatern_b = br.ReadDouble();
		head.quatern_c = br.ReadDouble();
		head.quatern_d = br.ReadDouble();
		head.qoffset_x = br.ReadDouble();
		head.qoffset_y = br.ReadDouble();
		head.qoffset_z = br.ReadDouble();
		head.srow_x = ReadDoubles(br, 4);
		head.srow_y = ReadDoubles(br, 4);
		head.srow_z = ReadDoubles(br, 4);
		head.slice_code = br.ReadInt32();
		head.xyzt_units = br.ReadInt32();
		head.intent_code = br.ReadInt32();
		head.intent_name = ReadCharsAsByte(br, 16);
		head.dim_info = br.ReadByte();
		head.unused_str = ReadCharsAsByte(br, 15);
		return head;
	}

	private static double[] ReadDoubles(BinaryReader br, int count)
	{
		double[] dat = new double[count];
		for (int i = 0; i < count; i++)
		{
			dat[i] = br.ReadDouble();
		}
		return dat;
	}

	private static char[] ReadCharsAsByte(BinaryReader br, int count)
	{
		byte[] dat = br.ReadBytes(count);
		char[] datC = new char[count];
		for (int i = 0; i < count; i++)
		{
			datC[i] = (char)dat[i];
		}
		return datC;
	}
}
