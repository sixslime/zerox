namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;

public static class SelectMultipleCancellable<RIn, ROut>
where RIn : class, Rog
where ROut : class, Rog
{
    public static Korvessa<IMulti<RIn>, MetaFunction<IMulti<RIn>, ROut>, MetaFunction<ROut>, MetaFunction<Number, ROut>> Construct(IKorssa<Multi<RIn>> pool, IKorssa<Number> count, IKorssa<MetaFunction<IMulti<RIn>, ROut>> selectPath, IKorssa<MetaFunction<ROut>> cancelPath) =>
        new(pool, selectPath, cancelPath)
        {
            Du = Axoi.Korvedu("SelectMultipleCancellable"),
            Definition =
                (_, iPool, iSelectPath, iCancelPath) =>
                    Core.kNollaFor<MetaFunction<Number, ROut>>()

        };
}