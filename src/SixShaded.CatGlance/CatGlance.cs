// quite a mess, but being clean is not in CatGlance's objectives

namespace SixShaded.CatGlance;

using DeTes.Analysis;
using DeTes.Declaration;
using DeTes.Realization;
using SixLib.ICEE;
using SixLib.ICEE.FZO;
using C = Console;
using CCol = ConsoleColor;

public interface ICatGlanceable : IDeTesTest
{
    public string Name { get; }
}

public class GlancableTest : ICatGlanceable
{
    public GlancableTest() { Name = "(unnamed)"; }
    public GlancableTest(string name) { Name = name; }
    public string Name { get; }
    public required IMemoryFZO InitialMemory { get; init; }
    public required TokenDeclaration Token { get; init; }
}

public record Glancer
{
    private IOption<IResult<RecursiveEvalTree<IDeTesResult, bool>, EDeTesInvalidTest>[]> _testEvals =
        new None<IResult<RecursiveEvalTree<IDeTesResult, bool>, EDeTesInvalidTest>[]>();

    public required string Name { get; init; }
    public required IEnumerable<ICatGlanceable> Tests { get; init; }
    public required IDeTesFZOSupplier Supplier { get; init; }

    public async Task<IResult<RecursiveEvalTree<IDeTesResult, bool>, EDeTesInvalidTest>[]> Glance()
    {
        await EvalTests();
        var results = _testEvals.Unwrap();
        WriteLn($"==[ GLANCER '{Name}' ]==", CCol.Yellow);
        foreach ((int i, var (test, result)) in Tests.ZipShort(results).Enumerate())
        {
            Write($"({i + 1}) ", CCol.Blue);
            Write($"\"{test.Name}\" ", CCol.Blue);
            C.WriteLine();
            if (result.Split(out var rootTree, out var invalid))
            {
                PrintEvalSummary(rootTree, 0);
            }
            else
            {
                PrintInvalidTest(invalid);
            }
        }

        return _testEvals.Unwrap();
    }

    private static void PrintInvalidTest(EDeTesInvalidTest test)
    {
        WriteLn("-- INVALID TEST --", CCol.Magenta);
        const ConsoleColor nearTokenColor = CCol.DarkGray;
        switch (test)
        {
        case EDeTesInvalidTest.EmptyDomain v:
            {
                Write("empty domain: ");
                if (v.Description is not null) Write($"\"{v.Description}\" ", CCol.DarkCyan);
                Write(FormatLinkedToken(v.NearToken), nearTokenColor);
            }
        break;
        case EDeTesInvalidTest.NoSelectionDomainDefined v:
            {
                Write("no domain defined for: ");
                Write(v.SelectionToken.ToString(), CCol.DarkYellow);
            }
        break;
        case EDeTesInvalidTest.InvalidDomainSelection v:
            {
                Write("invalid domain: ");
                if (v.Description is not null) Write($"\"{v.Description}\" ", CCol.DarkCyan);
                WriteLn(FormatLinkedToken(v.NearToken), nearTokenColor);
                Write(v.InvalidSelection.ICEE(), CCol.DarkYellow);
                if (v.InvalidSelection.Length != v.ExpectedSelectionSize)
                {
                    Write(" expects ");
                    Write(v.ExpectedSelectionSize.ToString(), CCol.DarkGreen);
                    Write(" elements got ");
                    Write(v.InvalidSelection.Length.ToString(), CCol.Red);
                }

                if (v.InvalidSelection.Any(x => x < 0))
                {
                    Write(" contains negative indicies");
                }

                if (v.InvalidSelection.Any(x => x > v.ExpectedMaxIndex))
                {
                    Write(" expects max of ");
                    Write(v.ExpectedMaxIndex.ToString(), CCol.DarkGreen);
                    Write(" found ");
                    Write(v.InvalidSelection.FirstMatch(x => x > v.ExpectedMaxIndex).Unwrap().ToString(), CCol.Red);
                }

                C.WriteLine();
                Write("of: ", CCol.DarkGray);
                Write(v.Domain.Map(x => x.ICEE()).ToArray().ICEE(), CCol.DarkGray);
            }
        break;
        case EDeTesInvalidTest.DomainUsedOutsideOfScope v:
            {
                Write("domain referenced outside of scope: ");
                if (v.Description is not null) Write($"\"{v.Description}\" ", CCol.DarkCyan);
                Write(FormatLinkedToken(v.NearToken), nearTokenColor);
            }
        break;
        case EDeTesInvalidTest.ReferenceUsedBeforeEvaluated v:
            {
                Write("referenced used before valid: ");
                if (v.Description is not null) Write($"\"{v.Description}\" ", CCol.DarkCyan);
                Write(FormatLinkedToken(v.NearToken), nearTokenColor);
            }
        break;
        }

        C.WriteLine();
        WriteLn("------------------", CCol.Magenta);
    }

