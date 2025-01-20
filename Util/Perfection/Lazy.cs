using System.Collections.Generic;
using System.Collections;
using System;

#nullable enable
namespace Perfection
{
    
    public class LazyEnumerable<T> : IEnumerable<T>
    {
        public bool IsFullyInitialized => _iter is null;
        private IEnumerator<T>? _iter;
        private readonly List<T> _list;
        private int _cachedIndex = -1;

        public LazyEnumerable(IEnumerable<T> enumerable, int initialCapacity)
        {
            _iter = enumerable.GetEnumerator();
            _list = new(initialCapacity);
        }
        public LazyEnumerable(IEnumerable<T> enumerable)
        {
            _iter = enumerable.GetEnumerator();
            _list = new();
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; true; i++)
            {
                if (i <= _cachedIndex)
                {
                    yield return _list[i];
                } else if (_iter is not null && _iter.MoveNext())
                {
                    yield return _iter.Current;
                } else
                {
                    _iter = null;
                    yield break;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}