namespace SixShaded.FourZeroOne.Roggi.Defined;

public abstract record ConcreteMetaFunction<R>(params Addr[] argAddresses) : NoOp, Unsafe.IMetaFunction<R>
    where R : class, Rog
{
    protected abstract IRoda<Unsafe.IMetaFunction<R>> SelfIdentifierInternal { get; }
    public Addr[] ArgAddresses { get; } = argAddresses;
    public required IMemory CapturedMemory { get; init; }
    public required IKorssa<R> Korssa { get; init; }
    public required Addr[] CapturedVariables { get; init; }
    IRoda<Unsafe.IMetaFunction<R>> Unsafe.IMetaFunction<R>.SelfAddress => SelfIdentifierInternal;
}