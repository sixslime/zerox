
#nullable enable
namespace SixShaded.FourZeroOne.FZOSpec
{
    public abstract record EProcessorStep
    {
        public sealed record TokenMutate : EProcessorStep
        {
            public required ETokenMutation Mutation { get; init; }
        }
        public sealed record Resolve : EProcessorStep
        {
            public required IResult<ResOpt, EStateImplemented> Resolution { get; init; }
        }
        public sealed record PushOperation : EProcessorStep
        {
            public required Tok OperationToken { get; init; }
        }
    }
}