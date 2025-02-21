using FourZeroOne.FZOSpec;
#nullable enable
namespace SixShaded.DeTes.Realization
{
    using Analysis;
    using Declaration;
    using SixShaded.DeTes.Analysis;
    using SixShaded.DeTes.Declaration;
    using SixShaded.NotRust;

    public interface IDeTesFZOSupplier
    {
        public IStateFZO UnitializedState { get; }
        public IProcessorFZO Processor { get; }
    }
    public class DeTesRealizer
    {
        public Task<IResult<IDeTesResult, EDeTesInvalidTest>> Realize(IDeTesTest test, IDeTesFZOSupplier supplier)
            => new DeTesRealizerImpl().Realize(test, supplier);
    }
    public class DeTesInternalException(Exception inner) : Exception("Internal DeTes error.", inner);
    public class DeTesInvalidTestException : Exception
    {
        public required EDeTesInvalidTest Value { get; init; }
    }
}
