using System;

namespace FlipProof.Image.Nifti;

public static class EnumMethods
{
	public static short BitsPerPixel(this DataType dt)
	{
		return dt switch
		{
			DataType.unknown => 0, 
			DataType.boolean => 1, 
			DataType.unsignedChar => 8, 
			DataType.signedShort => 16, 
			DataType.signedInt => 32, 
			DataType.Float => 32, 
			DataType.complex => 64, 
			DataType.Double => 64, 
			DataType.rgb => 24, 
			DataType.All => 0, 
			DataType.signedChar => 8, 
			DataType.unsignedShort => 16, 
			DataType.unsignedInt => 32, 
			DataType.longlong => 64, 
			DataType.unsignedLongLong => 64, 
			DataType.longDouble => 128, 
			DataType.doublePair => 128, 
			DataType.longDoublePair => 256, 
			DataType.rgba => 32, 
			_ => 0, 
		};
	}

	public static DataType Type2DataType(this Type dt, bool crashIfUnknown)
	{
		if (dt == typeof(bool))
		{
			return DataType.boolean;
		}
		if (dt == typeof(double))
		{
			return DataType.Double;
		}
		if (dt == typeof(float))
		{
			return DataType.Float;
		}
		if (dt == typeof(long))
		{
			return DataType.longlong;
		}
		if (dt == typeof(sbyte))
		{
			return DataType.signedChar;
		}
		if (dt == typeof(int))
		{
			return DataType.signedInt;
		}
		if (dt == typeof(short))
		{
			return DataType.signedShort;
		}
		if (dt == typeof(byte))
		{
			return DataType.unsignedChar;
		}
		if (dt == typeof(uint))
		{
			return DataType.unsignedInt;
		}
		if (dt == typeof(ulong))
		{
			return DataType.unsignedLongLong;
		}
		if (dt == typeof(ushort))
		{
			return DataType.unsignedShort;
		}
		if (crashIfUnknown)
		{
			throw new NotSupportedException("Unknown datatype");
		}
		return DataType.unknown;
	}
}
