using System.Collections.Generic;
using System.Collections;
using System;

#nullable enable
namespace Perfection
{
    // MUST: methods that start with '_' must return the instatiating type.
    public interface IHasElements<out T>
    {
        public int Count { get; }
        public IEnumerable<T> Elements { get; }
    }
    public interface IEntryAddable<in T>
    {
        public IEntryAddable<T> _WithEntries(IEnumerable<T> entries);
    }
    public interface IEntryRemovable<in T>
    {
        public IEntryRemovable<T> _WithoutEntries(IEnumerable<T> entries);
    }
    public interface IIndexReadable<in I, out T>
    {
        public T At(I index);
    }
    public interface IMergable<U> where U : IMergable<U>
    {
        public U _Merge(U union);
    }
    public interface IMap<K, T> : IHasElements<ITiple<K, T>>, IEntryAddable<ITiple<K, T>>, IEntryRemovable<K>, IIndexReadable<K, IOption<T>>, IMergable<IMap<K, T>>
    { }
    public interface ISet<T> : IHasElements<T>, IEntryAddable<T>, IEntryRemovable<T>, IIndexReadable<T, bool>, IMergable<ISet<T>>
    { }
    public interface ISequence<T> : IHasElements<T>, IIndexReadable<int, T>, IEntryAddable<ITiple<int, T>>, IEntryAddable<T>, IEntryAddable<ITiple<int, IEnumerable<T>>>, IMergable<ISequence<T>>, IEntryRemovable<Range>
    { }

    public static class SelfTypeAssumption
    {
        public static Self WithEntries<Self, T>(this Self s, IEnumerable<T> values) where Self : IEntryAddable<T>
        { return (Self)s._WithEntries(values); }
        public static Self WithoutEntries<Self, T>(this Self s, IEnumerable<T> values) where Self : IEntryRemovable<T>
        { return (Self)s._WithoutEntries(values); }
        public static Self Merge<Self, T>(this Self s, Self other) where Self : IMergable<Self>
        { return s._Merge(other); }
    }

    // DEV/FIXME: temporary bare-functionality inneficient implementations.

    public class PMap<K, T>() : IMap<K, T> where K : notnull
    {
        private readonly Dictionary<K, T> _dict = new();
        public int Count => _dict.Count;

        public IEnumerable<ITiple<K, T>> Elements => _dict.Map(x => (x.Key, x.Value).Tiple().ITiple());

        private PMap(Dictionary<K, T> dict) : this() => _dict = dict;
        public IOption<T> At(K index)
        {
            return _dict.TryGetValue(index, out var v).ToOption(v)!;
        }

        IMap<K, T> IMergable<IMap<K, T>>._Merge(IMap<K, T> union)
        {
            throw new NotImplementedException();
        }

        IEntryAddable<ITiple<K, T>> IEntryAddable<ITiple<K, T>>._WithEntries(IEnumerable<ITiple<K, T>> entries)
        {
            var ndict = new Dictionary<K, T>(_dict);
            foreach (var e in entries) ndict[e.A] = e.B;
            return new PMap<K, T>(ndict);
        }

        IEntryRemovable<K> IEntryRemovable<K>._WithoutEntries(IEnumerable<K> entries)
        {
            var ndict = new Dictionary<K, T>(_dict);
            foreach (var r in entries) _ = ndict.Remove(r);
            return new PMap<K, T>(ndict);
        }
    }
    public clas
}