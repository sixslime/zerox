namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Resolved;
using Core = Core.Syntax.Core;
using u.Constructs.Ability.Types;
using u.Identifier;
using u.Data;
using Infinite = Syntax.Infinite;

public static class DoTargetChecks
{
    public static Korvessa<IRoveggi<uSourcedAbility>, IRoveggi<uUnitIdentifier>, IRoveggi<uUnitIdentifier>, IRoveggi<uTargetChecks>> Construct(IKorssa<IRoveggi<uSourcedAbility>> ability, IKorssa<IRoveggi<uUnitIdentifier>> unit, IKorssa<IRoveggi<uUnitIdentifier>> source) =>
        new(ability, unit, source)
        {
            Du = Axoi.Korvedu("DoTargetChecks"),
            Definition =
                (_, iAbility, iUnit, iSource) =>
                    Core.kCompose<uTargetChecks>()

                        // correct team:
                        .kWithRovi(
                        uTargetChecks.CORRECT_TEAM,
                        Core.kSubEnvironment<Bool>(
                        new()
                        {
                            Environment =
                            [
                                iUnit.kRef()
                                    .kRead()
                                    .kGetRovi(uUnitData.OWNER)
                                    .kEquals(Infinite.CurrentPlayer)
                                    .kAsVariable(out var iMatchesTeam),
                                iAbility.kRef()
                                    .kGetRovi(uSourcedAbility.TYPE)
                                    .kAsVariable(out var iType)
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
                        }))

                        // hit area:
                        .kWithRovi(
                        uTargetChecks.HIT_AREA,
                        iAbility.kRef()
                            .kGetRovi(uSourcedAbility.HIT_AREA)
                            .kAffixToUnit(iSource.kRef())
                            .kContains(
                            iUnit.kRef()
                                .kRead()
                                .kGetRovi(uUnitData.POSITION)))

                        // line of sight:
                        .kWithRovi(
                        uTargetChecks.LINE_OF_SIGHT,
                        Core.kSubEnvironment<Bool>(
                        new()
                        {
                            Environment =
                            [
                                iSource.kRef()
                                    .kRead()
                                    .kGetRovi(uUnitData.POSITION)
                                    .kLineIntersectionsTo(
                                    iUnit.kRef()
                                        .kRead()
                                        .kGetRovi(uUnitData.POSITION))
                                    .kAsVariable(out var iIntersections),
                                Infinite.AllUnits
                                    .kWhere(
                                    iOtherUnit =>
                                        iOtherUnit.kRef()
                                            .kRead()
                                            .kGetRovi(uUnitData.OWNER)
                                            .kEquals(Infinite.CurrentPlayer)
                                            .kNot())
                                    .kMap(
                                    iEnemy =>
                                        iEnemy.kRef()
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

                        }))

                        // effects check:
                        .kWithRovi(
                        uTargetChecks.EFFECT_CHECK,
                        iAbility.kRef()
                            .kGetRovi(uSourcedAbility.TYPE)
                            .kIsType<uDefense>()
                            .kExists()
                            .kIfTrue<Bool>(
                            new()
                            {
                                Then =
                                    iUnit.kRef()
                                        .kRead()
                                        .kGetRovi(uUnitData.EFFECTS)
                                        .kContains(Core.kCompose<u.Constructs.UnitEffects.uShockEffect>())
                                        .kNot(),
                                Else = true.kFixed()
                            }))

        };
}