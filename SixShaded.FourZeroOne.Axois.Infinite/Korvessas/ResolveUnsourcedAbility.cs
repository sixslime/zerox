namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Resolved;
using Core = Core.Syntax.Core;

public static class ResolveUnsourcedAbility
{
    public static Korvessa<IRoveggi<uUnsourcedAbility>, IRoveggi<uResolvedUnsourcedAbility>> Construct(IKorssa<IRoveggi<uUnsourcedAbility>> ability)
    => new(ability)
    {
        Du = Axoi.Korvedu("ResolveUnsourcedAbility"),
        Definition = 
            (_, iAbility) =>
                Core.kSelector<IRoveggi<uResolvedAbility>>(new()
                {
                    () => 
                        iAbility.kRef().kCast<IRoveggi<uUnsourcedAbility>>(),
                    () =>
                })
    }
}