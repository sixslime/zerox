namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korssas.Memory;
using SixShaded.FourZeroOne.Core.Korssas.Memory.Object;
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

    public static Insert<R> kWrite<R>(this IKorssa<IRoveggi<IRovedantu<R>>> address, IKorssa<R> data)
        where R : class, Rog =>
        new(address, data);

    public static Get<R> kGet<R>(this IKorssa<IRoveggi<IRovedantu<R>>> address)
        where R : class, Rog =>
        new(address);

    public static Remove kRedact(this IKorssa<IRoveggi<IRovedantu<Rog>>> address) => new(address);

    public static Korvessa<IRoveggi<IRovedantu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> kUpdate<R>(this IKorssa<IRoveggi<IRovedantu<R>>> address, IKorssa<MetaFunction<R, R>> updateFunction)
        where R : class, Rog =>
        Korvessas.UpdateMemory<R>.Construct(address, updateFunction);

    public static Korvessa<IRoveggi<IRovedantu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> kUpdate<R>(this IKorssa<IRoveggi<IRovedantu<R>>> address, IEnumerable<IRoda<>> captures, Func<DynamicRoda<R>, IKorssa<R>> updateFunction)
        where R : class, Rog =>
        Korvessas.UpdateMemory<R>.Construct(address, Core.kMetaFunction(captures, updateFunction));
}