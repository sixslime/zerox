namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Ability.Types;
using u.Constructs.Resolved;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;
using Infinite = Syntax.Infinite;
using ResolvedObj = IRoveggi<u.Constructs.Resolved.uResolvedSourcedAbility>;
public static class ResolveSourcedAbility
{
    public static Korvessa<IRoveggi<uSourcedAbility>, ResolvedObj> Construct(IKorssa<IRoveggi<uSourcedAbility>> ability) =>
        new(ability)
        {
            Du = Axoi.Korvedu("ResolveSourcedAbility"),
            Definition =
                (_, iAbility) =>
                    Core.kSubEnvironment<ResolvedObj>(
                    new()
                    {
                        Environment =
                        [
                            // get source:
                            Infinite.AllUnits.kWhere(
                                iPotentialSource =>
                                    iAbility.kRef()
                                        .kGetRovi(uSourcedAbility.SOURCE_SELECTOR)
                                        .kExecuteWith(
                                        new()
                                        {
                                            A = iAbility.kRef()
                                                .kSourceChecks(iPotentialSource.kRef())
                                        })
                                        .kExecuteWith(
                                        new()
                                        {
                                            A = iPotentialSource.kRef()
                                        }))
                                .kIOSelectOne()
                                .kAsVariable(out var iSourceUnit)
                        ],
                        Value =
                            iSourceUnit.kRef()
                                .kKeepNolla(
                                () =>
                                    Core.kSubEnvironment<ResolvedObj>(
                                    new()
                                    {
                                        Environment =
                                        [
                                            iAbility.kRef()
                                                .kGetRovi(uSourcedAbility.TYPE)
                                                .kAsVariable(out var iType),
                                            iAbility.kRef()
                                                .kGetRovi(uSourcedAbility.HIT_AREA)
                                                .kAffixToUnit(iSourceUnit.kRef())
                                                .kAsVariable(out var iHitZone),

                                            // get target:
                                            Infinite.AllUnits.kWhere(
                                                iPotentialTarget =>
                                                    iAbility.kRef()
                                                        .kGetRovi(uSourcedAbility.TARGET_SELECTOR)
                                                        .kExecuteWith(
                                                        new()
                                                        {
                                                            A =
                                                                Core.kSubEnvironment<Bool>(
                                                                new()
                                                                {
                                                                    Environment =
                                                                    [
                                                                        iPotentialTarget.kRef()
                                                                            .kRead()
                                                                            .kGetRovi(uUnitData.OWNER)
                                                                            .kEquals(Infinite.CurrentPlayer)
                                                                            .kAsVariable(out var iMatchesTeam)
                                                                    ],
                                                                    Value =
                                                                        Core.kSelector<Bool>(
                                                                        new()
                                                                        {
                                                                            () =>
                                                                                iType.kRef()
                                                                                    .kIsType<uAttack>()
                                                                                    .kKeepNolla(() => iMatchesTeam.kRef().kNot()),
                                                                            () =>
                                                                                iType.kRef()
                                                                                    .kIsType<uDefense>()
                                                                                    .kKeepNolla(() => iMatchesTeam.kRef()),
                                                                        })
                                                                }),
                                                            B =
                                                                iHitZone.kRef()
                                                                    .kContains(
                                                                    iPotentialTarget.kRef()
                                                                        .kRead()
                                                                        .kGetRovi(uUnitData.POSITION)),
                                                            C =
                                                                Core.kSubEnvironment<Bool>(
                                                                new()
                                                                {
                                                                    Environment =
                                                                    [
                                                                        iSourceUnit.kRef()
                                                                            .kRead()
                                                                            .kGetRovi(uUnitData.POSITION)
                                                                            .kLineIntersectionsTo(
                                                                            iPotentialTarget.kRef()
                                                                                .kRead()
                                                                                .kGetRovi(uUnitData.POSITION))
                                                                            .kAsVariable(out var iIntersections),
                                                                        Infinite.AllUnits
                                                                            .kWhere(
                                                                            iUnit =>
                                                                                iUnit.kRef()
                                                                                    .kRead()
                                                                                    .kGetRovi(uUnitData.OWNER)
                                                                                    .kEquals(Infinite.CurrentPlayer)
                                                                                    .kNot())
                                                                            .kMap(
                                                                            iUnit =>
                                                                                iUnit.kRef()
                                                                                    .kRead()
                                                                                    .kGetRovi(uUnitData.POSITION))
                                                                            .kAsVariable(out var iEnemyPositions)
                                                                    ],
                                                                    Value =
                                                                        iIntersections.kRef()
                                                                            .kAllMatch(
                                                                            iPathStep =>
                                                                                iPathStep.kRef()
                                                                                    .kAllMatch(
                                                                                    iX =>
                                                                                        Core.kSubEnvironment<Bool>(
                                                                                        new()
                                                                                        {
                                                                                            Environment =
                                                                                            [
                                                                                                iX.kRef()
                                                                                                    .kAsAbsolute()
                                                                                                    .kAsVariable(out var iPathHex)
                                                                                            ],
                                                                                            Value =
                                                                                                iPathHex.kRef()
                                                                                                    .kRead()
                                                                                                    .kGetRovi(uHexData.TYPE)
                                                                                                    .kIsType<u.Constructs.HexTypes.uWallHex>()
                                                                                                    .kExists()
                                                                                                    .kIfTrue<Bool>(
                                                                                                    new()
                                                                                                    {
                                                                                                        Then = true.kFixed(),
                                                                                                        Else =
                                                                                                            iEnemyPositions.kRef()
                                                                                                                .kContains(iPathHex.kRef())
                                                                                                    })
                                                                                        }))
                                                                                    .kNot())

                                                                })
                                                        })
                                                        .kExecuteWith(
                                                        new()
                                                        {
                                                            A = iSourceUnit.kRef(),
                                                            B = iPotentialTarget.kRef()
                                                        }))
                                                .kIOSelectOne()
                                                .kAsVariable(out var iTargetUnit)
                                        ],

                                        // create resolved object:
                                        Value =
                                            iTargetUnit.kRef()
                                                .kKeepNolla(
                                                () =>
                                                    Core.kCompose<uResolvedSourcedAbility>()
                                                        .kWithRovi(uResolvedSourcedAbility.ABILITY, iAbility.kRef())
                                                        .kWithRovi(uResolvedSourcedAbility.SOURCE, iSourceUnit.kRef())
                                                        .kWithRovi(uResolvedSourcedAbility.TARGET, iTargetUnit.kRef())
                                                        .kWithRovi(
                                                        Core.Hint<uResolvedSourcedAbility>(),
                                                        uResolved.INSTRUCTIONS, Core.kMulti<Rog>(
                                                        [
                                                            iTargetUnit.kRef()
                                                                .kSafeUpdate(
                                                                iTargetData =>
                                                                    iTargetData.kRef()
                                                                        .kSafeUpdateRovi(
                                                                        uUnitData.EFFECTS,
                                                                        iExistingEffects =>
                                                                            iExistingEffects.kRef()
                                                                                .kConcat(
                                                                                iAbility.kRef()
                                                                                    .kGetRovi(uSourcedAbility.EFFECTS)))),
                                                            iAbility.kRef()
                                                                .kGetRovi(uSourcedAbility.FOLLOWUP)
                                                                .kExecuteWith(
                                                                new()
                                                                {
                                                                    A = iSourceUnit.kRef(),
                                                                    B = iTargetUnit.kRef()
                                                                })
                                                        ])))
                                    }))
                    })
        };
}