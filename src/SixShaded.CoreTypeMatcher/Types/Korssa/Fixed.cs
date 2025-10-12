namespace SixShaded.CoreTypeMatcher.Types.Korssa;

public record Fixed : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
    public required Func<Kor, Rog> RoggiGetter { get; init; }
}