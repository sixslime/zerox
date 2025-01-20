using System.Collections.Generic;
using System.Collections;
using System;

#nullable enable
namespace Perfection
{
    
    public class CachingEnumerable<T> : IEnumerable<T>, IIndexReadable<int, T>
    {
        public bool IsFullyCached => _iter is null;
        public int CachedIndex => _cachedIndex;

        private IEnumerator<T>? _iter;
        private readonly List<T> _list;
        private int _cachedIndex = -1;

        public CachingEnumerable(IEnumerable<T> enumerable, int initialCapacity)
        {
            _iter = enumerable.GetEnumerator();
            _list = new(initialCapacity);
        }
        public CachingEnumerable(IEnumerable<T> enumerable)
        {
            _iter = enumerable.GetEnumerator();
            _list = new();
        }
        public int CountAndCache()
        {
            if (_iter is not null)
            {
                while (_iter.MoveNext()) _list.Add(_iter.Current);
                _list.TrimExcess();
                _iter = null;
                _cachedIndex = _list.Count;
            }
            return _list.Count;
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
                    _cachedIndex++;
                    _list.Add(_iter.Current);
                    yield return _iter.Current;
                } else
                {
                    if (!IsFullyCached)
                    {
                        _iter = null;
                        _list.TrimExcess();
                    }
                    yield break;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T At(int index)
        {
            while (_cachedIndex < index && _iter is not null)
            {
                if (!_iter.MoveNext()) throw new IndexOutOfRangeException();
                _cachedIndex++;
                _list.Add(_iter.Current);
            }
            return _list[index];
        }
    }
}