#nullable enable
namespace SixShaded.FourZeroOne.Token.Defined
{
    public abstract record PureValue<R> : Value<R>
        where R : Res
    {
        protected PureValue() : base() { }
        protected sealed override ITask<IOption<R>> Evaluate(ITokenContext _)
        {
            return EvaluatePure().AsSome().ToCompletedITask();
        }
        protected abstract R EvaluatePure();
    }
}