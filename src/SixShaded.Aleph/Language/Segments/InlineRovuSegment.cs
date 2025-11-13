namespace SixShaded.Aleph.Language.Segments;

internal class InlineRovuSegment : ITranslationSegment
{
    public required FourZeroOne.Roveggi.Unsafe.IRovu Value { get; init; }
}