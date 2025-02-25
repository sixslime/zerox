namespace SixShaded.DeTes.Declaration.Impl;
internal interface IContextAccessor
{
    IDeTesContext PublicContext { get; }
    IDomainAccessor[] Domains { get; }
    IReferenceAccessor[] References { get; }
    IAssertionAccessor<ResOpt>[] ResolutionAssertions { get; }
    IAssertionAccessor<IMemoryFZO>[] MemoryAssertions { get; }
    IAssertionAccessor<Tok>[] TokenAssertions { get; }
}