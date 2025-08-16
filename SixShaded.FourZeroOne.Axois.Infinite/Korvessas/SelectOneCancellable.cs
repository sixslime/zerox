namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;

public static class SelectOneCancellable<RIn, ROut>
where RIn : class, Rog
where ROut : class, Rog
{
    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, MetaFunction<ROut>, ROut> Construct(IKorssa<Multi<RIn>> pool, IKorssa<MetaFunction<RIn, ROut>> selectPath, IKorssa<MetaFunction<ROut>> cancelPath) =>
        new(pool, selectPath, cancelPath)
        {
            Du = Axoi.Korvedu("SelectOneCancellable"),
            Definition =
                (_, iPool, iSelectPath, iCancelPath) =>
                    Core.kSubEnvironment<ROut>(
                    new()
                    {
                        Environment =
                        [
                            Core.kMulti<IMulti<Rog>>(
                                [
                                    iPool.kRef(),
                                    Core.kCompose<u.uCancelMarker>().kYield()
                                ])
                                .kFlatten()
                                .kIOSelectOne()
                                .kIsType<RIn>()
                                .kAsVariable(out var iSelection)
                        ],
                        Value =
                            iSelection.kRef()
                                .kExists()
                                .kIfTrue<ROut>(
                                new()
                                {
                                    Then =
                                        iSelectPath.kRef()
                                            .kExecuteWith(
                                            new()
                                            {
                                                A = iSelection.kRef()
                                            }),
                                    Else = iCancelPath.kRef().kExecute()
                                })
                    })

        };
}