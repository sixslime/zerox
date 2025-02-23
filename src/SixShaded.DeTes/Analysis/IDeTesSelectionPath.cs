namespace SixShaded.DeTes.Analysis;

public interface IDeTesSelectionPath : IDeTesResult
{
    public Tok RootSelectionToken { get; }
    public IDeTesDomainData Domain { get; }
    public int[] Selection { get; }
}