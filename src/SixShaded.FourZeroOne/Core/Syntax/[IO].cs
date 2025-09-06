namespace SixShaded.FourZeroOne.Core.Syntax;

using Korssas.IO;
using Roggis;

public static partial class KorssaSyntax
{
    public static SelectOne<R> kIOSelectOne<R>(this IKorssa<IMulti<R>> source)
        where R : class, Rog =>
        new(source);

    public static SelectMultiple<R> kIOSelectMultiple<R>(this IKorssa<IMulti<R>> source, IKorssa<NumRange> count)
        where R : class, Rog =>
        new(source, count);

    public static SelectMultiple<R> kIOSelectMultiple<R>(this IKorssa<IMulti<R>> source, IKorssa<Number> count)
        where R : class, Rog =>
        new(source, count.kSingleRange());
}