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
    public record ITupleWrapper<T1, T2> : ITuple<T1, T2>
    {
        public ITupleWrapper((T1, T2) tuple)
        {
            Unwrapped = tuple;
        }
        public readonly (T1, T2) Unwrapped;
        public T1 Item1 => Unwrapped.Item1;
        public T2 Item2 => Unwrapped.Item2;

    }
    public record ITupleWrapper<T1, T2, T3> : ITuple<T1, T2, T3>
    {
        public ITupleWrapper((T1, T2, T3) tuple)
        {
            Unwrapped = tuple;
        }
        public readonly (T1, T2, T3) Unwrapped;
        public T1 Item1 => Unwrapped.Item1;
        public T2 Item2 => Unwrapped.Item2;
        public T3 Item3 => Unwrapped.Item3;
    }
    public static class _Extensions
    {
        public static ITupleWrapper<T1, T2> AsITuple<T1, T2>(this (T1, T2) tuple)
        {
            return new(tuple);
        }
        public static ITupleWrapper<T1, T2, T3> AsITuple<T1, T2, T3>(this (T1, T2, T3) tuple)
        {
            return new(tuple);
        }
    }
}
