namespace SixShaded.FourZeroOne.Token.Defined;

public abstract record Combiner<RArg, ROut> : StandardToken<ROut>, IHasCombinerArgs<RArg, ROut>
    where RArg : class, Res
    where ROut : class, Res
{
    protected Combiner(IEnumerable<IToken<RArg>> tokens) : base(tokens) { }
    public IEnumerable<IToken<RArg>> Args => ArgTokens.Map(x => (IToken<RArg>)x);
    protected sealed override ITask<IOption<ROut>> StandardResolve(ITokenContext runtime, IOption<Res>[] tokens) => Evaluate(runtime, tokens.Map(x => x.RemapAs(x => (RArg)x)));

    protected abstract ITask<IOption<ROut>> Evaluate(ITokenContext runtime, IEnumerable<IOption<RArg>> inputs);
}