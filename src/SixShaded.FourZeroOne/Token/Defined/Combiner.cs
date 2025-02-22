#nullable enable
namespace SixShaded.FourZeroOne.Token.Defined
{
    public abstract record Combiner<RArg, ROut> : StandardToken<ROut>, IHasCombinerArgs<RArg, ROut>
        where RArg : class, ResObj
        where ROut : class, ResObj
    {
        public IEnumerable<IToken<RArg>> Args => ArgTokens.Map(x => (IToken<RArg>)x);
        protected sealed override ITask<IOption<ROut>> StandardResolve(ITokenContext runtime, IOption<ResObj>[] tokens)
        {
            return Evaluate(runtime, tokens.Map(x => x.RemapAs(x => (RArg)x)));
        }

        protected abstract ITask<IOption<ROut>> Evaluate(ITokenContext runtime, IEnumerable<IOption<RArg>> inputs);
        protected Combiner(IEnumerable<IToken<RArg>> tokens) : base(tokens) { }
    }
}