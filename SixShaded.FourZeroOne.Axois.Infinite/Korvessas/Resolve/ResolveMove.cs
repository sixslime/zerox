namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;
using SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;
using ResolvedObj = IRoveggi<u.Constructs.Resolved.uResolvedMove>;
using Core = Core.Syntax.Core;

public static class ResolveMove
{
    public static Korvessa<IRoveggi<uMove>, IMulti<ResolvedObj>> Construct(IKorssa<IRoveggi<uMove>> move) =>
        new(move)
        {
            Du = Axoi.Korvedu("ResolveMove"),
            Definition =
                (_, iMove) =>
                    Core.kSubEnvironment<IMulti<ResolvedObj>>(
                    new()
                    {
                        Environment =
                        [
                            iMove.kRef()
                                .kGetRovi(uMove.ENVIRONMENT_PREMOD)
                                .kExecute()
                        ],
                        Value =
                            Core.kSelector<IMulti<ResolvedObj>>(
                            [
                                // NUMERICAL:
                                () =>
                                    iMove.kRef()
                                        .kIsType<IRoveggi<uNumericalMove>>()
                                        .kResolve(),

                                // POSITIONL:
                                () =>
                                    iMove.kRef()
                                        .kIsType<IRoveggi<uPositionalMove>>()
                                        .kResolve()
                                        .kYield(),
                            ])
                    })

        };
}