namespace SixShaded.FourZeroOne.Roggi;

internal record MemoryRoveggiWrapper<R>(IRoveggi<IMemoryRovetu<R>> Roveggi) : IMemoryAddress<R>
    where R : class, Rog
{
    public override string? ToString() => Roveggi.ToString();
}
