namespace SixShaded.FourZeroOne.Korssa.Defined;

using FZOSpec;

public abstract record MetaFunctionDefinition<R, M>(params Addr[] argAddresses) : Korssa<M>, IMetaFunctionDefinition<R, M>
    where M : class, Roggi.Unsafe.IMetaFunction<R>
    where R : class, Rog
{
    public DynamicAddress<M> SelfAddress { get; } = new();
    public abstract M ConstructConcreteMetaFunction(IMemory memory);
    protected override IResult<ITask<IOption<M>>, EStateImplemented> Resolve(IKorssaContext context, RogOpt[] args) => ConstructConcreteMetaFunction(context.CurrentMemory).AsSome().ToCompletedITask().AsOk(Hint<EStateImplemented>.HINT);
    protected override IOption<string> CustomToString() => $"#{SelfAddress}({string.Join(", ", ArgAddresses.IEnumerable())})::{{{Korssa}}}".AsSome();
    public required Addr[] Captures { get; init; }
    public Addr[] ArgAddresses { get; } = argAddresses;
    public abstract IKorssa<R> Korssa { get; }
    IMemoryAddress<M> IMetaFunctionDefinition<R, M>.SelfAddress => SelfAddress;
    Roggi.Unsafe.IMetaFunction<R> IMetaFunctionDefinition<R, M>.ConstructConcreteMetaFunction(IMemory memory) => ConstructConcreteMetaFunction(memory);
}