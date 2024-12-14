using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MorseCode.ITask;
#nullable enable
namespace Perfection
{
    public interface IResult<out T, out E> { }
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
        public T Value => value;
    }
    public record Err<T, E>(E value) : IErr<T, E>
    {
        public E Value => value;
    }
    public struct Hint<T> { }
    public static class Result
    {
        public static T UnwrapOk<T, E>(this IResult<T, E> result)
        {
            return result is IOk<T, E> ok ? ok.Value : throw new Exception("IResult UnwrapOk() did not get Ok.");
        }
        public static E UnwrapErr<T, E>(this IResult<T, E> result)
        {
            return result is IErr<T, E> ok ? ok.Value : throw new Exception("IResult UnwrapErr() did not get Err.");
        }
        public static bool Break<T, E>(this IResult<T, E> result, out T ok, out E err)
        {
            ok = default!;
            err = default!;
            if (result is IOk<T, E> v) { ok = v.Value; return true; }
            if (result is IErr<T, E> e) { err = e.Value; return false; }
            throw new Exception("Unsupported IResult type?");
        }
        public static IResult<T, E> AsOk<T, E>(this T value, Hint<E> _) => new Ok<T, E>(value);
        public static IResult<T, E> AsErr<T, E>(this E value, Hint<T> _) => new Err<T, E>(value);
        public static IResult<T, Exception> CatchException<T>(this Func<T> tryExpr)
        {
            try { return new Ok<T, Exception>(tryExpr()); }
            catch (Exception e) { return new Err<T, Exception>(e); }
        }
        public static async Task<IResult<T, Exception>> CatchExceptionAsync<T>(this Func<ITask<T>> tryExpr)
        {
            try { return new Ok<T, Exception>(await tryExpr()); }
            catch (Exception e) { return new Err<T, Exception>(e); }
        }
        public static IResult<TOut, E> RemapOk<T, E, TOut>(this IResult<T, E> result, Func<T, TOut> func)
        {
            return result.Break(out var ok, out var err)
                ? new Ok<TOut, E>(func(ok))
                : new Err<TOut, E>(err);
        }
        public static IResult<T, EOut> RemapErr<T, E, EOut>(this IResult<T, E> result, Func<E, EOut> func)
        {
            return result.Break(out var ok, out var err)
                ? new Ok<T, EOut>(ok)
                : new Err<T, EOut>(func(err));
        }
        public static IResult<E, T> Invert<T, E>(this IResult<T, E> result)
        {
            return result.Break(out var ok, out var err)
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
            return result.Break(out var ok, out var _)
                ? new Some<T>(ok)
                : new None<T>();
        }
        public static IOption<E> TakeErr<T, E>(this IResult<T, E> result)
        {
            return result.Break(out var _, out var err)
                ? new Some<E>(err)
                : new None<E>();
        }

        public static bool IsOk<T, E>(this IResult<T, E> result) => result is IOk<T, E>;
    }
}
