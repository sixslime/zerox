namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using Rovetus.Constructs.Ability;
using Rovetus.Constructs.Resolved;
using Rovetus.Data;
using Core = Core.Syntax.Core;
using Infinite = Syntax.Infinite;
using ResolvedObj = IRoveggi<Rovetus.Constructs.Resolved.uResolvedSourcedAbility>;
using u.Constructs;
public record ResolveSourcedAbility(IKorssa<IRoveggi<uSourcedAbility>> ability) : Korvessa<IRoveggi<uSourcedAbility>, ResolvedObj>(ability)
{
    protected override RecursiveMetaDefinition<IRoveggi<uSourcedAbility>, ResolvedObj> InternalDefinition() =>
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
                                    A =
                                        iAbility.kRef()
                                            .kSourceChecks(iPotentialSource.kRef()),
                                })
                                .kExecuteWith(
                                new()
                                {
                                    A = iPotentialSource.kRef(),
                                }))
                    .kIOSelectOne()
                    .kAsVariable(out var iSourceUnit),
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
                                                        iAbility.kRef()
                                                            .kTargetChecks(iPotentialTarget.kRef(), iSourceUnit.kRef()),
                                                })
                                                .kExecuteWith(
                                                new()
                                                {
                                                    A = iSourceUnit.kRef(),
                                                    B = iPotentialTarget.kRef(),
                                                }))
                                    .kIOSelectOne()
                                    .kAsVariable(out var iTargetUnit),
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
                                                uResolved.INSTRUCTIONS, Core.kMulti(
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
                                                                            .kGetRovi(uSourcedAbility.EFFECTS)
                                                                            .kMap(
                                                                            iEffectType =>
                                                                                Core.kCompose<uUnitEffect>()
                                                                                    .kWithRovi(uUnitEffect.INFLICTED_BY, Infinite.CurrentPlayer)
                                                                                    .kWithRovi(uUnitEffect.TYPE, iEffectType.kRef()))))),
                                                    iAbility.kRef()
                                                        .kGetRovi(uSourcedAbility.FOLLOWUP)
                                                        .kExecuteWith(
                                                        new()
                                                        {
                                                            A = iSourceUnit.kRef(),
                                                            B = iTargetUnit.kRef(),
                                                        }),
                                                ])))),
                            })),
            });
}
