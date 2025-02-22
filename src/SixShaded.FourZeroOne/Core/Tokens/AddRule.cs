#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens
{
    public sealed record AddRule : PureValue<r.Instructions.RuleAdd>
    {
        public readonly Rule.Unsafe.IRule<Res> Rule;
        public AddRule(Rule.Unsafe.IRule<Res> rule)
        {
            Rule = rule;
        }
        protected override r.Instructions.RuleAdd EvaluatePure()
        {
            return new() { Rule = Rule };
        }
    }
}