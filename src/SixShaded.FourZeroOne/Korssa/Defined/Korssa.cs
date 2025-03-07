namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record Korssa<R>(params Kor[] argKorssas) : IKorssa<R> where R : class, Rog
{

    protected Korssa(IEnumerable<Kor> args) : this(args.ToArray()) { }
    public Kor[] ArgKorssas { get; } = argKorssas;
    public IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> ResolveWith(FZOSpec.IProcessorFZO.IKorssaContext context, RogOpt[] args) => Resolve(context.ToHandle(), args);
    protected abstract IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext runtime, RogOpt[] args);
    protected virtual IOption<string> CustomToString() => new None<string>();

    public sealed override string ToString() =>
        CustomToString()
            .OrElse(() => $"{GetType().Name}( {ArgKorssas.AccumulateInto("", (msg, arg) => $"{msg}{arg} ")})");
}