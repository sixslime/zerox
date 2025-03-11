namespace SixShaded.FourZeroOne.Roveggi;
public static class Extensions
{
    internal static MemoryRoveggiWrapper<R> MemWrapped<R>(this IRoveggi<IMemoryRovetu<R>> roveggi)
        where R : class, Rog
    {
        return new(roveggi);
    }
}