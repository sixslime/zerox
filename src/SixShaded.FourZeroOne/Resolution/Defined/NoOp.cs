#nullable enable
namespace SixShaded.FourZeroOne.Resolution.Defined
{
    public abstract record NoOp : Construct
    {
        public override IEnumerable<IInstruction> Instructions => [];
    }
}
