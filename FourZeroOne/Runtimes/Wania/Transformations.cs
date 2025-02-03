
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
    public partial class Wania
    {
        // pushed whenever ANYTHING changes
        private IPStack<IRuntimeSnapshot> _fullHistory = new PStack<IRuntimeSnapshot>();
        // pushed on resolve (element of '_fullHistory').
        private IPStack<IPStack<IRuntimeSnapshot>> _backtrackFrames = new PStack<IPStack<IRuntimeSnapshot>>();

        public IRuntimeSnapshot CurrentSnapshot => _fullHistory.TopValue.Unwrap();

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
    }
}