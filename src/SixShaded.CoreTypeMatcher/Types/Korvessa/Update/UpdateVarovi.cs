namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Update;

public record UpdateVarovi : IKorssaType
{
    public required Func<Kor, VarovuInfo> VarovuInfoGetter { get; init; }
    public required RoggiTypeInfo KeyType { get; init; }
    public required RoggiTypeInfo DataType { get; init; }
}