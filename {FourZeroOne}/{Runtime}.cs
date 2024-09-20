
using System.Collections.Generic;
using Perfection;
using ControlledFlows;
using System.Threading.Tasks;
using System.Diagnostics;
#nullable enable
namespace FourZeroOne.Runtime
{
    using ResObj = Resolution.IResolution;
    using Resolved = IOption<Resolution.IResolution>;
    using IToken = Token.Unsafe.IToken;
    using Token;
    public interface IRuntime
    {
        public State GetState();
        public Task<Resolved> Run();
        public ICeasableFlow<IOption<R>> PerformAction<R>(IToken<R> action) where R : class, ResObj;
        public ICeasableFlow<IOption<IEnumerable<R>>> ReadSelection<R>(IEnumerable<R> from, int count) where R : class, ResObj;
    }

    public abstract class FrameSaving : Runtime.IRuntime
    {
        public FrameSaving(State startingState, IToken program)
        {
            _stateStack = new LinkedStack<State>(startingState).AsSome();
            _operationStack = new LinkedStack<IToken>(program).AsSome();
            _resolutionStack = new None<LinkedStack<Resolved>>();
            _evalThread = ControlledFlow.Resolved(new None<ResObj>());
            _runThread = ControlledFlow.Resolved((Resolved)(new None<ResObj>()));
            _frameStack = new None<LinkedStack<Frame>>();
            StoreFrame(program, new None<Resolved>());
        }
        public async Task<Resolved> Run()
        {
            _runThread = new ControlledFlow<Resolved>();
            StartEvalThread();
            return await _runThread;
        }
        private void ResolveRun(Resolved resolution)
        {
            _runThread.Resolve(resolution);
        }
        public State GetState() => _stateStack.Check(out var state) ? state.Value : throw new Exception("[FrameSaving Runtime] No state exists on state stack?");
        public ICeasableFlow<IOption<R>> PerformAction<R>(IToken<R> action) where R : class, ResObj
        {
            var node = _operationStack.Unwrap();
            if (node.Value is not Core.Tokens.PerformAction<R> pToken)
            {
                throw new System.Exception("[FrameSaving Runtime] PerformAction() called when a PerformAction token was not at the top of the operation stack.");
            }
            // directly replaces the PerformAction token with it's stored Action token in the operation stack.
            _operationStack = (node with
            {
                Value = pToken.Arg1
            }).AsSome();
            var thisThread = _evalThread;
            StartEvalThread();
            thisThread.Cease();
            // This thread should be ceased, as it is part of the eval thread.

            throw new System.Exception("Fuck!");
        }
        public ICeasableFlow<IOption<IEnumerable<R>>> ReadSelection<R>(IEnumerable<R> from, int count) where R : class, ResObj
        {
            return SelectionImplementation(from, count);
        }

        protected abstract void RecieveToken(IToken token);
        protected abstract void RecieveResolution(IOption<ResObj> resolution);
        protected abstract void RecieveFrame(LinkedStack<Frame> frameStackNode);
        protected abstract void RecieveMacroExpansion(IToken macro, IToken expanded);
        protected abstract void RecieveRuleSteps(IEnumerable<(IToken token, Rule.IRule appliedRule)> steps);
        protected abstract ICeasableFlow<IOption<IEnumerable<R>>> SelectionImplementation<R>(IEnumerable<R> from, int count) where R : class, ResObj;

        protected void GoToFrame(LinkedStack<Frame> frameStack)
        {
            var frame = frameStack.Value;
            _operationStack = frame.OperationStack;
            _resolutionStack = frame.ResolutionStack;
            _stateStack = frame.StateStack;
            _frameStack = frameStack.AsSome();
            _evalThread.Cease();
            StartEvalThread();
        }

        protected record Frame
        {
            public required IToken Token { get; init; }
            public required IOption<Resolved> Resolution { get; init; }
            public required IOption<LinkedStack<State>> StateStack { get; init; }
            public required IOption<LinkedStack<IToken>> OperationStack { get; init; }
            public required IOption<LinkedStack<Resolved>> ResolutionStack { get; init; }
        }
        protected record LinkedStack<T>
        {
            public readonly IOption<LinkedStack<T>> Link;
            public readonly int Depth;
            public T Value { get; init; }
            public LinkedStack(T value)
            {
                Value = value;
                Link = this.None();
                Depth = 0;
            }
            public static IOption<LinkedStack<T>> Linked(IOption<LinkedStack<T>> parent, int depth, IEnumerable<T> values)
            {
                return values.AccumulateInto(parent, (stack, x) => new LinkedStack<T>(stack, x, depth).AsSome());
            }
            public static IOption<LinkedStack<T>> Linked(IOption<LinkedStack<T>> parent, int depth, params T[] values) { return Linked(parent, depth, values.IEnumerable()); }
            private LinkedStack(IOption<LinkedStack<T>> link, T value, int depth)
            {
                Link = link;
                Value = value;
                Depth = depth;
            }
        }

