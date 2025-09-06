namespace SixShaded.CoreTypeMatcher.Types.Korssa.Rovi;

public record Get : IKorssaType
{
    public required Func<Kor, RovuInfo> RovuInfoGetter { get; init; }
    public required RoggiTypeInfo DataType { get; init; }
    public required RovetuTypeInfo RovetuType { get; init; }
}