using System.Collections.Generic;
using System.Collections;
using System;

#nullable disable
namespace Perfection
{
    public interface ITuple<out T1, out T2>
    {
        public T1 Item1 { get; }
        public T2 Item2 { get; }
    }
    public interface ITuple<out T1, out T2, out T3>
    {
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
    }
    public record ReadonlyTuple<T1, T2> : ITuple<T1, T2>
    {
        public required T1 Item1 { get; init; }
        public required T2 Item2 { get; init; }
    }
    public record ReadonlyTuple<T1, T2, T3> : ITuple<T1, T2, T3>
    {
        public required T1 Item1 { get; init; }
        public required T2 Item2 { get; init; }
        public required T3 Item3 { get; init; }
    }
    public static class _Extensions
    {
        public static ITuple<T1, T2> AsITuple<T1, T2>(this Tuple<T1, T2> tuple)
        {
            return new ReadonlyTuple<T1, T2>() { Item1 = tuple.Item1, Item2 = tuple.Item2 };
        }
        public static ITuple<T1, T2, T3> AsITuple<T1, T2, T3>(this Tuple<T1, T2, T3> tuple)
        {
            return new ReadonlyTuple<T1, T2, T3>() { Item1 = tuple.Item1, Item2 = tuple.Item2, Item3 = tuple.Item3};
        }
    }
}
