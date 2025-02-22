
namespace SixShaded.FourZeroOne.FZOSpec;

public abstract record EStateImplemented
{
    public sealed record MetaExecute : EStateImplemented
    {
        public required Tok Token { get; init; }
        public IEnumerable<ITiple<Addr, ResOpt>> ObjectWrites { get; init; } = [];
        public IEnumerable<RuleID> RuleMutes { get; init; } = [];
        public IEnumerable<RuleID> RuleAllows { get; init; } = [];
    }
}