using MorseCode.ITask;
using Perfection;
using FourZeroOne.Handles;
#nullable enable
namespace FourZeroOne.Logical
{
    using Token;
    using IToken = Token.Unsafe.IToken;
    using ResObj = Resolution.IResolution;
    using ResOpt = IOption<Resolution.IResolution>;
    using Resolution;
    using Resolution.Unsafe;
    using Rule;
    
    public interface IProcessor
    {
        public ITask<IResult<EStep, EHalt>> GetNextStep(IState state, IInput input);
        public interface ITokenContext
        {
            public IMemory CurrentMemory { get; }
            public IInput Input { get; }
        }
    }
    public interface IInput
    {
        public ITask<int[]> GetSelection(IHasElements<ResObj> pool, int count);
    }
    public interface IState
    {
        public IEnumerable<IOperationNode> OperationStack { get; }
        public IEnumerable<IMemory> MemoryStack { get; }
        public IEnumerable<EPreprocess> PreprocessStack { get; }

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
        /// A new <see cref="IState"/> with the above changes.<br></br>
        /// This <b>must</b> not mutate the original <see cref="IState"/>.
        /// </returns>
        public IState WithStep(EStep step);

        public interface IOperationNode
        {
            public IToken Operation { get; }
            public IEnumerable<ResOpt> ResolvedArgs { get; }
        }
    }
    public interface IMemory
    {
        public IEnumerable<ITiple<IStateAddress, ResObj>> Objects { get; }
        public IEnumerable<IRule> Rules { get; }
        public IOption<R> GetObject<R>(IStateAddress<R> address) where R : class, ResObj;
        public IMemory WithRules(IEnumerable<IRule> rules);
        public IMemory WithObjects<R>(IEnumerable<ITiple<IStateAddress<R>, R>> insertions) where R : class, ResObj;
        public IMemory WithClearedAddresses(IEnumerable<IStateAddress> removals);
    }
    public abstract record EStep
    {
        public sealed record Preprocess : EStep
        {
            public required EPreprocess Value { get; init; }
        }
        public sealed record Resolve : EStep
        {
            public required IResult<ResOpt, Resolution.EEvaluatorHandled> Resolution { get; init; }
        }
        public sealed record PushOperation : EStep
        {
            public required IToken OperationToken { get; init; }
        }
    }
    public abstract record EHalt
    {
        public required IState HaltingState { get; init; }
        public sealed record InvalidState : EHalt { }
        public sealed record Completed : EHalt
        {
            public required ResOpt Resolution { get; init; }
        }
    }
    public abstract record EPreprocess
    {
        public required IToken Result { get; init; }
        public sealed record Identity : EPreprocess { }
        public sealed record MacroExpansion : EPreprocess { }
        public sealed record RuleApplication : EPreprocess
        {
            public required Rule.IRule Rule { get; init; }
        }
    }
}