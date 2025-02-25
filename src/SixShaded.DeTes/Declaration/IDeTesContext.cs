namespace SixShaded.DeTes.Declaration;

public interface IDeTesContext
{
    public void AddAssertionRoggi<R>(IKorssa<R> subject, Predicate<R> assertion, string? description)
        where R : class, Rog;

    public void AddAssertionRoggiUnstable<R>(IKorssa<R> subject, Predicate<IOption<R>> assertion, string? description)
        where R : class, Rog;

    public void AddAssertionKorssa<R>(IKorssa<R> subject, Predicate<IKorssa<R>> assertion, string? description)
        where R : class, Rog;

    public void AddAssertionMemory(Kor subject, Predicate<IMemoryFZO> assertion, string? description);

    public void MakeReference<R>(IKorssa<R> subject, out IDeTesReference<R> reference, string? description)
        where R : class, Rog;

    public void MakeSingleSelectionDomain(Kor subject, int[] selections, out IDeTesSingleDomain domainHandle, string? description);
    public void MakeMultiSelectionDomain(Kor subject, int[][] selections, out IDeTesMultiDomain domainHandle, string? description);
}