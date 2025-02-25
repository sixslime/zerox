namespace SixShaded.FourZeroOne.FZOSpec;

public abstract record EProcessorHalt
{
    public required IStateFZO HaltingState { get; init; }

    public sealed record InvalidState : EProcessorHalt
    { }

    public sealed record Completed : EProcessorHalt
    {
        public required ResOpt Resolution { get; init; }
    }
}