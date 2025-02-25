namespace SixShaded.FourZeroOne.Roggi.Unsafe;

public interface IBoxedMetaFunction<out R> : Rog
    where R : class, Rog
{
    public IKorssa<R> Korssa { get; }
    public IMemoryAddress<IBoxedMetaFunction<R>> SelfIdentifier { get; }
    public IEnumerable<IMemoryAddress<Rog>> ArgAddresses { get; }
}