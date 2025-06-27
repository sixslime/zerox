namespace SixShaded.FourZeroOne.Roveggi;

public class RovunggiWrapper<C, R>(IRoveggi<IRovundantu<C, R>> roveggi) : IRovu<C, R>
    where C : IRovetu
    where R : class, Rog
{
    public IRoveggi<IRovundantu<C, R>> Roveggi { get; } = roveggi;
    public override bool Equals(object? obj) => obj is RovunggiWrapper<C, R> other && Roveggi.Equals(other.Roveggi);
    public override int GetHashCode() => Roveggi.GetHashCode();
    public override string? ToString() => Roveggi.ToString();
    protected bool Equals(RovunggiWrapper<C, R> other) => Roveggi.Equals(other.Roveggi);
}