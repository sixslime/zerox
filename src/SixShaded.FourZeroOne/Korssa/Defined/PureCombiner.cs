namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record PureCombiner<RArg, ROut> : Combiner<RArg, ROut>
    where RArg : class, Rog
    where ROut : class, Rog
{
    protected PureCombiner(params IKorssa<RArg>[] korssas) : base(korssas)
    { }

    protected PureCombiner(IEnumerable<IKorssa<RArg>> korssas) : base(korssas)
    { }

    protected abstract ROut EvaluatePure(IEnumerable<IOption<RArg>> inputs);
    protected sealed override ITask<IOption<ROut>> Evaluate(IKorssaContext _, IEnumerable<IOption<RArg>> inputs) => EvaluatePure(inputs).AsSome().ToCompletedITask();
}