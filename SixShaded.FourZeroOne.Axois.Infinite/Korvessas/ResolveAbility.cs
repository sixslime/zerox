namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Resolved;
using Core = Core.Syntax.Core;

public static class ResolveAbility
{
    public static Korvessa<IRoveggi<uAbility>, IRoveggi<uResolvedAbility>> Construct(IKorssa<IRoveggi<uAbility>> ability) =>
        new(ability)
        {
            Du = Axoi.Korvedu("ResolveAbility"),
            Definition =
                (_, iAbility) =>
                    Core.kSubEnvironment<IRoveggi<uResolvedAbility>>(
                    new()
                    {
                        Environment =
                        [
                            iAbility.kRef()
                                .kGetRovi(uAbility.ENVIRONMENT_PREMOD)
                                .kExecute()
                        ],
                        Value =
                            Core.kSelector<IRoveggi<uResolvedAbility>>(
                            [
                                // SOURCED:
                                () =>
                                    iAbility.kRef()
                                        .kCast<IRoveggi<uSourcedAbility>>()
                                        .kResolve(),

                                // UNSOURCED:
                                () =>
                                    Core.kSubEnvironment<IRoveggi<uResolvedUnsourcedAbility>>(
                                    new()
                                    {
                                        Environment =
                                        [
                                            iAbility.kRef()
                                                .kCast<IRoveggi<uUnsourcedAbility>>()
                                                .kAsVariable(out var iUnsourced)
                                        ],
                                        Value =
                                            iUnsourced.kRef()
                                                .kKeepNolla(
                                                () =>
                                                    Core.kSubEnvironment<IRoveggi<uResolvedUnsourcedAbility>>(
                                                    new()
                                                    {
                                                        Environment =
                                                        [
                                                            iUnsourced.kRef()
                                                                .kGetRovi(uUnsourcedAbility.ACTION)
                                                                .kExecute()
                                                                .kAsVariable(out var iInstructions)
                                                        ],
                                                        Value =
                                                            iInstructions.kRef()
                                                                .kKeepNolla(
                                                                () =>
                                                                    Core.kCompose<uResolvedUnsourcedAbility>()
                                                                        .kWithRovi(uResolvedUnsourcedAbility.ABILITY, iUnsourced.kRef())
                                                                        .kWithRovi(uResolved.INSTRUCTIONS, iInstructions.kRef())
                                                                        .kCast<IRoveggi<uResolvedUnsourcedAbility>>())

                                                    }))

                                    })
                            ])
                    })

        };
}