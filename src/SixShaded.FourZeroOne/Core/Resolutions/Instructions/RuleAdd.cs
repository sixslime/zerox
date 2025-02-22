#nullable enable
namespace SixShaded.FourZeroOne.Core.Resolutions.Instructions
{
    public sealed record RuleAdd : Resolution.Defined.Instruction
    {
        public required Rule.Unsafe.IRule<Res> Rule { get; init; }
        public override IMemory TransformMemory(IMemory state)
        {
            return state.WithRules([Rule]);
        }
        public override string ToString()
        {
            return $"<?>+{Rule}";
        }
    }
}