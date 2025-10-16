namespace SixShaded.Aleph.Language;

public class Translation
{
    internal Translation(IEnumerable<ITranslationSegment> segments)
    {
        Segments = segments.ToPSequence();
    }

    internal IPSequence<ITranslationSegment> Segments { get; }
}