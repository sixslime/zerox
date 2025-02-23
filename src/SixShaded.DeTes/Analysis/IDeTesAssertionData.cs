namespace SixShaded.DeTes.Analysis;

public interface IDeTesAssertionData<A> : IDeTesAssertionDataUntyped
{
    public Predicate<A> Condition { get; }
}