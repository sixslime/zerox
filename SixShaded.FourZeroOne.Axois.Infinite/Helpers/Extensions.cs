namespace SixShaded.FourZeroOne.Axois.Infinite.Helpers;

internal static class Extensions
{
    public static IOption<HexPos> ToStruct(this IRoveggi<u.Constructs.uRelativeCoordinate> coordinate) =>
        coordinate.GetComponent(u.Constructs.uRelativeCoordinate.R).Check(out var rc) &&
        coordinate.GetComponent(u.Constructs.uRelativeCoordinate.U).Check(out var uc) &&
        coordinate.GetComponent(u.Constructs.uRelativeCoordinate.D).Check(out var dc)
            ? new HexPos(rc.Value, uc.Value, dc.Value).AsSome()
            : new None<HexPos>();
}