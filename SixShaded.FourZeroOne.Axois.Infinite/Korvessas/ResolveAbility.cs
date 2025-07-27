namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Resolved;
public static class ResolveAbility
{
    public static Korvessa<IRoveggi<uAbility>, IRoveggi<uResolvedAbility>> Construct(IKorssa<IRoveggi<uAbility>> ability)
    => new(ability)
    {
        Du = Axoi.Korvedu("ResolveAbility"),
        Definition = 
            (_, iAbility) =>
                iAbility.kRef().kSwitch([
                    ()
                    ]
    }
}