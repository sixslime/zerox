namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korssas.Memory;
using SixShaded.FourZeroOne.Core.Korssas.Memory.Object;
using Korvessa.Defined;

public static partial class KorssaSyntax
{
    public static DynamicAssign<R> tAsVariable<R>(this IKorssa<R> korssa, out DynamicAddress<R> ident) where R : class, Rog
    {
        ident = new();
        return new(ident, korssa);
    }

    public static DynamicReference<R> tRef<R>(this IMemoryAddress<R> ident) where R : class, Rog => new(ident);

    public static Insert<R> tWrite<R>(this IKorssa<IMemoryObject<R>> address, IKorssa<R> data)
        where R : class, Rog =>
        new(address, data);

    public static Get<R> tGet<R>(this IKorssa<IMemoryObject<R>> address)
        where R : class, Rog =>
        new(address);

    public static Remove tRedact(this IKorssa<IMemoryObject<Rog>> address) => new(address);

    public static Korvessa<IMemoryObject<R>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> tUpdate<R>(this IKorssa<IMemoryObject<R>> address, IKorssa<MetaFunction<R, R>> updateFunction)
        where R : class, Rog =>
        Korvessas.UpdateMemoryObject<R>.Construct(address, updateFunction);

    public static Korvessa<IMemoryObject<R>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> tUpdate<R>(this IKorssa<IMemoryObject<R>> address, Func<DynamicAddress<R>, IKorssa<R>> updateFunction)
        where R : class, Rog =>
        Korvessas.UpdateMemoryObject<R>.Construct(address, Core.tMetaFunction(updateFunction));
}