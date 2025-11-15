namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using Rovetus.Constructs.Ability;
using Rovetus.Constructs.Resolved;
using Core = Core.Syntax.Core;

public record ResolveAbility(IKorssa<IRoveggi<uAbility>> ability) : Korvessa<IRoveggi<uAbility>, IRoveggi<uResolvedAbility>>(ability)
{
    protected override RecursiveMetaDefinition<IRoveggi<uAbility>, IRoveggi<uResolvedAbility>> InternalDefinition() =>
        (_, iAbility) =>
            Core.kSubEnvironment<IRoveggi<uResolvedAbility>>(
            new()
            {
                Environment =
                [
                    iAbility.kRef()
                        .kGetRovi(uAbility.ENVIRONMENT_PREMOD)
                        .kExecute(),
                ],
                Value =
                    Core.kSelector<IRoveggi<uResolvedAbility>>(
                    [
                        // SOURCED:
                        () =>
                            iAbility.kRef()
                                .kIsType<IRoveggi<uSourcedAbility>>()
                                .kResolve(),

                        // UNSOURCED:
                        () =>
                            iAbility.kRef()
                                .kIsType<IRoveggi<uUnsourcedAbility>>()
                                .kResolve(),
                    ]),
            });
}
