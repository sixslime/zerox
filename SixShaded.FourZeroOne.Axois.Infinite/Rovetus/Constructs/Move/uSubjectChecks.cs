namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;

using Identifier;

public interface uSubjectChecks : IConcreteRovetu, uCheckable
{
    public static readonly Rovu<uSubjectChecks, Bool> EFFECT_CHECK = new(Axoi.Du, "effect_check");
    public static readonly ImplementationStatement<uSubjectChecks> __IMPLEMENTS =
        c =>
            c.ImplementGet(
            PASSED,
            iThis =>
                iThis.kRef()
                    .kGetRovi(EFFECT_CHECK));
}