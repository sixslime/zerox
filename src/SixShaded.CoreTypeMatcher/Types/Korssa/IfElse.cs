namespace SixShaded.CoreTypeMatcher.Types.Korssa;

public record IfElse : IKorssaType
{
    public required RoggiTypeInfo OutputType { get; init; }
}