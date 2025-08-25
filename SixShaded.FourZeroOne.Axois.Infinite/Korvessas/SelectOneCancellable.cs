namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;

public record SelectOneCancellable<RIn, ROut>(IKorssa<IMulti<RIn>> pool, IKorssa<MetaFunction<RIn, ROut>> selectPath, IKorssa<MetaFunction<ROut>> cancelPath) : Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, MetaFunction<ROut>, ROut>(pool, selectPath, cancelPath)
    where RIn : class, Rog
    where ROut : class, Rog
{
    protected override RecursiveMetaDefinition<IMulti<RIn>, MetaFunction<RIn, ROut>, MetaFunction<ROut>, ROut> InternalDefinition() =>
        (_, iPool, iSelectPath, iCancelPath) =>
            Core.kSubEnvironment<ROut>(
            new()
            {
                Environment =
                [
                    Core.kMulti<IMulti<Rog>>(
                        [
                            iPool.kRef(),
                            Core.kCompose<u.uCancelMarker>().kYield(),
                        ])
                        .kFlatten()
                        .kIOSelectOne()
                        .kAsVariable(out var iSelection),
                ],
                Value =
                    iSelection.kRef()
                        .kIsType<IRoveggi<u.uCancelMarker>>()
                        .kExists()
                        .kIfTrue<ROut>(
                        new()
                        {
                            Then = iCancelPath.kRef().kExecute(),
                            Else =
                                iSelectPath.kRef()
                                    .kExecuteWith(
                                    new()
                                    {
                                        A =
                                            iSelection.kRef()
                                                .kIsType<RIn>(),
                                    }),
                        }),
            });
}
