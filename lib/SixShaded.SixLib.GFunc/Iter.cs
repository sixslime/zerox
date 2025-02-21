using System;

// nothing in this namespace is validated; it assumes you use it perfectly.
#nullable enable
namespace SixShaded.SixLib.GFunc
{
    public static class Iter
    {
        // "Map" is just a better name, fight me.
        public static IEnumerable<TResult> Map<TIn, TResult>(this IEnumerable<TIn> enumerable, Func<TIn, TResult> mapFunction)
        {
            //foreach (var e in enumerable) yield return mapFunction(e);
            // using Linq is probably ""optimized""
            return enumerable.Select(mapFunction);
        }
        public static IEnumerable<TResult> FilterMap<TIn, TResult>(this IEnumerable<TIn> enumerable, Func<TIn, IOption<TResult>> mapFunction)
        {
            foreach (var e in enumerable) if (mapFunction(e).Check(out var some)) yield return some;
        }
        public static IEnumerable<TCast> FilterCast<TCast>(this IEnumerable<object> enumerable)
        {
            foreach (var e in enumerable) if (e is TCast casted) yield return casted;
        }
        public static IEnumerable<T> Also<T>(this IEnumerable<T> enumerable, IEnumerable<T> also)
        {
            // same situation as 'Map()'
            return enumerable.Concat(also);
            //foreach (var v in enumerable) yield return v;
            //foreach (var v in also) yield return v;
        }

        public static IEnumerable<(int index, T value)> Enumerate<T>(this IEnumerable<T> enumerable)
        {
            int i = 0;
            foreach (var v in enumerable) yield return (i++, v);
        }

        /// <summary>
        /// WARNING: generates *infinite* values. Must be used with a terminator (such as <see cref="Until{T}(IEnumerable{T}, Predicate{T})"/>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> Sequence<T>(this T startingValue, Func<T, T> function)
        {
            var o = startingValue;
            while (true)
            {
                yield return o;
                o = function(o);
            }
        }

        public static TResult AccumulateInto<TIn, TResult>(this IEnumerable<TIn> enumerable, TResult startingValue, Func<TResult, TIn, TResult> function)
        {
            var o = startingValue;
            foreach (var v in enumerable)
            {
                o = function(o, v);
            }
            return o;
        }
        public static IOption<T> Accumulate<T>(this IEnumerable<T> enumerable, Func<T, T, T> function) => enumerable.Accumulate(i => i, function);
        public static IOption<TResult> Accumulate<TIn, TResult>(this IEnumerable<TIn> enumerable, Func<TIn, TResult> placementFunction, Func<TResult, TIn, TResult> function)
        {
            var iter = enumerable.GetEnumerator();
            if (!iter.MoveNext()) return new None<TResult>();
            var o = placementFunction(iter.Current);
            while (iter.MoveNext())
            {
                o = function(o, iter.Current);
            }
            return o.AsSome();
        }

        public static IEnumerable<T> Until<T>(this IEnumerable<T> enumerable, Predicate<T> breakCondition)
        {
            foreach (var v in enumerable)
            {
                yield return v;
                if (breakCondition(v)) yield break;
            }
        }

        public static IEnumerable<T> After<T>(this IEnumerable<T> enumerable, Predicate<T> startCondition)
        {
            var iter = enumerable.GetEnumerator();
            while (iter.MoveNext())
            {
                if (startCondition(iter.Current))
                {
                    yield return iter.Current;
                    break;
                }
            }
            while (iter.MoveNext()) yield return iter.Current;
        }
        public static IOption<T> FirstMatch<T>(this IEnumerable<T> enumerable, Predicate<T> matchPredicate)
        {
            foreach (var v in enumerable) if (matchPredicate(v)) return v.AsSome();
            return new None<T>();
        }
        public static IOption<T> GetAt<T>(this IEnumerable<T> enumerable, int index)
        {
            if (index < 0) return new None<T>();
            var iter = enumerable.GetEnumerator();
            for (int i = 0; i <= index; i++)
            {
                if (!iter.MoveNext()) return new None<T>();
            }
            return iter.Current.AsSome();
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> enumerable)
        {
            foreach (var list in enumerable)
                foreach (var e in list) yield return e;
        }

        public static bool HasMatch<T>(this IEnumerable<T> enumerable, Predicate<T> matchCondition)
        {
            foreach (var v in enumerable) if (matchCondition(v)) return true;
            return false;
        }

        public static IEnumerable<T> Yield<T>(this T value, int amount = 1)
        {
            for (int i = 0; i < amount; i++) yield return value;
        }

        public static IEnumerable<(T1 a, T2 b)> ZipShort<T1, T2>(this IEnumerable<T1> enumerableA, IEnumerable<T2> enumerableB)
        {
            var iter1 = enumerableA.GetEnumerator();
            var iter2 = enumerableB.GetEnumerator();
            while (iter1.MoveNext() && iter2.MoveNext()) yield return (iter1.Current, iter2.Current);
        }

        public static IEnumerable<(IOption<T1> a, IOption<T2> b)> ZipLong<T1, T2>(this IEnumerable<T1> enumerableA, IEnumerable<T2> enumerableB)
        {
            var iter1 = enumerableA.GetEnumerator();
            var iter2 = enumerableB.GetEnumerator();
            while (iter1.MoveNext() && iter2.MoveNext()) yield return (iter1.Current.AsSome(), iter2.Current.AsSome());
            while (iter1.MoveNext()) yield return (iter1.Current.AsSome(), new None<T2>());
            while (iter2.MoveNext()) yield return (new None<T1>(), iter2.Current.AsSome());
        }

        public static IEnumerable<T> ContinueAfter<T>(this IEnumerable<T> enumerable, Func<IEnumerable<T>, IEnumerable<T>> consumer)
        {
            var iter = enumerable.GetEnumerator();
            foreach (var v in consumer(iter.AsEnumerable())) yield return v;
            foreach (var v in iter.AsEnumerable()) yield return v;
        }

        public static IEnumerable<T> AsEnumerable<T>(this IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext()) yield return enumerator.Current;
        }

        public static IEnumerable<T> IEnumerable<T>(this IEnumerable<T> enumerable) => enumerable;

        public static IEnumerable<T> Over<T>(params T[] arr) => arr;

        public static IEnumerable<int> ToIter(this Range range, bool inclusiveEnd = false)
            => Range(range.Start.Value, range.End.Value, inclusiveEnd);
        public static IEnumerable<int> Range(int start, int end, bool inclusiveEnd = false)
        {
            for (int i = start; i < end; i++)
            {
                yield return i;
            }
            if (inclusiveEnd) yield return end;
        }
    }

}