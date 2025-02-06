using MorseCode.ITask;
using Perfection;
#nullable enable
namespace FourZeroOne.Implementations.Wania
{
    using Logical;
    using Token;
    using IToken = Token.Unsafe.IToken;
    using ResObj = Resolution.IResolution;
    using Resolution;
    using Resolution.Unsafe;
    using Rule;
    using ResOpt = IOption<Resolution.IResolution>;
    
    public class Evaluator() : IProcessor
    {
        public ITask<IResult<EStep, EHalt>> GetNextStep(IState state, IInput input) => StaticImplementation(state, input);
        private static async ITask<IResult<EStep, EHalt>> StaticImplementation(IState state, IInput input)
        {
            // i bet this shit could be written in 1 line.

            // - caching
            var stdHint = Hint<EHalt>.HINT;
            IResult<EStep, EHalt> invalidStateResult = new EHalt.InvalidState { HaltingState = state }.AsErr(Hint<EStep>.HINT);

            // assert that both a state and operation are present:
            if (state.OperationStack.GetAt(0).CheckNone(out var topNode) ||
                state.MemoryStack.GetAt(0).CheckNone(out var topState))
                return invalidStateResult;

            // - caching
            TokenContext tokenContext = new()
            {
                CurrentMemory = topState,
                Input = input
            };
            var argsArray = topNode.ResolvedArgs.ToArray();

            // assert there are not more resolutions than the operation takes:
            if (argsArray.Length > topNode.Operation.ArgTokens.Length) return invalidStateResult;

            // send 'Resolve' if all operation's args are resolved:
            if (argsArray.Length == topNode.Operation.ArgTokens.Length)
            {
                var resolvedOperation =
                    topNode.Operation.UnsafeResolve(tokenContext, argsArray)
                    .CheckOk(out var resolutionTask, out var runtimeHandled)
                        ? (await resolutionTask).AsOk(Hint<Resolution.EEvaluatorHandled>.HINT)
                        : runtimeHandled.AsErr(Hint<ResOpt>.HINT);
                return
                    (resolvedOperation.CheckErr(out var _, out var finalResolution) || state.OperationStack.GetAt(1).IsSome())
                    .ToResult(
                        new EStep.Resolve() { Resolution = resolvedOperation },
                        new EHalt.Completed() { HaltingState = state, Resolution = finalResolution });
            }

            // continue token processing if there exists preprocesses on the stack:
            if (state.PreprocessStack.GetAt(0).Check(out var processingToken))
            {
                var token = processingToken.Result;
                Dictionary<Rule.IRule, int> previousApplications = new();

                // WARNING:
                // this method of uncached checking for previously applied rules is inefficient for large amounts of preprocess steps.

                // - populate previousApplications
                foreach (var process in state.PreprocessStack)
                {
                    if (process is not EPreprocess.RuleApplication ra) continue;
                    var appliedRule = ra.Rule;
                    previousApplications[appliedRule] = previousApplications.TryGetValue(appliedRule, out var count) ? count + 1 : 1;
                }

                foreach (var rule in topState.Rules)
                {
                    // - skip already applied rules
                    if (previousApplications.TryGetValue(rule, out var count) && count > 0)
                    {
                        previousApplications[rule] = count - 1;
                        continue;
                    }

                    // send 'RuleApplication' if the processing token is rulable by an unapplied rule.
                    if (rule.TryApply(token).Check(out var ruledToken))
                        return new EStep.Preprocess()
                        {
                            Value = new EPreprocess.RuleApplication()
                            {
                                Rule = rule,
                                Result = ruledToken
                            }
                        }.AsOk(stdHint);
                }

                // send 'MacroExpansion' if processing token is a macro
                if (token is Macro.Unsafe.IMacro macro)
                    return new EStep.Preprocess()
                    {
                        Value = new EPreprocess.MacroExpansion()
                        {
                            Result = macro.ExpandUnsafe()
                        }
                    }.AsOk(stdHint);

                // send 'PushOperation' if no more preprocessing is needed.
                return new EStep.PushOperation() { OperationToken = token }.AsOk(stdHint);
            }

            // send 'Identity' if next operation arg is ready to be processed
            return new EStep.Preprocess()
            {
                Value = new EPreprocess.Identity()
                {
                    Result = topNode.Operation.ArgTokens[argsArray.Length]
                }
            }.AsOk(stdHint);
        }

        private class TokenContext : IProcessor.ITokenContext
        {
            public required IMemory CurrentMemory { get; init; }
            public required IInput Input { get; init; }
        }
    }
    public record Memory : IMemory
    {
        public IEnumerable<ITiple<IStateAddress, IResolution>> Objects => _objects.Elements;
        public IEnumerable<IRule> Rules => _rules.Elements;

        public Memory()
        {
            _objects = new();
            _rules = new();
        }
        IOption<R> IMemory.GetObject<R>(IStateAddress<R> address)
        {
            return _objects.At(address).RemapAs(x => (R)x);
        }

        IMemory IMemory.WithRules(IEnumerable<IRule> rules)
        {
            return this with { _rules = _rules.WithEntries(rules) };
        }

        IMemory IMemory.WithObjects<R>(IEnumerable<ITiple<IStateAddress<R>, R>> insertions)
        {
            return this with { _objects = _objects.WithEntries(insertions) };
        }

        IMemory IMemory.WithClearedAddresses(IEnumerable<IStateAddress> removals)
        {
            return this with { _objects = _objects.WithoutEntries(removals) };
        }

        private PMap<IStateAddress, IResolution> _objects;
        private PSequence<IRule> _rules;
    }
}