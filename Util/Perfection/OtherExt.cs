using System;
using System.Collections.Generic;
using MorseCode.ITask;

#nullable enable
namespace Perfection
{
    public static class Mut
    {
        /// <summary>
        /// Catch me dead before I start typing variable names twice.<br></br>
        /// <i>This method just sets <paramref name="value"/> according to <paramref name="transformFunction"/>.</i>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="transformFunction"></param>
        public static void the<T>(ref T value, Func<T, T> transformFunction)
        {
            value = transformFunction(value);
        }
    }
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
        public static TOut ExprAs<T, TOut>(this T value, Func<T, TOut> transformFunction)
        {
            return transformFunction(value);
        }
        public static T Mut<T>(this T value, Action<T> mutFunction)
        {
            mutFunction(value);
            return value;
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
