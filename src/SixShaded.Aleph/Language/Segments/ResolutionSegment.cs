namespace SixShaded.Aleph.Language.Segments;

internal class ResolutionSegment<T> : ITranslationSegment
{
    public required Func<T, string> Expression { get; init; }
}