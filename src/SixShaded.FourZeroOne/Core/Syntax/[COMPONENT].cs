namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Roveggitus;
using Korvessa.Defined;

public static partial class Core
{
    public static Korvessa<IRoveggi<C>> tCompose<C>() where C : IRoveggitu => Korvessas.Compose<C>.Construct();
}

public static partial class KorssaSyntax
{
    public static Korssas.Component.Get<C, R> tGetComponent<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu)
        where C : IRoveggitu
        where R : class, Rog =>
        new(holder) { Rovu = rovu };

    public static Korssas.Component.With<C, R> tWithComponent<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu, IKorssa<R> component)
        where C : IRoveggitu
        where R : class, Rog =>
        new(holder, component) { Rovu = rovu };

    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> tUpdateComponent<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu, IKorssa<MetaFunction<R, R>> changeFunc)
        where C : IRoveggitu
        where R : class, Rog =>
        Korvessas.UpdateComponent<C, R>.Construct(holder, changeFunc, rovu);

    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> tUpdateComponent<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu, Func<DynamicAddress<R>, IKorssa<R>> changeFunc)
        where C : IRoveggitu
        where R : class, Rog =>
        Korvessas.UpdateComponent<C, R>.Construct(holder, Core.tMetaFunction(changeFunc), rovu);

    public static Korssas.Component.Without<H> tWithoutComponent<H>(this IKorssa<IRoveggi<H>> holder, Roggi.Unsafe.IRovu<H> rovu)
        where H : IRoveggitu =>
        new(holder) { Rovu = rovu };

    public static Korssas.Component.With<MergeSpec<C>, R> tWithMerged<C, R>(this IKorssa<IRoveggi<MergeSpec<C>>> mergeObject, IRovu<C, R> mergingIdentifier, IKorssa<R> component)
        where C : IRoveggitu
        where R : class, Rog =>
        mergeObject.tWithComponent(MergeSpec<C>.MERGE(mergingIdentifier), component);

    public static Korssas.Component.DoMerge<C> tMerge<C>(this IKorssa<IRoveggi<C>> subject, IKorssa<IRoveggi<MergeSpec<C>>> mergeObject)
        where C : IRoveggitu =>
        new(subject, mergeObject);
}