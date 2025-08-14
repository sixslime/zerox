namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Resolved;
using Core = Core.Syntax.Core;

public static class ResolveSourcedAbility
{
    public static Korvessa<IRoveggi<uSourcedAbility>, IRoveggi<uResolvedSourcedAbility>> Construct(IKorssa<IRoveggi<uSourcedAbility>> ability) =>
        new(ability)
        {
            Du = Axoi.Korvedu("ResolveSourcedAbility"),
            Definition =
                (_, iAbility) =>
                    Core.kSubEnvironment<IRoveggi<uResolvedSourcedAbility>>(
                    new()
                    {
                        Environment =
                        [
                            iAbility.kRef()
                                .kGetRovi(uSourcedAbility.ACTION)
                                .kExecute()
                                .kAsVariable(out var iInstructions)
                        ],
                        Value =
                            iInstructions.kRef()
                                .kKeepNolla(
                                () =>
                                    Core.kCompose<uResolvedSourcedAbility>()
                                        .kWithRovi(uResolvedSourcedAbility.ABILITY, iAbility.kRef())
                                        .kWithRovi(uResolved.INSTRUCTIONS, iInstructions.kRef())
                                        .kCast<IRoveggi<uResolvedSourcedAbility>>())

                    })
        };
}