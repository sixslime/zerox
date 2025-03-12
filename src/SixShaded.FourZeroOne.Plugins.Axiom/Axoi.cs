namespace SixShaded.FourZeroOne.Plugins.Axiom;

using Roveggi;
using Roveggi.Defined;

public static class Axoi
{
    public static Axodu Du { get; } =
        new()
        {
            Name = "axiom",
        };

    internal static Rovu<C, R> Rovu<C, R>(string identifier)
        where C : IRovetu
        where R : class, Res =>
        new(Du, identifier);
}