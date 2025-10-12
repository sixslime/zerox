namespace SixShaded.CoreTypeMatcher.Types.Roggi.Instructions;

public record Assign : IRoggiType
{
    public required RoggiTypeInfo DataType { get; init; }
    public required Func<Rog, Addr> RodaGetter { get; init; }
    public required Func<Rog, RogOpt> DataGetter { get; init; }

}