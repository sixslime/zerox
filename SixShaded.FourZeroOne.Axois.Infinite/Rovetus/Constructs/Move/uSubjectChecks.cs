namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;

using u.Identifier;
public interface uSubjectChecks : IConcreteRovetu
{
    public static readonly Rovu<uSubjectChecks, Bool> EFFECT_CHECK = new(Axoi.Du, "effect_check");
}