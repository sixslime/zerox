namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record PureCombiner<RArg, ROut> : Combiner<RArg, ROut>
    where RArg : class, Rog
    where ROut : class, Rog
{
    protected PureCombiner(IEnumerable<IKorssa<RArg>> korssas) : base(korssas) { }
    protected abstract ROut EvaluatePure(IEnumerable<RArg> inputs);
    protected sealed override ITask<IOption<ROut>> Evaluate(IKorssaContext _, IEnumerable<IOption<RArg>> inputs) => EvaluatePure(inputs.Where(x => x.IsSome()).Map(x => x.Unwrap())).AsSome().ToCompletedITask();
}