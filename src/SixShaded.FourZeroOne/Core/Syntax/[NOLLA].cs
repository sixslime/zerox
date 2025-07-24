namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class Core
{
    public static Korssas.Nolla<R> kNollaFor<R>()
        where R : class, Rog =>
        new();
}

public static partial class KorssaSyntax
{
    public static Korvessa<R, MetaFunction<R>, R> kCatchNolla<R>(this IKorssa<R> value, IKorssa<MetaFunction<R>> fallback)
        where R : class, Rog =>
        Korvessas.CatchNolla<R>.Construct(value, fallback);

    public static Korvessa<R, MetaFunction<R>, R> kCatchNolla<R>(this IKorssa<R> value, Func<IKorssa<R>> fallback)
        where R : class, Rog =>
        Korvessas.CatchNolla<R>.Construct(value, fallback().kMetaBoxed([]));

    public static Korssas.Exists kExists(this Kor korssa) => new(korssa);

    public static IKorssa<R> ksRemapNonNolla<R>(this Kor potentialNolla, IKorssa<R> value)
        where R : class, Rog =>
        potentialNolla.kExists()
            .kIfTrue<R>(
            new()
            {
                Then = value,
                Else = Core.kNollaFor<R>()
            });
}