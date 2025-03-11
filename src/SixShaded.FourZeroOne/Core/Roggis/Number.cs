namespace SixShaded.FourZeroOne.Core.Roggis;

public sealed record Number : Roggi.Defined.NoOp
{
    public required int Value { get; init; }

    public static implicit operator Number(int value) =>
        new()
        {
            Value = value,
        };

    public override string ToString() => $"{Value}";
}