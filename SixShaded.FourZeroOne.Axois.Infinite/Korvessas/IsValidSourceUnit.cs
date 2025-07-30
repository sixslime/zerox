namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Resolved;
using Core = Core.Syntax.Core;
using u.Constructs.Ability.Types;
using u.Identifier;
using u.Data;
using Infinite = Syntax.Infinite;

public static class IsValidSourceUnit
{
    public static Korvessa<IRoveggi<uUnitIdentifier>, IRoveggi<uAbility>, Bool> Construct(IKorssa<IRoveggi<uUnitIdentifier>> unit, IKorssa<IRoveggi<uSourcedAbility>> ability) =>
        new(unit, ability)
        {
            Du = Axoi.Korvedu("IsValidSourceUnit"),
            Definition =
                (_, iUnit, iAbility) =>
                    iUnit.kRef()
                        .kGet()
                        .kGetRovi(uUnitData.OWNER)
                        .kEquals(Infinite.CurrentPlayer)
                        .kAnd(
                        iUnit.kRef()
                            .kGet()
                            .kGetRovi(uUnitData.EFFECTS)
                            .kContains(Core.kCompose<u.Constructs.UnitEffects.uShockEffect>())
                            .kNot())
        };
}