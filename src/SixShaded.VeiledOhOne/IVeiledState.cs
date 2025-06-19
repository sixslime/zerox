namespace SixShaded.VeiledOhOne;

public interface IVeiledState
{
    public IStateFZO Unveiled { get; }
    public Veil Veil { get; }
    public IStateView ViewedFrom(int observer);
}

