#nullable enable
namespace SixShaded.FourZeroOne.Token.Defined
{
    public abstract record PureCombiner<RArg, ROut> : Combiner<RArg, ROut>
        where RArg : class, ResObj
        where ROut : class, ResObj
    {
        protected abstract ROut EvaluatePure(IEnumerable<RArg> inputs);
        protected PureCombiner(IEnumerable<IToken<RArg>> tokens) : base(tokens) { }
        protected sealed override ITask<IOption<ROut>> Evaluate(ITokenContext _, IEnumerable<IOption<RArg>> inputs) => EvaluatePure(inputs.Where(x => x.IsSome()).Map(x => x.Unwrap())).AsSome().ToCompletedITask();
    }
}