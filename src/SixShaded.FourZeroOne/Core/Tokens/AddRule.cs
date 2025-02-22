#nullable enable

namespace SixShaded.FourZeroOne.Core.Tokens
{
    public sealed record AddRule : Token.Defined.PureValue<Resolutions.Instructions.RuleAdd>
    {
        public readonly Rule.Unsafe.IRule<Res> Rule;
        public AddRule(Rule.Unsafe.IRule<Res> rule)
        {
            Rule = rule;
        }
        protected override Resolutions.Instructions.RuleAdd EvaluatePure()
        {
            return new() { Rule = Rule };
        }
    }
}