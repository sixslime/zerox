namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus;

public interface uPlayerLinked : IRovetu
{
    public static readonly Rovu<uPlayerLinked, IRoveggi<uPlayerIdentifier>> PLAYER_ID = new(Axoi.Du, "player_id");
}