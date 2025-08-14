namespace SixShaded.FourZeroOne.Axois.Infinite.Helpers;

internal readonly struct Vector2(float xVal, float yVal)
{
    public readonly float X = xVal;
    public readonly float Y = yVal;
    public HexPos Add(HexPos other) => new(R + other.R, U + other.U, D + other.D);
    public HexPos Subtract(HexPos other) => new(R - other.R, U - other.U, D - other.D);
}