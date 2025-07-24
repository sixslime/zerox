namespace SixShaded.FourZeroOne.Axois.Infinite;

public class Axoi : IsAxoi
{
    private Axoi()
    { }

    public static Axodu Du { get; } =
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