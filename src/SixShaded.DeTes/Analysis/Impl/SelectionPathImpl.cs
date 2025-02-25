namespace SixShaded.DeTes.Analysis.Impl;
internal class SelectionPathImpl : IDeTesSelectionPath
{
    public required Kor RootSelectionKorssa { get; init; }
    public required IDeTesResult ResultObject { get; init; }
    public required DomainDataImpl DomainData { get; init; }
    public required int[] ThisSelection { get; init; }

    IDeTesDomainData IDeTesSelectionPath.Domain => DomainData;
    int[] IDeTesSelectionPath.Selection => ThisSelection;
    Kor IDeTesSelectionPath.RootSelectionKorssa => RootSelectionKorssa;
    CriticalPointType IDeTesResult.CriticalPoint => ResultObject.CriticalPoint;
    EDeTesFrame[] IDeTesResult.EvaluationFrames => ResultObject.EvaluationFrames;
    TimeSpan IDeTesResult.TimeTaken => ResultObject.TimeTaken;
}