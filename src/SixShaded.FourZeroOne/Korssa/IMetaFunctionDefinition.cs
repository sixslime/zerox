namespace SixShaded.FourZeroOne.Korssa;

public interface IMetaFunctionDefinition<out R, out M> : IKorssa<M>
    where R : class, Rog
    where M : class, Roggi.Unsafe.IMetaFunction<R>
{
    public IKorssa<R> Korssa { get; }
    public IRoda<>[] Captures { get; }
    public IRoda<>[] ArgAddresses { get; }
    public IRoda<M> SelfAddress { get; }
    public Roggi.Unsafe.IMetaFunction<R> ConstructConcreteMetaFunction(IMemory memory);
}