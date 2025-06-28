namespace SixShaded.FourZeroOne.Roveggi;

public class VaroveggiWrapper<C, R>(IRoveggi<IVarovu<C, R>> roveggi) : IRovu<C, R>
    where C : IRovetu
    where R : class, Rog
{
    public IRoveggi<IVarovu<C, R>> Roveggi { get; } = roveggi;
    public override bool Equals(object? obj) => obj is VaroveggiWrapper<C, R> other && Roveggi.Equals(other.Roveggi);
    public override int GetHashCode() => Roveggi.GetHashCode();
    public override string? ToString() => Roveggi.ToString();
    protected bool Equals(VaroveggiWrapper<C, R> other) => Roveggi.Equals(other.Roveggi);
}