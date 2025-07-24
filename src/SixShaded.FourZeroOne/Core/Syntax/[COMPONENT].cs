namespace SixShaded.FourZeroOne.Core.Syntax;

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
    public static Korssas.Component.Get<C, R> kGetRovi<C, R>(this IKorssa<IRoveggi<C>> holder, IGetRovu<C, R> rovu)
        where C : IRovetu
        where R : class, Rog =>
        new(holder)
        {
            Rovu = rovu,
        };

    public static Korssas.Component.With<C, R> kWithRovi<C, R>(this IKorssa<IRoveggi<C>> holder, ISetRovu<C, R> rovu, IKorssa<R> component)
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

    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> kUpdateRovi<C, R>(this IKorssa<IRoveggi<C>> holder, IRovu<C, R> rovu, Func<DynamicRoda<R>, IKorssa<R>> changeFunc)
        where C : IRovetu
        where R : class, Rog =>
        Korvessas.UpdateComponent<C, R>.Construct(holder, Core.kMetaFunction([], changeFunc), rovu);

    public static Korssas.Component.Without<H> kWithoutRovi<H>(this IKorssa<IRoveggi<H>> holder, IRovu<H> rovu)
        where H : IRovetu =>
        new(holder)
        {
            Rovu = rovu,
        };


    public static Korssas.Component.Attachment.With<C, RKey, RVal> kWithVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key, IKorssa<RVal> value)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject, key, value)
        {
            Varovu = varovu
        };
    public static Korssas.Component.Attachment.Without<C, RKey, RVal> kWithoutVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject, key)
        {
            Varovu = varovu
        };
    public static Korssas.Component.Attachment.Get<C, RKey, RVal> kGetVarovi<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu, IKorssa<RKey> key)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject, key)
        {
            Varovu = varovu
        };
    public static Korssas.Component.Attachment.GetKeys<C, RKey, RVal> kGetVarovaKeys<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject)
        {
            Varovu = varovu
        };
    public static Korssas.Component.Attachment.GetValues<C, RKey, RVal> kGetVarovaValues<C, RKey, RVal>(this IKorssa<IRoveggi<C>> subject, IVarovu<C, RKey, RVal> varovu)
        where C : IRovetu
        where RKey : class, Rog
        where RVal : class, Rog =>
        new(subject)
        {
            Varovu = varovu
        };
}