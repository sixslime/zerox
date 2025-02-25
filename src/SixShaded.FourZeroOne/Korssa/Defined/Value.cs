namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record Value<R> : RegularKorssa<R>, IHasNoArgs<R>
    where R : class, Rog
{
    protected sealed override ITask<IOption<R>> StandardResolve(IKorssaContext runtime, RogOpt[] _) => Evaluate(runtime);
    protected abstract ITask<IOption<R>> Evaluate(IKorssaContext runtime);
}