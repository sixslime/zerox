namespace SixShaded.FourZeroOne.Rule.Defined;

public record RealizeProxy<R> : Token.Defined.RuntimeHandledFunction<IProxy<R>, R>
    where R : class, Res
{
    public RealizeProxy(IToken<IProxy<R>> proxy) : base(proxy) { }

    protected override FZOSpec.EStateImplemented MakeData(IProxy<R> proxy) =>
        new FZOSpec.EStateImplemented.MetaExecute { Token = proxy.Token, RuleAllows = proxy.ReallowsRule ? proxy.FromRule.Yield() : [] };
}