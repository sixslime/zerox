namespace SixShaded.FourZeroOne.FZOSpec;

public abstract record EStateImplemented
{
    public sealed record MetaExecute : EStateImplemented
    {
        public required Kor Korssa { get; init; }
        public IEnumerable<ITiple<Addr, RogOpt>> ObjectWrites { get; init; } = [];
        public IEnumerable<MellsanoID> MellsanoMutes { get; init; } = [];
        public IEnumerable<MellsanoID> MellsanoAllows { get; init; } = [];
    }
}