namespace SixShaded.DeTes.Analysis;

internal class SelectionPathImpl : IDeTesSelectionPath
{
    public required Tok RootSelectionToken { get; init; }
    public required IDeTesResult ResultObject { get; init; }
    public required DomainDataImpl DomainData { get; init; }
    public required int[] ThisSelection { get; init; }

    IDeTesDomainData IDeTesSelectionPath.Domain => DomainData;
    int[] IDeTesSelectionPath.Selection => ThisSelection;
    Tok IDeTesSelectionPath.RootSelectionToken => RootSelectionToken;
    CriticalPointType IDeTesResult.CriticalPoint => ResultObject.CriticalPoint;
    EDeTesFrame[] IDeTesResult.EvaluationFrames => ResultObject.EvaluationFrames;
    TimeSpan IDeTesResult.TimeTaken => ResultObject.TimeTaken;
}