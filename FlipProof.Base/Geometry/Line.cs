using System;

namespace FlipProof.Base.Geometry;

public struct Line : IEquatable<Line>
{
	public readonly XYZf p0;

	public readonly XYZf p1;

	public Line(XYZf p_0, XYZf p_1)
	{
		p0 = p_0;
		p1 = p_1;
	}

	public Line(Line l, float length, bool leaveUnchangedIfOriginalLineIsLength0)
	{
		p0 = l.p0;
		float origLength = l.GetLength();
		if (origLength == 0f)
		{
			if (!leaveUnchangedIfOriginalLineIsLength0)
			{
				throw new ArgumentException("Original line has no length to extend");
			}
			p1 = l.p1;
		}
		else
		{
			float scaleBy = length / origLength;
			float x = (l.p1.X - p0.X) * scaleBy + p0.X;
			float y = (l.p1.Y - p0.Y) * scaleBy + p0.Y;
			float z = (l.p1.Z - p0.Z) * scaleBy + p0.Z;
			p1 = new XYZf(x, y, z);
		}
	}

	public XYZf GetLengths()
	{
		return p1 - p0;
	}

	public float GetLength()
	{
		return p0.DistanceTo(p1);
	}

	public float GetLengthX()
	{
		return p1.X - p0.X;
	}

	public XYZf GetDirection()
	{
		return p1 - p0;
	}

	public float GetLengthY()
	{
		return p1.Y - p0.Y;
	}

	public float GetLengthZ()
	{
		return p1.Z - p0.Z;
	}

	public XYZf ExtrapolateTowardStart(float moveBy_Raw)
	{
		float fractionOfLength = moveBy_Raw / GetLength();
		float x = p0.X - GetLengthX() * fractionOfLength;
		float newY = p0.Y - GetLengthY() * fractionOfLength;
		float newZ = p0.Z - GetLengthZ() * fractionOfLength;
		return new XYZf(x, newY, newZ);
	}

	public XYZf GetPointAlong(float fractionOfLength)
	{
		float x = p0.X + GetLengthX() * fractionOfLength;
		float newY = p0.Y + GetLengthY() * fractionOfLength;
		float newZ = p0.Z + GetLengthZ() * fractionOfLength;
		return new XYZf(x, newY, newZ);
	}

	public XYZf GetPointAlong_Dist(float length)
	{
		float thisLength = GetLength();
		float fractionOfLength = length / thisLength;
		if (thisLength == 0f)
		{
			return p0;
		}
		return GetPointAlong(fractionOfLength);
	}

	public bool PointIsInside(XYZf c, float epsilon = 1E-06f)
	{
		if (!PointIsInsideBoundingBox(c))
		{
			return false;
		}
		XYZf j = GetDirection();
		float i = (c.X - p0.X) / j.X;
		if (i > 0f && i < 1f && Math.Abs(j.Y * i + p0.Y - c.Y) < epsilon && Math.Abs(j.Z * i + p0.Z - c.Z) < epsilon)
		{
			return true;
		}
		return false;
	}

	public bool PointIsInsideBoundingBox(XYZf c)
	{
		if ((c.X < p0.X && c.X < p1.X) || (c.Y < p0.Y && c.Y < p1.Y) || (c.Z < p0.Z && c.Z < p1.Z) || (c.X > p0.X && c.X > p1.X) || (c.Y > p0.Y && c.Y > p1.Y) || (c.Z > p0.Z && c.Z > p1.Z))
		{
			return false;
		}
		return true;
	}

	public bool PointIsInsideBoundingBoxZ(XYZf c)
	{
		if (!(c.Z < p0.Z) || !(c.Z < p1.Z))
		{
			if (c.Z > p0.Z)
			{
				return !(c.Z > p1.Z);
			}
			return true;
		}
		return false;
	}

	public XYZf GetPointInCommon(Line secondSide, bool checkForNoMatchingSide = true)
	{
		if (p0.Equals(secondSide.p0) || p0.Equals(secondSide.p1))
		{
			return p0;
		}
		if (checkForNoMatchingSide && !p1.Equals(secondSide.p0) && !p1.Equals(secondSide.p1))
		{
			throw new Exception("No matching sides");
		}
		return p1;
	}

	public void ExtrapolatePastX(float x)
	{
		throw new NotImplementedException();
	}

