using MorseCode.ITask;
using SixShaded.NotRust;

namespace SixShaded.SixLib.GFunc;

public static class Mut
{
    /// <summary>
    ///     Catch me dead before I start typing variable names twice.<br></br>
    ///     <i>This method just sets <paramref name="value" /> according to <paramref name="transformFunction" />.</i>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="transformFunction"></param>
    public static void the<T>(ref T value, Func<T, T> transformFunction) => value = transformFunction(value);
}

public static class Casting
{
    public static T IsA<T>(this object value) => (T)value;
    public static IOption<T> MightBeA<T>(this object value) => value is T o ? o.AsSome() : new None<T>();
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

    public static TOut ExprAs<T, TOut>(this T value, Func<T, TOut> transformFunction) => transformFunction(value);

    public static T Mut<T>(this T value, Action<T> mutFunction)
    {
        mutFunction(value);
        return value;
    }

    /// <summary>
    ///     All RN's hate the '!' symbol.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static bool Not(this bool v) => !v;
}

public static class Integer
{
    public static int Abs(this int value) => Math.Abs(value);
}

public static class Async
{
    public static ITask<T> ToCompletedITask<T>(this T obj) => Task.FromResult(obj).AsITask();
    public static Task<T> ToCompletedTask<T>(this T obj) => Task.FromResult(obj);
}