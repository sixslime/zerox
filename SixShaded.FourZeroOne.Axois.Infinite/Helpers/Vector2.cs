namespace SixShaded.FourZeroOne.Axois.Infinite.Helpers;

internal readonly struct Vector2(float xVal, float yVal)
{
    public readonly float X = xVal;
    public readonly float Y = yVal;
    public static Vector2 operator +(Vector2 v1, Vector2 v2) => new Vector2(v1.X + v2.X, v1.Y + v2.Y);
    public static Vector2 operator -(Vector2 v1, Vector2 v2) => new Vector2(v1.X - v2.X, v1.Y - v2.Y);
    public static Vector2 operator -(Vector2 v) => new Vector2(-v.X, -v.Y);
}