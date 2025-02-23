namespace SixShaded.DeTes.Realization;

public interface IDeTesFZOSupplier
{
    public IStateFZO UnitializedState { get; }
    public IProcessorFZO Processor { get; }
}