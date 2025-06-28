namespace SixShaded.FourZeroOne.Roveggi;

public static class Extensions
{
    internal static RovedanggiWrapper<R> MemWrapped<R>(this IRoveggi<Rovedantu<R>> roveggi)
        where R : class, Rog =>
        new(roveggi);
    internal static VaroveggiWrapper<C, R> MemWrapped<C, R>(this IRoveggi<IVarovu<C, R>> roveggi)
        where C : IRovetu
        where R : class, Rog => new (roveggi);
}