namespace SixShaded.DeTes.Analysis;

public interface IDeTesOnResolveAssertions
{
    public IDeTesAssertionData<Tok>[] Token { get; }
    public IDeTesAssertionData<IMemoryFZO>[] Memory { get; }
    public IDeTesAssertionData<ResOpt>[] Resolution { get; }
}