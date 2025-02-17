using FourZeroOne.FZOSpec;
using FourZeroOne.Token;
using FourZeroOne.Resolution;
using Perfection;
using NollableRes = Perfection.IOption<FourZeroOne.Resolution.IResolution>;
#nullable enable
namespace DeTes.Analysis
{
    using IToken = IToken<IResolution>;
    public interface IDeTesResult
    {
        public IResult<IResult<EProcessorHalt, Exception>, IDeTesSelectionPath[]> CriticalPoint { get; }
        public EDeTesFrame[] EvaluationFrames { get; }
    }
    public interface IDeTesSelectionPath : IDeTesResult
    {
        public IDeTesDomainData Domain { get; }
        public int[] Selection { get; }
    }
    public interface IDeTesDomainData
    {
        public string? Description { get; }
        public int[][] Values { get; }
    }
    public interface IDeTesAssertionData<A>
    {
        public IResult<bool, Exception> Result { get; }
        public string? Description { get; }
        public Predicate<A> Condition { get; }
    }
    public interface IDeTesOnPushAssertions
    {
        public IDeTesAssertionData<IToken>[] Token { get; }
    }
    public interface IDeTesOnResolveAssertions
    {
        public IDeTesAssertionData<IMemoryFZO>[] Memory { get; }
        public IDeTesAssertionData<NollableRes>[] Resolution { get; }
    }
    public abstract record EDeTesFrame
    {
        public required IStateFZO PreState { get; init; }
        public sealed record TokenPrep : EDeTesFrame
        {
            public required EProcessorStep.TokenMutate NextStep { get; init; }
        }
        public sealed record PushOperation : EDeTesFrame
        {
            public required EProcessorStep.PushOperation NextStep { get; init; }
            public required IDeTesOnPushAssertions Assertions { get; init; }
        }
        public sealed record Resolve : EDeTesFrame
        {
            public required EProcessorStep.Resolve NextStep { get; init; }
            public required IDeTesOnResolveAssertions Assertions { get; init; }
        }
        public sealed record Complete : EDeTesFrame
        {
            public required EProcessorHalt.Completed CompletionHalt { get; init; }
            public required IDeTesOnResolveAssertions Assertions { get; init; }
        }

    }
    public abstract record EDeTesInvalidTest
    {
        public sealed record ReferenceUsedBeforeEvaluated : EDeTesInvalidTest
        {
        public required IToken NearToken { get; init; }
            public required string? Description { get; init; }
        }
        public sealed record EmptyDomain : EDeTesInvalidTest
        {
        public required IToken NearToken { get; init; }
            public required string? Description { get; init; }
        }
        public sealed record NoSelectionDomainDefined : EDeTesInvalidTest
        {
            public required IToken SelectionToken { get; init; }
        }
        public sealed record DomainUsedOutsideOfScope : EDeTesInvalidTest
        {
        public required IToken NearToken { get; init; }
            public required string? Description { get; init; }
            public required int[][] Domain { get; init; }
        }
        public sealed record InvalidDomainSelection : EDeTesInvalidTest
        {
            public required IToken NearToken { get; init; }
            public required string? Description { get; init; }
            public required IToken SelectionToken { get; init; }
            public required int[][] Domain { get; init; }
            public required int[] InvalidSelection { get; init; }
            public required int ExpectedSelectionSize { get; init; }
            public required int ExpectedMaxIndex { get; init; }
        }
    }
    public class UnexpectedNollaException() : Exception { }
    public class UnexpectedResolutionTypeException() : Exception { }
}
