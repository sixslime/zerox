namespace SixShaded.FourZeroOne.Roveggi;
internal record MemoryRoveggiWrapper<R>(IRoveggi<IMemoryRovetu<R>> Roveggi) : IMemoryAddress<R>
    where R : class, Rog
{
    public override string? ToString() => Roveggi.ToString();
}
