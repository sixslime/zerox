namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using u.Constructs.Move;
using u.Identifier;
using u.Data;
using u.Constructs.Resolved;
using ResolvedObj = IRoveggi<u.Constructs.Resolved.uResolvedNumericalMove>;
using HexIdent = IRoveggi<u.Identifier.uHexIdentifier>;
using UnitIdent = IRoveggi<u.Identifier.uUnitIdentifier>;
using Core = Core.Syntax.Core;
using u.Constructs.UnitEffects;

public static class ResolveNumericalMove
{
    public static Korvessa<IRoveggi<uNumericalMove>, Multi<ResolvedObj>> Construct(IKorssa<IRoveggi<uNumericalMove>> move) =>
        new(move)
        {
            Du = Axoi.Korvedu("ResolveNumericalMove"),
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
                                                    .kMoveSubjectChecks(iUnit.kRef())
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
                                .kAsVariable(out var iMaxPerUnit),
                        ],
                        Value =

                            // nolla if impossible move:
                            // DEV: honestly i think this should just force a max move from all units.
                            // (moveRange.Min < (maxPerUnit * maxUnits))
                            iMoveRange.kRef()
                                .kStart()
                                .kIsGreaterThan(iMaxSubjects.kRef().kMultiply(iMaxPerUnit.kRef()))
                                .kIfTrue<Multi<ResolvedObj>>(
                                new()
                                {
                                    Then = Core.kNollaFor<Multi<ResolvedObj>>(),
                                    Else =
                                        Core.kMetaFunctionRecursive<IMulti<UnitIdent>, Number, Number, Multi<ResolvedObj>>(
                                            [], (iRecurseUnitSelection, iAvailableSubjects, iMovedDistance, iRemainingCount) =>
                                                iAvailableSubjects.kRef()
                                                    .kCount()
                                                    .kIsGreaterThan(0.kFixed())
                                                    .kIfTrue<Multi<ResolvedObj>>(
                                                    new()
                                                    {
                                                        // TODO LEFTOFF:
                                                        Then =
                                                            Core.kSubEnvironment<Multi<ResolvedObj>>(
                                                            new()
                                                            {
                                                                Environment =
                                                                [
                                                                    iMoveRange.kRef()
                                                                        .kStart()
                                                                        .kSubtract(iMovedDistance.kRef())
                                                                        .kSubtract(
                                                                        iRemainingCount.kRef()
                                                                            .kSubtract(1.kFixed())
                                                                            .kMultiply(iMaxPerUnit.kRef()))
                                                                        .kAsVariable(out var iMinMoveUnclamped),
                                                                    Core.kMetaFunction<UnitIdent, NumRange, ResolvedObj>(

                                                                        // 'iThisMoveRange' should be non-zero here.
                                                                        [], (iSelectedSubject, iThisMoveRange) =>
                                                                            Core.kSubEnvironment<ResolvedObj>(
                                                                            new()
                                                                            {
                                                                                Environment =
                                                                                [
                                                                                    iSelectedSubject.kRef()
                                                                                        .kRead()
                                                                                        .kGetRovi(uUnitData.POSITION)
                                                                                        .kAsVariable(out var iSubjectPos),
                                                                                    Core.kMetaFunctionRecursive<IMulti<HexIdent>, IMulti<HexIdent>, Number, Multi<HexIdent>>(
                                                                                        [],
                                                                                        (iRecursePathing, iRoots, iSeen, iDistance) =>
                                                                                            iDistance.kRef()
                                                                                                .kIsGreaterThan(iThisMoveRange.kRef().kEnd())
                                                                                                .kOr(
                                                                                                iRoots.kRef()
                                                                                                    .kCount()
                                                                                                    .kIsGreaterThan(0.kFixed())
                                                                                                    .kNot())
                                                                                                .kIfTrue<Multi<HexIdent>>(
                                                                                                new()
                                                                                                {
                                                                                                    Then = Core.kMulti<HexIdent>([]),
                                                                                                    Else =
                                                                                                        Core.kSubEnvironment<Multi<HexIdent>>(
                                                                                                        new()
                                                                                                        {
                                                                                                            Environment =
                                                                                                            [
                                                                                                                iRoots.kRef()
                                                                                                                    .kMap(
                                                                                                                    iRoot =>
                                                                                                                        iRoot.kRef()
                                                                                                                            .kAdjacent()
                                                                                                                            .kWhere(
                                                                                                                            iStep =>
                                                                                                                                Core.kSubEnvironment<Bool>(
                                                                                                                                new()
                                                                                                                                {
                                                                                                                                    Environment =
                                                                                                                                    [
                                                                                                                                        iStep.kRef()
                                                                                                                                            .kAsAbsolute()
                                                                                                                                            .kAsVariable(out var iStepAbsolute)
                                                                                                                                    ],
                                                                                                                                    Value =
                                                                                                                                        iSeen.kRef()
                                                                                                                                            .kContains(iStepAbsolute.kRef())
                                                                                                                                            .kNot()
                                                                                                                                            .ksLazyAnd(
                                                                                                                                            iMove.kRef()
                                                                                                                                                .kGetRovi(uNumericalMove.PATH_SELECTOR)
                                                                                                                                                .kExecuteWith(
                                                                                                                                                new()
                                                                                                                                                {
                                                                                                                                                    A =
                                                                                                                                                        iStepAbsolute.kRef()
                                                                                                                                                            .kMovePathChecks(iSelectedSubject.kRef())
                                                                                                                                                })
                                                                                                                                                .kExecuteWith(
                                                                                                                                                new()
                                                                                                                                                {
                                                                                                                                                    A = iRoot.kRef(),
                                                                                                                                                    B = iStepAbsolute.kRef().kAsAbsolute()
                                                                                                                                                }))
                                                                                                                                })))
                                                                                                                    .kFlatten()
                                                                                                                    .kMap(iHex => iHex.kRef().kAsAbsolute())
                                                                                                                    .kDistinct()
                                                                                                                    .kAsVariable(out var iSteps)
                                                                                                            ],
                                                                                                            Value =
                                                                                                                iThisMoveRange.kRef()
                                                                                                                    .kStart()
                                                                                                                    .kIsGreaterThan(iDistance.kRef())
                                                                                                                    .kIfTrue<Multi<HexIdent>>(
                                                                                                                    new()
                                                                                                                    {
                                                                                                                        Then = Core.kMulti<HexIdent>([]),
                                                                                                                        Else = iSteps.kRef()
                                                                                                                    })
                                                                                                                    .kUnion(
                                                                                                                    iRecursePathing.kRef()
                                                                                                                        .kExecuteWith(
                                                                                                                        new()
                                                                                                                        {
                                                                                                                            A = iSteps.kRef(),
                                                                                                                            B = iSeen.kRef().kUnion(iSteps.kRef()),
                                                                                                                            C = iDistance.kRef().kAdd(1.kFixed())
                                                                                                                        }))
                                                                                                        })
                                                                                                }))
                                                                                        .kExecuteWith(
                                                                                        new()
                                                                                        {
                                                                                            A = iSubjectPos.kRef().kYield(),
                                                                                            B = iSubjectPos.kRef().kYield(),
                                                                                            C = 1.kFixed()
                                                                                        })
                                                                                        .kAsVariable(out var iAvailableHexes)
                                                                                ],
                                                                                Value = 
                                                                            }))
                                                                        .kAsVariable(out var iUnitMoveFunction)
                                                                ],
                                                                Value =
                                                                    iMinMoveUnclamped.kRef()
                                                                        .kIsGreaterThan(0.kFixed())
                                                                        .kIfTrue(
                                                                        new()
                                                                            { })
                                                            }),
                                                        Else = Core.kMulti<ResolvedObj>([])
                                                    }))
                                            .kExecuteWith(
                                            new()
                                            {
                                                A = iValidSubjects.kRef(),
                                                B = 0.kFixed(),
                                                C = iMaxSubjects.kRef()
                                            })
                                })
                    })
        };
}