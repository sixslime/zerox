namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record Combiner<RArg, ROut> : RegularKorssa<ROut>, IHasCombinerArgs<RArg, ROut>
    where RArg : class, Rog
    where ROut : class, Rog
{
    protected Combiner(IEnumerable<IKorssa<RArg>> korssas) : base(korssas) { }
    public IEnumerable<IKorssa<RArg>> Args => ArgKorssas.Map(x => (IKorssa<RArg>)x);
    protected sealed override ITask<IOption<ROut>> StandardResolve(IKorssaContext runtime, RogOpt[] korssas) => Evaluate(runtime, korssas.Map(x => x.RemapAs(x => (RArg)x)));

    protected abstract ITask<IOption<ROut>> Evaluate(IKorssaContext runtime, IEnumerable<IOption<RArg>> inputs);
}