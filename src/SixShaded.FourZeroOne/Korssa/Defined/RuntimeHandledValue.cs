namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record RuntimeHandledValue<RVal> : Korssa<RVal>,
    IHasNoArgs<RVal>
    where RVal : class, Rog
{
    protected sealed override IResult<ITask<IOption<RVal>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext _, RogOpt[] args) => new Err<ITask<IOption<RVal>>, FZOSpec.EStateImplemented>(MakeData());
    protected abstract FZOSpec.EStateImplemented MakeData();
}