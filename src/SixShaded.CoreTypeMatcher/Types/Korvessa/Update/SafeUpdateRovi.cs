namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Update;

public record SafeUpdateRovi : IKorssaType
{
    public required Func<Kor, IResult<RovuInfo, AbstractRovuInfo>> RovuInfoGetter { get; init; }
    public required RoggiTypeInfo DataType { get; init; }
}