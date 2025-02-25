namespace SixShaded.FourZeroOne.Core.Syntax;

using Resolutions;
using SixShaded.FourZeroOne.Core.Tokens.Memory;
using SixShaded.FourZeroOne.Core.Tokens.Memory.Object;

public static partial class TokenSyntax
{
    public static DynamicAssign<R> tAsVariable<R>(this IToken<R> token, out DynamicAddress<R> ident) where R : class, Res
    {
        ident = new();
        return new(ident, token);
    }

    public static DynamicReference<R> tRef<R>(this IMemoryAddress<R> ident) where R : class, Res => new(ident);

    public static Insert<R> tMemoryWrite<R>(this IToken<IMemoryObject<R>> address, IToken<R> data)
        where R : class, Res =>
        new(address, data);

    public static Get<R> tMemoryGet<R>(this IToken<IMemoryObject<R>> address)
        where R : class, Res =>
        new(address);

    public static Remove tMemoryRemove(this IToken<IMemoryObject<Res>> address) => new(address);

    public static Macro<IMemoryObject<R>, MetaFunction<R, R>, Resolutions.Instructions.Assign<R>> tMemoryUpdate<R>(this IToken<IMemoryObject<R>> address, IToken<MetaFunction<R, R>> updateFunction)
        where R : class, Res =>
        Macros.UpdateMemoryObject<R>.Construct(address, updateFunction);

    public static Macro<IMemoryObject<R>, MetaFunction<R, R>, Resolutions.Instructions.Assign<R>> tMemoryUpdate<R>(this IToken<IMemoryObject<R>> address, Func<DynamicAddress<R>, IToken<R>> updateFunction)
        where R : class, Res =>
        Macros.UpdateMemoryObject<R>.Construct(address, Core.tMetaFunction(updateFunction));
}