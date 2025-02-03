
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

        public IRuntimeSnapshot CurrentSnapshot => _internalCurrentSnapshot;
        private Snapshot _internalCurrentSnapshot => (Snapshot)_fullHistory.TopValue.Unwrap();

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

        // OPTIMIZE: could probably use less bullshit abusing.
        private class TransformationHandler(Wania parent)
        {
            public readonly Wania Parent = parent;
            private Wania _p => Parent;
            private readonly static EventArgs NO_ARG = EventArgs.Empty;
            public void StartRuntime(IToken program, IState state)
            {
                _p._fullHistory = new PStack<IRuntimeSnapshot>();
                _p._backtrackFrames = new PStack<IPStack<IRuntimeSnapshot>>();
            }
            public void NextToken(IToken token)
            {
                MakeSnapshot(s => s with
                {
                    TokenTransmutationStack = new PStack<ETokenTransmuteStep>()
                        .WithEntries(new ETokenTransmuteStep.Identity() { Result = token }),
                    AppliedRuleSet = new PSet<IRule>()
                });
                _p.NextTokenEvent?.Invoke(_p, NO_ARG);
            }
            public void RuleApplied(IRule rule, IToken result)
            {
                MakeSnapshot(s => s with
                {
                    TokenTransmutationStack =
                        s.TokenTransmutationStack.WithEntries(new ETokenTransmuteStep.RuleApplication()
                        {
                            Rule = rule,
                            Result = result
                        }),
                    AppliedRuleSet = s.AppliedRuleSet.WithEntries(rule)
                });
                _p.RuleAppliedEvent?.Invoke(_p, NO_ARG);
            }
            public void MacroExpanded(IToken result)
            {
                MakeSnapshot(s => s with
                {
                    TokenTransmutationStack =
                        s.TokenTransmutationStack.WithEntries(new ETokenTransmuteStep.MacroExpansion()
                        {
                            Result = result
                        })
                });
                _p.MacroExpandedEvent?.Invoke(_p, NO_ARG);
            }
            // CAUTION: assumes runtime works correctly :)
            public void OperationPush(IToken operation)
            {
                bool increaseDepth =
                    _p._internalCurrentSnapshot.OperationStack.Count == 0 ||
                    _p.CurrentSnapshot.OperationStack.ExprAs(
                        x => x.Ascend(1).TopValue.ArgTokens.Length == x.ArgIndex + 1);
                MakeSnapshot(s => s with
                {
                    OperationStack =
                        s.OperationStack.ExprAs(
                            levels => (increaseDepth)
                                ? levels.WithEntries(new PStack<IToken>().WithEntries(operation))
                                : levels.MapTopValue(levelOps => levelOps.WithEntries(operation)))
                });
                _p.OperationPushedEvent?.Invoke(_p, NO_ARG);
            }
            public void OperationResolve(IOption<ResObj> resolution)
            {
                MakeSnapshot(s => s with
                {
                    OperationStack =
                        s.OperationStack.ExprAs(
                            levels => (levels.TopValue.Expect("Empty OperationStack on OperationResolve()").Count > 1)
                                ? levels.MapTopValue(levelOps => levelOps.At(1).Unwrap())
                                : levels.At(1).Unwrap()),
                    ResolutionStack =
                        s.ResolutionStack.WithEntries(
                        
                                    
                });
                _p.OperationResolvedEvent?.Invoke(_p, NO_ARG);
            }
            private void MakeSnapshot(Func<Snapshot, Snapshot> change)
            {
                PushSnapshotDirect(change(_p._internalCurrentSnapshot));
            }
            private void PushSnapshotDirect(Snapshot snapshot)
            {
                Mut.the(ref Parent._fullHistory, x => x.WithEntries(snapshot));
            }
        }
    }
}