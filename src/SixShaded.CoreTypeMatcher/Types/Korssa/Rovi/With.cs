namespace SixShaded.CoreTypeMatcher.Types.Korssa.Rovi;

public record With : IKorssaType
{
    public required Func<Kor, IResult<RovuInfo, AbstractRovuInfo>> RovuInfoGetter { get; init; }
    public required RoggiTypeInfo DataType { get; init; }
    public required RovetuTypeInfo RovetuType { get; init; }
}