namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;

public interface uResolvedUnsourcedAbility : IConcreteRovetu, uResolvedAbility
{
    public static new readonly Rovu<uResolvedUnsourcedAbility, IRoveggi<Ability.uUnsourcedAbility>> ABILITY = new(Axoi.Du, "ability");
    public static readonly ImplementationStatement<uResolvedUnsourcedAbility> __IMPLEMENTS =
        c => c.ImplementGet(uResolvedAbility.ABILITY, iSelf => iSelf.kRef().kGetRovi(ABILITY));
}