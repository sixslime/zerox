namespace SixShaded.Aleph.Language.Segments;

internal class ResolutionSegment<T> : ITranslationResolution
{
    public required Func<T, string> Expression { get; init; }
}