namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

public interface uWinChecks : IConcreteRovetu, uCheckable
{
    public static readonly Rovu<uWinChecks, Bool> CONTROL = new(Axoi.Du, "control");
    public static readonly Rovu<uWinChecks, Bool> LAST_ALIVE = new(Axoi.Du, "last_alive");

    public static readonly ImplementationStatement<uWinChecks> __IMPLEMENTS =
        c =>
            c.ImplementGet(
            PASSED,
            iThis =>
                iThis.kRef()
                    .kGetRovi(CONTROL)
                    .kOr(iThis.kRef().kGetRovi(LAST_ALIVE)));
}