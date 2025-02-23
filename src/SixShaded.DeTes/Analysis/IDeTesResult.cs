namespace SixShaded.DeTes.Analysis;

public interface IDeTesResult
{
    public TimeSpan TimeTaken { get; }
    public IResult<IResult<EProcessorHalt, Exception>, IDeTesSelectionPath[]> CriticalPoint { get; }
    public EDeTesFrame[] EvaluationFrames { get; }
}