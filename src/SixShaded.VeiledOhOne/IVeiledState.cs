namespace SixShaded.VeiledOhOne;

public interface IVeiledState
{
    public IStateFZO Unveiled { get; }
    public IStateView ViewedFrom(int observer);
}

