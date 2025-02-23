namespace SixShaded.DeTes;
public record DeTesFZOSupplier : IDeTesFZOSupplier
{
    public required IStateFZO UnitializedState { get; init; }
    public required IProcessorFZO Processor { get; init; }
}
