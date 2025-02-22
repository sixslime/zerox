#nullable enable
namespace SixShaded.FourZeroOne.Rule.Defined
{
    public record RealizeProxy<R> : RuntimeHandledFunction<IProxy<R>, R>
        where R : class, Res
    {
        public RealizeProxy(IToken<IProxy<R>> proxy) : base(proxy) { }
        protected override EStateImplemented MakeData(IProxy<R> proxy)
        {
            return new EStateImplemented.MetaExecute
            {
                Token = proxy.Token,
                RuleAllows = proxy.ReallowsRule ? proxy.FromRule.Yield() : [],
            };
        }
    }
}
