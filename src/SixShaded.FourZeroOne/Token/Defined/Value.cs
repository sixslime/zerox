namespace SixShaded.FourZeroOne.Token.Defined;

public abstract record Value<R> : StandardToken<R>, IHasNoArgs<R>
    where R : class, Res
{
    protected sealed override ITask<IOption<R>> StandardResolve(ITokenContext runtime, IOption<Res>[] _) => Evaluate(runtime);
    protected abstract ITask<IOption<R>> Evaluate(ITokenContext runtime);
}