    private static string FormatLinkedToken(Tok token) => $"{{{token}}}";

    private async Task EvalTests()
    {
        if (_testEvals.IsSome()) return;
        var tests = Tests.ToArray();
        int count = tests.Length;
        var results = new IResult<RecursiveEvalTree<IDeTesResult, bool>, EDeTesInvalidTest>[count];

        for (int i = 0; i < count; i++)
        {
            results[i] = (await new DeTesRealizer().Realize(tests[i], Supplier))
                .RemapOk(x =>
                    x.RecursiveEvalTree(deTesResult =>
                            deTesResult.CriticalPoint.RemapOk(stop =>
                                stop.CheckOk(out var halt) &&
                                halt is EProcessorHalt.Completed &&
                                deTesResult.EvaluationFrames.All(frame => frame switch
                                {
                                    EDeTesFrame.Resolve v
                                        => v.Assertions.Token.All(AssertPassed) &&
                                           v.Assertions.Resolution.All(AssertPassed) &&
                                           v.Assertions.Memory.All(AssertPassed),
                                    EDeTesFrame.Complete v
                                        => //DEBUG
                                        //new Func<bool>(() => { Console.WriteLine(v.CompletionHalt.Resolution); return true; })() &&
                                        v.Assertions.Token.All(AssertPassed) &&
                                        v.Assertions.Resolution.All(AssertPassed) &&
                                        v.Assertions.Memory.All(AssertPassed),
                                    _ => true,
                                })),
                        others => others.All(y => y)));
        }

        _testEvals = results.AsSome();
    }

    private static void PrintEvalSummary(RecursiveEvalTree<IDeTesResult, bool> tree, int depth)
    {
        List<IDeTesAssertionData<Tok>> tokenAsserts = new();
        List<IDeTesAssertionData<ResOpt>> resolutionAsserts = new();
        List<IDeTesAssertionData<IMemoryFZO>> memoryAsserts = new();
        foreach (var frame in tree.Object.EvaluationFrames)
        {
            switch (frame)
            {
            case EDeTesFrame.Resolve v:
                tokenAsserts.AddRange(v.Assertions.Token);
                resolutionAsserts.AddRange(v.Assertions.Resolution);
                memoryAsserts.AddRange(v.Assertions.Memory);
            break;
            case EDeTesFrame.Complete v:
                tokenAsserts.AddRange(v.Assertions.Token);
                resolutionAsserts.AddRange(v.Assertions.Resolution);
                memoryAsserts.AddRange(v.Assertions.Memory);
            break;
            }
        }

        DepthPad(depth);
        Write("└");
        if (tree.Object is IDeTesSelectionPath selectionPath)
            Write($"{selectionPath.Selection.ICEE()} ");

        if (tree.Evaluation)
        {
            Write("PASS", CCol.Green);
            if (tree.Object.CriticalPoint.CheckErr(out var p)) Write($"[{p.Length}]");
            PrintPostHeader(tokenAsserts, resolutionAsserts, memoryAsserts, tree.Object.TimeTaken);
            C.WriteLine();
            return;
        }

        if (tree.Object.CriticalPoint.CheckOk(out var stopped))
        {
            if (stopped.Invert().Split(out var exception, out var halt))
            {
                Write("EXCEPTION!", CCol.Magenta);
                Write(" : ");
                Write(exception.ToString(), CCol.DarkRed);
                C.WriteLine();
                return;
            }

            if (halt is EProcessorHalt.InvalidState invalid)
            {
                Write("INVALID STATE!", CCol.Magenta);
                Write(" : ");
                Write(invalid.HaltingState.ICEE(), CCol.DarkRed);
                return;
            }
        }

        if (Iter.Over<IEnumerable<IDeTesAssertionDataUntyped>>(tokenAsserts, resolutionAsserts, memoryAsserts).Flatten()
            .Any(x => !AssertPassed(x)))
        {
            Write("FAIL", CCol.Red);
            PrintPostHeader(tokenAsserts, resolutionAsserts, memoryAsserts, tree.Object.TimeTaken);
            C.WriteLine();
            PrintFailedAssertionSummary(tokenAsserts, "t: ", depth);
            PrintFailedAssertionSummary(resolutionAsserts, "r: ", depth);
            PrintFailedAssertionSummary(memoryAsserts, "m: ", depth);
            return;
        }

        var subTrees = tree.Branches.Expect("All cases where branches are not present should have been covered.");
        var nextInfo = subTrees[0].Object.IsA<IDeTesSelectionPath>();
        Write("IN ", CCol.DarkBlue);
        Write($"{FormatDescription(nextInfo.Domain.Description)}", CCol.DarkCyan);
        Write($": {nextInfo.RootSelectionToken}", CCol.DarkGray);
        C.WriteLine();
        foreach (var subTree in subTrees)
        {
            PrintEvalSummary(subTree, depth + 1);
        }
    }

