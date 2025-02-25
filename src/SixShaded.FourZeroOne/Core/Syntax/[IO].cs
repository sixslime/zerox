namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;

public static partial class KorssaSyntax
{
    public static Korssas.IO.Select.One<R> tIOSelectOne<R>(this IKorssa<IMulti<R>> source) where R : class, Rog => new(source);

    public static Korssas.IO.Select.Multiple<R> tIOSelectMultiple<R>(this IKorssa<IMulti<R>> source, IKorssa<Number> count) where R : class, Rog => new(source, count);
}