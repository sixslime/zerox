namespace SixShaded.FourZeroOne.Roggi.Defined;

public abstract record Roggi : Rog
{
    public abstract IEnumerable<IInstruction> Instructions { get; }
}