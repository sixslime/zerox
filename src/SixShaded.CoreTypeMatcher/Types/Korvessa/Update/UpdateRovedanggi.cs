namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Update;

public record UpdateRovedanggi : IKorssaType
{
    public required RoggiTypeInfo DataType { get; init; }
}