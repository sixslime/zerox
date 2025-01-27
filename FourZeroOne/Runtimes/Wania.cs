
using ControlledFlows;
using FourZeroOne.Resolution;
using FourZeroOne.Rule;
using FourZeroOne.Token.Unsafe;
using Perfection;
using MorseCode.ITask;
using FourZeroOne;
using FourZeroOne.Runtime;

namespace FourZeroOne.Runtimes
{
    using System;
    using FourZeroOne.Proxy.Unsafe;
    using LookNicePls;
    public record Wania : IRuntime
    {
        public IPStack<IToken> OperationStack => throw new NotImplementedException();

        public IPStack<IResolution> ResolutionStack => throw new NotImplementedException();

        public IPStack<IState> StateStack => throw new NotImplementedException();

        public IPStack<IToken> PreProcessingStack => throw new NotImplementedException();

        public IPStack<IPSet<IRule>> AppliedRuleStack => throw new NotImplementedException();

        public IPStack<IPSet<IProxy>> MacroExpansionStack => throw new NotImplementedException();

        public IOption<SelectionRequest> RequestedSelection => throw new NotImplementedException();

        public event EventHandler? NextTokenEvent;
        public event EventHandler? RuleAppliedEvent;
        public event EventHandler? MacroExpandedEvent;
        public event EventHandler? OperationPushedEvent;
        public event EventHandler? OperationResolvedEvent;
        public event EventHandler? SelectionRequestedEvent;
        public event EventHandler? SelectionRecievedEvent;
        public event EventHandler? BacktrackingEvent;
        public event EventHandler? BacktrackEvent;

        public void Backtrack(int resolvedOperationAmount)
        {
            throw new NotImplementedException();
        }

        void IRuntime.SendSelectionResponse<R>(SelectionRequest request, params int[] selectedIndicies)
        {
            throw new NotImplementedException();
        }
    }
}