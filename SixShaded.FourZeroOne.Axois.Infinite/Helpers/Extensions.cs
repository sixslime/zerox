namespace SixShaded.FourZeroOne.Axois.Infinite.Helpers;

using System.Diagnostics;
using Rovetus.Constructs;

internal static class Extensions
{
    public static IOption<HexPos> GetStruct(this IRoveggi<uHexCoordinates> coordinate) =>
        coordinate.GetComponent(uHexCoordinates.R).Check(out var rc) &&
        coordinate.GetComponent(uHexCoordinates.U).Check(out var uc) &&
        coordinate.GetComponent(uHexCoordinates.D).Check(out var dc)
            ? new HexPos(rc.Value, uc.Value, dc.Value).AsSome()
            : new None<HexPos>();

    public static IRoveggi<uHexOffset> GetRoveggi(this HexPos pos) =>
        new Roveggi<uHexOffset>()
            .WithComponent(uHexCoordinates.R, pos.R)
            .WithComponent(uHexCoordinates.U, pos.U)
            .WithComponent(uHexCoordinates.D, pos.D);
    public static Vector2 GetCartesian(this HexPos hexPos)
    {
        var rv = new Vector2(hexPos.R, 0);
        var uv = new Vector2(MathF.Cos((2 * MathF.PI) / 3) * hexPos.U, MathF.Sin((2 * MathF.PI) / 3) * hexPos.U);
        var dv = new Vector2(MathF.Cos((4 * MathF.PI) / 3) * hexPos.U, MathF.Sin((4 * MathF.PI) / 3) * hexPos.U);
        return rv + uv + dv;
    }

    public static HexPos Shift(this HexPos pos, int times) =>
        (times % 3) switch
        {
            0 => pos,
            1 => new HexPos(pos.D, pos.R, pos.U),
            2 => new HexPos(pos.U, pos.D, pos.R),
            _ => throw new UnreachableException(),
        };

    public static IEnumerable<int> IterValues(this HexPos pos)
    {
        yield return pos.R;
        yield return pos.U;
        yield return pos.D;
    }

    public static int DistanceTo(this HexPos a, HexPos b) => (Math.Abs(a.R - b.R) + Math.Abs(a.U - b.U) + Math.Abs(a.D - b.D)) / 2;
}