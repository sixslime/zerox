namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using Rovetus.Constructs.Ability;
using Rovetus.Constructs.Resolved;
using Core = Core.Syntax.Core;

public static class ResolveUnsourcedAbility
{
    public static Korvessa<IRoveggi<uUnsourcedAbility>, IRoveggi<uResolvedUnsourcedAbility>> Construct(IKorssa<IRoveggi<uUnsourcedAbility>> ability) =>
        new(ability)
        {
            Du = Axoi.Korvedu("ResolveUnsourcedAbility"),
            Definition =
                (_, iAbility) =>
                    Core.kSubEnvironment<IRoveggi<uResolvedUnsourcedAbility>>(
                    new()
                    {
                        Environment =
                        [
                            iAbility.kRef()
                                .kGetRovi(uUnsourcedAbility.ACTION)
                                .kExecute()
                                .kAsVariable(out var iInstructions),
                        ],
                        Value =
                            iInstructions.kRef()
                                .kKeepNolla(
                                () =>
                                    Core.kCompose<uResolvedUnsourcedAbility>()
                                        .kWithRovi(uResolvedUnsourcedAbility.ABILITY, iAbility.kRef())
                                        .kWithRovi(Core.Hint<uResolvedUnsourcedAbility>(), uResolved.INSTRUCTIONS, iInstructions.kRef())),
                    }),
        };
}