	public Line ExtrapolateBeyondX(float x, bool crashIfParallelToAxis)
	{
		float curDiffX = GetDirection().X;
		if (curDiffX < 0f)
		{
			float proportionToGrow = (p0.X - x) / curDiffX + 1f;
			return new Line(p0, GetPointAlong(proportionToGrow));
		}
		if (curDiffX == 0f)
		{
			if (crashIfParallelToAxis)
			{
				throw new Exception("Line is parallel to x axis");
			}
			return this;
		}
		float proportionToGrow2 = (p1.X - x) / curDiffX + 1f;
		return new Line(GetPointAlong(0f - proportionToGrow2), p1);
	}

	public Line ExtrapolateBeyondY(float y, bool crashIfParallelToAxis)
	{
		float curDiffY = GetDirection().Y;
		if (curDiffY < 0f)
		{
			float proportionToGrow = (p0.Y - y) / curDiffY + 1f;
			return new Line(p0, GetPointAlong(proportionToGrow));
		}
		if (curDiffY == 0f)
		{
			if (crashIfParallelToAxis)
			{
				throw new Exception("Line is parallel to y axis");
			}
			return this;
		}
		float proportionToGrow2 = (p1.Y - y) / curDiffY + 1f;
		return new Line(GetPointAlong(0f - proportionToGrow2), p1);
	}

	public Line ExtrapolateBeyondZ(float z, bool crashIfParallelToAxis)
	{
		float curDiffZ = GetDirection().Z;
		if (curDiffZ < 0f)
		{
			float proportionToGrow = (p0.Z - z) / curDiffZ + 1f;
			return new Line(p0, GetPointAlong(proportionToGrow));
		}
		if (curDiffZ == 0f)
		{
			if (crashIfParallelToAxis)
			{
				throw new Exception("Line is parallel to z axis");
			}
			return this;
		}
		float proportionToGrow2 = (p1.Z - z) / curDiffZ + 1f;
		return new Line(GetPointAlong(0f - proportionToGrow2), p1);
	}

	public bool Intersects(Line S2)
	{
		XYZf I0;
		XYZf I1;
		switch (Intersects_Sub(this, S2, out I0, out I1))
		{
		case 0:
			return false;
		case 1:
			return PointIsInside(I0);
		default:
		{
			float minZ = Math.Min(I0.Z, I1.Z);
			float maxZ = Math.Max(I0.Z, I1.Z);
			if (PointIsInsideBoundingBoxZ(I0) || PointIsInsideBoundingBoxZ(I1) || (I0.Z > maxZ && I1.Z < minZ) || (I1.Z > maxZ && I0.Z < minZ))
			{
				throw new NotImplementedException("easiest to ditch this and edit intersects sub to do the above, and then change its checks when we have a value of '2'");
			}
			return false;
		}
		}
	}

	public static int Intersects_Sub(Line S1, Line S2, out XYZf I0, out XYZf I1)
	{
		Func<XYZf, XYZf, float> dot = (XYZf u, XYZf v) => u.X * v.X + u.Y * v.Y + u.Z * v.Z;
		Func<XYZf, XYZf, float> perp = (XYZf u, XYZf v) => u.X * v.Y - u.Y * v.X;
		I0 = default(XYZf);
		I1 = default(XYZf);
		XYZf u2 = S1.p1 - S1.p0;
		XYZf v2 = S2.p1 - S2.p0;
		XYZf w = S1.p0 - S2.p0;
		float D = perp(u2, v2);
		if (Math.Abs(D) < 1E-08f)
		{
			if (perp(u2, w) != 0f || perp(v2, w) != 0f)
			{
				return 0;
			}
			float du = dot(u2, u2);
			float dv = dot(v2, v2);
			if (du == 0f && dv == 0f)
			{
				if (!S1.p0.Equals(S2.p0))
				{
					return 0;
				}
				I0 = S1.p0;
				return 1;
			}
			if (du == 0f)
			{
				if (!inSegment2D(S1.p0, S2))
				{
					return 0;
				}
				I0 = S1.p0;
				return 1;
			}
			if (dv == 0f)
			{
				if (!inSegment2D(S2.p0, S1))
				{
					return 0;
				}
				I0 = S2.p0;
				return 1;
			}
			XYZf w2 = S1.p1 - S2.p0;
			float t0;
			float t1;
			if (v2.X != 0f)
			{
				t0 = w.X / v2.X;
				t1 = w2.X / v2.X;
			}
			else
			{
				t0 = w.Y / v2.Y;
				t1 = w2.Y / v2.Y;
			}
			if (t0 > t1)
			{
				float num = t0;
				t0 = t1;
				t1 = num;
			}
			if (t0 > 1f || t1 < 0f)
			{
				return 0;
			}
			t0 = ((t0 < 0f) ? 0f : t0);
			t1 = ((t1 > 1f) ? 1f : t1);
			if (t0 == t1)
			{
				I0 = S2.p0 + v2 * t0;
				return 1;
			}
			I0 = S2.p0 + v2 * t0;
			I1 = S2.p0 + v2 * t1;
			return 2;
		}
		float sI = perp(v2, w) / D;
		if (sI < 0f || sI > 1f)
		{
			return 0;
		}
		float tI = perp(u2, w) / D;
		if (tI < 0f || tI > 1f)
		{
			return 0;
		}
		I0 = S1.p0 + u2 * sI;
		return 1;
	}

