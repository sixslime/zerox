namespace SixShaded.DeTes.Analysis.Impl;

internal class OnResolveAssertionsImpl : IDeTesOnResolveAssertions
{
    public required IDeTesAssertionData<Kor>[] Korssa { get; init; }
    public required IDeTesAssertionData<RogOpt>[] Roggi { get; init; }
    public required IDeTesAssertionData<IMemoryFZO>[] Memory { get; init; }
}