namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using Rovetus.Constructs.Ability;
using Rovetus.Constructs.Resolved;
using Core = Core.Syntax.Core;

public record ResolveUnsourcedAbility(IKorssa<IRoveggi<uUnsourcedAbility>> ability) : Korvessa<IRoveggi<uUnsourcedAbility>, IRoveggi<uResolvedUnsourcedAbility>>(ability)
{
    protected override RecursiveMetaDefinition<IRoveggi<uUnsourcedAbility>, IRoveggi<uResolvedUnsourcedAbility>> InternalDefinition() =>
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
            });
}
