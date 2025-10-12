namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record IMulti : IRoggiType
{
    public required RoggiTypeInfo ElementType { get; init; }
    public required Func<Rog, RogOpt[]> ElementGetter { get; init; }

}