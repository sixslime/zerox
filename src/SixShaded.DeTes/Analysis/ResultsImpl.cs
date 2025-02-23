namespace SixShaded.DeTes.Analysis;

internal class ResultImpl : IDeTesResult
{
    public required TimeSpan TimeTaken { get; init; }
    public required CriticalPointType CriticalPoint { get; init; }
    public required EDeTesFrame[] EvaluationFrames { get; init; }
}