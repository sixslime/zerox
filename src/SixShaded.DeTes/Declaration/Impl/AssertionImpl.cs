namespace SixShaded.DeTes.Declaration.Impl;

internal class AssertionImpl<T> : IAssertionAccessor<T>
{
    public required Kor LinkedKorssa { get; init; }
    public required Predicate<T> Condition { get; init; }
    public required string? Description { get; init; }
    Kor IKorssaLinked.LinkedKorssa => LinkedKorssa;
    Predicate<T> IAssertionAccessor<T>.Condition => Condition;
}