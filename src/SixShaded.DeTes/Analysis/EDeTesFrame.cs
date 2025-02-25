namespace SixShaded.DeTes.Analysis;

public abstract record EDeTesFrame
{
    public required IStateFZO PreState { get; init; }
    public required Kor Origin { get; init; }

    public sealed record KorssaPrep : EDeTesFrame
    {
        public required EProcessorStep.KorssaMutate NextStep { get; init; }
    }

    public sealed record PushOperation : EDeTesFrame
    {
        public required EProcessorStep.PushOperation NextStep { get; init; }
    }

    public sealed record Resolve : EDeTesFrame
    {
        public required EProcessorStep.Resolve NextStep { get; init; }
        public required IDeTesOnResolveAssertions Assertions { get; init; }
    }

    public sealed record Complete : EDeTesFrame
    {
        public required EProcessorHalt.Completed CompletionHalt { get; init; }
        public required IDeTesOnResolveAssertions Assertions { get; init; }
    }
}