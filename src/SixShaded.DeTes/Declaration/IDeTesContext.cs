namespace SixShaded.DeTes.Declaration;

public interface IDeTesContext
{
    public void AddAssertionResolution<R>(IToken<R> subject, Predicate<R> assertion, string? description)
        where R : class, Res;
    public void AddAssertionResolutionUnstable<R>(IToken<R> subject, Predicate<IOption<R>> assertion, string? description)
        where R : class, Res;
    public void AddAssertionToken<R>(IToken<R> subject, Predicate<IToken<R>> assertion, string? description)
        where R : class, Res;
    public void AddAssertionMemory(Tok subject, Predicate<IMemoryFZO> assertion, string? description);
    public void MakeReference<R>(IToken<R> subject, out IDeTesReference<R> reference, string? description)
        where R : class, Res;
    public void MakeSingleSelectionDomain(Tok subject, int[] selections, out IDeTesSingleDomain domainHandle, string? description);
    public void MakeMultiSelectionDomain(Tok subject, int[][] selections, out IDeTesMultiDomain domainHandle, string? description);
}