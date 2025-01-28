
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

        public void SendSelectionResponse<R>(SelectionRequest request, params int[] selectedIndicies) where R : class, ResObj
        {
            throw new NotImplementedException();
        }

        private class TokenHandle(Wania parent) : ITokenContext
        {
            readonly Wania _parent = parent;
            IState ITokenContext.CurrentState => _parent.CurrentSnapshot.StateStack.TopValue.Unwrap();

            // consider having wania be able to 'spawn' multiple execution threads to simulate creating a new instance.
            ITask<IOption<R>> ITokenContext.MetaExecute<R>(IToken<R> token, IEnumerable<ITiple<IStateAddress, IOption<ResObj>>> args)
            {
                throw new NotImplementedException();
            }

            ITask<IOption<IEnumerable<R>>> ITokenContext.ReadSelection<R>(IEnumerable<R> from, int count)
            {
                throw new NotImplementedException();
            }
        }
    }
}