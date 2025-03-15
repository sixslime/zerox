namespace SixShaded.FourZeroOne.Roggi.Unsafe;

public interface IMetaFunction<out R> : Rog
    where R : class, Rog
{
    public IKorssa<R> Korssa { get; }
    public IRoda<IMetaFunction<R>> SelfAddress { get; }
    public Addr[] ArgAddresses { get; }
    public Addr[] CapturedVariables { get; }
    public IMemory CapturedMemory { get; }
}