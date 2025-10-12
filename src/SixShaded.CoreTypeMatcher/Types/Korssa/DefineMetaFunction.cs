namespace SixShaded.CoreTypeMatcher.Types.Korssa;

public record DefineMetaFunction : IKorssaType
{
    public required RoggiTypeInfo MetaOutputType { get; init; }
    public required RoggiTypeInfo[] MetaArgTypes { get; init; }
    public required Func<Kor, Kor> MetaKorssaGetter { get; init; }
    public required Func<Kor, Addr> SelfRodaGetter { get; init; }
    public required Func<Kor, Addr[]> ArgRodasGetter { get; init; }
    public required Func<Kor, Addr[]> CapturedRodasGetter { get; init; }
}