        private static (IToken token, State state) RuleStep(IToken token, State state, out List<(IToken fromToken, Rule.IRule appliedRule)> appliedRules)
        {
            var oToken = ApplyRules(token, state.Rules.Elements, out var internalAppliedRules);
            var oState = state with
            {
                dRules = Q => Q with
                {
                    dElements = Q => Q.Where(x => !internalAppliedRules.Map(a => a.rule).HasMatch(y => x == y))
                }
            };
            appliedRules = internalAppliedRules;
            return (oToken, oState);
        }
        private async void StartEvalThread()
        {
            while (_operationStack.Check(out var operationNode))
            {
                // _stateStack should never be empty, depth 0 is the starting state.
                var currentStateNode = _stateStack.Unwrap();

                var (ruledToken, stateMinusApplied) = RuleStep(operationNode.Value, currentStateNode.Value, out var appliedRules);
                if (ruledToken is Macro.Unsafe.IMacro macro)
                {
                    var expanded = macro.ExpandUnsafe();
                    RecieveMacroExpansion(macro, expanded);
                    (ruledToken, stateMinusApplied) = RuleStep(expanded, stateMinusApplied, out var appliedPostMacro);
                    // Assert(appliedRules.Count = 0 || appliedPostMacro.Count = 0); logically right?
                    appliedRules.AddRange(appliedPostMacro);
                }
                PushToStack(ref _stateStack, currentStateNode.Depth, stateMinusApplied);
                RecieveRuleSteps(appliedRules);
                RecieveToken(ruledToken);
                operationNode = operationNode with { Value = ruledToken };
                _operationStack = operationNode.AsSome();

                int argAmount = operationNode.Value.ArgTokens.Length;

                if (argAmount == 0 || (_resolutionStack.Check(out var resolutionNode) && resolutionNode.Depth == operationNode.Depth + 1))
                {
                    var argPass = new Resolved[argAmount];
                    for (int i = argAmount - 1; i >= 0; i--)
                    {
                        argPass[i] = PopFromStack(ref _resolutionStack).Value;
                    }
                    _evalThread = operationNode.Value.ResolveUnsafe(this, argPass);
                    var resolution = await _evalThread;
                    RecieveResolution(resolution);
                    if (resolution.Check(out var notNolla)) _currentState = _currentState.WithResolution(notNolla);
                    PushToStack(ref _resolutionStack, operationNode.Depth, resolution);
                    PopFromStack(ref _operationStack);
                    StoreFrame(operationNode.Value, resolution.AsSome());
                }
                else
                {
                    PushToStack(ref _operationStack, operationNode.Depth + 1, operationNode.Value.ArgTokens.AsMutList().Reversed());
                }
            }

            Debug.Assert(_resolutionStack.Check(out var finalNode) && !finalNode.Link.IsSome());
            ResolveRun(finalNode.Value);
        }
        private void StoreFrame(IToken token, IOption<Resolved> resolution)
        {
            var newFrame = new Frame()
            {
                Resolution = resolution,
                Token = token,
                StateStack = _stateStack,
                OperationStack = _operationStack,
                ResolutionStack = _resolutionStack,
            };
            PushToStack(ref _frameStack, 0, newFrame);
            RecieveFrame(_frameStack.Unwrap());
        }
        private static void PushToStack<T>(ref IOption<LinkedStack<T>> stack, int depth, IEnumerable<T> values)
        {
            stack = LinkedStack<T>.Linked(stack, depth, values);
        }
        private static LinkedStack<T> PopFromStack<T>(ref IOption<LinkedStack<T>> stack)
        {
            var o = stack.Check(out var popped) ? popped : throw new System.Exception("[FrameSaving Runtime] tried to pop from empty LinkedStack.");
            if (stack.Check(out var node)) stack = node.Link;
            return o;
        }
        private static void PushToStack<T>(ref IOption<LinkedStack<T>> stack, int depth, params T[] values) { PushToStack(ref stack, depth, values.IEnumerable()); }
        private static IToken ApplyRules(IToken token, IEnumerable<Rule.IRule> rules, out List<(IToken fromToken, Rule.IRule rule)> appliedRules)
        {
            var o = token;
            appliedRules = new();
            foreach (var rule in rules)
            {
                if (rule.TryApply(o).Check(out var newToken))
                {
                    appliedRules.Add((o, rule));
                    o = newToken;
                }
            }
            return o;
        }
        private ICeasableFlow<Resolved> _evalThread;
        private ControlledFlow<Resolved> _runThread;
        private IOption<LinkedStack<PList<Rule.IRule>>> _appliedRuleStack;
        private IOption<LinkedStack<State>> _stateStack;
        private IOption<LinkedStack<Frame>> _frameStack;
        private IOption<LinkedStack<IToken>> _operationStack;
        private IOption<LinkedStack<Resolved>> _resolutionStack;
    }
}