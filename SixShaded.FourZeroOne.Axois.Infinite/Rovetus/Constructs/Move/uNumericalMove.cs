namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;

public interface uNumericalMove : uMove, IConcreteRovetu
{
    public static readonly Rovu<uNumericalMove, NumRange> DISTANCE = new(Axoi.Du, "distance");
    /// <summary>
    /// may be nolla ~ no max.
    /// </summary>
    public static readonly Rovu<uNumericalMove, NumRange> MAX_PER_UNIT = new(Axoi.Du, "max_per_unit");
    /// <summary>
    /// may be nolla ~ not shared, equiv to 1.
    /// </summary>
    public static readonly Rovu<uNumericalMove, Number> SHARED_UNITS = new(Axoi.Du, "shared_units");
    /// <summary>
    /// (spaceChecks) -> (prevSpaace, thisSpace) -> isValidStep <br></br>
    /// runs for each pathing step. <br></br>
    /// default should be (spaceChecks) => ((_, _) => subjectChecks.AllTrue)
    /// </summary>
    public static readonly Rovu<uNumericalMove, MetaFunction<IRoveggi<uSpaceChecks>, MetaFunction<IRoveggi<Identifier.uHexIdentifier>, IRoveggi<Identifier.uHexIdentifier>, Bool>>> PATH_SELECTOR = new(Axoi.Du, "path_selector");
}
