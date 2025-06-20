namespace SixShaded.FourZeroOne.Axois.Infinite.Roggis;

using Roggis;
using Roggi.Defined;
public record UnitEffect : NoOp
{
    public EType Type { get; init; }

    public static implicit operator UnitEffect(EType type) =>
        new()
        {
            Type = type,
        };
    public enum EType
    {
        Foo,
        Bar,
    }
}