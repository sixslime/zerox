namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Ability;

public interface uSourceChecks : IConcreteRovetu, uCheckable
{
    public static readonly Rovu<uSourceChecks, Bool> CORRECT_TEAM = new(Axoi.Du, "correct_team");
    public static readonly Rovu<uSourceChecks, Bool> EFFECT_CHECK = new(Axoi.Du, "effect_check");

    public static readonly ImplementationStatement<uSourceChecks> __IMPLEMENTS =
        c =>
            c.ImplementGet(
            PASSED,
            iThis =>
                iThis.kRef()
                    .kGetRovi(CORRECT_TEAM)
                    .kAnd(iThis.kRef().kGetRovi(EFFECT_CHECK)));
}