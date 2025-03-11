namespace SixShaded.FourZeroOne.Core;

using Roveggi;
using Roveggi.Defined;

public static class Axoi
{
    public static Axodu Du =
        new()
        {
            Name = "core",
        };
    internal static Korvessa.Defined.Korvedu Korvedu(string identifier) => new(Du, identifier);

    internal static Rovu<C, R> Rovu<C, R>(string identifier)
        where C : IRovetu
        where R : class, Rog =>
        new(Du, identifier);
}