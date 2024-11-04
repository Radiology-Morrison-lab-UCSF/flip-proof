using System.Numerics;

namespace FlipProof.Base;

public static class ArrayMath
{
   public static T[] Add<T>(this T[] arr, T val) where T: INumber<T>
   {
      T[] result = new T[arr.Length];
      for (int i = 0; i < arr.Length; i++)
      {
         result[i] = arr[i] + val;
      }

      return result;
   }
   public static T[] Divide<T>(this T[] arr, T val) where T: INumber<T>
   {
      T[] result = new T[arr.Length];
      for (int i = 0; i < arr.Length; i++)
      {
         result[i] = arr[i] / val;
      }

      return result;
   }
   
   public static T[] Divide<T>(this T val, T[] right) where T: INumber<T>
   {
      T[] result = new T[right.Length];
      for (int i = 0; i < right.Length; i++)
      {
         result[i] = val / right[i];
      }

      return result;
   }
   
   public static T[] Multiply<T>(this T[] arr, T val) where T: INumber<T>
   {
      T[] result = new T[arr.Length];
      for (int i = 0; i < arr.Length; i++)
      {
         result[i] = arr[i] * val;
      }

      return result;
   }   
   public static Complex[] Multiply(this double[] arr, Complex val)
   {
      Complex[] result = new Complex[arr.Length];
      for (int i = 0; i < arr.Length; i++)
      {
         result[i] = arr[i] * val;
      }

      return result;
   }   
   public static T[] Negate<T>(this T[] arr) where T: INumber<T>
   {
      T[] result = new T[arr.Length];
      for (int i = 0; i < arr.Length; i++)
      {
         result[i] = -arr[i];
      }

      return result;
   }
   public static T[] Subtract<T>(this T[] arr, T val) where T: INumber<T>
   {
      T[] result = new T[arr.Length];
      for (int i = 0; i < arr.Length; i++)
      {
         result[i] = arr[i] - val;
      }

      return result;
   }
   
   public static T[] Add<T>(this T[] left, T[] right) where T: INumber<T>
   {
      CheckLengthsSame(left, right);
         
      T[] result = new T[left.Length];
      for (int i = 0; i < left.Length; i++)
      {
         result[i] = left[i] + right[i];
      }

      return result;
   }


   public static T[] Divide<T>(this T[] left, T[] right) where T: INumber<T>
   {
      CheckLengthsSame(left, right);
         
      T[] result = new T[left.Length];
      for (int i = 0; i < left.Length; i++)
      {
         result[i] = left[i] / right[i];
      }

      return result;
   }
   
   public static T[] Multiply<T>(this T[] left, T[] right) where T: INumber<T>
   {
      CheckLengthsSame(left, right);
         
      T[] result = new T[left.Length];
      for (int i = 0; i < left.Length; i++)
      {
         result[i] = left[i] * right[i];
      }

      return result;
   }
   public static Complex[] Multiply(this double[] left, Complex[] right) 
   {
      CheckLengthsSame(left, right);
         
      Complex[] result = new Complex[left.Length];
      for (int i = 0; i < left.Length; i++)
      {
         result[i] = left[i] * right[i];
      }

      return result;
   }
   public static T[] Subtract<T>(this T[] left, T[] right) where T: INumber<T>
   {
      CheckLengthsSame(left, right);
         
      T[] result = new T[left.Length];
      for (int i = 0; i < left.Length; i++)
      {
         result[i] = left[i] - right[i];
      }

      return result;
   }
   
   static void CheckLengthsSame(Array left, Array right)
   {
      if (left.Length != right.Length)
      {
         throw new ArgumentException("Array sizes mismatch");
      }
   }
   
   /// <summary>
   /// Returns the next item above the percentile specified
   /// </summary>
   /// <param name="dat">Data</param>
   /// <param name="percentile">0 - 1, where 0 means the smallest and 1 means the largest</param>
   /// <typeparam name="T"></typeparam>
   /// <returns></returns>
   public static T ApproxPercentile<T>(this T[] dat, float percentile) where T: INumber<T>
   {
      if (percentile is < 0 or > 1)
      {
         throw new ArgumentOutOfRangeException(nameof(percentile));
      }
      return dat.OrderBy(a => a).Skip(Math.Max(0,Convert.ToInt32(percentile * dat.Length - 1))).First();
   }

   /// <summary>
   /// Applies the provided function to each element, one at a time, and replaces the value stored. 
   /// Value is replaced upon calculation for that element, not after all elements have been calculated
   /// </summary>
   /// <typeparam name="T">Data type</typeparam>
   /// <param name="dat">Data</param>
   /// <param name="func">To apply</param>
   public static void ApplyInPlace<T>(this T[] dat, Func<T,T> func)
   {
      for (int i = 0;i < dat.Length;i++)
      {
         dat[i] = func(dat[i]);
      }
   }

   public static double StdDev(this IReadOnlyList<double> values)
   {
      double ret = 0;
      int count = values.Count;
      if (count > 1)
      {
         //Compute the Average
         double avg = values.Average();

         //Perform the Sum of (value-avg)^2
         double sum = values.Sum(d => (d - avg) * (d - avg));

         //Put it all together
         ret = Math.Sqrt(sum / count);
      }
      return ret;
   }   
   
   public static double StdDev(this IReadOnlyList<float> values)
   {
      double ret = 0;
      int count = values.Count;
      if (count > 1)
      {
         //Compute the Average
         double avg = values.Average();

         //Perform the Sum of (value-avg)^2
         double sum = values.Sum(d => (d - avg) * (d - avg));

         //Put it all together
         ret = Math.Sqrt(sum / count);
      }
      return ret;
   }

   public static T[] ElementWiseSquare<T>(this T[] arr) where T: INumber<T>
   {
      T[] result = new T[arr.Length];
      for (int i = 0; i < arr.Length; i++)
      {
         T el = arr[i];
         result[i] = el * el;
      }
      return result;
   }

}