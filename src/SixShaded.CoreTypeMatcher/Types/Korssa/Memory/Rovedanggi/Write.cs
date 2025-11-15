namespace SixShaded.CoreTypeMatcher.Types.Korssa.Memory.Rovedanggi;

public record Write : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}