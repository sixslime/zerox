namespace SixShaded.FourZeroOne.Korssa.Defined;

using FZOSpec;

public abstract record MetaFunctionDefinition<R, M>(params IRoda<>[] argAddresses) : Korssa<M>, IMetaFunctionDefinition<R, M>
    where M : class, Roggi.Unsafe.IMetaFunction<R>
    where R : class, Rog
{
    public DynamicRoda<M> SelfRoda { get; } = new();
    public abstract M ConstructConcreteMetaFunction(IMemory memory);
    protected override IResult<ITask<IOption<M>>, EStateImplemented> Resolve(IKorssaContext context, RogOpt[] args) => ConstructConcreteMetaFunction(context.CurrentMemory).AsSome().ToCompletedITask().AsOk(Hint<EStateImplemented>.HINT);
    protected override IOption<string> CustomToString() => $"#{SelfRoda}({string.Join(", ", ArgAddresses.IEnumerable())})::{{{Korssa}}}".AsSome();
    public required IRoda<>[] Captures { get; init; }
    public IRoda<>[] ArgAddresses { get; } = argAddresses;
    public abstract IKorssa<R> Korssa { get; }
    IRoda<M> IMetaFunctionDefinition<R, M>.SelfAddress => SelfRoda;
    Roggi.Unsafe.IMetaFunction<R> IMetaFunctionDefinition<R, M>.ConstructConcreteMetaFunction(IMemory memory) => ConstructConcreteMetaFunction(memory);
}