    private static string FormatDescription(string? desc) => desc.NullToNone().RemapAs(desc => $"\"{desc}\" ").Or("");

    private static void PrintPostHeader(IEnumerable<IDeTesAssertionData<Tok>> tokenAsserts,
        IEnumerable<IDeTesAssertionData<ResOpt>> resolutionAsserts,
        IEnumerable<IDeTesAssertionData<IMemoryFZO>> memoryAsserts, TimeSpan time)
    {
        const ConsoleColor blankColor = CCol.DarkGray;
        const ConsoleColor failedColor = CCol.DarkYellow;
        const ConsoleColor timerColor = CCol.DarkGray;
        foreach ((int total, int failed) in Iter
                     .Over<IEnumerable<IDeTesAssertionDataUntyped>>(tokenAsserts, resolutionAsserts, memoryAsserts)
                     .Map(x => (x.Count(), x.Count(y => !AssertPassed(y)))))
        {
            Write(" | ", blankColor);
            if (total == 0)
            {
                Write("-", blankColor);
                continue;
            }

            if (failed > 0)
            {
                Write(failed.ToString(), failedColor);
                Write("/" + total);
                continue;
            }

            Write($"{total}", blankColor);
        }

        Write(" | ", blankColor);
        //Write($"{Math.Round(time.TotalMilliseconds, 0)}ms", timerColor);
    }

    private static void PrintFailedAssertionSummary(IEnumerable<IDeTesAssertionDataUntyped> assertions, string starter,
        int depth)
    {
        foreach (var failed in assertions.Where(x => !AssertPassed(x)))
        {
            DepthPad(depth + 1);
            Write(starter, CCol.DarkYellow);
            Write($"{FormatDescription(failed.Description)}", CCol.DarkYellow);
            Write($"{FormatLinkedToken(failed.OnToken)}", CCol.DarkGray);
            if (failed.Result.CheckErr(out var exception))
            {
                Write($" threw: '{exception.Message}'", CCol.DarkMagenta);
            }

            C.WriteLine();
        }
    }

    private static bool AssertPassed(IDeTesAssertionDataUntyped assertion) => assertion.Result.CheckOk(out bool v) && v;

    private static void DepthPad(int depth) => Write(" " + string.Concat("  ".Yield(depth)));

    private static void Write(string? text, CCol color = CCol.Gray)
    {
        C.ForegroundColor = color;
        C.Write(text);
        C.ResetColor();
    }

    private static void WriteLn(string? text, CCol color = CCol.Gray)
    {
        C.ForegroundColor = color;
        C.WriteLine(text);
        C.ResetColor();
    }
}