﻿namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Rovetus;
using Korvessa.Defined;
using Roveggi.Unsafe;
using Roveggi;

public static partial class Core
{
    public static Korvessa<IRoveggi<C>> kCompose<C>()
        where C : IRovetu =>
        Korvessas.Compose<C>.Construct();
}

public static partial class KorssaSyntax
{
    public static Korssas.Component.Get<C, R> kGetRovi<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu)
        where C : IRovetu
        where R : class, Rog =>
        new(holder)
        {
            Rovu = rovu,
        };

    public static Korssas.Component.With<C, R> kWithRovi<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu, IKorssa<R> component)
        where C : IRovetu
        where R : class, Rog =>
        new(holder, component)
        {
            Rovu = rovu,
        };

    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> kUpdateRovi<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu, IKorssa<MetaFunction<R, R>> changeFunc)
        where C : IRovetu
        where R : class, Rog =>
        Korvessas.UpdateComponent<C, R>.Construct(holder, changeFunc, rovu);

    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> kUpdateRovi<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu, IEnumerable<Addr> captures, Func<DynamicRoda<R>, IKorssa<R>> changeFunc)
        where C : IRovetu
        where R : class, Rog =>
        Korvessas.UpdateComponent<C, R>.Construct(holder, Core.kMetaFunction(captures, changeFunc), rovu);

    public static Korssas.Component.Without<H> kWithoutRovi<H>(this IKorssa<IRoveggi<H>> holder, IRovu<H> rovu)
        where H : IRovetu =>
        new(holder)
        {
            Rovu = rovu,
        };

    public static Korssas.Component.With<MergeSpec<C>, R> kMergeRovi<C, R>(this IKorssa<IRoveggi<MergeSpec<C>>> mergeObject, IRovu<C, R> mergingIdentifier, IKorssa<R> component)
        where C : IRovetu
        where R : class, Rog =>
        mergeObject.kWithRovi(MergeSpec<C>.MERGE(mergingIdentifier), component);

    public static Korssas.Component.DoMerge<C> kMerge<C>(this IKorssa<IRoveggi<C>> subject, IKorssa<IRoveggi<MergeSpec<C>>> mergeObject)
        where C : IRovetu =>
        new(subject, mergeObject);

    public static Korssas.Component.Attachment.Get<C, RKey, RVal> kGetVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key)
        where C : IVarovetu<RKey, RVal>
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject, key)
        {
            Varovu = varovu
        };
    public static Korssas.Component.Attachment.With<C, RKey, RVal> kWithVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key, IKorssa<RVal> value)
        where C : IVarovetu<RKey, RVal>
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject, key, value)
        {
            Varovu = varovu
        };
    public static Korssas.Component.Attachment.Without<C, RKey, RVal> kWithoutVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key)
        where C : IVarovetu<RKey, RVal>
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject, key)
        {
            Varovu = varovu
        };
}