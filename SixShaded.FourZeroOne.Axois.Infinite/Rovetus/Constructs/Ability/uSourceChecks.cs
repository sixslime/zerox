namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Ability;

public interface uSourceChecks : IConcreteRovetu
{
    public static readonly Rovu<uSourceChecks, Bool> CORRECT_TEAM = new(Axoi.Du, "correct_team");
    public static readonly Rovu<uSourceChecks, Bool> EFFECT_CHECK = new(Axoi.Du, "effect_check");
}