namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Update;

public record SafeUpdateRovedanggi : IKorssaType
{
    public required RoggiTypeInfo DataType { get; init; }
}