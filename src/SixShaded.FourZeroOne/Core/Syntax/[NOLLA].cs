namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class Core
{
    public static Korssas.Nolla<R> tNollaFor<R>() where R : class, Rog => new();
}

public static partial class KorssaSyntax
{
    public static Korvessa<R, MetaFunction<R>, R> tCatchNolla<R>(this IKorssa<R> value, IKorssa<MetaFunction<R>> fallback)
        where R : class, Rog =>
        Korvessas.CatchNolla<R>.Construct(value, fallback);

    public static Korvessa<R, MetaFunction<R>, R> tCatchNolla<R>(this IKorssa<R> value, Func<IKorssa<R>> fallback)
        where R : class, Rog =>
        Korvessas.CatchNolla<R>.Construct(value, fallback().tMetaBoxed());

    public static Korssas.Exists tExists(this IKorssa<Rog> korssa) => new(korssa);
}