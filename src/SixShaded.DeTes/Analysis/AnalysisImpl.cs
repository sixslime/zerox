using FourZeroOne.FZOSpec;
using FourZeroOne.Resolution;
using SixShaded.FourZeroOne;
using SixShaded.NotRust;
#nullable enable
namespace SixShaded.DeTes.Analysis
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
        public required Token RootSelectionToken { get; init; }
        public required IDeTesResult ResultObject { get; init; }
        public required DomainDataImpl DomainData { get; init; }
        public required int[] ThisSelection { get; init; }
        IDeTesDomainData IDeTesSelectionPath.Domain => DomainData;
        int[] IDeTesSelectionPath.Selection => ThisSelection;
        Token IDeTesSelectionPath.RootSelectionToken => RootSelectionToken;
        CriticalPointType IDeTesResult.CriticalPoint => ResultObject.CriticalPoint;
        EDeTesFrame[] IDeTesResult.EvaluationFrames => ResultObject.EvaluationFrames;
        TimeSpan IDeTesResult.TimeTaken => ResultObject.TimeTaken;
    }
    internal class ResultImpl : IDeTesResult
    {
        public required TimeSpan TimeTaken { get; init; }
        public required CriticalPointType CriticalPoint { get; init; }
        public required EDeTesFrame[] EvaluationFrames { get; init; }
    }
    internal class OnResolveAssertionsImpl : IDeTesOnResolveAssertions
    {
        public required IDeTesAssertionData<Token>[] Token { get; init; }
        public required IDeTesAssertionData<ResOpt>[] Resolution { get; init; }
        public required IDeTesAssertionData<IMemoryFZO>[] Memory { get; init; }
    }
}
