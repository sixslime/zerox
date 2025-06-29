namespace SixShaded.DeTes.Declaration.Impl;

using System.Diagnostics;

internal class DummyContext : IDeTesContext
{
    void IDeTesContext.AddAssertionRoggi<R>(IKorssa<R> subject, Predicate<R> assertion, string? description) { }
    void IDeTesContext.AddAssertionRoggiUnstable<R>(IKorssa<R> subject, Predicate<IOption<R>> assertion, string? description) { }
    void IDeTesContext.AddAssertionKorssa<R>(IKorssa<R> subject, Predicate<IKorssa<R>> assertion, string? description) { }
    void IDeTesContext.AddAssertionMemory(Kor subject, Predicate<IMemoryFZO> assertion, string? description) { }

    void IDeTesContext.MakeReference<R>(IKorssa<R> subject, out IDeTesReference<R> reference, string? description)
    {
        reference = new DummyReference<R>();
    }

    void IDeTesContext.MakeSingleSelectionDomain(Kor subject, int[] selections, out IDeTesSingleDomain domainHandle, string? description)
    {
        domainHandle = new DummySingleDomain();
    }

    void IDeTesContext.MakeMultiSelectionDomain(Kor subject, int[][] selections, out IDeTesMultiDomain domainHandle, string? description)
    {
        domainHandle = new DummyMultiDomain();
    }

    private class DummySingleDomain : IDeTesSingleDomain
    {
        public int SelectedIndex() => throw new UnreachableException();
    }

    private class DummyMultiDomain : IDeTesMultiDomain
    {
        public int[] SelectedIndicies() => throw new UnreachableException();
    }
    private class DummyReference<R> : IDeTesReference<R>
        where R : class, Rog
    {
        IKorssa<R> IDeTesReference<R>.Korssa => throw new UnreachableException();
        R IDeTesReference<R>.Roggi => throw new UnreachableException();
        IOption<R> IDeTesReference<R>.RoggiUnstable => throw new UnreachableException();
        IMemoryFZO IDeTesReference<R>.Memory => throw new UnreachableException();
    }
}