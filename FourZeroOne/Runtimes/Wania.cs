
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
        public event EventHandler? BacktrackedEvent;

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
            public required PStack<IPStack<IToken>> OperationStack { get; init; }
            public required PStack<IPStack<ResObj>> ResolutionStack { get; init; }
            public required PStack<IPStack<IState>> StateStack { get; init; }
            public required PStack<ETokenTransmuteStep> TokenTransmutationStack { get; init; }
            public required IOption<SelectionRequest> RequestedSelection { get; init; }

            public required PSet<IRule> AppliedRuleSet { get; init; }

            IEvaluationStack<IToken> IRuntimeSnapshot.OperationStack
                => new EvaluationStackWrapper<IToken>(OperationStack);
            IEvaluationStack<ResObj> IRuntimeSnapshot.ResolutionStack
                => new EvaluationStackWrapper<ResObj>(ResolutionStack);
            IEvaluationStack<IState> IRuntimeSnapshot.StateStack
                => new EvaluationStackWrapper<IState>(StateStack);
            IPStack<ETokenTransmuteStep> IRuntimeSnapshot.TokenTransmutationStack => TokenTransmutationStack;
            IOption<SelectionRequest> IRuntimeSnapshot.RequestedSelection => RequestedSelection;
            private class EvaluationStackWrapper<T>(IPStack<IPStack<T>> evalStack) : IEvaluationStack<T>
            {
                public readonly IPStack<IPStack<T>> Reference = evalStack;
                public T TopValue => Reference.TopValue.Unwrap().TopValue.Unwrap();
                public int ExpressionDepth => Reference.Count;
                public int ArgIndex => Reference.TopValue.Unwrap().Count;

                public IEvaluationStack<T> Ascend(int amount)
                    => new EvaluationStackWrapper<T>(Reference.At(amount).Unwrap());

                public IEvaluationStack<T> Back(int amount)
                {
                    return new EvaluationStackWrapper<T>(
                    (amount, Reference)
                        .Sequence(x => (x.amount - x.Reference.Count, x.Reference.At(1).Unwrap()))
                        .Until(x => x.amount < x.Reference.Count)
                        .Last()
                        .ExprAs(x => x.Reference.At(x.amount))
                        .Unwrap());
                }
            }
        }
    }
}