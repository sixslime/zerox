namespace SixShaded.FourZeroOne.Roveggi;

public static class Extensions
{
    internal static RovedanggiWrapper<R> MemWrapped<R>(this IRoveggi<IRovedantu<R>> roveggi)
        where R : class, Rog =>
        new(roveggi);
}