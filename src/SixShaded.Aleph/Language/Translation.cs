namespace SixShaded.Aleph.Language;

using System.Collections;

public class Translation : IEnumerable<ITranslationSegment>
{
    internal Translation(IEnumerable<ITranslationSegment> segments)
    {
        Segments = segments.ToPSequence();
    }

    internal IPSequence<ITranslationSegment> Segments { get; }
    public IEnumerator<ITranslationSegment> GetEnumerator() => Segments.Elements.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}