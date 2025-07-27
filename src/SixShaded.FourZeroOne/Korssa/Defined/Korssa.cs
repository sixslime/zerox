namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record Korssa<R> : IKorssa<R>
    where R : class, Rog
{
    protected Korssa(params Kor[] argKorssas)
    {
        ArgKorssas = argKorssas;
    }

    protected Korssa(IEnumerable<Kor> argKorssas)
    {
        ArgKorssas = argKorssas.ToArray();
    }
    public sealed override string ToString() =>
        CustomToString()
            .OrElse(() => $"{GetType().Name}( {ArgKorssas.AccumulateInto("", (msg, arg) => $"{msg}{arg} ")})");

    protected abstract IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext runtime, RogOpt[] args);
    protected virtual IOption<string> CustomToString() => new None<string>();
    public Kor[] ArgKorssas { get; }
    public IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> ResolveWith(FZOSpec.IProcessorFZO.IKorssaContext context, RogOpt[] args) => Resolve(context.ToHandle(), args);
}