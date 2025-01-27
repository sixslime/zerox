using MorseCode.ITask;
using Perfection;
#nullable enable
namespace FourZeroOne.Runtime
{
    using Token;
    using IToken = Token.Unsafe.IToken;
    using ResObj = Resolution.IResolution;
    using Resolved = IOption<Resolution.IResolution>;
    public interface ITokenContext
    {
        public IState CurrentState { get; }
        public ITask<IOption<R>> MetaExecute<R>(IToken<R> token, IEnumerable<(Resolution.Unsafe.IStateAddress, Resolved)> args) where R : class, ResObj;
        public ITask<IOption<IEnumerable<R>>> ReadSelection<R>(IEnumerable<R> from, int count) where R : class, ResObj;
    }

    public interface IRuntime
    {
        public IPStack<IToken> OperationStack { get; }
        public IPStack<ResObj> ResolutionStack { get; }
        public IPStack<IState> StateStack { get; }
        public IPStack<IToken> PreProcessingStack { get; }
        public IPStack<IPSet<Rule.IRule>> AppliedRuleStack { get; }
        public IPStack<IPSet<Proxy.Unsafe.IProxy>> MacroExpansionStack { get; }

        public IOption<SelectionRequest> RequestedSelection { get; }

        public event EventHandler? NextTokenEvent;

        public event EventHandler? RuleAppliedEvent;
        public event EventHandler? MacroExpandedEvent;

        public event EventHandler? OperationPushedEvent;
        public event EventHandler? OperationResolvedEvent;

        public event EventHandler? SelectionRequestedEvent;
        public event EventHandler? SelectionRecievedEvent;

        public event EventHandler? BacktrackingEvent;
        public event EventHandler? BacktrackEvent;
        
        public void Backtrack(int resolvedOperationAmount);
        public void SendSelectionResponse<R>(SelectionRequest request, params int[] selectedIndicies) where R : class, ResObj;
    }

    // fuck it, you have to use stacks, theres no reason should use anything else, fuck you.
    public interface IRuntimeData
    {
        public IPStack<IToken> OperationStack { get; }
        public IPStack<ResObj> ResolutionStack { get; }
        public IPStack<IState> StateStack { get; }
        public IPStack<IToken> PreProcessingStack { get; }
        public IPStack<IPSet<Rule.IRule>> AppliedRuleStack { get; }
        public IPStack<IPSet<Proxy.Unsafe.IProxy>> MacroExpansionStack { get; }
    }
    public class SelectionRequest
    {
        public required int Amount { get; init; }
        public required IPSequence<ResObj> Pool { get; init; }
    }

}