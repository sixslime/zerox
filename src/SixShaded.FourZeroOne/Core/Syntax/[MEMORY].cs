namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korssas.Memory;
using Korssas.Memory.Rovedanggi;
using Korvessa.Defined;
using Korvessas.Update;
using Roveggi;
public static partial class KorssaSyntax
{
    public static Assign<R> kAsVariable<R>(this IKorssa<R> korssa, out DynamicRoda<R> ident)
        where R : class, Rog
    {
        ident = new();
        return new(korssa)
        {
            Roda = ident
        };
    }

    public static Reference<R> kRef<R>(this IRoda<R> ident)
        where R : class, Rog =>
        new()
        {
            Roda = ident
        };

    public static Write<R> kWrite<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address, IKorssa<R> data)
        where R : class, Rog =>
        new(address, data);
    public static Write<R> kRedact<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address)
        where R : class, Rog =>
        new(address, Core.kNollaFor<R>());

    public static Read<R> kRead<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address)
        where R : class, Rog =>
        new(address);

    public static Korssas.Memory.ProgramState.Load kLoad(this IKorssa<ProgramState> state) => new(state);
    public static UpdateRovedanggi<R> kUpdate<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address, IKorssa<MetaFunction<R, R>> updateFunction)
        where R : class, Rog =>
        new(address, updateFunction);

    public static UpdateRovedanggi<R> kUpdate<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address, MetaDefinition<R, R> updateFunction)
        where R : class, Rog =>
        new(address, Core.kMetaFunction([], updateFunction));
    public static SafeUpdateRovedanggi<R> kSafeUpdate<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address, IKorssa<MetaFunction<R, R>> updateFunction)
        where R : class, Rog =>
        new(address, updateFunction);

    public static SafeUpdateRovedanggi<R> kSafeUpdate<R>(this IKorssa<IRoveggi<Rovedantu<R>>> address, MetaDefinition<R, R> updateFunction)
        where R : class, Rog =>
        new(address, Core.kMetaFunction([], updateFunction));

}

partial class Core
{
    public static Korssas.Memory.ProgramState.Get kGetProgramState() => new();
    public static AllKeys<C, R> kAllRovedanggiKeys<C, R>()
        where C : Rovedantu<R>
        where R : class, Rog =>
        new();
    public static AllKeys<C, R> kAllRovedanggiKeys<C, R>(Structure.Hint<C> _)
        where C : Rovedantu<R>
        where R : class, Rog =>
        new();
    public static AllValues<C, R> kAllRovedanggiValues<C, R>()
        where C : Rovedantu<R>
        where R : class, Rog =>
        new();
    public static AllValues<C, R> kAllRovedanggiValues<C, R>(Structure.Hint<C> _)
        where C : Rovedantu<R>
        where R : class, Rog =>
        new();
}