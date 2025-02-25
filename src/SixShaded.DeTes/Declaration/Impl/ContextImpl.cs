namespace SixShaded.DeTes.Declaration.Impl;

internal class ContextImpl : IDeTesContext, IContextAccessor
{
    private readonly AssertSet _assertions = new();
    private readonly List<IDomainAccessor> _domains = [];
    private readonly List<IReferenceAccessor> _references = [];

    IDomainAccessor[] IContextAccessor.Domains => _domains.ToArray();
    IReferenceAccessor[] IContextAccessor.References => _references.ToArray();
    IAssertionAccessor<RogOpt>[] IContextAccessor.RoggiAssertions => _assertions.Roggi.ToArray();
    IAssertionAccessor<IMemoryFZO>[] IContextAccessor.MemoryAssertions => _assertions.Memory.ToArray();
    IAssertionAccessor<Kor>[] IContextAccessor.KorssaAssertions => _assertions.Korssa.ToArray();
    IDeTesContext IContextAccessor.PublicContext => this;

    public void AddAssertionRoggi<R>(IKorssa<R> subject, Predicate<R> assertion, string? description)
        where R : class, Rog =>
        _assertions.Roggi.Add(new AssertionImpl<RogOpt>
        {
            Description = description,
            LinkedKorssa = subject,
            Condition = x => x.Check(out var res)
                ? res is R r ? assertion(r) : throw new UnexpectedRoggiTypeException(res.GetType(), typeof(R))
                : throw new UnexpectedNollaException(),
        });

    public void AddAssertionRoggiUnstable<R>(IKorssa<R> subject, Predicate<IOption<R>> assertion, string? description)
        where R : class, Rog =>
        _assertions.Roggi.Add(new AssertionImpl<RogOpt>
        {
            Description = description,
            LinkedKorssa = subject,
            Condition = x => x is IOption<R> r ? assertion(r) : throw new UnexpectedRoggiTypeException(x.GetType(), typeof(IOption<R>)),
        });

    public void AddAssertionKorssa<R>(IKorssa<R> subject, Predicate<IKorssa<R>> assertion, string? description)
        where R : class, Rog =>
        _assertions.Korssa.Add(new AssertionImpl<Kor>
        {
            Description = description,
            LinkedKorssa = subject,
            Condition = x => assertion((IKorssa<R>)x),
        });

    public void AddAssertionMemory(Kor subject, Predicate<IMemoryFZO> assertion, string? description) =>
        _assertions.Memory.Add(new AssertionImpl<IMemoryFZO>
        {
            Description = description,
            LinkedKorssa = subject,
            Condition = assertion,
        });

    public void MakeReference<R>(IKorssa<R> subject, out IDeTesReference<R> reference, string? description)
        where R : class, Rog
    {
        var o = new ReferenceImpl<R> { Description = description, LinkedKorssa = subject };
        reference = o;
        _references.Add(o);
    }

    public void MakeSingleSelectionDomain(Kor subject, int[] selections, out IDeTesSingleDomain domainHandle, string? description)
    {
        MakeSelectionDomain(subject, selections.Map(x => x.Yield().ToArray()).ToArray(), out var impl, description);
        domainHandle = impl;
    }

    public void MakeMultiSelectionDomain(Kor subject, int[][] selections, out IDeTesMultiDomain domainHandle, string? description)
    {
        MakeSelectionDomain(subject, selections, out var impl, description);
        domainHandle = impl;
    }

    private void MakeSelectionDomain(Kor subject, int[][] selections, out DomainImpl impl, string? description)
    {
        impl = new() { LinkedKorssa = subject, Selections = selections.Map(x => x.ToArray()).ToArray(), Description = description };
        _domains.Add(impl);
    }

    private class AssertSet
    {
        public readonly List<IAssertionAccessor<IMemoryFZO>> Memory = [];
        public readonly List<IAssertionAccessor<RogOpt>> Roggi = [];
        public readonly List<IAssertionAccessor<Kor>> Korssa = [];
    }
}