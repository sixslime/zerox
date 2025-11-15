namespace SixShaded.CoreTypeMatcher.Types.Korssa.Rovi.Varovi;

public record With : IKorssaType
{
    public required Func<Kor, VarovuInfo> VarovuInfoGetter { get; init; }
    public required RoggiTypeInfo KeyType { get; init; }
    public required RoggiTypeInfo DataType { get; init; }
    public required RovetuTypeInfo RovetuType { get; init; }
}