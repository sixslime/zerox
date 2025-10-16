namespace SixShaded.NotRust;
public interface IOption<out T>
{ }

public interface ISome<out T> : IOption<T>
{
    public T Value { get; }
}

public record Some<T> : ISome<T>
{
    public Some(T value)
    {
        Value = value;
    }

    public override string ToString() => $"Some({Value})";
    public T Value { get; }
}

public record None<T> : IOption<T>
{
    public override string ToString() => $"None<{typeof(T).Name}>";
}

public static class Option
{
    public static T Expect<T>(this IOption<T> option, string message) =>
        option switch
        {
            ISome<T> ok => ok.Value,
            _ => throw new ExpectedValueException(message),
        };

    public static T Expect<T>(this IOption<T> option, Func<Exception> exceptionExpr) =>
        option switch
        {
            ISome<T> ok => ok.Value,
            _ => throw exceptionExpr(),
        };

    public static T Unwrap<T>(this IOption<T> option) => option.Expect("'None' value unwrapped, expected 'Some'");
    public static IOption<T> AsSome<T>(this T value) => new Some<T>(value);

    public static IOption<T> Retain<T>(this IOption<T> option, Predicate<T> predicate) =>
        option.Check(out var v)
            ? predicate(v) ? v.AsSome() : new None<T>()
            : option;

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

    public static bool CheckNone<T>(this IOption<T> option, out T val) => !option.Check(out val);
    public static IOption<TOut> RemapAs<TIn, TOut>(this IOption<TIn> option, Func<TIn, TOut> func) => option.CheckNone(out var o) ? new None<TOut>() : func(o).AsSome();
    public static IOption<T> AsNone<T>(this T? _) => new None<T>();

    public static IOption<T> ToOptionLazy<T>(this bool some, Func<T> someValue) =>
        some
            ? someValue().AsSome()
            : new None<T>();
    public static IOption<T> ToOption<T>(this bool some, T someValue) =>
        some
            ? someValue.AsSome()
            : new None<T>();

    public static bool IsSome<T>(this IOption<T> option) => option.Check(out _);
    public static IOption<T> NullToNone<T>(this T? value) => value is not null ? value.AsSome() : new None<T>();
    public static T Or<T>(this IOption<T> option, T noneValue) => option.Check(out var val) ? val : noneValue;
    public static T OrElse<T>(this IOption<T> option, Func<T> noneEval) => option.Check(out var val) ? val : noneEval();
    public static IOption<T> OrTry<T>(this IOption<T> option, IOption<T> noneValue) => option.IsSome() ? option : noneValue;
    public static IOption<T> OrElseTry<T>(this IOption<T> option, Func<IOption<T>> noneEval) => option.IsSome() ? option : noneEval();

    public static IResult<T, E> OrErr<T, E>(this IOption<T> option, E err) =>
        option.Check(out var ok)
            ? new Ok<T, E>(ok)
            : new Err<T, E>(err);

    public static IResult<T, E> OrElseErr<T, E>(this IOption<T> option, Func<E> err) =>
        option.Check(out var ok)
            ? new Ok<T, E>(ok)
            : new Err<T, E>(err());

    public static IOption<T> Press<T>(this IOption<IOption<T>> option) => option.Check(out var inner) ? inner.Check(out var val) ? val.AsSome() : new None<T>() : new None<T>();
    public static IOption<T> None<T>(this IOption<T>? option) => option is null || option.IsSome() ? new None<T>() : option;
    public static IOption<T> Some<T>(this IOption<T>? _, T value) => new Some<T>(value);
}