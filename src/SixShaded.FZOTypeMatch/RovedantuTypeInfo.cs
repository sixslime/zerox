namespace SixShaded.FZOTypeMatch;

public record RovedantuTypeInfo : RovetuTypeInfo
{
    public required RoggiTypeInfo StoredDataType { get; init; }
}