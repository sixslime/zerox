namespace SixShaded.FourZeroOne.Roveggi;

public class MemoryRoveggiWrapper<R>(IRoveggi<IMemoryRovetu<R>> roveggi) : IMemoryAddress<R>
    where R : class, Rog
{
    public IRoveggi<IMemoryRovetu<R>> Roveggi { get; } = roveggi;
    public override bool Equals(object? obj) => obj is MemoryRoveggiWrapper<R> other && Roveggi.Equals(other.Roveggi);
    public override int GetHashCode() => Roveggi.GetHashCode();
    public override string? ToString() => Roveggi.ToString();
    protected bool Equals(MemoryRoveggiWrapper<R> other) => Roveggi.Equals(other.Roveggi);
}