namespace SixShaded.DeTes.Declaration;

internal class AssertionImpl<T> : IAssertionAccessor<T>
{
    public required string? Description { get; init; }
    public required Tok LinkedToken { get; init; }
    public required Predicate<T> Condition { get; init; }
    Tok ITokenLinked.LinkedToken => LinkedToken;
    Predicate<T> IAssertionAccessor<T>.Condition => Condition;
}