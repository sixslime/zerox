namespace SixShaded.FourZeroOne.Axois.Infinite.Helpers;

using System.Diagnostics;

internal readonly struct HexPos(int rVal, int uVal, int dVal)
{
    public readonly int R = rVal;
    public readonly int U = uVal;
    public readonly int D = dVal;
    public static HexPos operator -(HexPos a, HexPos b) => new HexPos(a.R - b.R, a.U - b.U, a.D - b.D);
    public static HexPos operator +(HexPos a, HexPos b) => new HexPos(a.R + b.R, a.U + b.U, a.D + b.D);
    public static HexPos operator *(HexPos pos, int scale) => new HexPos(pos.R * scale, pos.U * scale, pos.D * scale);
    public static HexPos operator -(HexPos pos) => new HexPos(-pos.R, -pos.U, -pos.D);

    public int this[int i] =>
        i switch
        {
            0 => R,
            1 => U,
            2 => D,
            _ => throw new IndexOutOfRangeException()
        };
}