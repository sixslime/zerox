
namespace SixShaded.MinimaFZO;

using FourZeroOne.Mellsano;
public class MinimaProcessorFZO : IProcessorFZO
{
    public Task<IResult<EProcessorStep, EProcessorHalt>> GetNextStep(IStateFZO state, IInputFZO input) => StaticImplementation(state, input);

    private static async Task<IResult<EProcessorStep, EProcessorHalt>> StaticImplementation(IStateFZO state, IInputFZO input)
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

        // continue korssa prep if prep stack has at least 1 element:
        if (state.KorssaMutationStack.GetAt(0).Check(out var processingKorssa))
        {
            var korssa = processingKorssa.Result;

            if (state.OperationStack.GetAt(0).Check(out var t) && t.MemoryStack.GetAt(0).Check(out var topMem))
            {
                // DEBUG
                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine(topMem.LookNicePls());
                //Console.ResetColor();
                Dictionary<MellsanoID, int> seenMellsanos = new();
                foreach (var mellsano in topMem.Mellsanos)
                {
                    // - skip muted mellsanos:
                    int timesSeen = seenMellsanos.At(mellsano.ID).Or(0);
                    if (topMem.GetMellsanoMuteCount(mellsano.ID) > timesSeen)
                    {
                        seenMellsanos[mellsano.ID] = timesSeen + 1;
                        continue;
                    }

                    // send 'MellsanoApplication' if the processing korssa is rulable by an unapplied mellsano:
                    if (mellsano.TryApply(korssa).Check(out var mellsanodKorssa))
                    {
                        // DEBUG
                        //Console.ForegroundColor = ConsoleColor.Blue;
                        //Console.WriteLine("MELLSANO: " + korssa + " => " + mellsanodKorssa);
                        //Console.ResetColor();
                        return new EProcessorStep.KorssaMutate
                        {
                            Mutation = new EKorssaMutation.MellsanoApply
                            {
                                Mellsano = mellsano,
                                Result = mellsanodKorssa,
                            },
                        }.AsOk(stdHint);
                    }
                }
            }
            // DEBUG
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine(korssa);
            //Console.ResetColor();
            // send 'PushOperation' if no more preprocessing is needed:
            return new EProcessorStep.PushOperation { OperationKorssa = korssa }.AsOk(stdHint);
        }

        // send initial 'Identity' if no operations are on the stack:
        if (state.OperationStack.GetAt(0).CheckNone(out var topNode))
        {
            return new EProcessorStep.KorssaMutate
            {
                Mutation = new EKorssaMutation.Identity
                {
                    Result = init.Program,
                },
            }.AsOk(stdHint);
        }

        // - caching
        KorssaContext korssaContext = new()
        {
            CurrentMemory = topNode.MemoryStack.GetAt(0).Unwrap(),
            Input = input,
        };
        var argsArray = topNode.ArgRoggiStack.ToMutList().Mut(x => x.Reverse()).ToArray();
        // assert there are not more roggis than the operation takes:
        if (argsArray.Length > topNode.Operation.ArgKorssas.Length) return invalidStateResult;

        // send 'Resolve' if all operation's args are resolved:
        if (argsArray.Length == topNode.Operation.ArgKorssas.Length)
        {
            var resolvedOperation =
                topNode.Operation.ResolveWith(korssaContext, argsArray)
                    .Split(out var roggiTask, out var runtimeHandled)
                    ? (await roggiTask).AsOk(Hint<EStateImplemented>.HINT)
                    : runtimeHandled.AsErr(Hint<RogOpt>.HINT);
            // DEBUG
            //Console.ForegroundColor = ConsoleColor.Red;
            //if (resolvedOperation.CheckOk(out var r)) Console.WriteLine(r);
            Console.ResetColor();
            return
                (resolvedOperation.CheckOk(out var finalRoggi) && !state.OperationStack.GetAt(1).IsSome())
                .Not().ToResult(
                    new EProcessorStep.Resolve { Roggi = resolvedOperation },
                    new EProcessorHalt.Completed { HaltingState = state, Roggi = finalRoggi });
        }

        // send 'Identity' if next operation arg is ready to be processed
        return new EProcessorStep.KorssaMutate
        {
            Mutation = new EKorssaMutation.Identity
            {
                Result = topNode.Operation.ArgKorssas[argsArray.Length],
            },
        }.AsOk(stdHint);
    }

    private class KorssaContext : IProcessorFZO.IKorssaContext
    {
        public required IMemoryFZO CurrentMemory { get; init; }
        public required IInputFZO Input { get; init; }
    }
}