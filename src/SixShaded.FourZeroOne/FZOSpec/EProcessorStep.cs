namespace SixShaded.FourZeroOne.FZOSpec;

public abstract record EProcessorStep
{
    public sealed record KorssaMutate : EProcessorStep
    {
        public required EKorssaMutation Mutation { get; init; }
    }

    public sealed record Resolve : EProcessorStep
    {
        public required IResult<RogOpt, EStateImplemented> Roggi { get; init; }
    }

    public sealed record PushOperation : EProcessorStep
    {
        public required Kor OperationKorssa { get; init; }
    }
}