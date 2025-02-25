namespace SixShaded.DeTes.Analysis;

public interface IDeTesOnResolveAssertions
{
    public IDeTesAssertionData<Kor>[] Korssa { get; }
    public IDeTesAssertionData<IMemoryFZO>[] Memory { get; }
    public IDeTesAssertionData<RogOpt>[] Roggi { get; }
}