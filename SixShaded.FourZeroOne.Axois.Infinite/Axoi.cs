namespace SixShaded.FourZeroOne.Axois.Infinite;

public static class Axoi
{
    public static Axodu Du =
        new()
        {
            Name = "infinite",
        };
    internal static Korvedu Korvedu(string identifier) => new(Du, identifier);

    internal static Rovu<C, R> Rovu<C, R>(string identifier)
        where C : IRovetu
        where R : class, Rog =>
        new(Du, identifier);

}