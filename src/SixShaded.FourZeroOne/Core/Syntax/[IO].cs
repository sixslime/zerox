namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;

public static partial class KorssaSyntax
{
    public static Korssas.IO.Select.One<R> kIOSelectOne<R>(this IKorssa<IMulti<R>> source)
        where R : class, Rog =>
        new(source);

    public static Korssas.IO.Select.Multiple<R> kIOSelectMultiple<R>(this IKorssa<IMulti<R>> source, IKorssa<Number> count)
        where R : class, Rog =>
        new(source, count);
}