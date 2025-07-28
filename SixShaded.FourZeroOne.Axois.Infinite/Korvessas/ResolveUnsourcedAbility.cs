namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Resolved;
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
                                .kAsVariable(out var iInstructions)
                        ],
                        Value =
                            iInstructions.kRef()
                                .ksKeepNolla(
                                () =>
                                    Core.kCompose<uResolvedUnsourcedAbility>()
                                        .kWithRovi(uResolvedUnsourcedAbility.ABILITY, iAbility.kRef())
                                        .kWithRovi(uResolved.INSTRUCTIONS, iInstructions.kRef())
                                        .kCast<IRoveggi<uResolvedUnsourcedAbility>>())

                    })
        };
}