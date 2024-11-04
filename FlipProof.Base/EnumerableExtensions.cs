namespace FlipProof.Base;

public static class EnumerableExtensions
{
   /// <summary>
   /// Repeats the list the set number of times, saving the result to a list
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="items"></param>
   /// <param name="times"></param>
   /// <returns></returns>
   public static T[] RepeatToArray<T>(this IReadOnlyList<T> items, int times)
   {
      T[] result = new T[times * items.Count];

      int offset = 0;
      for (int i = 0;i < times; i++)
      {
         for(int k = 0; k < items.Count; k++)
         {
            result[offset++] = items[k];
         }
      }

      return result;
   }

    public static void Foreach<T>(this IEnumerable<T> items, Action<T> toDo)
    {
        foreach (T item in items)
        {
            toDo(item);
        }
    }

    public static List<TOut> Foreach<T, TOut>(this IEnumerable<T> items, Func<T, TOut> toDo)
    {
        List<TOut> results = new();
        foreach (T item in items)
        {
            results.Add(toDo(item));
        }

        return results;
    }
    
    public static async Task<List<TOut>> ForeachAsync<T, TOut>(this IEnumerable<T> items, Func<T, Task<TOut>> toDo)
    {
        List<TOut> results = new();
        foreach (T item in items)
        {
            results.Add(await toDo(item));
        }

        return results;
    }

    /// <summary>
    /// Runs an async task once per item, sequentially
    /// </summary>
    /// <param name="items">The items to process</param>
    /// <param name="act">The task to run</param>
    /// <typeparam name="T">Type of the items</typeparam>
    public static async Task ForeachAsync<T>(this IEnumerable<T> items, Func<T, Task> act)
    {
        foreach (T item in items)
        {
            await act(item);
        }
    }


    public static IEnumerable<T[]> Split<T>(this T[] arr, T delimeter, bool removeEmptyEntries = true) where T:notnull
    {
        return SplitIntoRange(arr, delimeter, removeEmptyEntries).Select(r => arr[r]);
    }
    public static IEnumerable<Range> SplitIntoRange<T>(this T[] arr, T delimeter, bool removeEmptyEntries=true) where T:notnull
    {
        bool allowEmpty = removeEmptyEntries;
        int nextFirst = 0;
        foreach (int curEnd in arr.IndicesOf(delimeter))
        {
            if (allowEmpty || curEnd > nextFirst)
                yield return nextFirst .. curEnd;
            
            nextFirst = curEnd+1;
        }

        if (allowEmpty || nextFirst < arr.Length - 1)
        {
            yield return nextFirst .. arr.Length;
        }
    }

    public static IEnumerable<int> IndicesOf<T>(this T[] arr, T delimeter) where T:notnull
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (delimeter.Equals(arr[i]))
            {
                yield return i;
            }
        }
    }
    
    public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> arr, T delimeter) where T:IEquatable<T>
    {
        return IndicesOf(arr, delimeter.Equals);
    }


    public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> arr, Func<T,bool> filter) where T : notnull
    {
        int i = 0;
        foreach (T item in arr)
        {
            if (filter(item))
            {
                yield return i;
            }
            i++;
        }
    }

    /// <summary>
    /// Runs the function exactly once on each entry and returns an array of the results
    /// </summary>
    /// <param name="arr">Input</param>
    /// <param name="transform">Function to run</param>
    /// <typeparam name="T">Input type</typeparam>
    /// <typeparam name="S">Output type</typeparam>
    /// <returns>Array of the results of the transform</returns>
    public static S[] ToArray<T,S>(this T[] arr, Func<T,S> transform)
    {
        S[] results = new S[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            results[i] = transform(arr[i]);
        }

        return results;
    }
}