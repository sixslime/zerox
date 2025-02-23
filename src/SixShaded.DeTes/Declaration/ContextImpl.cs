namespace SixShaded.DeTes.Declaration;

internal class ContextImpl : IDeTesContext, IContextAccessor
{
    private List<IDomainAccessor> _domains = [];
    private List<IReferenceAccessor> _references = [];
    private AssertSet _assertions = new();

    private class AssertSet
    {
        public List<IAssertionAccessor<ResOpt>> Resolution = [];
        public List<IAssertionAccessor<IMemoryFZO>> Memory = [];
        public List<IAssertionAccessor<IToken>> Tok = [];
    }

    IDomainAccessor[] IContextAccessor.Domains => _domains.ToArray();
    IReferenceAccessor[] IContextAccessor.References => _references.ToArray();
    IAssertionAccessor<ResOpt>[] IContextAccessor.ResolutionAssertions => _assertions.Resolution.ToArray();
    IAssertionAccessor<IMemoryFZO>[] IContextAccessor.MemoryAssertions => _assertions.Memory.ToArray();
    IAssertionAccessor<IToken>[] IContextAccessor.TokenAssertions => _assertions.Token.ToArray();
    IDeTesContext IContextAccessor.PublicContext => this;

    public void AddAssertionResolution<R>(IToken<R> subject, Predicate<R> assertion, string? description)
        where R : class, Res
    {
        _assertions.Resolution.Add(new AssertionImpl<ResOpt>
        {
            Description = description,
            LinkedToken = subject,
            Condition = x => x.Check(out var res)
                ? res is R r ? assertion(r) : throw new UnexpectedResolutionTypeException(res.GetType(), typeof(R))
                : throw new UnexpectedNollaException(),
        });
    }

    public void AddAssertionResolutionUnstable<R>(IToken<R> subject, Predicate<IOption<R>> assertion, string? description)
        where R : class, Res
    {
        _assertions.Resolution.Add(new AssertionImpl<ResOpt>
        {
            Description = description,
            LinkedToken = subject,
            Condition = x => x is IOption<R> r ? assertion(r) : throw new UnexpectedResolutionTypeException(x.GetType(), typeof(IOption<R>))
        });
    }

    public void AddAssertionToken<R>(IToken<R> subject, Predicate<IToken<R>> assertion, string? description)
        where R : class, Res
    {
        _assertions.Token.Add(new AssertionImpl<IToken>
        {
            Description = description,
            LinkedToken = subject,
            Condition = x => assertion((IToken<R>)x)
        });
    }

    public void AddAssertionMemory(Tok subject, Predicate<IMemoryFZO> assertion, string? description)
    {
        _assertions.Memory.Add(new AssertionImpl<IMemoryFZO>
        {
            Description = description,
            LinkedToken = subject,
            Condition = assertion
        });
    }

    public void MakeReference<R>(IToken<R> subject, out IDeTesReference<R> reference, string? description)
        where R : class, Res
    {
        var o = new ReferenceImpl<R> { Description = description, LinkedToken = subject };
        reference = o;
        _references.Add(o);
    }

    public void MakeSingleSelectionDomain(Tok subject, int[] selections, out IDeTesSingleDomain domainHandle, string? description)
    {
        MakeSelectionDomain(subject, selections.Map(x => x.Yield().ToArray()).ToArray(), out var impl, description);
        domainHandle = impl;
    }

    public void MakeMultiSelectionDomain(Tok subject, int[][] selections, out IDeTesMultiDomain domainHandle, string? description)
    {
        MakeSelectionDomain(subject, selections, out var impl, description);
        domainHandle = impl;
    }

    private void MakeSelectionDomain(Tok subject, int[][] selections, out DomainImpl impl, string? description)
    {
        impl = new() { LinkedToken = subject, Selections = selections.Map(x => x.ToArray()).ToArray(), Description = description };
        _domains.Add(impl);
    }
}