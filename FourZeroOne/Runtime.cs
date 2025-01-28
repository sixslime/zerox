using MorseCode.ITask;
using Perfection;
#nullable enable
namespace FourZeroOne.Runtime
{
    using Token;
    using IToken = Token.Unsafe.IToken;
    using ResObj = Resolution.IResolution;
    public interface ITokenContext
    {
        public IState CurrentState { get; }
        public ITask<IOption<R>> MetaExecute<R>(IToken<R> token, IEnumerable<ITiple<Resolution.Unsafe.IStateAddress, IOption<ResObj>>> args) where R : class, ResObj;
        public ITask<IOption<IEnumerable<R>>> ReadSelection<R>(IEnumerable<R> from, int count) where R : class, ResObj;
    }

    public interface IRuntime
    {
        public IRuntimeSnapshot CurrentSnapshot { get; }

        public event EventHandler? ProgramStartingEvent;

        public event EventHandler? NextTokenEvent;

        public event EventHandler? RuleAppliedEvent;
        public event EventHandler? MacroExpandedEvent;

        public event EventHandler? OperationPushedEvent;
        public event EventHandler? OperationResolvedEvent;

        public event EventHandler? SelectionRequestedEvent;
        public event EventHandler? SelectionRecievedEvent;

        public event EventHandler? BacktrackingEvent;
        public event EventHandler? BacktrackEvent;

        public void RunProgram(IState startingState, IToken program);
        public void Backtrack(int resolvedOperationAmount);
        public void SendSelectionResponse<R>(SelectionRequest request, params int[] selectedIndicies) where R : class, ResObj;
    }

    // fuck it, you have to use stacks, theres no reason should use anything else, fuck you.
    public interface IRuntimeSnapshot
    {
        // fucking stack of stacks because we need the token depth.
        public IEvaluationStack<IToken> OperationStack { get; }
        public IEvaluationStack<ResObj> ResolutionStack { get; }
        public IEvaluationStack<IState> StateStack { get; }
        public IPStack<IToken> TokenResultStack { get; }
        public IPStack<IPSet<Rule.IRule>> AppliedRuleStack { get; }
        public IPStack<IPSet<Proxy.Unsafe.IProxy>> MacroExpansionStack { get; }
        public IOption<SelectionRequest> RequestedSelection { get; }
    }
    public interface IEvaluationStack<out T>
    {
        public T TopValue { get; }
        public int ExpressionDepth { get; }
        public int ArgIndex { get; }
        public IEvaluationStack<T> Ascend(int amount);
        public IEvaluationStack<T> Back(int amount);
    }
    public class SelectionRequest
    {
        public required int Amount { get; init; }
        public required IPSequence<ResObj> Pool { get; init; }
    }

}