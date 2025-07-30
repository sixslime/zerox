namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Resolved;
using Core = Core.Syntax.Core;
using u.Constructs.Ability.Types;
using u.Identifier;
using u.Data;
using Infinite = Syntax.Infinite;

public static class IsValidTargetUnit
{
    public static Korvessa<IRoveggi<uUnitIdentifier>, IRoveggi<uAbility>, Bool> Construct(IKorssa<IRoveggi<uUnitIdentifier>> unit, IKorssa<IRoveggi<uSourcedAbility>> ability) =>
        new(unit, ability)
        {
            Du = Axoi.Korvedu("IsValidTargetUnit"),
            Definition =
                (_, iUnit, iAbility) =>
                    // TODO
                    // find a way to make a korvessa/korssa that does the hex line algorithm for intersecting walls.
                    throw new NotImplementedException()
        };
}