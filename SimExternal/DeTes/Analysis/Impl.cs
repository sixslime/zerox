using FourZeroOne.FZOSpec;
using Perfection;
using FourZeroOne.Resolution;
using FourZeroOne.Token;
#nullable enable
namespace DeTes.Analysis
{
    using CriticalPointType = IResult<IResult<EProcessorHalt, Exception>, IDeTesSelectionPath[]>;
    using Token = IToken<IResolution>;
    using ResOpt = IOption<IResolution>;
    internal class DomainDataImpl : IDeTesDomainData
    {
        public required string? Description { get; init; }
        public required int[][] Values { get; init; }
    }
    internal class AssertionDataImpl<A> : IDeTesAssertionData<A>
    {
        public required Token OnToken { get; init; }
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
        CriticalPointType IDeTesResult.CriticalPoint => ResultObject.CriticalPoint;
        EDeTesFrame[] IDeTesResult.EvaluationFrames => ResultObject.EvaluationFrames;
    }
    internal class ResultImpl : IDeTesResult
    {
        public required CriticalPointType CriticalPoint { get; init; }
        public required EDeTesFrame[] EvaluationFrames { get; init; }
    }
    internal class OnResolveAssertionsImpl : IDeTesOnResolveAssertions
    {
        public required IDeTesAssertionData<IMemoryFZO>[] Memory { get; init; }
        public required IDeTesAssertionData<ResOpt>[] Resolution { get; init; }
    }
    internal class OnPushAssertionsImpl : IDeTesOnPushAssertions
    {
        public required IDeTesAssertionData<Token>[] Token { get; init; }
    }
}
