namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;

using Identifier;

public interface uMove : IRovetu
{
    public static readonly Rovu<uMove, MetaFunction<Rog>> ENVIRONMENT_PREMOD = new("environment_premod");
    public static readonly Rovu<uMove, MetaFunction<IMulti<IRoveggi<uUnitIdentifier>>>> SUBJECT_COLLECTOR = new("subject_collector");
    /// <summary>
    ///     (subjectChecks) -> (unit) -> isValidSubject <br></br>
    ///     selects from subjects retrieved from SUBJECT_COLLECTOR <br></br>
    ///     default should be (subjectChecks) => ((_) => subjectChecks.AllTrue)
    /// </summary>
    public static readonly Rovu<uMove, MetaFunction<IRoveggi<uSubjectChecks>, MetaFunction<IRoveggi<uUnitIdentifier>, Bool>>> SUBJECT_SELECTOR = new("subject_selector");
    public static readonly Rovu<uMove, MetaFunction<IRoveggi<uSpaceChecks>, MetaFunction<IRoveggi<uHexIdentifier>, Bool>>> DESTINATION_SELECTOR = new("destination_selector");
}