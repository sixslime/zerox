namespace SixShaded.FourZeroOne.Roveggi;

public static class Extensions
{
    internal static RovedanggiWrapper<R> MemWrapped<R>(this IRoveggi<Rovedantu<R>> roveggi)
        where R : class, Rog =>
        new(roveggi);
}