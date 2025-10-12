namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record MetaFunction : IRoggiType
{
    public required RoggiTypeInfo[] ArgTypes { get; init; }
    public required RoggiTypeInfo OutputType { get; init; }
    public required Func<Rog, Kor> MetaKorssaGetter { get; init; }
    public required Func<Rog, Addr> SelfRodaGetter { get; init; }
    public required Func<Rog, Addr[]> ArgRodasGetter { get; init; }
    public required Func<Rog, Addr[]> CapturedRodasGetter { get; init; }
    public required Func<Rog, FourZeroOne.FZOSpec.IMemoryFZO> CapturedMemoryGetter { get; init; }

}