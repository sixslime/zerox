using MorseCode.ITask;
using Perfection;
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
        public ITask<IResult<EDelta, EProcessorHalt>> GetNextStep(IStateFZO state, IInputFZO input);
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
        /// <b>PushOperation:</b><br></br>
        /// - Append to <i>OperationStack</i><br></br>
        /// - Set <i>OperationStack[0].Operation</i> to 'OperationToken'<br></br>
        /// - Set <i>OperationStack[0].ResolvedArgs</i> to empty<br></br>
        /// <b>Resolve:</b><br></br>
        /// 'Resolution' is <b>Ok( x )</b>:<br></br>
        /// - Pop from <i>OperationStack</i><br></br>
        /// - Append 'x' to <i>OperationStack[0].ResolvedArgs</i><br></br>
        /// 'Resolution' is <b>Err( x )</b>:<br></br>
        /// . 'x' is <b>MetaExecute</b>:<br></br>
        /// . - Append '
        /// <br></br>
        /// IEnumerables <b>must</b> behave as top-down stack iterators.
        /// </summary>
        /// <param name="step"></param>
        /// <returns>
        /// A new <see cref="IStateFZO"/> with the above changes.<br></br>
        /// This <b>must</b> not mutate the original <see cref="IStateFZO"/>.
        /// </returns>
        public IStateFZO WithStep(EDelta step);
        public IStateFZO Initialize(FZOSource source);
        public interface IOperationNode
        {
            public IToken Operation { get; }
            public IEnumerable<ResOpt> ArgResolutionStack { get; }
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

    public sealed record FZOSource
    {
        public required IToken Program { get; init; }
        public required IMemoryFZO InitialMemory { get; init; }
    }
    public abstract record EDelta
    {
        public sealed record TokenPrep : EDelta
        {
            public required ETokenPrep Value { get; init; }
        }
        public sealed record Resolve : EDelta
        {
            public required IResult<ResOpt, EExternalImplementation> Resolution { get; init; }
        }
        public sealed record PushOperation : EDelta
        {
            public required IToken OperationToken { get; init; }
        }
    }
    public abstract record EExternalImplementation
    {
        public sealed record MetaExecute : EExternalImplementation
        {
            public required IToken FunctionToken { get; init; }
            public required IEnumerable<ITiple<IStateAddress<ResObj>, ResOpt>> StateWrites { get; init; }
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