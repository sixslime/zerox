using System;
using System.Collections.Generic;
using MorseCode.ITask;

#nullable enable
namespace Perfection
{
    public static class Misc
    {
        public static List<T> Reversed<T>(this List<T> list)
        {
            List<T> o = new(list);
            o.Reverse();
            return o;
        }
        public static List<T> FillEmpty<T>(this List<T> list, T item)
        {
            for (int i = list.Count; i < list.Capacity; i++) list.Add(item);
            return list;
        }
        public static T Mut<T>(this T value, Func<T, T> transformFunction)
        {
            return transformFunction(value);
        }
    }
    public static class Integer
    {
        public static int Abs(this int value) => Math.Abs(value);
    }
    public static class Async
    {
        public static ITask<T> ToCompletedITask<T>(this T obj)
        {
            return Task.FromResult(obj).AsITask();
        }
    }
}
