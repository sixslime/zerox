using FourZeroOne.FZOSpec;
using MorseCode.ITask;
using Perfection;
#nullable enable
namespace DeTes.Realization
{
    using Analysis;
    using Declaration;
    public interface IDeTesFZOSupplier
    {
        public IStateFZO UnitializedState { get; } 
        public IProcessorFZO Processor { get; }
    }
    public class DeTesRealizer
    {
        public ITask<IResult<IDeTesResult, EDeTesInvalidTest>> Realize(IDeTesTest test, IDeTesFZOSupplier supplier)
            => new DeTesRealizerImpl().Realize(test, supplier);
    }
    public class DeTesInternalException(Exception inner) : Exception("Internal DeTes error.", inner);
    public class DeTesInvalidTestException : Exception
    {
        public required EDeTesInvalidTest Value { get; init; }
    }
}
