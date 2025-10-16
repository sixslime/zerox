namespace SixShaded.Aleph.Language;

public class Translation
{
    internal IPSequence<ITranslationSegment> Segments { get; }
    internal Translation(IEnumerable<ITranslationSegment> segments)
    {
        Segments = segments.ToPSequence();
    }
}