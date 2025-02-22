#nullable enable
namespace SixShaded.FourZeroOne.Token.Defined
{
    public abstract record Value<R> : StandardToken<R>, IHasNoArgs<R>
        where R : Res
    {
        protected sealed override ITask<IOption<R>> StandardResolve(ITokenContext runtime, IOption<Res>[] _)
        {
            return Evaluate(runtime);
        }
        protected Value() : base() { }
        protected abstract ITask<IOption<R>> Evaluate(ITokenContext runtime);
    }
}