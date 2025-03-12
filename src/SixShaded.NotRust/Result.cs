namespace SixShaded.NotRust;

/// <summary>
///     Representation of no value or data.
/// </summary>
public class NoVal
{ }

public interface IResult<out T, out E>
{ }

public interface IOk<out T, out E> : IResult<T, E>
{
    T Value { get; }
}

public interface IErr<out T, out E> : IResult<T, E>
{
    E Value { get; }
}

public record Ok<T, E>(T value) : IOk<T, E>
{
    public override string ToString() => $"Ok({Value})";
    public T Value { get; } = value;
}

public record Err<T, E>(E value) : IErr<T, E>
{
    public override string ToString() => $"Err({Value})";
    public E Value { get; } = value;
}

public class ExpectedValueException(string message) : Exception(message)
{ }

public class Hint<T>
{
    public static readonly Hint<T> HINT = new();

    private Hint()
    { }
}

public static class Result
{
    public static T ExpectOk<T, E>(this IResult<T, E> result, string message) => result is IOk<T, E> ok ? ok.Value : throw new ExpectedValueException(message);
    public static E ExpectErr<T, E>(this IResult<T, E> result, string message) => result is IErr<T, E> ok ? ok.Value : throw new ExpectedValueException(message);
    public static T UnwrapOk<T, E>(this IResult<T, E> result) => result.ExpectOk("'Err' value unwrapped, expected 'Ok'.");
    public static E UnwrapErr<T, E>(this IResult<T, E> result) => result.ExpectErr("'Ok' value unwrapped, expected 'Err'.");

    public static bool Split<T, E>(this IResult<T, E> result, out T ok, out E err)
    {
        ok = default!;
        err = default!;
        if (result is IOk<T, E> v)
        {
            ok = v.Value;
            return true;
        }
        if (result is IErr<T, E> e)
        {
            err = e.Value;
            return false;
        }
        throw new("Unsupported IResult type?");
    }

    public static bool CheckOk<T, E>(this IResult<T, E> result, out T ok) => result.Split(out ok, out _);
    public static bool CheckErr<T, E>(this IResult<T, E> result, out E err) => !result.Split(out _, out err);
    public static IResult<T, E> AsOk<T, E>(this T value, Hint<E> _) => new Ok<T, E>(value);
    public static IResult<T, E> AsErr<T, E>(this E value, Hint<T> _) => new Err<T, E>(value);

    public static IResult<T, Exception> CatchException<T>(this Func<T> tryExpr)
    {
        try
        {
            return new Ok<T, Exception>(tryExpr());
        }
        catch (Exception e)
        {
            return new Err<T, Exception>(e);
        }
    }

    public static async Task<IResult<T, Exception>> CatchExceptionAsync<T>(this Func<Task<T>> tryExpr)
    {
        try
        {
            return new Ok<T, Exception>(await tryExpr());
        }
        catch (Exception e)
        {
            return new Err<T, Exception>(e);
        }
    }

    public static IResult<TOut, E> RemapOk<T, E, TOut>(this IResult<T, E> result, Func<T, TOut> func) =>
        result.Split(out var ok, out var err)
            ? new Ok<TOut, E>(func(ok))
            : new Err<TOut, E>(err);

    public static IResult<T, EOut> RemapErr<T, E, EOut>(this IResult<T, E> result, Func<E, EOut> func) =>
        result.Split(out var ok, out var err)
            ? new Ok<T, EOut>(ok)
            : new Err<T, EOut>(func(err));

    public static IResult<E, T> Invert<T, E>(this IResult<T, E> result) =>
        result.Split(out var ok, out var err)
            ? new Err<E, T>(ok)
            : new Ok<E, T>(err);

    public static IResult<T, E> ToOk<T, E>(this IResult<T, E> _, T value) => new Ok<T, E>(value);
    public static IResult<T, E> ToErr<T, E>(this IResult<T, E> _, E value) => new Err<T, E>(value);

    public static IOption<T> TakeOk<T, E>(this IResult<T, E> result) =>
        result.Split(out var ok, out _)
            ? new Some<T>(ok)
            : new None<T>();

    public static IOption<E> TakeErr<T, E>(this IResult<T, E> result) =>
        result.Split(out _, out var err)
            ? new Some<E>(err)
            : new None<E>();

    public static IResult<T, E> ToResult<T, E>(this bool condition, T ok, E err) => condition ? new Ok<T, E>(ok) : new Err<T, E>(err);
    public static IResult<T, E> ToResultLazy<T, E>(this bool condition, Func<T> ok, Func<E> err) => condition ? new Ok<T, E>(ok()) : new Err<T, E>(err());
    public static IOption<T> KeepOk<T, E>(this IResult<T, E> result) => result.CheckOk(out var v).ToOption(v);
    public static IOption<E> KeepErr<T, E>(this IResult<T, E> result) => result.CheckErr(out var v).ToOption(v);
    public static IResult<T, E> Ok<T, E>(this IResult<T, E>? _, T ok) => new Ok<T, E>(ok);
    public static IResult<T, E> Err<T, E>(this IResult<T, E>? _, E err) => new Err<T, E>(err);
    public static bool IsOk<T, E>(this IResult<T, E> result) => result is IOk<T, E>;
    public static bool IsErr<T, E>(this IResult<T, E> result) => result is IErr<T, E>;
}