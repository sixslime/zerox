namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using u.Constructs.Move;
using u.Identifier;
using u.Data;
using u.Constructs.Resolved;
using ResolvedObj = IRoveggi<u.Constructs.Resolved.uResolvedNumericalMove>;
using Core = Core.Syntax.Core;

public static class ResolveNumericalMove
{
    public static Korvessa<IRoveggi<uNumericalMove>, Multi<ResolvedObj>> Construct(IKorssa<IRoveggi<uNumericalMove>> move) =>
        new(move)
        {
            Du = Axoi.Korvedu("ResolveNumericalMove"),
            Definition =
                (_, iMove) =>
                    Core.kSubEnvironment<ResolvedObj>(
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
                                .kAsVariable(out var iValidSubjects),
                            iMove.kRef()
                                .kGetRovi(uNumericalMove.DISTANCE)
                                .kAsVariable(out var iMoveRange),
                            iMove.kRef()
                                .kGetRovi(uNumericalMove.MAX_SUBJECTS)
                                .kCatchNolla(() => iValidSubjects.kRef().kCount())
                                .kAsVariable(out var iMaxSubjects),
                            iMove.kRef()
                                .kGetRovi(uNumericalMove.MAX_DISTANCE_PER_SUBJECT)
                                .kCatchNolla(() => iMoveRange.kRef().kEnd())
                                .kAsVariable(out var iMaxPerUnit)
                        ],
                        Value =
                            // nolla if impossible move:
                            // (moveRange.Min < (maxPerUnit * maxUnits))
                            iMoveRange.kRef()
                                .kStart()
                                .kIsGreaterThan(
                                iMaxSubjects.kRef().kMultiply(iMaxPerUnit.kRef()))
                                .kIfTrue<Multi<ResolvedObj>>(new()
                                {
                                    Then = Core.kNollaFor<Multi<ResolvedObj>>(),
                                    Else = Core.kMetaFunctionRecursive<IMulti<IRoveggi<uUnitIdentifier>>, Number, Multi<ResolvedObj>>(
                                        [], (iRecurse, iAvailableSubjects, iMovedDistance) =>
                                            iAvailableSubjects.kRef()
                                                .kCount()
                                                .kIsGreaterThan(0.kFixed())
                                                .kIfTrue<Multi<ResolvedObj>>(new()
                                                {
                                                    Then = Core.kSubEnvironment<Multi<ResolvedObj>>(new()
                                                    {
                                                        Environment =
                                                        [

                                                        ],
                                                        Value =
                                                    }),
                                                    Else = Core.kMulti<ResolvedObj>([])
                                                })
                                            )
                                        .kExecuteWith(
                                        new()
                                        {
                                            A = iValidSubjects.kRef(),
                                            B = 0.kFixed()
                                        })
                                })
                    })
        };
}