namespace SixShaded.DeTes.StdEval;

public static class Extensions
{
    public static RecursiveEvalTree<IDeTesResult, bool> StdEvalTree(this IDeTesResult result) =>
        result.RecursiveEvalTree(
        deTesResult =>
            deTesResult.CriticalPoint.RemapOk(
            stop =>
                stop.CheckOk(out var halt) &&
                halt is EProcessorHalt.Completed &&
                AllAssertsPassed(deTesResult)),
        (self, others) => AllAssertsPassed(self) && others.All(y => y));

    public static bool StdEval(this IDeTesResult result) => StdEvalTree(result).Evaluation;
    public static bool StdEvalAssertion<A>(this IDeTesAssertionData<A> assertion) => assertion.Result.KeepOk().Or(false);

    private static bool AllAssertsPassed(IDeTesResult result) =>
        result.EvaluationFrames.All(
        frame =>
            frame switch
            {
                EDeTesFrame.Resolve v
                    => v.Assertions.Korssa.All(StdEvalAssertion) &&
                       v.Assertions.Roggi.All(StdEvalAssertion) &&
                       v.Assertions.Memory.All(StdEvalAssertion),
                EDeTesFrame.Complete v
                    => //DEBUG
                    //new Func<bool>(() => { Console.WriteLine(v.CompletionHalt.Roggi); return true; })() &&
                    v.Assertions.Korssa.All(StdEvalAssertion) &&
                    v.Assertions.Roggi.All(StdEvalAssertion) &&
                    v.Assertions.Memory.All(StdEvalAssertion),
                _ => true,
            });
}