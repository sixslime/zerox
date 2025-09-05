namespace SixShaded.CoreTypeMatcher.Types.Korssa;

public record ToBoxedArgs : IKorssaType
{
    public required RoggiTypeInfo[] ArgTypes { get; init; }
}