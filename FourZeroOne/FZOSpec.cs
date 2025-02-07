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
        public IEnumerable<ETokenPrep> TokenPrepStack { get; }

        /// <summary>
        /// If <paramref name="step"/> is:<br></br>
        /// <b><see cref="EProcessorStep.TokenPrep"/>:</b><br></br>
        /// - Push 'Value' to <i>TokenPrepStack</i><br></br>
        /// <b><see cref="EProcessorStep.PushOperation"/>:</b><br></br>
        /// - Push the following to <i>OperationStack</i>:<br></br>
        /// . ~ <i>Operation</i> = 'OperationToken'<br></br>
        /// . ~ <i>MemoryStack</i> = { <i>OperationStack[0].MemoryStack[0]</i> }<br></br>
        /// . ~ <i>ArgResolutionStack</i> = { }<br></br>
        /// <b><see cref="EProcessorStep.Resolve"/>:</b><br></br>
        /// - Pop from <i>OperationStack</i><br></br>
        /// - If 'Resolution' is <b><see cref="IOk{int,}"/></b>:<br></br>
        /// - - Push 'Value' to <i>OperationStack[0].ArgResolutionStack</i><br></br>
        /// - - If 'Value' is <b><see cref="IOk{T, E}"/></b>:<br></br>
        /// - - - Push the equivalent of the following to <i>OperationStack[0].MemoryStack</i>:<br></br>
        /// - - . # <c> Value.Instructions </c>\<br></br>
        /// - - . # <c> .AccumulateInto(OperationStack[0].MemoryStack[0], </c>\<br></br>
        /// - - . # <c> (memory, instruction) => instruction.TransformMemory(memory)); </c><br></br>
        /// - If 'Resolution' is <b><see cref="IErr{T, E}"/></b>:<br></br>
        /// - - If 'Value' is <b><see cref="EStateImplemented.MetaExecute"/></b>:<br></br>
        /// - - - Push the following to <i>OperationStack</i>:<br></br>
        /// - - . ~ <i>Operation</i> = 'FunctionToken'<br></br>
        /// - - . ~ <i>MemoryStack</i> = { <i>OperationStack[0].MemoryStack[0]</i> }<br></br>
        /// - - . ~ <i>ArgResolutionStack</i> = { }<br></br>
        /// </summary>
        /// <param name="step"></param>
        /// <returns>
        /// A new <see cref="IStateFZO"/> with the above changes.<br></br>
        /// This <b>must</b> not mutate the original <see cref="IStateFZO"/>.
        /// </returns>
        public IStateFZO WithStep(EProcessorStep step);
        public IStateFZO Initialize(FZOSource source);
        public interface IOperationNode
        {
            public IToken Operation { get; }
            public IEnumerable<ResOpt> ArgResolutionStack { get; }
            public IEnumerable<IMemoryFZO> MemoryStack { get; }
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
    public abstract record EProcessorStep
    {
        public sealed record TokenPrep : EProcessorStep
        {
            public required ETokenPrep Value { get; init; }
        }
        public sealed record Resolve : EProcessorStep
        {
            public required IResult<ResOpt, EStateImplemented> Resolution { get; init; }
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
    public abstract record EStateImplemented
    {
        public sealed record MetaExecute : EStateImplemented
        {
            public required IToken FunctionToken { get; init; }
            public required IEnumerable<ITiple<IStateAddress<ResObj>, ResOpt>> StateWrites { get; init; }
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