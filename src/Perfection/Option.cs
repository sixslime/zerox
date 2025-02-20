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
        public T Value { get; }
        public Some(T value) { Value = value; }
        public override string ToString()
        {
            return $"Some({Value})";
        }
    }
    public record None<T> : IOption<T>
    {
        public override string ToString()
        {
            return $"None<{typeof(T).Name}>";
        }
    }
    public static class Option
    {
        public static T Expect<T>(this IOption<T> option, string message)
        {
            return option switch
            {
                ISome<T> ok => ok.Value,
                _ => throw new ExpectedValueException(message)
            };
        }
        public static T Unwrap<T>(this IOption<T> option)
            => Expect(option, "'None' value unwrapped, expected 'Some'");
        public static IOption<T> AsSome<T>(this T value)
        {
            return new Some<T>(value);
        }
        public static IOption<T> Retain<T>(this IOption<T> option, Predicate<T> predicate)
        {
            return option.Check(out var v)
                ? predicate(v) ? v.AsSome() : new None<T>()
                : option;
        }
        // kinda gay that it cannot be compiler asserted that val is not null if returns true.
        public static bool Check<T>(this IOption<T> option, out T val)
        {
            if (option is ISome<T> ok)
            {
                val = ok.Value;
                return true;
            }
            val = default!;
            return false;
        }
        public static bool CheckNone<T>(this IOption<T> option, out T val) => !Check(option, out val);
        public static IOption<TOut> RemapAs<TIn, TOut>(this IOption<TIn> option, Func<TIn, TOut> func)
        {
            return CheckNone(option, out var o) ? new None<TOut>() : func(o).AsSome();
        }
        public static IOption<T> AsNone<T>(this T? _)
        {
            return new None<T>();
        }
        public static IOption<T> ToOptionLazy<T>(this bool some, Func<T> someValue)
        {
            return some
                ? someValue().AsSome()
                : new None<T>();
        }
        public static IOption<T> ToOption<T>(this bool some, T someValue)
        {
            return some
                ? someValue.AsSome()
                : new None<T>();
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
        public static IResult<T, E> OrErr<T, E>(this IOption<T> option, E err)
        {
            return option.Check(out var ok)
                ? new Ok<T, E>(ok)
                : new Err<T, E>(err);
        }
        public static IResult<T, E> OrElseErr<T, E>(this IOption<T> option, Func<E> err)
        {
            return option.Check(out var ok)
                ? new Ok<T, E>(ok)
                : new Err<T, E>(err());
        }
        public static IOption<T> Press<T>(this IOption<IOption<T>> option)
        {
            return option.Check(out var inner) ? (inner.Check(out var val) ? val.AsSome() : new None<T>()) : new None<T>();
        }
        public static IOption<T> None<T>(this IOption<T>? option) =>  option is null || option.IsSome() ? new None<T>() : option;
        public static IOption<T> Some<T>(this IOption<T>? _, T value) => new Some<T>(value);
    }
    
}