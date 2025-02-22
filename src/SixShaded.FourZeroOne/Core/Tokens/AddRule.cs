#nullable enable
namespace FourZeroOne.Core.Tokens
{
    public sealed record AddRule : PureValue<r.Instructions.RuleAdd>
    {
        public readonly Rule.Unsafe.IRule<ResObj> Rule;
        public AddRule(Rule.Unsafe.IRule<ResObj> rule)
        {
            Rule = rule;
        }
        protected override r.Instructions.RuleAdd EvaluatePure()
        {
            return new() { Rule = Rule };
        }
    }
}