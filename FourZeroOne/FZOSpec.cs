using MorseCode.ITask;
using Perfection;
#nullable enable
namespace FourZeroOne.FZOSpec
{
    using Token;
    using any_res = Resolution.IResolution;
    using any_rule = Rule.Unsafe.IRule<Resolution.IResolution>;
    using res_opt = IOption<Resolution.IResolution>;
    using any_token = Token.IToken<Resolution.IResolution>;
    using mem_address = Resolution.IMemoryAddress<Resolution.IResolution>;
    using Resolution;
    using Resolution.Unsafe;
    using Rule;
    using System.Diagnostics.CodeAnalysis;
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
        public ITask<int[]> GetSelection(IHasElements<any_res> pool, int count);
    }
    public interface IStateFZO
    {
        public IOption<FZOSource> Initialized { get; }
        public IEnumerable<IOperationNode> OperationStack { get; }
        public IEnumerable<ETokenMutation> TokenPrepStack { get; }

        // MetaExecute is not accurate, plz update.
        /// <summary>
        /// If <paramref name="step"/> is:<br></br>
        /// <b><see cref="EProcessorStep.TokenMutate"/>:</b><br></br>
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

        // FIXME: FZOSource.Program cannot be directly pushed to operation stack if it's a macro
        public IStateFZO Initialize(FZOSource source);
        public interface IOperationNode
        {
            public any_token Operation { get; }
            public IEnumerable<res_opt> ArgResolutionStack { get; }
            public IEnumerable<IMemoryFZO> MemoryStack { get; }
        }
    }
    public interface IMemoryFZO
    {
        public IEnumerable<ITiple<mem_address, any_res>> Objects { get; }
        public IEnumerable<any_res> Rules { get; }
        public IEnumerable<ITiple<RuleID, int>> RuleMutes { get; }
        public IOption<R> GetObject<R>(IMemoryAddress<R> address) where R : class, any_res;
        public int GetRuleMuteCount(RuleID ruleId);
        // Rules is an ordered sequence allowing duplicates.
        // 'WithRules' appends.
        // 'WithoutRules' removes the *first* instance of each rule in sequence.
        // Rules have a public ID assigned at creation, equality is based on ID.
        public IMemoryFZO WithRules(IEnumerable<any_res> rules);
        public IMemoryFZO WithRuleMutes(IEnumerable<RuleID> mutes);
        public IMemoryFZO WithoutRuleMutes(IEnumerable<RuleID> mutes);
        public IMemoryFZO WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) where R : class, any_res;
        public IMemoryFZO WithClearedAddresses(IEnumerable<mem_address> removals);
    }

    namespace Shorthands
    {
        public static class Extensions
        {
            public static IMemoryFZO WithResolution(this IMemoryFZO memory, any_res resolution)
            {
                return resolution.Instructions.AccumulateInto(memory, (mem, instruction) => instruction.TransformMemoryUnsafe(mem));
            }
            public static IMemoryFZO WithResolution(this IMemoryFZO memory, res_opt resolution)
            {
                return resolution.Check(out var r) ? memory.WithResolution(r) : memory;
            }
        }
    }

    public sealed record FZOSource
    {
        public required any_token Program { get; init; }
        public required IMemoryFZO InitialMemory { get; init; }
    }
    public abstract record EProcessorStep
    {
        public sealed record TokenMutate : EProcessorStep
        {
            public required ETokenMutation Mutation { get; init; }
        }
        public sealed record Resolve : EProcessorStep
        {
            public required IResult<res_opt, EStateImplemented> Resolution { get; init; }
        }
        public sealed record PushOperation : EProcessorStep
        {
            public required any_token OperationToken { get; init; }
        }
    }
    public abstract record EProcessorHalt
    {
        public required IStateFZO HaltingState { get; init; }
        public sealed record InvalidState : EProcessorHalt { }
        public sealed record Completed : EProcessorHalt
        {
            public required res_opt Resolution { get; init; }
        }
    }
    public abstract record EStateImplemented
    {
        public sealed record MetaExecute : EStateImplemented
        {
            public required any_token Token { get; init; }
            public IHasElements<ITiple<mem_address, res_opt>> ObjectWrites { get; init; } = new PSequence<ITiple<mem_address, res_opt>>();
            public IHasElements<RuleID> RuleMutes { get; init; } = new PSequence<RuleID>();
            public IHasElements<RuleID> RuleAllows { get; init; } = new PSequence<RuleID>();
        }
    }
    public abstract record ETokenMutation
    {
        public required any_token Result { get; init; }
        public sealed record Identity : ETokenMutation { }
        public sealed record RuleApply : ETokenMutation
        {
            public required any_rule Rule { get; init; }
        }
    }
}