namespace SixShaded.DeTes.Analysis;

internal class AssertionDataImpl<A> : IDeTesAssertionData<A>
{
    public required Tok OnToken { get; init; }
    public required IResult<bool, Exception> Result { get; init; }
    public required string? Description { get; init; }
    public required Predicate<A> Condition { get; init; }
}