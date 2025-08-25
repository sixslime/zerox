namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;

public record SelectMultipleCancellable<RIn, ROut>(IKorssa<IMulti<RIn>> pool, IKorssa<MetaFunction<IMulti<RIn>, ROut>> selectPath, IKorssa<MetaFunction<ROut>> cancelPath) : Korvessa<IMulti<RIn>, MetaFunction<IMulti<RIn>, ROut>, MetaFunction<ROut>, MetaFunction<NumRange, ROut>>(pool, selectPath, cancelPath)
    where RIn : class, Rog
    where ROut : class, Rog
{
    protected override RecursiveMetaDefinition<IMulti<RIn>, MetaFunction<IMulti<RIn>, ROut>, MetaFunction<ROut>, MetaFunction<NumRange, ROut>> InternalDefinition() =>
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
            });
}
