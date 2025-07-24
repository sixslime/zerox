namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korssas.Memory;
using Korssas.Memory.Rovedanggi;
using Korvessa.Defined;
using Roveggi;

public static partial class KorssaSyntax
{
    public static Assign<R> kAsVariable<R>(this IKorssa<R> korssa, out DynamicRoda<R> ident)
        where R : class, Rog
    {
        ident = new();
        return new(ident, korssa);
    }

    public static Reference<R> kRef<R>(this IRoda<R> ident)
        where R : class, Rog =>
        new(ident);

    public static Insert<R> kWrite<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address, IKorssa<R> data)
        where R : class, Rog =>
        new(address, data);
    public static Insert<R> kRedact<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address)
        where R : class, Rog =>
        new(address, Core.kNollaFor<R>());

    public static Get<R> kGet<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address)
        where R : class, Rog =>
        new(address);

    public static Korvessa<IRoveggi<Rovedantu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> kUpdate<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address, IKorssa<MetaFunction<R, R>> updateFunction)
        where R : class, Rog =>
        Korvessas.UpdateRovedanggi<R>.Construct(address, updateFunction);

    public static Korvessa<IRoveggi<Rovedantu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> kUpdate<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address, Func<DynamicRoda<R>, IKorssa<R>> updateFunction)
        where R : class, Rog =>
        Korvessas.UpdateRovedanggi<R>.Construct(address, Core.kMetaFunction([], updateFunction));
}

partial class Core
{
    public static AllKeys<C, R> kAllRovedanggiKeys<C, R>()
        where C : Rovedantu<R>
        where R : class, Rog =>
        new();
    public static AllValues<C, R> kAllRovedanggiValues<C, R>()
        where C : Rovedantu<R>
        where R : class, Rog =>
        new();
}