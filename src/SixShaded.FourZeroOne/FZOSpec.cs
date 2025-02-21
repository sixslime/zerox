using MorseCode.ITask;
using SixShaded.FourZeroOne;
using SixShaded.NotRust;
using SixShaded.SixLib.GFunc;
#nullable enable
namespace SixShaded.FourZeroOne.FZOSpec
{
    using Token;
    using Res = Resolution.IResolution;
    using Rule = Rule.Unsafe.IRule<Resolution.IResolution>;
    using ResOpt = IOption<Resolution.IResolution>;
    using Token = IToken<Resolution.IResolution>;
    using Addr = Resolution.IMemoryAddress<Resolution.IResolution>;
    using Resolution;
    using Resolution.Unsafe;
    using Rule;
    using System.Diagnostics.CodeAnalysis;
    using SixShaded.NotRust;

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
        public ITask<int[]> GetSelection(IHasElements<Res> pool, int count);
    }
    public interface IStateFZO
    {
        public IOption<FZOSource> Initialized { get; }
        public IEnumerable<IOperationNode> OperationStack { get; }
        public IEnumerable<ETokenMutation> TokenMutationStack { get; }

        // FIXME: needs updated specification
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
        public IStateFZO Initialize(FZOSource source);
        public interface IOperationNode
        {
            public Token Operation { get; }
            public IEnumerable<ResOpt> ArgResolutionStack { get; }
            public IEnumerable<IMemoryFZO> MemoryStack { get; }
        }
    }
    public interface IMemoryFZO
    {
        public IEnumerable<ITiple<Addr, Res>> Objects { get; }
        public IEnumerable<Rule> Rules { get; }
        public IEnumerable<ITiple<RuleID, int>> RuleMutes { get; }
        public IOption<R> GetObject<R>(IMemoryAddress<R> address) where R : class, Res;
        public int GetRuleMuteCount(RuleID ruleId);
        // Rules is an ordered sequence allowing duplicates.
        // 'WithRules' appends.
        // 'WithoutRules' removes the *first* instance of each rule in sequence.
        // Rules have a public ID assigned at creation, equality is based on ID.
        public IMemoryFZO WithRules(IEnumerable<Rule> rules);
        public IMemoryFZO WithRuleMutes(IEnumerable<RuleID> mutes);
        public IMemoryFZO WithoutRuleMutes(IEnumerable<RuleID> mutes);
        public IMemoryFZO WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) where R : class, Res;
        public IMemoryFZO WithClearedAddresses(IEnumerable<Addr> removals);
    }

    namespace Shorthands
    {
        public static class Extensions
        {
            public static IMemoryFZO WithResolution(this IMemoryFZO memory, Res resolution)
            {
                return resolution.Instructions.AccumulateInto(memory, (mem, instruction) => instruction.TransformMemoryUnsafe(mem));
            }
            public static IMemoryFZO WithResolution(this IMemoryFZO memory, ResOpt resolution)
            {
                return resolution.Check(out var r) ? memory.WithResolution(r) : memory;
            }
        }
    }

    public sealed record FZOSource
    {
        public required Token Program { get; init; }
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
            public required IResult<ResOpt, EStateImplemented> Resolution { get; init; }
        }
        public sealed record PushOperation : EProcessorStep
        {
            public required Token OperationToken { get; init; }
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
            public required Token Token { get; init; }
            public IEnumerable<ITiple<Addr, ResOpt>> ObjectWrites { get; init; } = [];
            public IEnumerable<RuleID> RuleMutes { get; init; } = [];
            public IEnumerable<RuleID> RuleAllows { get; init; } = [];
        }
    }
    public abstract record ETokenMutation
    {
        public required Token Result { get; init; }
        public sealed record Identity : ETokenMutation { }
        public sealed record RuleApply : ETokenMutation
        {
            public required Rule Rule { get; init; }
        }
    }
}