namespace SixShaded.DeTes.Analysis;

public interface IDeTesResult
{
    public TimeSpan TimeTaken { get; }
    public CriticalPointType CriticalPoint { get; }
    public EDeTesFrame[] EvaluationFrames { get; }
}