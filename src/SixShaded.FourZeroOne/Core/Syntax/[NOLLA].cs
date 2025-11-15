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
    public static Korvessas.CatchNolla<R> kCatchNolla<R>(this IKorssa<R> value, IKorssa<MetaFunction<R>> fallback)
        where R : class, Rog =>
        new(value, fallback);

    public static Korvessas.CatchNolla<R> kCatchNolla<R>(this IKorssa<R> value, MetaDefinition<R> fallback)
        where R : class, Rog =>
        new(value, fallback().kMetaBoxed([]));

    public static Korssas.Exists kExists(this Kor korssa) => new(korssa);

    public static Korvessas.KeepNolla<R> kKeepNolla<R>(this Kor potentialNolla, MetaDefinition<R> value)
        where R : class, Rog =>
        new(potentialNolla, Core.kMetaFunction([], value));

    public static Korvessas.KeepNolla<R> kKeepNolla<R>(this Kor potentialNolla, IKorssa<MetaFunction<R>> value)
        where R : class, Rog =>
        new(potentialNolla, value);
}