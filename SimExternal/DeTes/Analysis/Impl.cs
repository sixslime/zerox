using FourZeroOne.FZOSpec;
using FourZeroOne.Token.Unsafe;
using Perfection;
using ResOpt = Perfection.IOption<FourZeroOne.Resolution.IResolution>;
#nullable enable
namespace DeTes.Analysis
{
    internal class DomainDataImpl : IDeTesDomainData
    {
        public required string? Description { get; init; }
        public required int[][] Values { get; init; }
    }
    internal class AssertionDataImpl<A> : IDeTesAssertionData<A>
    {
        public required IResult<bool, Exception> Result { get; init; }
        public required string? Description { get; init; }
        public required Predicate<A> Condition { get; init; }
    }
    internal class SelectionPathImpl : IDeTesSelectionPath
    {
        public required IDeTesResult ResultObject { get; init; }
        public required DomainDataImpl DomainData { get; init; }
        public required int[] ThisSelection { get; init; }
        IDeTesDomainData IDeTesSelectionPath.Domain => DomainData;
        int[] IDeTesSelectionPath.Selection => ThisSelection;
        IResult<IResult<EProcessorHalt, Exception>, IDeTesSelectionPath[]> IDeTesResult.CriticalPoint => ResultObject.CriticalPoint;
        EDeTesFrame[] IDeTesResult.EvaluationFrames => ResultObject.EvaluationFrames;
    }
    internal class ResultImpl : IDeTesResult
    {
        public required IResult<IResult<EProcessorHalt, Exception>, IDeTesSelectionPath[]> CriticalPoint { get; init; }
        public required EDeTesFrame[] EvaluationFrames { get; init; }
    }

}
