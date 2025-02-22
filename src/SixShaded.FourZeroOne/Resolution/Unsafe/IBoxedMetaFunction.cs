namespace SixShaded.FourZeroOne.Resolution.Unsafe;

public interface IBoxedMetaFunction<out R> : Res
    where R : class, Res
{
    public IToken<R> Token { get; }
    public IMemoryAddress<IBoxedMetaFunction<R>> SelfIdentifier { get; }
    public IEnumerable<IMemoryAddress<Res>> ArgAddresses { get; }
}