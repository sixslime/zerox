namespace SixShaded.FourZeroOne.Roveggi;

public class RovedanggiWrapper<R>(IRoveggi<IRovedantu<R>> roveggi) : IRoda<R>
    where R : class, Rog
{
    public IRoveggi<IRovedantu<R>> Roveggi { get; } = roveggi;
    public override bool Equals(object? obj) => obj is RovedanggiWrapper<R> other && Roveggi.Equals(other.Roveggi);
    public override int GetHashCode() => Roveggi.GetHashCode();
    public override string? ToString() => Roveggi.ToString();
    protected bool Equals(RovedanggiWrapper<R> other) => Roveggi.Equals(other.Roveggi);
}