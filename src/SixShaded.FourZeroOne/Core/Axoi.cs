namespace SixShaded.FourZeroOne.Core;

using Roveggi;
using Roveggi.Defined;

public class Axoi : FourZeroOne.Axoi
{
    public static Axodu Du =
        new()
        {
            Name = "core",
        };
    private Axoi()
    { }


    internal static Rovu<C, R> Rovu<C, R>(string identifier)
        where C : IRovetu
        where R : class, Rog =>
        new(Du, identifier);
}