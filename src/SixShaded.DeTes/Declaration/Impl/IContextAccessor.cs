namespace SixShaded.DeTes.Declaration.Impl;
internal interface IContextAccessor
{
    IDeTesContext PublicContext { get; }
    IDomainAccessor[] Domains { get; }
    IReferenceAccessor[] References { get; }
    IAssertionAccessor<RogOpt>[] RoggiAssertions { get; }
    IAssertionAccessor<IMemoryFZO>[] MemoryAssertions { get; }
    IAssertionAccessor<Kor>[] KorssaAssertions { get; }
}