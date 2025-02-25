namespace SixShaded.DeTes.Analysis;

public interface IDeTesSelectionPath : IDeTesResult
{
    public Kor RootSelectionKorssa { get; }
    public IDeTesDomainData Domain { get; }
    public int[] Selection { get; }
}