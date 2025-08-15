namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Ability;

public interface uTargetChecks : IConcreteRovetu
{
    public static readonly Rovu<uTargetChecks, Bool> CORRECT_TEAM = new(Axoi.Du, "correct_team");
    public static readonly Rovu<uTargetChecks, Bool> LINE_OF_SIGHT = new(Axoi.Du, "line_of_sight");
    public static readonly Rovu<uTargetChecks, Bool> HIT_AREA = new(Axoi.Du, "hit_area");
    public static readonly Rovu<uTargetChecks, Bool> EFFECT_CHECK = new(Axoi.Du, "effect_check");
}