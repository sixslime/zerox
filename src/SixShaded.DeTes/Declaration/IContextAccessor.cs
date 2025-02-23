namespace SixShaded.DeTes.Declaration;

internal interface IContextAccessor
{
    IDeTesContext PublicContext { get; }
    IDomainAccessor[] Domains { get; }
    IReferenceAccessor[] References { get; }
    IAssertionAccessor<ResOpt>[] ResolutionAssertions { get; }
    IAssertionAccessor<IMemoryFZO>[] MemoryAssertions { get; }
    IAssertionAccessor<IToken>[] TokenAssertions { get; }
}