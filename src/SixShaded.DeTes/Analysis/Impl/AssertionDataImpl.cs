namespace SixShaded.DeTes.Analysis.Impl;

internal class AssertionDataImpl<A> : IDeTesAssertionData<A>
{
    public required Kor OnKorssa { get; init; }
    public required IResult<bool, Exception> Result { get; init; }
    public required string? Description { get; init; }
    public required Predicate<A> Condition { get; init; }
}