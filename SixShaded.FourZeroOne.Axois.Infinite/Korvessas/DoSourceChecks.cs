namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Resolved;
using Core = Core.Syntax.Core;
using u.Constructs.Ability.Types;
using u.Identifier;
using u.Data;
using Infinite = Syntax.Infinite;

public static class DoSourceChecks
{
    public static Korvessa<IRoveggi<uSourcedAbility>, IRoveggi<uUnitIdentifier>, IRoveggi<uSourceChecks>> Construct(IKorssa<IRoveggi<uSourcedAbility>> ability, IKorssa<IRoveggi<uUnitIdentifier>> unit) =>
        new(ability, unit)
        {
            Du = Axoi.Korvedu("DoTargetChecks"),
            Definition =
                (_, iAbility, iUnit) =>
                    Core.kSubEnvironment<IRoveggi<uSourceChecks>>(new()
                    {
                        Environment =
                            [
                                iUnit.kRef()
                                    .kRead()
                                    .kAsVariable(out var iData)
                            ],
                        Value =
                            Core.kCompose<uSourceChecks>()
                                .kWithRovi(
                                uSourceChecks.CORRECT_TEAM,
                                iData.kRef()
                                    .kGetRovi(uUnitData.OWNER)
                                    .kEquals(Infinite.CurrentPlayer))
                                .kWithRovi(
                                uSourceChecks.EFFECT_CHECK,
                                iData.kRef()
                                    .kGetRovi(uUnitData.EFFECTS)
                                    .kAnyMatch(iEffect => iEffect.kRef().kIsType<u.Constructs.UnitEffects.uShockEffect>().kExists())
                                    .kNot())
                    })
        };
}