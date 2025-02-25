namespace SixShaded.FourZeroOne.Core;

public static class Axoi
{
    public static Axodu Du = new() { Name = "core" };
    internal static Korvessa.Defined.Korvedu Korvedu(string identifier) => new(Du, identifier);

    internal static Roggi.Defined.Rovu<C, R> Rovu<C, R>(string identifier)
        where C : IRoveggitu
        where R : class, Rog =>
        new(Du, identifier);
}