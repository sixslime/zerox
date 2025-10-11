namespace SixShaded.Aleph.Logical;

public record Trackpoint
{
    public required IProgressor Progressor { get; init; }
    public required IPSequence<int[]> Selections { get; init; }
    public required IPSequence<Step> ForwardSteps { get; init; }

    public record Step
    {
        public required IStateFZO State { get; init; }
        public required IResult<EProcessorStep, EProcessorHalt> NextStep { get; init; }
    }
}