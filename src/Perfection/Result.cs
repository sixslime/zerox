#nullable enable
namespace Perfection
{
    /// <summary>
    /// Representation of no value or data.
    /// </summary>
    public class NoVal { }
    public interface IResult<out T, out E> { }
    public interface ICanErr<out E> : IResult<NoVal, E> { }
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
        public T Value { get; } = value;
        public override string ToString()
        {
            return $"Ok({Value})";
        }
    }
    public record Err<T, E>(E value) : IErr<T, E>
    {
        public E Value { get; } = value;
        public override string ToString()
        {
            return $"Err({Value})";
        }
    }
    public class ExpectedValueException(string message) : Exception(message) { }
    public class Hint<T>
    {
        public static readonly Hint<T> HINT = new();
        private Hint() { }
    }
    public static class Result
    {
        
        public static T ExpectOk<T, E>(this IResult<T, E> result, string message)
        {
            return result is IOk<T, E> ok ? ok.Value : throw new ExpectedValueException(message);
        }
        public static E ExpectErr<T, E>(this IResult<T, E> result, string message)
        {
            return result is IErr<T, E> ok ? ok.Value : throw new ExpectedValueException(message);
        }
        public static T UnwrapOk<T, E>(this IResult<T, E> result)
            => ExpectOk(result, "'Err' value unwrapped, expected 'Ok'.");
        public static E UnwrapErr<T, E>(this IResult<T, E> result)
            => ExpectErr(result, "'Ok' value unwrapped, expected 'Err'.");

        public static bool Split<T, E>(this IResult<T, E> result, out T ok, out E err)
        {
            ok = default!;
            err = default!;
            if (result is IOk<T, E> v) { ok = v.Value; return true; }
            if (result is IErr<T, E> e) { err = e.Value; return false; }
            throw new Exception("Unsupported IResult type?");
        }
        public static bool CheckOk<T, E>(this IResult<T, E> result, out T ok)
            => Split(result, out ok, out var _);
        public static bool CheckErr<T, E>(this IResult<T, E> result, out E err)
            => !Split(result, out var _, out err);
        public static IResult<T, E> AsOk<T, E>(this T value, Hint<E> _) => new Ok<T, E>(value);
        public static IResult<T, E> AsErr<T, E>(this E value, Hint<T> _) => new Err<T, E>(value);
        public static IResult<T, Exception> CatchException<T>(this Func<T> tryExpr)
        {
            try { return new Ok<T, Exception>(tryExpr()); }
            catch (Exception e) { return new Err<T, Exception>(e); }
        }
        public static async Task<IResult<T, Exception>> CatchExceptionAsync<T>(this Func<Task<T>> tryExpr)
        {
            try { return new Ok<T, Exception>(await tryExpr()); }
            catch (Exception e) { return new Err<T, Exception>(e); }
        }
        public static IResult<TOut, E> RemapOk<T, E, TOut>(this IResult<T, E> result, Func<T, TOut> func)
        {
            return result.Split(out var ok, out var err)
                ? new Ok<TOut, E>(func(ok))
                : new Err<TOut, E>(err);
        }
        public static IResult<T, EOut> RemapErr<T, E, EOut>(this IResult<T, E> result, Func<E, EOut> func)
        {
            return result.Split(out var ok, out var err)
                ? new Ok<T, EOut>(ok)
                : new Err<T, EOut>(func(err));
        }
        public static IResult<E, T> Invert<T, E>(this IResult<T, E> result)
        {
            return result.Split(out var ok, out var err)
                ? new Err<E, T>(ok)
                : new Ok<E, T>(err);
        }
        public static IResult<T, E> ToOk<T, E>(this IResult<T, E> _, T value)
        {
            return new Ok<T, E>(value);
        }
        public static IResult<T, E> ToErr<T, E>(this IResult<T, E> _, E value)
        {
            return new Err<T, E>(value);
        }
        public static IOption<T> TakeOk<T, E>(this IResult<T, E> result)
        {
            return result.Split(out var ok, out var _)
                ? new Some<T>(ok)
                : new None<T>();
        }
        public static IOption<E> TakeErr<T, E>(this IResult<T, E> result)
        {
            return result.Split(out var _, out var err)
                ? new Some<E>(err)
                : new None<E>();
        }
        public static IResult<T, E> ToResult<T, E>(this bool condition, T ok, E err)
        {
            return condition ? new Ok<T, E>(ok) : new Err<T, E>(err);
        }
        public static IResult<T, E> ToResultLazy<T, E>(this bool condition, Func<T> ok, Func<E> err)
        {
            return condition ? new Ok<T, E>(ok()) : new Err<T, E>(err());
        }
        public static IOption<T> KeepOk<T, E>(this IResult<T, E> result)
        {
            return result.CheckOk(out var v).ToOption(v);
        }
        public static IOption<E> KeepErr<T, E>(this IResult<T, E> result)
        {
            return result.CheckErr(out var v).ToOption(v);
        }
        public static IResult<T, E> Ok<T, E>(this IResult<T, E>? _, T ok) => new Ok<T, E>(ok);
        public static IResult<T, E> Err<T, E>(this IResult<T, E>? _, E err) => new Err<T, E>(err);
        public static bool IsOk<T, E>(this IResult<T, E> result) => result is IOk<T, E>;
        public static bool IsErr<T, E>(this IResult<T, E> result) => result is IErr<T, E>;
    }
}
