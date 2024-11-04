// COPYRIGHT MUSINK LIMITED, NZ
// All rights reserveed
// Reuse, modification, or redistribution of this code is prohibited without explicit permission
// To obtain rights, contact Musink via https://musink.net/contact

using System.Collections;

namespace FlipProof.Base;

/// <summary>
/// Value-type IReadOnlyList with a single non-null item in it. Designed to keep pressure off the GC
/// </summary>
/// <typeparam name="T">Type contained</typeparam>
public readonly struct Singleton<T> : IReadOnlyList<T> where T:notnull
{
   readonly T _value;
   public T Value => _value;

   public int Count => 1;

   public T this[int index] => index == 0 ? _value : throw new IndexOutOfRangeException($"Only contains a single item but index {index} was recieved");

   /// <summary>
   /// Do NOT use. Creates a singleton with the default value of T set.
   /// </summary>
   public Singleton() : this(default!) { }
   
   public Singleton(T item)
   {
      _value = item;
   }

   public IEnumerator<T> GetEnumerator() => new Enumerator(this);
   IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);


   /// <summary>
   /// Based on the MSFT source code for List.Enumerator
   /// </summary>
   struct Enumerator : IEnumerator<T>
   {
      readonly Singleton<T> _list;
      int _index;
      T? _current;

      internal Enumerator(Singleton<T> list)
      {
         _list = list;
         _index = 0;
         _current = default;
      }

      public readonly void Dispose() { }

      public bool MoveNext()
      {
         if (_index == 0)
         {
            _current = _list.Value;
            _index++;
            return true;
         }
         _index = 1;
         _current = default;
         return false;
      }


      public readonly T Current => _current!;

      readonly object? IEnumerator.Current => _index != 1 ? throw new InvalidOperationException("Loop ended or not started") : (object?)Current;

      void IEnumerator.Reset()
      {
         _index = 0;
         _current = default;
      }
   }
}