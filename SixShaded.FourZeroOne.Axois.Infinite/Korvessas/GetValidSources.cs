namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Resolved;
using Core = Core.Syntax.Core;
using u.Constructs.Ability.Types;

public static class GetValidSources
{
    public static Korvessa<IRoveggi<uSourcedAbility>, IMulti<IRoveggi<u.Identifier.uUnitIdentifier>>> Construct(IKorssa<IRoveggi<uSourcedAbility>> ability)
    => new(ability)
        {
            Du = Axoi.Korvedu("GetValidSources"),
            Definition = 
                (_, iAbility) =>
                    Core.kSubEnvironment<IMulti<IRoveggi<u.Identifier.uUnitIdentifier>>>(
                    new()
                    {
                        Environment =
                            [
                                iAbility.kRef().kGetRovi(uSourcedAbility.TYPE)
                                    .kAsVariable(out var iType),
                            ],
                        Value =
                            
                    })
        }
}