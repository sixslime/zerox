using System.Collections;
using System.Collections.Generic;
using System;

#nullable enable
namespace Perfection
{
    public interface IOption<out T> { }
    public interface ISome<out T> : IOption<T>
    {
        public T Value { get; }
    }
    public record Some<T> : ISome<T>
    {
        public T Value => _value;
        public Some(T value) => _value = value;
        private readonly T _value;
        public override string ToString()
        {
            return $"| {_value} |";
        }
    }
    public record None<T> : IOption<T>
    {
        public override string ToString()
        {
            return $"(None<{typeof(T).Name}>)";
        }
    }
    public static class Option
    {
        public static T Unwrap<T>(this IOption<T> option)
        {
            return option switch
            {
                ISome<T> ok => ok.Value,
                _ => throw new System.Exception("Unwrapped non-Some option.")
            };
        }
        public static ISome<T> AsSome<T>(this T value)
        {
            return new Some<T>(value);
        }
        // kinda gay that it cannot be compiler asserted that val is not null if returns true.
        public static bool Check<T>(this IOption<T> option, out T val)
        {
            if (option is ISome<T> ok)
            {
                val = ok.Value;
                return true;
            }
            val = default;
            return false;
        }
        public static bool CheckNone<T>(this IOption<T> option, out T val) => !Check(option, out val);
        public static IOption<TOut> RemapAs<TIn, TOut>(this IOption<TIn> option, Func<TIn, TOut> func)
        {
            return CheckNone(option, out var o) ? new None<TOut>() : func(o).AsSome();
        }
        public static IOption<T> None<T>(this T _)
        {
            return new None<T>();
        }
        public static bool IsSome<T>(this IOption<T> option) { return option.Check(out var _); }
        public static IOption<T> NullToNone<T>(this T? value)
        {
            return value is not null ? value.AsSome() : new None<T>();
        }
        public static T Or<T>(this IOption<T> option, T noneValue)
        {
            return option.Check(out var val) ? val : noneValue;
        }
        public static T OrElse<T>(this IOption<T> option, Func<T> noneEval)
        {
            return option.Check(out var val) ? val : noneEval();
        }
        public static IOption<T> Press<T>(this IOption<IOption<T>> option)
        {
            return option.Check(out var inner) ? (inner.Check(out var val) ? val.AsSome() : new None<T>()) : new None<T>();
        }
    }
    
}