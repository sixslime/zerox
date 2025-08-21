namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;

public interface uNumericalMove : uMove, IConcreteRovetu
{
    public static readonly Rovu<uNumericalMove, NumRange> DISTANCE = new(Axoi.Du, "distance");
    /// <summary>
    /// may be nolla ~ equiv to infinity.
    /// </summary>
    public static readonly Rovu<uNumericalMove, Number> MAX_DISTANCE_PER_SUBJECT = new(Axoi.Du, "max_distance_per_subject");
    /// <summary>
    /// max amount of subjects this move can be shared between, out of the available subjects. <br></br>
    /// may be nolla ~ equiv to infinity.
    /// </summary>
    public static readonly Rovu<uNumericalMove, Number> MAX_SUBJECTS = new(Axoi.Du, "max_subjects");
    /// <summary>
    /// (spaceChecks) -> (prevSpace, thisSpace) -> isValidStep <br></br>
    /// runs for each pathing step. <br></br>
    /// default should be (spaceChecks) => ((_, _) => subjectChecks.AllTrue)
    /// </summary>
    public static readonly Rovu<uNumericalMove, MetaFunction<IRoveggi<uSpaceChecks>, MetaFunction<IRoveggi<Identifier.uHexIdentifier>, IRoveggi<Identifier.uHexIdentifier>, Bool>>> PATH_SELECTOR = new(Axoi.Du, "path_selector");
}
