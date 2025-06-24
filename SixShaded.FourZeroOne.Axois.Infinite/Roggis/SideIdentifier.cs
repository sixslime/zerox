namespace SixShaded.FourZeroOne.Axois.Infinite.Roggis;

public record SideIdentifier : NoOp
{
    public int Id { get; init; }

    public static implicit operator SideIdentifier(int id) =>
        new()
        {
            Id = id,
        };
}