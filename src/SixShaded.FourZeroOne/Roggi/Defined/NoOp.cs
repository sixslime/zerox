namespace SixShaded.FourZeroOne.Roggi.Defined;

public abstract record NoOp : Roggi
{
    public override IEnumerable<IInstruction> Instructions => [];
}