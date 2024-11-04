using System;
using System.Globalization;

namespace FlipProof.Image.Maths;

public class DoubleConverter
{
    private class ArbitraryDecimal
    {
        private byte[] digits;

        private int decimalPoint;

        internal ArbitraryDecimal(long x)
        {
            string tmp = x.ToString(CultureInfo.InvariantCulture);
            digits = new byte[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                digits[i] = (byte)(tmp[i] - 48);
            }
            Normalize();
        }

        internal void MultiplyBy(int amount)
        {
            byte[] result = new byte[digits.Length + 1];
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                int resultDigit = digits[i] * amount + result[i + 1];
                result[i] = (byte)(resultDigit / 10);
                result[i + 1] = (byte)(resultDigit % 10);
            }
            if (result[0] != 0)
            {
                digits = result;
            }
            else
            {
                Array.Copy(result, 1, digits, 0, digits.Length);
            }
            Normalize();
        }

        internal void Shift(int amount)
        {
            decimalPoint += amount;
        }

        internal void Normalize()
        {
            int first;
            for (first = 0; first < digits.Length && digits[first] == 0; first++)
            {
            }
            int last = digits.Length - 1;
            while (last >= 0 && digits[last] == 0)
            {
                last--;
            }
            if (first != 0 || last != digits.Length - 1)
            {
                byte[] tmp = new byte[last - first + 1];
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = digits[i + first];
                }
                decimalPoint -= digits.Length - (last + 1);
                digits = tmp;
            }
        }

        public override string ToString()
        {
            char[] digitString = new char[digits.Length];
            for (int i = 0; i < digits.Length; i++)
            {
                digitString[i] = (char)(digits[i] + 48);
            }
            if (decimalPoint == 0)
            {
                return new string(digitString);
            }
            if (decimalPoint < 0)
            {
                return new string(digitString) + new string('0', -decimalPoint);
            }
            if (decimalPoint >= digitString.Length)
            {
                return "0." + new string('0', decimalPoint - digitString.Length) + new string(digitString);
            }
            return new string(digitString, 0, digitString.Length - decimalPoint) + "." + new string(digitString, digitString.Length - decimalPoint, decimalPoint);
        }
    }

    public static string ToExactString(double d)
    {
        if (double.IsPositiveInfinity(d))
        {
            return "+Infinity";
        }
        if (double.IsNegativeInfinity(d))
        {
            return "-Infinity";
        }
        if (double.IsNaN(d))
        {
            return "NaN";
        }
        long num = BitConverter.DoubleToInt64Bits(d);
        bool negative = num < 0;
        int exponent = (int)(num >> 52 & 0x7FF);
        long mantissa = num & 0xFFFFFFFFFFFFFL;
        if (exponent == 0)
        {
            exponent++;
        }
        else
        {
            mantissa |= 0x10000000000000L;
        }
        exponent -= 1075;
        if (mantissa == 0L)
        {
            return "0";
        }
        while ((mantissa & 1) == 0L)
        {
            mantissa >>= 1;
            exponent++;
        }
        ArbitraryDecimal ad = new ArbitraryDecimal(mantissa);
        if (exponent < 0)
        {
            for (int i = 0; i < -exponent; i++)
            {
                ad.MultiplyBy(5);
            }
            ad.Shift(-exponent);
        }
        else
        {
            for (int j = 0; j < exponent; j++)
            {
                ad.MultiplyBy(2);
            }
        }
        if (negative)
        {
            return "-" + ad.ToString();
        }
        return ad.ToString();
    }
}
