namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;

public interface uResolvedUnsourcedAbility : uResolved, IConcreteRovetu
{
    public static readonly Rovu<uResolvedUnsourcedAbility, IRoveggi<Ability.uUnsourcedAbility>> ABILITY = new(Axoi.Du, "ability");
    public static readonly ImplementationStatement<uResolvedUnsourcedAbility> __IMPLEMENTS =
        c =>
            c.ImplementGet(
            INSTRUCTIONS,
            iSelf =>
                iSelf.kRef()
                    .kGetRovi(ABILITY)
                    .kGetRovi(Ability.uUnsourcedAbility.ACTION)
                    .kExecute());
}