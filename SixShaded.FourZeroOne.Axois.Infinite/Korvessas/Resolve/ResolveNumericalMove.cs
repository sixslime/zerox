namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using u.Constructs.Move;
using u.Identifier;
using u.Data;
using u.Constructs.Resolved;
using ResolvedObj = IRoveggi<u.Constructs.Resolved.uResolvedNumericalMove>;
using HexIdent = IRoveggi<u.Identifier.uHexIdentifier>;
using UnitIdent = IRoveggi<u.Identifier.uUnitIdentifier>;
using ResolvedType = u.Constructs.Resolved.uResolvedNumericalMove;
using Core = Core.Syntax.Core;

public record ResolveNumericalMove(IKorssa<IRoveggi<uNumericalMove>> move) : Korvessa<IRoveggi<uNumericalMove>, Multi<ResolvedObj>>(move)
{
    protected override RecursiveMetaDefinition<IRoveggi<uNumericalMove>, Multi<ResolvedObj>> InternalDefinition() =>
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
                                                    .kMoveSubjectChecks(iUnit.kRef()),
                                        })
                                        .kExecuteWith(
                                        new()
                                        {
                                            A = iUnit.kRef(),
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
                            Core.kMetaFunctionRecursive<IMulti<UnitIdent>, Number, Number, Multi<ResolvedObj>>(
                                [],
                                (iRecurseUnitSelection, iAvailableSubjects, iMovedDistance, iRemainingCount) =>
                                    iAvailableSubjects.kRef()
                                        .kCount()
                                        .kIsGreaterThan(0.kFixed())
                                        .kAnd(iRemainingCount.kRef().kIsGreaterThan(0.kFixed()))
                                        .kIfTrue<Multi<ResolvedObj>>(
                                        new()
                                        {
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
                                                            .kClamp(0.kFixed().kRangeTo(iMaxPerUnit.kRef()))
                                                            .kRangeTo(
                                                            iMoveRange.kRef()
                                                                .kEnd()
                                                                .kSubtract(iMovedDistance.kRef())
                                                                .kClamp(0.kFixed().kRangeTo(iMaxPerUnit.kRef())))
                                                            .kAsVariable(out var iAbsoluteMoveRange),
                                                        Core.kMetaFunction<UnitIdent, Multi<ResolvedObj>>(
                                                            [],
                                                            iSelectedSubject =>
                                                                Core.kSubEnvironment<Multi<ResolvedObj>>(
                                                                new()
                                                                {
                                                                    Environment =
                                                                    [
                                                                        Core.kSubEnvironment<ResolvedObj>(
                                                                            new()
                                                                            {
                                                                                Environment =
                                                                                [
                                                                                    iSelectedSubject.kRef()
                                                                                        .kRead()
                                                                                        .kGetRovi(uUnitData.POSITION)
                                                                                        .kAsVariable(out var iSubjectPos),
                                                                                    iSelectedSubject.kRef()
                                                                                        .kGetMoveRange(iAbsoluteMoveRange.kRef())
                                                                                        .kAsVariable(out var iThisMoveRange),

                                                                                    // DEV: creates a Multi<[Number, Multi<HexIdent>]>.
                                                                                    // each element is a step of the pathing function.
                                                                                    // e[1] is the distance traveled.
                                                                                    // e[2] is the hexes in the step.
                                                                                    // "it takes e[1] steps to get to e[2]".
                                                                                    Core.kMetaFunctionRecursive<IMulti<HexIdent>, IMulti<HexIdent>, Number, Multi<Multi<Rog>>>(
                                                                                        [],
                                                                                        (iRecursePathing, iRoots, iSeen, iDistance) =>
                                                                                            iDistance.kRef()
                                                                                                .kIsGreaterThan(iThisMoveRange.kRef().kEnd())
                                                                                                .kOr(
                                                                                                iRoots.kRef()
                                                                                                    .kCount()
                                                                                                    .kIsGreaterThan(0.kFixed())
                                                                                                    .kNot())
                                                                                                .kIfTrue<Multi<Multi<Rog>>>(
                                                                                                new()
                                                                                                {
                                                                                                    Then = Core.kMulti<Multi<Rog>>([]),
                                                                                                    Else =
                                                                                                        Core.kSubEnvironment<Multi<Multi<Rog>>>(
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
                                                                                                                                            .kAsVariable(out var iStepAbsolute),
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
                                                                                                                                                            .kMovePathChecks(iSelectedSubject.kRef()),
                                                                                                                                                })
                                                                                                                                                .kExecuteWith(
                                                                                                                                                new()
                                                                                                                                                {
                                                                                                                                                    A = iRoot.kRef(),
                                                                                                                                                    B = iStepAbsolute.kRef().kAsAbsolute(),
                                                                                                                                                })),
                                                                                                                                })))
                                                                                                                    .kFlatten()
                                                                                                                    .kMap(iHex => iHex.kRef().kAsAbsolute())
                                                                                                                    .kDistinct()
                                                                                                                    .kAsVariable(out var iSteps),
                                                                                                            ],
                                                                                                            Value =
                                                                                                                Core.kMulti<Rog>(
                                                                                                                    new()
                                                                                                                    {
                                                                                                                        iDistance.kRef(),
                                                                                                                        iThisMoveRange.kRef()
                                                                                                                            .kStart()
                                                                                                                            .kIsGreaterThan(iDistance.kRef())
                                                                                                                            .kIfTrue<Multi<HexIdent>>(
                                                                                                                            new()
                                                                                                                            {
                                                                                                                                Then = Core.kMulti<HexIdent>([]),
                                                                                                                                Else = iSteps.kRef(),
                                                                                                                            }),
                                                                                                                    })
                                                                                                                    .kYield()
                                                                                                                    .kConcat(
                                                                                                                    iRecursePathing.kRef()
                                                                                                                        .kExecuteWith(
                                                                                                                        new()
                                                                                                                        {
                                                                                                                            A = iSteps.kRef(),
                                                                                                                            B = iSeen.kRef().kUnion(iSteps.kRef()),
                                                                                                                            C = iDistance.kRef().kAdd(1.kFixed()),
                                                                                                                        })),
                                                                                                        }),
                                                                                                }))
                                                                                        .kExecuteWith(
                                                                                        new()
                                                                                        {
                                                                                            A = iSubjectPos.kRef().kYield(),
                                                                                            B = iSubjectPos.kRef().kYield(),
                                                                                            C = 1.kFixed(),
                                                                                        })
                                                                                        .kAsVariable(out var iIndexedAvailableMoves),
                                                                                ],
                                                                                Value =
                                                                                    Core.kSubEnvironment<ResolvedObj>(
                                                                                    new()
                                                                                    {
                                                                                        Environment =
                                                                                        [
                                                                                            iIndexedAvailableMoves.kRef()
                                                                                                .kFlatten()
                                                                                                .kMap(iObj => iObj.kRef().kIsType<HexIdent>())
                                                                                                .kClean()
                                                                                                .kIOSelectOne()
                                                                                                .kAsVariable(out var iSelectedHex),
                                                                                        ],
                                                                                        Value =
                                                                                            iSelectedHex.kRef()
                                                                                                .kKeepNolla(
                                                                                                () =>
                                                                                                    Core.kCompose<ResolvedType>()
                                                                                                        .kWithRovi(
                                                                                                        Core.Hint<ResolvedType>(),
                                                                                                        uResolvedMove.SUBJECT,
                                                                                                        iSelectedSubject.kRef())
                                                                                                        .kWithRovi(
                                                                                                        Core.Hint<ResolvedType>(),
                                                                                                        uResolvedMove.FROM,
                                                                                                        iSubjectPos.kRef())
                                                                                                        .kWithRovi(
                                                                                                        Core.Hint<ResolvedType>(),
                                                                                                        uResolvedMove.DESTINATION,
                                                                                                        iSelectedHex.kRef())
                                                                                                        .kWithRovi(
                                                                                                        Core.Hint<ResolvedType>(),
                                                                                                        ResolvedType.MOVE,
                                                                                                        iMove.kRef())
                                                                                                        .kWithRovi(
                                                                                                        Core.Hint<ResolvedType>(),
                                                                                                        ResolvedType.PATH_DISTANCE,
                                                                                                        iIndexedAvailableMoves.kRef()
                                                                                                            .kFirstMatch(
                                                                                                            iIndexedMove =>
                                                                                                                iIndexedMove.kRef()
                                                                                                                    .kGetIndex(2.kFixed())
                                                                                                                    .kIsType<IMulti<HexIdent>>()
                                                                                                                    .kContains(iSelectedHex.kRef()))
                                                                                                            .kGetIndex(1.kFixed())
                                                                                                            .kIsType<Number>())
                                                                                                        .kWithRovi(
                                                                                                        Core.Hint<ResolvedType>(),
                                                                                                        uResolved.INSTRUCTIONS,
                                                                                                        iSelectedSubject.kRef()
                                                                                                            .kSafeUpdate(
                                                                                                            iSubjectData =>
                                                                                                                iSubjectData.kRef()
                                                                                                                    .kWithRovi(uUnitData.POSITION, iSelectedHex.kRef())))),
                                                                                    }),
                                                                            })
                                                                            .kAsVariable(out var iResolved),
                                                                    ],
                                                                    Value =
                                                                        Core.kSubEnvironment<Multi<ResolvedObj>>(
                                                                        new()
                                                                        {
                                                                            Environment =
                                                                            [
                                                                                iResolved.kRef()
                                                                                    .kExists()
                                                                                    .kIfTrue<Number>(
                                                                                    new()
                                                                                    {
                                                                                        Then =
                                                                                            iMovedDistance.kRef()
                                                                                                .kAdd(
                                                                                                iResolved.kRef()
                                                                                                    .kGetRovi(ResolvedType.PATH_DISTANCE)
                                                                                                    .kMultiply(
                                                                                                    iAbsoluteMoveRange.kRef()
                                                                                                        .kEnd()
                                                                                                        .kDivide(
                                                                                                        iThisMoveRange.kRef()
                                                                                                            .kEnd())
                                                                                                        .kAtLeast(1.kFixed()))),
                                                                                        Else = iMovedDistance.kRef(),
                                                                                    })
                                                                                    .kAsVariable(out var iRemainingDistance),
                                                                            ],
                                                                            Value =
                                                                                iResolved.kRef()
                                                                                    .kYield()
                                                                                    .kClean()
                                                                                    .kConcat(
                                                                                    iRecurseUnitSelection.kRef()
                                                                                        .kExecuteWith(
                                                                                        new()
                                                                                        {
                                                                                            A =
                                                                                                iAvailableSubjects.kRef()
                                                                                                    .kExcept(iSelectedSubject.kRef().kYield()),
                                                                                            B = iRemainingDistance.kRef(),
                                                                                            C = iRemainingCount.kRef().kSubtract(1.kFixed()),
                                                                                        })),
                                                                        }),
                                                                }))
                                                            .kAsVariable(out var iUnitMoveFunction),
                                                    ],
                                                    Value =
                                                        iMoveRange.kRef()
                                                            .kStart()
                                                            .kSubtract(iMovedDistance.kRef())
                                                            .kIsGreaterThan(0.kFixed())
                                                            .kIfTrue<Multi<ResolvedObj>>(
                                                            new()
                                                            {
                                                                Then =
                                                                    iUnitMoveFunction.kRef()
                                                                        .kExecuteWith(
                                                                        new()
                                                                        {
                                                                            A = iAvailableSubjects.kRef().kIOSelectOne(),
                                                                        }),
                                                                Else =
                                                                    iAvailableSubjects.kRef()
                                                                        .kIOSelectOneCancellableDirect(
                                                                        Core.Hint<Multi<ResolvedObj>>(),
                                                                        new()
                                                                        {
                                                                            Select = iUnitMoveFunction.kRef(),
                                                                            Cancel = Core.kMulti<ResolvedObj>([]).kMetaBoxed([]),
                                                                        }),
                                                            }),
                                                }),
                                            Else = Core.kMulti<ResolvedObj>([]),
                                        }))
                                .kExecuteWith(
                                new()
                                {
                                    A = iValidSubjects.kRef(),
                                    B = 0.kFixed(),
                                    C = iMaxSubjects.kRef(),
                                }),
                    })
}
