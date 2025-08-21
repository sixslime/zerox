namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;

using u.Identifier;
public interface uSpaceChecks : IConcreteRovetu, uCheckable
{
    public static readonly Rovu<uSpaceChecks, Bool> WALL = new(Axoi.Du, "wall");
    public static readonly Rovu<uSpaceChecks, Bool> PROTECTED_BASE = new(Axoi.Du, "protected_base");
    /// <summary>
    /// nolla = true, if contains unit that unit is blocking (false).
    /// </summary>
    public static readonly Rovu<uSpaceChecks, IRoveggi<uUnitIdentifier>> UNIT = new(Axoi.Du, "unit");

    public static readonly ImplementationStatement<uSpaceChecks> __IMPLEMENTS =
        c =>
            c.ImplementGet(
            PASSED,
            iThis =>
                iThis.kRef()
                    .kGetRovi(WALL)
                    .kAnd(iThis.kRef().kGetRovi(PROTECTED_BASE))
                    .kAnd(iThis.kRef().kGetRovi(UNIT).kExists()));
}