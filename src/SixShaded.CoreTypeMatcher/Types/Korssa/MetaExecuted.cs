namespace SixShaded.CoreTypeMatcher.Types.Korssa;

public record MetaExecuted : IKorssaType
{
    public required RoggiTypeInfo OutputType { get; init; }
}