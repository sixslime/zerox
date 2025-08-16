namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using u.Constructs.Move;
using u.Identifier;
using u.Data;
using u.Constructs.Resolved;
using ResolvedObj = IRoveggi<u.Constructs.Resolved.uResolvedPositionalMove>;
using Core = Core.Syntax.Core;

public static class ResolvePositionalMove
{
    public static Korvessa<IRoveggi<uPositionalMove>, Multi<ResolvedObj>> Construct(IKorssa<IRoveggi<uPositionalMove>> move) =>
        new(move)
        {
            Du = Axoi.Korvedu("ResolvePositionalMove"),
            Definition =
                (_, iMove) =>
                    Core.kSubEnvironment<Multi<ResolvedObj>>(
                    new()
                    {
                        Environment =
                        [
                            iMove.kRef()
                                .kGetRovi(uMove.SUBJECT_COLLECTOR)
                                .kExecute()
                                .kWhere(
                                iUnit =>
                                    iMove.kRef()
                                        .kGetRovi(uMove.SUBJECT_SELECTOR)
                                        .kExecuteWith(
                                        new()
                                        {
                                            A =
                                                iMove.kRef()
                                                    .kSubjectChecks(iUnit.kRef())
                                        })
                                        .kExecuteWith(
                                        new()
                                        {
                                            A = iUnit.kRef()
                                        }))
                                .kIOSelectOne()
                                .kAsVariable(out var iSubjectUnit)
                        ],
                        Value =
                            iSubjectUnit.kRef()
                                .kKeepNolla(() =>
                                    Core.kSubEnvironment<Multi<ResolvedObj>>(
                                    new()
                                    {
                                        Environment =
                                        [
                                            iSubjectUnit.kRef()
                                                .kRead()
                                                .kGetRovi(uUnitData.POSITION)
                                                .kAsVariable(out var iFromPosition),
                                            iMove.kRef()
                                                .kGetRovi(uPositionalMove.MOVE_FUNCTION)
                                                .kExecuteWith(
                                                new()
                                                {
                                                    A = iSubjectUnit.kRef()
                                                })
                                                .kWhere(
                                                iHex =>
                                                    iMove.kRef()
                                                        .kGetRovi(uMove.DESTINATION_SELECTOR)
                                                        .kExecuteWith(
                                                        new()
                                                        {
                                                            A =
                                                                iSubjectUnit.kRef()
                                                                    .kMoveDestinationChecks(iHex.kRef())
                                                        })
                                                        .kExecuteWith(
                                                        new()
                                                        {
                                                            A = iHex.kRef()
                                                        }))
                                                .kIOSelectOne()
                                                .kAsVariable(out var iDestinationPosition)
                                        ],
                                        Value =
                                            iDestinationPosition.kRef()
                                                .kKeepNolla(
                                                () =>
                                                    Core.kCompose<uResolvedPositionalMove>()
                                                        .kWithRovi(uResolvedPositionalMove.MOVE, iMove.kRef())
                                                        .kWithRovi(
                                                        Core.Hint<uResolvedPositionalMove>(),
                                                        uResolvedMove.SUBJECT, iSubjectUnit.kRef())
                                                        .kWithRovi(
                                                        Core.Hint<uResolvedPositionalMove>(),
                                                        uResolvedMove.FROM,
                                                        iFromPosition.kRef())
                                                        .kWithRovi(
                                                        Core.Hint<uResolvedPositionalMove>(),
                                                        uResolvedMove.DESTINATION,
                                                        iDestinationPosition.kRef())
                                                        .kWithRovi(
                                                        Core.Hint<uResolvedPositionalMove>(),
                                                        uResolved.INSTRUCTIONS,
                                                        iSubjectUnit.kRef()
                                                            .kSafeUpdate(
                                                            iUnit =>
                                                                iUnit.kRef()
                                                                    .kWithRovi(uUnitData.POSITION, iDestinationPosition.kRef())))
                                                        .kYield())
                                    })

                    })
        };
}