#nullable enable
namespace SixShaded.FourZeroOne.Rule.Defined
{
    public abstract record ProxyBehavior<R> : Resolution.NoOp, IProxy<R>
        where R : class, Res
    {
        public required IToken<R> Token { get; init; }
        public required RuleID FromRule { get; init; }
        public abstract bool ReallowsRule { get; }
    }
}
