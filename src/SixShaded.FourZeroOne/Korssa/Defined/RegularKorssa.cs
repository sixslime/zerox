namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record RegularKorssa<R> : Korssa<R> where R : class, Rog
{
    public RegularKorssa(params Kor[] args) : base(args) { }
    public RegularKorssa(IEnumerable<Kor> args) : base(args) { }
    protected abstract ITask<IOption<R>> StandardResolve(IKorssaContext runtime, RogOpt[] args);
    protected override IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext runtime, RogOpt[] args) => new Ok<ITask<IOption<R>>, FZOSpec.EStateImplemented>(StandardResolve(runtime, args));
}