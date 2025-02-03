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
        public ITask<IOption<IHasElements<R>>> ReadSelection<R>(IHasElements<R> from, int count) where R : class, ResObj;
    }

    // would love to use a custom delegate with no EventArgs and just a sender
    // but i suppose this is where i arbitrarily choose to follow .NET design guidelines.
    public interface IRuntime
    {
        public IRuntimeSnapshot CurrentSnapshot { get; }
        public bool IsRunning { get; }

        public event EventHandler? ProgramStartedEvent;
        public event EventHandler? ProgramFinishedEvent;

        public event EventHandler? NextTokenEvent;

        public event EventHandler? RuleAppliedEvent;
        public event EventHandler? MacroExpandedEvent;

        public event EventHandler? OperationPushedEvent;
        public event EventHandler? OperationResolvedEvent;

        public event EventHandler? SelectionRequestedEvent;
        public event EventHandler? SelectionRecievedEvent;

        public event EventHandler? BacktrackingEvent;
        public event EventHandler? BacktrackedEvent;

        public bool RunProgram(IState startingState, IToken program);
        public void Backtrack(int resolvedOperationAmount);
        // should return false if 'request' has already been fulfilled.
        // throws if indicies are out of range.
        public bool SendSelectionResponse<R>(SelectionRequest request, params int[] selectedIndicies) where R : class, ResObj;
    }

    // fuck it, you have to use stacks, theres no reason should use anything else, fuck you.
    public interface IRuntimeSnapshot
    {
        // shouldnt be double stacked, we forgot that they get dynamically popped from.
        // instead of this, we probably need to make a stack of snapshots available for full time travel coverage.
        public IEvaluationStack<IToken> OperationStack { get; }
        public IEvaluationStack<ResObj> ResolutionStack { get; }
        public IEvaluationStack<IState> StateStack { get; }
        public IPStack<ETokenTransmuteStep> TokenTransmutationStack { get; }
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
    public sealed class SelectionRequest
    {
        public required int Amount { get; init; }
        public required IPSequence<ResObj> Pool { get; init; }
    }
    // we call this going full r-word (rust)
    public abstract record ETokenTransmuteStep
    {
        public required IToken Result { get; init; }
        public sealed record Identity : ETokenTransmuteStep { }
        public sealed record MacroExpansion : ETokenTransmuteStep { }
        public sealed record RuleApplication : ETokenTransmuteStep
        {
            public required Rule.IRule Rule { get; init; }
        }
    }
    /*
    public abstract class ESelectionResponseError
    {
        public sealed class InvalidIndicies { }
        public sealed class RequestAlreadyFulfilled { }
    }
    */
}