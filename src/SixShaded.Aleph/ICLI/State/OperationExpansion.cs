namespace SixShaded.Aleph.ICLI.State;

using Logical;

internal record OperationExpansion
{
    public IPMap<int, OperationExpansion> ExpansionMap { get; init; } = new PMap<int, OperationExpansion>();
    public IEnumerable<IOption<OperationExpansion>> GetOrderedExpansions(int argCount) => Iter.Range(0, argCount).Map(ExpansionMap.At);
}