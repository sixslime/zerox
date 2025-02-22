#nullable enable
namespace SixShaded.FourZeroOne.Token.Defined
{
    public abstract record RuntimeHandledValue<RVal> : TokenBehavior<RVal>,
        IHasNoArgs<RVal>
        where RVal : class, Res
    {
        protected sealed override IResult<ITask<IOption<RVal>>, FZOSpec.EStateImplemented> Resolve(ITokenContext _, IOption<Res>[] args)
        {
            return new Err<ITask<IOption<RVal>>, FZOSpec.EStateImplemented>(MakeData());
        }
        protected abstract FZOSpec.EStateImplemented MakeData();
        protected RuntimeHandledValue() : base() { }
    }
}