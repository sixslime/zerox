namespace SixShaded.CoreTypeMatcher.Types.Korssa.Memory.Rovedanggi;

public record Read : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}