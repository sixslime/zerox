
using System.Collections.Generic;
using Perfection;
using ControlledFlows;
using System.Threading.Tasks;
using System.Diagnostics;
using MorseCode.ITask;
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
        public ITask<IOption<R>> PerformAction<R>(IToken<R> action) where R : class, ResObj;
        public ITask<IOption<IEnumerable<R>>> ReadSelection<R>(IEnumerable<R> from, int count) where R : class, ResObj;
    }

    public abstract class FrameSaving : Runtime.IRuntime
    {
        
        public FrameSaving(State startingState, IToken program)
        {
            _stateStack = new LinkedStack<State>(startingState).AsSome();
            _operationStack = new LinkedStack<IToken>(program).AsSome();
            _resolutionStack = new None<LinkedStack<Resolved>>();
            _runThread = ControlledFlow.Resolved((Resolved)(new None<ResObj>()));
            _frameStack = new None<LinkedStack<Frame>>();
            _appliedRuleStack = new None<LinkedStack<PList<Rule.IRule>>>();
            StoreFrame(program, new None<Resolved>());
            _discontinueEval = false;
        }
        public async Task<Resolved> Run()
        {
            _runThread = new ControlledFlow<Resolved>();
            StartEval();
            return await _runThread;
        }
        private void ResolveRun(Resolved resolution)
        {
            _runThread.Resolve(resolution);
        }
        public State GetState() => _stateStack.Check(out var state) ? state.Value : throw new Exception("[FrameSaving Runtime] No state exists on state stack?");
        public ITask<IOption<R>> PerformAction<R>(IToken<R> action) where R : class, ResObj
        {
            var node = _operationStack.Unwrap();
            if (node.Value is not Core.Tokens.PerformAction<R> pToken)
            {
                throw new System.Exception("[FrameSaving Runtime] PerformAction() called when a PerformAction token was not at the top of the operation stack.");
            }
            // directly replaces the PerformAction token with it's stored Action token in the operation stack.
            _operationStack = (node with
            {
                Value = action
            }).AsSome();
            _discontinueEval = true;
            return Task.FromResult(new None<R>()).AsITask();

            // This thread should be ceased, as it is part of the eval thread.

        }
        public ITask<IOption<IEnumerable<R>>> ReadSelection<R>(IEnumerable<R> from, int count) where R : class, ResObj
        {
            var o = SelectionImplementation(from, count, out var frameShiftOpt);
            if (frameShiftOpt.Check(out var targetFrame)) GoToFrame(targetFrame);
            return o;
        }
        protected abstract void RecieveToken(IToken token, int depth);
        protected abstract void RecieveResolution(IOption<ResObj> resolution, int depth);
        protected abstract void RecieveFrame(LinkedStack<Frame> frameStackNode);
        protected abstract void RecieveMacroExpansion(IToken macro, IToken expanded);
        protected abstract void RecieveRuleSteps(IEnumerable<(IToken token, Rule.IRule appliedRule)> steps);
        protected abstract ITask<IOption<IEnumerable<R>>> SelectionImplementation<R>(IEnumerable<R> from, int count, out IOption<LinkedStack<Frame>> orFrameShift) where R : class, ResObj;

        protected void GoToFrame(LinkedStack<Frame> frameStack)
        {
            var frame = frameStack.Value;
            _operationStack = frame.OperationStack;
            _resolutionStack = frame.ResolutionStack;
            _stateStack = frame.StateStack;
            _frameStack = frameStack.AsSome();
            _discontinueEval = true;
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
            public IEnumerable<LinkedStack<T>> ThroughStack()
            {
                return (this.AsSome() as IOption<LinkedStack<T>>)
                    .Sequence(x => x.RemapAs(y => y.Link).Press())
                    .TakeWhile(x => x.IsSome())
                    .Map(x => x.Unwrap());
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

        private async void StartEval()
        {
            while (_operationStack.Check(out var operationNode))
            {
                // _stateStack should never be empty, depth 0 is the starting state.
                var currentStateNode = _stateStack.Unwrap();
                var rulesToApply = currentStateNode.Value.Rules.Elements;

                if (_appliedRuleStack.Check(out var appliedRuleNode))
                {
                    for (int t = appliedRuleNode.Depth - currentStateNode.Depth; t > 0; t--)
                    {
                        _ = PopFromStack(ref _appliedRuleStack);
                    }
                    if ( _appliedRuleStack.Check(out appliedRuleNode) && appliedRuleNode is not null)
                    {
                        rulesToApply = rulesToApply.Except(appliedRuleNode.Value.Elements);
                    }
                }
                
                var ruledToken = ApplyRules(operationNode.Value, rulesToApply, out var appliedRules);
                rulesToApply = rulesToApply.Except(appliedRules.Elements.Map(x => x.rule));
                if (ruledToken is Macro.Unsafe.IMacro macro)
                {
                    var expanded = macro.ExpandUnsafe();
                    RecieveMacroExpansion(macro, expanded);
                    ruledToken = ApplyRules(expanded, rulesToApply, out var appliedPostMacro);
                    // Assert(appliedRules.Count = 0 || appliedPostMacro.Count = 0); logically right?
                    appliedRules = appliedRules with { dElements = Q => Q.Also(appliedPostMacro.Elements) };
                }
                RecieveRuleSteps(appliedRules.Elements);
                RecieveToken(ruledToken, operationNode.Depth);
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
                    var resolution = await operationNode.Value.ResolveUnsafe(this, argPass);
                    if (_discontinueEval)
                    {
                        _discontinueEval = false;
                        continue;
                    }
                    RecieveResolution(resolution, operationNode.Depth);

                    var poppedStateNode = PopFromStack(ref _stateStack);
                    if (_stateStack.Check(out var linkedStateNode) && linkedStateNode.Depth == poppedStateNode.Depth)
                    {
                        var newState = resolution.Check(out var notNolla)
                            ? poppedStateNode.Value.WithResolution(notNolla)
                            : poppedStateNode.Value;
                        _stateStack = (linkedStateNode with { Value = newState }).AsSome();
                    }
                    PushToStack(ref _resolutionStack, operationNode.Depth, resolution);
                    _ = PopFromStack(ref _operationStack);
                    StoreFrame(operationNode.Value, resolution.AsSome());
                }
                else
                {
                    PushToStack(ref _operationStack, operationNode.Depth + 1, operationNode.Value.ArgTokens.AsMutList().Reversed());
                    PushToStack(ref _stateStack, currentStateNode.Depth + 1, currentStateNode.Value.Yield(argAmount));
                    var previousRules = _appliedRuleStack.Check(out var ruleStack) ? ruleStack.Value.Elements : [];
                    PushToStack(ref _appliedRuleStack, operationNode.Depth + 1, new PList<Rule.IRule>() { Elements = previousRules.Also(appliedRules.Elements.Map(x => x.rule))});
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
        private static IToken ApplyRules(IToken token, IEnumerable<Rule.IRule> rules, out PList<(IToken fromToken, Rule.IRule rule)> appliedRules)
        {
            var o = token;
            var appliedRulesList = new List<(IToken fromToken, Rule.IRule rule)>();
            foreach (var rule in rules)
            {
                if (rule.TryApply(o).Check(out var newToken))
                {
                    appliedRulesList.Add((o, rule));
                    o = newToken;
                }
            }
            appliedRules = new() { Elements = appliedRulesList };
            return o;
        }
        private ControlledFlow<Resolved> _runThread;
        // I guess _appliedRuleStack could be a stack of normal IEnumerables, but PList has P in it
        private IOption<LinkedStack<PList<Rule.IRule>>> _appliedRuleStack;
        private IOption<LinkedStack<State>> _stateStack;
        private IOption<LinkedStack<Frame>> _frameStack;
        private IOption<LinkedStack<IToken>> _operationStack;
        private IOption<LinkedStack<Resolved>> _resolutionStack;
        private bool _discontinueEval;
    }
}