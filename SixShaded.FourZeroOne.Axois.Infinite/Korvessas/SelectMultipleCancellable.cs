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
    public static Korvessa<IMulti<RIn>, MetaFunction<IMulti<RIn>, ROut>, MetaFunction<ROut>, MetaFunction<NumRange, ROut>> Construct(IKorssa<IMulti<RIn>> pool, IKorssa<MetaFunction<IMulti<RIn>, ROut>> selectPath, IKorssa<MetaFunction<ROut>> cancelPath) =>
        new(pool, selectPath, cancelPath)
        {
            Du = Axoi.Korvedu("SelectMultipleCancellable"),
            Definition =
                (_, iPool, iSelectPath, iCancelPath) =>
                    Core.kSubEnvironment<MetaFunction<NumRange, ROut>>(
                    new()
                    {
                        Environment =
                        [
                            Core.kMulti<IMulti<Rog>>(
                                [
                                    iPool.kRef(),
                                    Core.kCompose<u.uCancelMarker>().kYield(),
                                ])
                                .kIOSelectOne()
                                .kAsVariable(out var iInitialSelection),
                        ],
                        Value =
                            iInitialSelection.kRef()
                                .kIsType<IRoveggi<u.uCancelMarker>>()
                                .kExists()
                                .kIfTrue<MetaFunction<NumRange, ROut>>(
                                new()
                                {
                                    Then = Core.kMetaFunction<NumRange, ROut>([iCancelPath], _ => iCancelPath.kRef().kExecute()),
                                    Else =
                                        Core.kMetaFunction<NumRange, ROut>(
                                        [
                                            iSelectPath,
                                            iPool,
                                        ],
                                        iSelectCount =>
                                            iSelectPath.kRef()
                                                .kExecuteWith(
                                                new()
                                                {
                                                    A =
                                                        iPool.kRef()
                                                            .kIOSelectMultiple(iSelectCount.kRef()),
                                                })),
                                }),
                    }),
        };
}