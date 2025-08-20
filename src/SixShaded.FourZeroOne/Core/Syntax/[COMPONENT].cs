namespace SixShaded.FourZeroOne.Core.Syntax;

using Korssas.Rovi;
using Korssas.Rovi.Varovi;
using Roggis;
using Korvessa.Defined;
using Roveggi.Unsafe;
using Roveggi;

public static partial class Core
{
    public static Korvessa<IRoveggi<C>> kCompose<C>()
        where C : IConcreteRovetu =>
        Korvessas.Compose<C>.Construct();
}

public static partial class KorssaSyntax
{
    public static Get<C, R> kGetRovi<C, R>(this IKorssa<IRoveggi<C>> holder, IGetRovu<C, R> rovu)
        where C : IRovetu
        where R : class, Rog =>
        new(holder)
        {
            Rovu = rovu,
        };

    public static With<C, R> kWithRovi<C, R>(this IKorssa<IRoveggi<C>> holder, ISetRovu<C, R> rovu, IKorssa<R> data)
        where C : IRovetu
        where R : class, Rog =>
        new(holder, data)
        {
            Rovu = rovu,
        };

    public static With<C, R> kWithRovi<C, R>(this IKorssa<IRoveggi<C>> holder, Structure.Hint<C> hint, ISetRovu<C, R> rovu, IKorssa<R> data)
        where C : IRovetu
        where R : class, Rog =>
        new(holder, data)
        {
            Rovu = rovu,
        };

    public static With<C, R> kWithoutRovi<C, R>(this IKorssa<IRoveggi<C>> holder, ISetRovu<C, R> rovu)
        where C : IRovetu
        where R : class, Rog =>
        new(holder, Core.kNollaFor<R>())
        {
            Rovu = rovu,
        };

    public static With<C, R> kWithoutRovi<C, R>(this IKorssa<IRoveggi<C>> holder, Structure.Hint<C> hint, ISetRovu<C, R> rovu)
        where C : IRovetu
        where R : class, Rog =>
        new(holder, Core.kNollaFor<R>())
        {
            Rovu = rovu,
        };

    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> kUpdateRovi<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu, IKorssa<MetaFunction<R, R>> changeFunc)
        where C : IRovetu
        where R : class, Rog =>
        Korvessas.UpdateRovi<C, R>.Construct(holder, changeFunc, rovu);

    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> kUpdateRovi<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu, MetaDefinition<R, R> changeFunc)
        where C : IRovetu
        where R : class, Rog =>
        Korvessas.UpdateRovi<C, R>.Construct(holder, Core.kMetaFunction([], changeFunc), rovu);

    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> kSafeUpdateRovi<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu, IKorssa<MetaFunction<R, R>> changeFunc)
        where C : IRovetu
        where R : class, Rog =>
        Korvessas.SafeUpdateRovi<C, R>.Construct(holder, changeFunc, rovu);

    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> kSafeUpdateRovi<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu, MetaDefinition<R, R> changeFunc)
        where C : IRovetu
        where R : class, Rog =>
        Korvessas.SafeUpdateRovi<C, R>.Construct(holder, Core.kMetaFunction([], changeFunc), rovu);

    public static With<C, RKey, RVal> kWithVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key, IKorssa<RVal> value)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject, key, value)
        {
            Varovu = varovu,
        };

    public static With<C, RKey, RVal> kWithoutVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject, key, Core.kNollaFor<RVal>())
        {
            Varovu = varovu,
        };

    public static Korvessa<IRoveggi<C>, RKey, MetaFunction<RVal, RVal>, IRoveggi<C>> kUpdateVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> holder, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key, IKorssa<MetaFunction<RVal, RVal>> changeFunc)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        Korvessas.UpdateVarovi<C, RKey, RVal>.Construct(holder, key, changeFunc, varovu);

    public static Korvessa<IRoveggi<C>, RKey, MetaFunction<RVal, RVal>, IRoveggi<C>> kSafeUpdateVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> holder, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key, MetaDefinition<RVal, RVal> changeFunc)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        Korvessas.SafeUpdateVarovi<C, RKey, RVal>.Construct(holder, key, Core.kMetaFunction([], changeFunc), varovu);

    public static Korvessa<IRoveggi<C>, RKey, MetaFunction<RVal, RVal>, IRoveggi<C>> kSafeUpdateVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> holder, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key, IKorssa<MetaFunction<RVal, RVal>> changeFunc)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        Korvessas.SafeUpdateVarovi<C, RKey, RVal>.Construct(holder, key, changeFunc, varovu);

    public static Get<C, RKey, RVal> kGetVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject, key)
        {
            Varovu = varovu,
        };

    public static GetKeys<C, RKey, RVal> kGetVarovaKeys<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject)
        {
            Varovu = varovu,
        };

    public static GetValues<C, RKey, RVal> kGetVarovaValues<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject)
        {
            Varovu = varovu,
        };
}