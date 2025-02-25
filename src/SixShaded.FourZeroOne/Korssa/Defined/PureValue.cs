namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record PureValue<R> : Value<R>
    where R : class, Rog
{
    protected sealed override ITask<IOption<R>> Evaluate(IKorssaContext _) => EvaluatePure().AsSome().ToCompletedITask();
    protected abstract R EvaluatePure();
}