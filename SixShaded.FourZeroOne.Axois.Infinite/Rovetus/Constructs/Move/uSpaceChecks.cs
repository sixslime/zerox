namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;

using u.Identifier;
public interface uSpaceChecks : IConcreteRovetu
{
    public static readonly Rovu<uSpaceChecks, Bool> WALL = new(Axoi.Du, "wall");
    public static readonly Rovu<uSpaceChecks, Bool> PROTECTED_BASE = new(Axoi.Du, "protected_base");
    /// <summary>
    /// nolla = true, if contains unit that unit is blocking (false).
    /// </summary>
    public static readonly Rovu<uSpaceChecks, IRoveggi<uUnitIdentifier>> UNIT = new(Axoi.Du, "unit");
}