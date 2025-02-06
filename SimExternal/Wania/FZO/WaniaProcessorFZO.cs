using MorseCode.ITask;
using Perfection;
using FourZeroOne.FZOSpec;
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using FourZeroOne.Macro.Unsafe;
using ResOpt = Perfection.IOption<FourZeroOne.Resolution.IResolution>;
#nullable enable
namespace Wania.FZO
{
    public class WaniaProcessorFZO() : IProcessorFZO
    {
        public ITask<IResult<EProcessorStep, EProcessorHalt>> GetNextStep(IStateFZO state, IInputFZO input) => StaticImplementation(state, input);
        private static async ITask<IResult<EProcessorStep, EProcessorHalt>> StaticImplementation(IStateFZO state, IInputFZO input)
        {
            // i bet this shit could be written in 1 line.

            // - caching
            var stdHint = Hint<EProcessorHalt>.HINT;
            IResult<EProcessorStep, EProcessorHalt> invalidStateResult = new EProcessorHalt.InvalidState { HaltingState = state }.AsErr(Hint<EProcessorStep>.HINT);

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
                        ? (await resolutionTask).AsOk(Hint<EProcessorImplemented>.HINT)
                        : runtimeHandled.AsErr(Hint<ResOpt>.HINT);
                return
                    (resolvedOperation.CheckErr(out var _, out var finalResolution) || state.OperationStack.GetAt(1).IsSome())
                    .ToResult(
                        new EProcessorStep.Resolve() { Resolution = resolvedOperation },
                        new EProcessorHalt.Completed() { HaltingState = state, Resolution = finalResolution });
            }

            // continue token processing if there exists preprocesses on the stack:
            if (state.TokenPrepStack.GetAt(0).Check(out var processingToken))
            {
                var token = processingToken.Result;
                Dictionary<IRule, int> previousApplications = new();

                // WARNING:
                // this method of uncached checking for previously applied rules is inefficient for large amounts of preprocess steps.

                // - populate previousApplications
                foreach (var process in state.TokenPrepStack)
                {
                    if (process is not ETokenPrep.RuleApplication ra) continue;
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
                        return new EProcessorStep.TokenPrep()
                        {
                            Value = new ETokenPrep.RuleApplication()
                            {
                                Rule = rule,
                                Result = ruledToken
                            }
                        }.AsOk(stdHint);
                }

                // send 'MacroExpansion' if processing token is a macro
                if (token is IMacro macro)
                    return new EProcessorStep.TokenPrep()
                    {
                        Value = new ETokenPrep.MacroExpansion()
                        {
                            Result = macro.ExpandUnsafe()
                        }
                    }.AsOk(stdHint);

                // send 'PushOperation' if no more preprocessing is needed.
                return new EProcessorStep.PushOperation() { OperationToken = token }.AsOk(stdHint);
            }

            // send 'Identity' if next operation arg is ready to be processed
            return new EProcessorStep.TokenPrep()
            {
                Value = new ETokenPrep.Identity()
                {
                    Result = topNode.Operation.ArgTokens[argsArray.Length]
                }
            }.AsOk(stdHint);
        }

        private class TokenContext : IProcessorFZO.ITokenContext
        {
            public required IMemoryFZO CurrentMemory { get; init; }
            public required IInputFZO Input { get; init; }
        }
    }
  
}