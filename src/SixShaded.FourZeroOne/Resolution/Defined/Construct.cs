#nullable enable
namespace SixShaded.FourZeroOne.Resolution.Defined
{
    public abstract record Construct : Res
    {
        public abstract IEnumerable<IInstruction> Instructions { get; }
    }
}
