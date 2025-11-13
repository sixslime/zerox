namespace SixShaded.Aleph.Language.Segments;

internal class InlineAbstractRovuSegment : ITranslationSegment
{
    public required FourZeroOne.Roveggi.Unsafe.IAbstractRovu Value { get; init; }
}