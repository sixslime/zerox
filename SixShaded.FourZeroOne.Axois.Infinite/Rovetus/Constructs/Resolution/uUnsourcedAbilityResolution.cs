namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolution;

public interface uUnsourcedAbilityResolution : uResolution, IConcreteRovetu
{
    public static readonly Rovu<uUnsourcedAbilityResolution, IRoveggi<Ability.uUnsourcedAbility>> ABILITY = new(Axoi.Du, "ability");
    public static readonly ImplementationStatement<uUnsourcedAbilityResolution> __IMPLEMENTS =
        c =>
            c.ImplementGet(
            INSTRUCTIONS,
            iSelf =>
                iSelf.kRef()
                    .kGetRovi(ABILITY)
                    .kGetRovi(Ability.uUnsourcedAbility.ACTION)
                    .kExecute());
}