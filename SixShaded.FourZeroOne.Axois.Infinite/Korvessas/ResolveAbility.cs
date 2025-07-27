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
                    Core.kSelector<IRoveggi<uResolvedAbility>>(
                    [
                        () =>
                            iAbility.kRef()
                                .kCast<IRoveggi<uSourcedAbility>>()
                                .kResolve(),
                        () =>
                            iAbility.kRef()
                                .kCast<IRoveggi<uUnsourcedAbility>>()
                                .kResolve(),
                    ])
        };
}