using System.Collections.Generic;
using System.Collections;
using System;

#nullable disable
namespace Perfection
{
    /// <summary>
    /// <paramref name="original"/> is to be named 'Q' by convention.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="original"></param>
    /// <returns></returns>
    public delegate T Updater<T>(T original);
    public struct Empty<T> : IEnumerable<T>
    {
        public readonly IEnumerator<T> GetEnumerator()
        {
            yield break;
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            yield break;
        }
        public static IEnumerable<T> Yield()
        {
            yield break;
        }
    }
    // Shitty ass HashSet
    public record PSet<T> : PIndexedSet<T, T>
    {
        public PSet(int modulo) : base(x => x, modulo) { }
        public override string ToString() => _storage
            .AccumulateInto($"PSet:\n", (msg1, x) => msg1 +
        $"{x.AccumulateInto(">", (msg2, y) => msg2 + $" [{y}]\n  ")}\n");
    }
    public record PIndexedSet<I, T> : IEnumerable<T>
    {
        protected readonly List<List<T>> _storage;
        public IEnumerable<T> Elements
        {
            get => _storage.Flatten(); init
            {
                Count = 0;
                _storage = new List<List<T>>(Modulo);
                _storage.AddRange(new List<T>(2).Sequence((_) => new(2)).Take(Modulo));
                foreach (var v in value)
                {
                    var bindex = IndexGenerator(v).GetHashCode().Abs() % Modulo;
                    var foundAt = _storage[bindex].FindIndex(x => IndexGenerator(v).Equals(IndexGenerator(x)));
                    if (foundAt == -1) _storage[bindex].Add(v);
                    else _storage[bindex][foundAt] = v;
                }
            }
        }
        public Updater<IEnumerable<T>> dElements { init => Elements = value(Elements); }
        public readonly int Modulo;
        public readonly int Count;
        public readonly Func<T, I> IndexGenerator;
        public PIndexedSet(Func<T, I> indexGenerator, int modulo)
        {
            Modulo = modulo;
            IndexGenerator = indexGenerator;
            Count = 0;
            _storage = new(0);
        }
        public bool Contains(I index) => GetBucket(index).HasMatch(x => IndexGenerator(x).Equals(index));
        public T this[I index] => GetBucket(index).Find(x => IndexGenerator(x).Equals(index));
        private List<T> GetBucket(I index) => _storage[index.GetHashCode().Abs() % Modulo];
        public IEnumerator<T> GetEnumerator() => _storage.Flatten().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _storage.Flatten().GetEnumerator();
        public override string ToString() => _storage.AccumulateInto("PIndexedSet:\n", (msg1, x) => msg1 +
        $"{x.AccumulateInto(">", (msg2, y) => msg2 + $" [{IndexGenerator(y)} : {y}]\n  ")}\n");
    }
    // Shitty ass Dictionary
    public record PMap<K, T>
    {
        private readonly List<List<(K key, T val)>> _storage;
        public IEnumerable<(K key, T val)> Elements
        {
            get => _storage.Flatten(); init
            {
                Count = 0;
                _storage = new List<List<(K key, T val)>>(Modulo);
                _storage.AddRange(new List<(K key, T val)>(2).Sequence((_) => new(2)).Take(Modulo));
                foreach (var v in value)
                {
                    var bindex = v.key.GetHashCode().Abs() % Modulo;
                    var foundAt = _storage[bindex].FindIndex(x => v.key.Equals(x.key));
                    if (foundAt == -1) _storage[bindex].Add(v);
                    else _storage[bindex][foundAt] = v;
                }
            }
        }
        public Updater<IEnumerable<(K key, T val)>> dElements { init => Elements = value(Elements); }
        public readonly int Modulo;
        public readonly int Count;
        public PMap(int modulo)
        {
            Modulo = modulo;
            Count = 0;
            _storage = new(0);
        }
        public T this[K indexer] => GetBucket(indexer).Find(x => indexer.Equals(x.key)).val;
        private List<(K key, T val)> GetBucket(K element) => _storage[element.GetHashCode().Abs() % Modulo];
        public override string ToString() => Elements.AccumulateInto("PMap:\n", (msg, x) => msg + $"- [{x.key} : {x.val}]\n");
    }
    public record PList<T>
    {
        public required IEnumerable<T> Elements { get
            {
                return _list.Also(ConsumingIter());
            }
            init
            {
                _enumerator = value.GetEnumerator();
            }
        }
        public Updater<IEnumerable<T>> dElements { init => Elements = value(Elements); }
        public PList()
        {
            _list = new(0);
            _cachedIndex = -1;
        }
        public T this[int i] { get
            {
                _ = ConsumeToIndex(i);
                return _list[i];
            } 
        }
        public T[] ToArray() { return _list.ToArray(); }
        public override string ToString() => Elements.AccumulateInto("PList:\n", (msg, x) => msg + $"- {x}\n");

        private IEnumerable<T> ConsumingIter()
        {
            while (_enumerator.MoveNext())
            {
                yield return Consume();
            }
        }
        private IEnumerable<T> ConsumeToIndex(int index)
        {
            while (index < _cachedIndex)
            {
                yield return Consume();
            }
        }
        private T Consume()
        {
            var v = _enumerator.Current;
            _list.Add(v);
            _cachedIndex++;
            return v;
        }
        private IEnumerator<T> _enumerator;
        private int _cachedIndex;
        private readonly List<T> _list;
    }
}