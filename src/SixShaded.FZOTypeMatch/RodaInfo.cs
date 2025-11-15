namespace SixShaded.FZOTypeMatch;

using FourZeroOne.Roveggi.Unsafe;

public record RodaInfo
{
    public required Addr Roda { get; init; }
    public required IOption<ulong> DynamicId { get; init; }
    public required RoggiTypeInfo DataType { get; init; }
}