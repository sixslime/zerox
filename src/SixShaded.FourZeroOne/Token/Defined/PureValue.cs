namespace SixShaded.FourZeroOne.Token.Defined;

public abstract record PureValue<R> : Value<R>
    where R : class, Res
{
    protected sealed override ITask<IOption<R>> Evaluate(ITokenContext _) => EvaluatePure().AsSome().ToCompletedITask();
    protected abstract R EvaluatePure();
}