	private static bool inSegment2D(XYZf P, Line S)
	{
		if (S.p0.X != S.p1.X)
		{
			if (S.p0.X <= P.X && P.X <= S.p1.X)
			{
				return true;
			}
			if (S.p0.X >= P.X && P.X >= S.p1.X)
			{
				return true;
			}
		}
		else
		{
			if (S.p0.Y <= P.Y && P.Y <= S.p1.Y)
			{
				return true;
			}
			if (S.p0.Y >= P.Y && P.Y >= S.p1.Y)
			{
				return true;
			}
		}
		return false;
	}

	public bool Equals(Line other)
	{
		if (p0.Equals(other.p0))
		{
			return p1.Equals(other.p1);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Line)
		{
			return Equals((Line)obj);
		}
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			return (p0.GetHashCode() * 3501) ^ p1.GetHashCode();
		}
	}

	public int SphereCollision(Sphere s, out XYZf intersect1, out XYZf intersect2)
	{
		int hits = 0;
		if (RaySphereCollision(p0, p1, s, out var mu1, out var mu2))
		{
			if (mu1 >= 0.0 && mu1 <= 1.0)
			{
				intersect1 = GetPointAlong((float)mu1);
				hits++;
			}
			else
			{
				intersect1 = default(XYZf);
			}
			if (mu2 >= 0.0 && mu2 <= 1.0)
			{
				intersect2 = GetPointAlong((float)mu2);
				hits++;
				if (hits == 1)
				{
					intersect1 = intersect2;
					intersect2 = default(XYZf);
				}
			}
			else
			{
				intersect2 = default(XYZf);
			}
		}
		else
		{
			intersect1 = default(XYZf);
			intersect2 = default(XYZf);
		}
		return hits;
	}

	public static bool RaySphereCollision(XYZf p0, XYZf p1, Sphere s, out double mu1, out double mu2)
	{
		
		XYZ<double> dp = new XYZ<double>( p1.X, p1.Y, p1.Z) - new XYZ<double>(p0.X, p0.Y, p0.Z);
		XYZ<double> sc = new XYZ<double>(s.p.X, s.p.Y, s.p.Z);
		double a = dp.X * dp.X + dp.Y * dp.Y + dp.Z * dp.Z;
		double b = 2.0 * (dp.X * ((double)p0.X - sc.X) + dp.Y * ((double)p0.Y - sc.Y) + dp.Z * ((double)p0.Z - sc.Z));
		double c = sc.X * sc.X + sc.Y * sc.Y + sc.Z * sc.Z;
		c += (double)(p0.X * p0.X + p0.Y * p0.Y + p0.Z * p0.Z);
		c -= 2.0 * (sc.X * (double)p0.X + sc.Y * (double)p0.Y + sc.Z * (double)p0.Z);
		c -= (double)s.r * (double)s.r;
		double bb4ac = b * b - 4.0 * a * c;
		if (Math.Abs(a) < double.Epsilon || bb4ac < 0.0)
		{
			mu1 = 0.0;
			mu2 = 0.0;
			return false;
		}
		mu1 = (0.0 - b + Math.Sqrt(bb4ac)) / (2.0 * a);
		mu2 = (0.0 - b - Math.Sqrt(bb4ac)) / (2.0 * a);
		return true;
	}
}
