using MorseCode.ITask;
using Perfection;
using FourZeroOne.FZOSpec;
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using FourZeroOne.Macro.Unsafe;
using ResOpt = Perfection.IOption<FourZeroOne.Resolution.IResolution>;
using LookNicePls;
#nullable enable
namespace Minima.FZO
{
    public class MinimaProcessorFZO() : IProcessorFZO
    {
        public ITask<IResult<EProcessorStep, EProcessorHalt>> GetNextStep(IStateFZO state, IInputFZO input) => StaticImplementation(state, input);
        private static async ITask<IResult<EProcessorStep, EProcessorHalt>> StaticImplementation(IStateFZO state, IInputFZO input)
        {
            // i bet this shit could be written in 1 line.

            // - caching
            var stdHint = Hint<EProcessorHalt>.HINT;
            IResult<EProcessorStep, EProcessorHalt> invalidStateResult = new EProcessorHalt.InvalidState { HaltingState = state }.AsErr(Hint<EProcessorStep>.HINT);

            if (state.Initialized.CheckNone(out var init)) return invalidStateResult;

            // assert that if an operation exists, it's memory stack must have at least 1 element:
            {
                if (state.OperationStack.GetAt(0).Check(out var ops) &&
                    ops.MemoryStack.GetAt(0).IsSome().Not())
                    return invalidStateResult;
            }

            // continue token prep if prep stack has at least 1 element:
            if (state.TokenPrepStack.GetAt(0).Check(out var processingToken))
            {
                var token = processingToken.Result;
                Dictionary<IRule, int> previousApplications = new();

                // WARNING:
                // this method of uncached checking for previously applied rules is inefficient for large amounts of preprocess steps.

                // - populate previousApplications:
                foreach (var process in state.TokenPrepStack)
                {
                    if (process is not ETokenMutation.RuleApply ra) continue;
                    var appliedRule = ra.Rule;
                    previousApplications[appliedRule] = previousApplications.TryGetValue(appliedRule, out var count) ? count + 1 : 1;
                }
                foreach (var rule in state.OperationStack.GetAt(0)
                    .RemapAs(x => x.MemoryStack.GetAt(0).Unwrap().Rules).Or([]))
                {
                    // - skip already applied rules:
                    if (previousApplications.TryGetValue(rule, out var count) && count > 0)
                    {
                        previousApplications[rule] = count - 1;
                        continue;
                    }

                    // send 'RuleApplication' if the processing token is rulable by an unapplied rule:
                    if (rule.TryApply(token).Check(out var ruledToken))
                        return new EProcessorStep.TokenMutate()
                        {
                            Mutation = new ETokenMutation.RuleApply()
                            {
                                Rule = rule,
                                Result = ruledToken
                            }
                        }.AsOk(stdHint);
                }

                // DEBUG
                //Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine(token);
                //Console.ResetColor();
                // send 'PushOperation' if no more preprocessing is needed:
                return new EProcessorStep.PushOperation() { OperationToken = token }.AsOk(stdHint);
            }

            // send initial 'Identity' if no operations are on the stack:
            if (state.OperationStack.GetAt(0).CheckNone(out var topNode))
                return new EProcessorStep.TokenMutate
                {
                    Mutation = new ETokenMutation.Identity
                    {
                        Result = init.Program
                    }
                }.AsOk(stdHint);

            // - caching
            TokenContext tokenContext = new()
            {
                CurrentMemory = topNode.MemoryStack.GetAt(0).Unwrap(),
                Input = input
            };
            var argsArray = topNode.ArgResolutionStack.ToMutList().Mut(x => x.Reverse()).ToArray();
            // assert there are not more resolutions than the operation takes:
            if (argsArray.Length > topNode.Operation.ArgTokens.Length) return invalidStateResult;

            // send 'Resolve' if all operation's args are resolved:
            if (argsArray.Length == topNode.Operation.ArgTokens.Length)
            {
                var resolvedOperation =
                    topNode.Operation.UnsafeResolve(tokenContext, argsArray)
                    .Split(out var resolutionTask, out var runtimeHandled)
                        ? (await resolutionTask).AsOk(Hint<EStateImplemented>.HINT)
                        : runtimeHandled.AsErr(Hint<ResOpt>.HINT);
                // DEBUG
                //if (resolvedOperation.CheckOk(out var r)) Console.WriteLine(r);
                //Console.ResetColor();
                return
                    (resolvedOperation.CheckOk(out var finalResolution) && !state.OperationStack.GetAt(1).IsSome())
                    .Not().ToResult(
                        new EProcessorStep.Resolve() { Resolution = resolvedOperation },
                        new EProcessorHalt.Completed() { HaltingState = state, Resolution = finalResolution });
            }

            

            // send 'Identity' if next operation arg is ready to be processed
            return new EProcessorStep.TokenMutate()
            {
                Mutation = new ETokenMutation.Identity()
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