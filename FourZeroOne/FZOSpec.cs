using MorseCode.ITask;
using Perfection;
using FourZeroOne.Handles;
#nullable enable
namespace FourZeroOne.FZOSpec
{
    using Token;
    using IToken = Token.Unsafe.IToken;
    using ResObj = Resolution.IResolution;
    using ResOpt = IOption<Resolution.IResolution>;
    using Resolution;
    using Resolution.Unsafe;
    using Rule;
    
    public interface IProcessorFZO
    {
        public ITask<IResult<EProcessorStep, EProcessorHalt>> GetNextStep(IStateFZO state, IInputFZO input);
        public interface ITokenContext
        {
            public IMemoryFZO CurrentMemory { get; }
            public IInputFZO Input { get; }
        }
    }
    public interface IInputFZO
    {
        public ITask<int[]> GetSelection(IHasElements<ResObj> pool, int count);
    }
    public interface IStateFZO
    {
        public IEnumerable<IOperationNode> OperationStack { get; }
        public IEnumerable<IMemoryFZO> MemoryStack { get; }
        public IEnumerable<ETokenPrep> TokenPrepStack { get; }

        /// <summary>
        /// Implementation <b>must</b> adhere to the following behavior, given <paramref name="step"/> is:<br></br>
        /// <b>Preprocess:</b><br></br>
        /// - Append 'Value' to <i>PreprocessStack</i><br></br>
        /// <b>Resolve:</b><br></br>
        /// - Pop from <i>OperationStack</i><br></br>
        /// - Append 'Resolution' to <i>OperationStack[0].ResolvedArgs</i><br></br>
        /// <b>PushOperation:</b><br></br>
        /// - Append to <i>OperationStack</i><br></br>
        /// - Set <i>OperationStack[0].Operation</i> to 'OperationToken'<br></br>
        /// - Set <i>OperationStack[0].ResolvedArgs</i> to empty<br></br>
        /// <br></br>
        /// IEnumerables <b>must</b> behave as top-down stack iterators.
        /// </summary>
        /// <param name="step"></param>
        /// <returns>
        /// A new <see cref="IStateFZO"/> with the above changes.<br></br>
        /// This <b>must</b> not mutate the original <see cref="IStateFZO"/>.
        /// </returns>
        public IStateFZO WithStep(EProcessorStep step);

        public interface IOperationNode
        {
            public IToken Operation { get; }
            public IEnumerable<ResOpt> ResolvedArgs { get; }
        }
    }
    public interface IMemoryFZO
    {
        public IEnumerable<ITiple<IStateAddress, ResObj>> Objects { get; }
        public IEnumerable<IRule> Rules { get; }
        public IOption<R> GetObject<R>(IStateAddress<R> address) where R : class, ResObj;
        public IMemoryFZO WithRules(IEnumerable<IRule> rules);
        public IMemoryFZO WithObjects<R>(IEnumerable<ITiple<IStateAddress<R>, R>> insertions) where R : class, ResObj;
        public IMemoryFZO WithClearedAddresses(IEnumerable<IStateAddress> removals);
    }
    public abstract record EProcessorStep
    {
        public sealed record TokenPrep : EProcessorStep
        {
            public required ETokenPrep Value { get; init; }
        }
        public sealed record Resolve : EProcessorStep
        {
            public required IResult<ResOpt, EProcessorHandled> Resolution { get; init; }
        }
        public sealed record PushOperation : EProcessorStep
        {
            public required IToken OperationToken { get; init; }
        }
    }
    public abstract record EProcessorHalt
    {
        public required IStateFZO HaltingState { get; init; }
        public sealed record InvalidState : EProcessorHalt { }
        public sealed record Completed : EProcessorHalt
        {
            public required ResOpt Resolution { get; init; }
        }
    }
    public abstract record ETokenPrep
    {
        public required IToken Result { get; init; }
        public sealed record Identity : ETokenPrep { }
        public sealed record MacroExpansion : ETokenPrep { }
        public sealed record RuleApplication : ETokenPrep
        {
            public required Rule.IRule Rule { get; init; }
        }
    }
}