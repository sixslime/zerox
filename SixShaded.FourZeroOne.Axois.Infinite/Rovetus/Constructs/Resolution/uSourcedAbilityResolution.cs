namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolution;

public interface uSourcedAbilityResolution : IRovetu
{
    public static readonly Rovu<uSourcedAbilityResolution, IRoveggi<Ability.uSourcedAbility>> ABILITY = new(Axoi.Du, "ability");
    public static readonly Rovu<uSourcedAbilityResolution, IRoveggi<Identifier.uUnitIdentifier>> SOURCE = new(Axoi.Du, "source");
    public static readonly Rovu<uSourcedAbilityResolution, IRoveggi<Identifier.uUnitIdentifier>> TARGET = new(Axoi.Du, "target");
}