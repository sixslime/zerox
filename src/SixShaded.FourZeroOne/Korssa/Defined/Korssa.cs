namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record Korssa<R> : IKorssa<R> where R : class, Rog
{
    public Korssa(params Kor[] args)
    {
        ArgKorssas = args;
    }

    public Korssa(IEnumerable<Kor> args) : this(args.ToArray()) { }
    public Kor[] ArgKorssas { get; }
    public IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> ResolveWith(FZOSpec.IProcessorFZO.IKorssaContext korssaContext, RogOpt[] args) => Resolve(korssaContext.ToHandle(), args);
    protected abstract IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext runtime, RogOpt[] args);
    protected virtual IOption<string> CustomToString() => new None<string>();

    public sealed override string ToString() =>
        CustomToString()
            .OrElse(() => $"{GetType().Name}( {ArgKorssas.AccumulateInto("", (msg, arg) => $"{msg}{arg} ")})");
}