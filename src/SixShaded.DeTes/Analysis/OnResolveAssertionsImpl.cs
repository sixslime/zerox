namespace SixShaded.DeTes.Analysis;

internal class OnResolveAssertionsImpl : IDeTesOnResolveAssertions
{
    public required IDeTesAssertionData<Tok>[] Token { get; init; }
    public required IDeTesAssertionData<ResOpt>[] Resolution { get; init; }
    public required IDeTesAssertionData<IMemoryFZO>[] Memory { get; init; }
}