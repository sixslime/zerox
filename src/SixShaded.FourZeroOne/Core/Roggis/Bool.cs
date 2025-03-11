namespace SixShaded.FourZeroOne.Core.Roggis;

public sealed record Bool : Roggi.Defined.NoOp
{
    public required bool IsTrue { get; init; }

    public static implicit operator Bool(bool value) =>
        new()
        {
            IsTrue = value,
        };

    public override string ToString() => $"{IsTrue}";
}