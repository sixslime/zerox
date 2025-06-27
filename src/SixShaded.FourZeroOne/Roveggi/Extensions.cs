namespace SixShaded.FourZeroOne.Roveggi;

public static class Extensions
{
    internal static RovedanggiWrapper<R> GgiWrapped<R>(this IRoveggi<Rovedantu<R>> roveggi)
        where R : class, Rog =>
        new(roveggi);
    internal static RovunggiWrapper<C, R> GgiWrapped<C, R>(this IRoveggi<IRovundantu<C, R>> roveggi)
        where C : IRovetu
        where R : class, Rog => new (roveggi);
}