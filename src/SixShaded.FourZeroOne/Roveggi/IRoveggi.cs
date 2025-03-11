namespace SixShaded.FourZeroOne.Roveggi;

using SixShaded.FourZeroOne.Roveggi.Unsafe;

// pretty silly bro im not going even to even lie even.
public interface IRoveggi<out C> : Rog where C : IRovetu
{
    public IEnumerable<ITiple<IRovu, Rog>> ComponentsUnsafe { get; }
    public IRoveggi<C> WithComponent<R>(IRovu<C, R> identifier, R data) where R : class, Rog;
    public IRoveggi<C> WithComponentsUnsafe(IEnumerable<ITiple<IRovu<C>, Rog>> components);
    public IRoveggi<C> WithoutComponents(IEnumerable<IRovu<C>> addresses);
    public IOption<R> GetComponent<R>(IRovu<C, R> address) where R : class, Rog;
}