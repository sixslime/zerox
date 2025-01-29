
using ControlledFlows;
using FourZeroOne.Resolution;
using FourZeroOne.Rule;
using FourZeroOne.Token.Unsafe;
using Perfection;
using MorseCode.ITask;
using FourZeroOne;
using FourZeroOne.Runtime;

#nullable enable
namespace FourZeroOne.Runtimes
{
    using System;
    using System.Collections.Generic;
    using FourZeroOne.Proxy.Unsafe;
    using FourZeroOne.Resolution.Unsafe;
    using FourZeroOne.Token;
    using LookNicePls;
    using ResObj = Resolution.IResolution;
    public class Wania : IRuntime
    {
        private IPStack<IRuntimeSnapshot> _snapshotStack = new PStack<IRuntimeSnapshot>();
        private IPStack<IPStack<IRuntimeSnapshot>> _backtrackFrames = new PStack<IPStack<IRuntimeSnapshot>>();

        public IRuntimeSnapshot CurrentSnapshot => throw new NotImplementedException();

        public event EventHandler? ProgramStartingEvent;
        public event EventHandler? ProgramFinishedEvent;

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

        public void RunProgram(IState startingState, IToken program)
        {
            throw new NotImplementedException();
        }

        public void SendSelectionResponse<R>(SelectionRequest request, params int[] selectedIndicies) where R : class, ResObj
        {
            throw new NotImplementedException();
        }

        private ITask<IOption<R>> RecieveMetaExecute<R>(IToken<R> token, IEnumerable<ITiple<IStateAddress, IOption<ResObj>>> args) where R : class, ResObj
        {
            throw new NotImplementedException();
        }
        private ITask<IOption<IHasElements<R>>> RecieveReadSelection<R>(IHasElements<R> from, int count)
        {
            throw new NotImplementedException();
        }
        private class TokenHandle(Wania parent) : ITokenContext
        {
            readonly Wania _parent = parent;
            IState ITokenContext.CurrentState => _parent.CurrentSnapshot.StateStack.TopValue;

            // consider having wania be able to 'spawn' multiple execution threads to simulate creating a new instance.
            ITask<IOption<R>> ITokenContext.MetaExecute<R>(IToken<R> token, IEnumerable<ITiple<IStateAddress, IOption<ResObj>>> args)
            => _parent.RecieveMetaExecute(token, args);

            ITask<IOption<IHasElements<R>>> ITokenContext.ReadSelection<R>(IHasElements<R> from, int count)
            => _parent.RecieveReadSelection(from, count);
        }
        private record Snapshot : IRuntimeSnapshot
        {
            public IEvaluationStack<IToken> OperationStack => throw new NotImplementedException();

            public IEvaluationStack<ResObj> ResolutionStack => throw new NotImplementedException();

            public IEvaluationStack<IState> StateStack => throw new NotImplementedException();

            public IPStack<IToken> TokenResultStack => throw new NotImplementedException();

            public IPStack<IPSet<IRule>> AppliedRuleStack => throw new NotImplementedException();

            public IPStack<IPSet<IProxy>> MacroExpansionStack => throw new NotImplementedException();

            public IOption<SelectionRequest> RequestedSelection => throw new NotImplementedException();

            // FIXME: heavy use of 'Unwrap()'
            // ironic. laugh.
            private class EvaluationStackWrapper<T>(IPStack<IPStack<T>> internalStack) : IEvaluationStack<T>
            {
                private IPStack<IPStack<T>> _internalStack = internalStack;
                public T TopValue => _internalStack.TopValue.Unwrap().TopValue.Unwrap();
                public int ExpressionDepth => _internalStack.Count;
                public int ArgIndex => _internalStack.TopValue.Unwrap().Count;

                public IEvaluationStack<T> Ascend(int amount)
                    => new EvaluationStackWrapper<T>(_internalStack.At(amount).Unwrap());

                public IEvaluationStack<T> Back(int amount)
                {
                    return new EvaluationStackWrapper<T>(
                    (amount, _internalStack)
                        .Sequence(x => (x.amount - x._internalStack.Count, x._internalStack.At(1).Unwrap()))
                        .Until(x => x.amount < x._internalStack.Count)
                        .Last()
                        .ExprAs(x => x._internalStack.At(x.amount))
                        .Unwrap());
                }
            }
        }
    }
}