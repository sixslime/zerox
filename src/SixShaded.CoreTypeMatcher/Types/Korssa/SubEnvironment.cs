namespace SixShaded.CoreTypeMatcher.Types.Korssa;

public record SubEnvironment : IKorssaType
{
    public required RoggiTypeInfo OutputType { get; init; }
}