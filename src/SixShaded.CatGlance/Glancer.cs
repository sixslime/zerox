// quite a mess, but being clean is not in CatGlance's objectives

namespace SixShaded.CatGlance;

using DeTes.Analysis;
using DeTes.Realization;
using SixLib.ICEE;
using SixLib.ICEE.FZO;
using C = Console;
using CCol = ConsoleColor;
using DeTes.StdEval;

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
        const ConsoleColor nearKorssaColor = CCol.DarkGray;
        switch (test)
        {
        case EDeTesInvalidTest.EmptyDomain v:
            {
                Write("empty domain: ");
                if (v.Description is not null) Write($"\"{v.Description}\" ", CCol.DarkCyan);
                Write(FormatLinkedKorssa(v.NearKorssa), nearKorssaColor);
            }
        break;
        case EDeTesInvalidTest.NoSelectionDomainDefined v:
            {
                Write("no domain defined for: ");
                Write(v.SelectionKorssa.ToString(), CCol.DarkYellow);
            }
        break;
        case EDeTesInvalidTest.InvalidDomainSelection v:
            {
                Write("invalid domain: ");
                if (v.Description is not null) Write($"\"{v.Description}\" ", CCol.DarkCyan);
                WriteLn(FormatLinkedKorssa(v.NearKorssa), nearKorssaColor);
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
                Write(FormatLinkedKorssa(v.NearKorssa), nearKorssaColor);
            }
        break;
        case EDeTesInvalidTest.ReferenceUsedBeforeEvaluated v:
            {
                Write("referenced used before valid: ");
                if (v.Description is not null) Write($"\"{v.Description}\" ", CCol.DarkCyan);
                Write(FormatLinkedKorssa(v.NearKorssa), nearKorssaColor);
            }
        break;
        }
        C.WriteLine();
        WriteLn("------------------", CCol.Magenta);
    }

    private static string FormatLinkedKorssa(Kor korssa) => $"`{korssa}`";

    private async Task EvalTests()
    {
        if (_testEvals.IsSome()) return;
        var tests = Tests.ToArray();
        int count = tests.Length;
        var results = new IResult<RecursiveEvalTree<IDeTesResult, bool>, EDeTesInvalidTest>[count];
        for (int i = 0; i < count; i++)
        {
            results[i] =
                (await new DeTesRealizer().Realize(tests[i], Supplier))
                .RemapOk(x => x.StdEvalTree());
        }
        _testEvals = results.AsSome();
    }

    private static void PrintEvalSummary(RecursiveEvalTree<IDeTesResult, bool> tree, int depth)
    {
        List<IDeTesAssertionData<Kor>> korssaAsserts = new();
        List<IDeTesAssertionData<RogOpt>> roggiAsserts = new();
        List<IDeTesAssertionData<IMemoryFZO>> memoryAsserts = new();
        foreach (var frame in tree.Object.EvaluationFrames)
        {
            switch (frame)
            {
            case EDeTesFrame.Resolve v:
                korssaAsserts.AddRange(v.Assertions.Korssa);
                roggiAsserts.AddRange(v.Assertions.Roggi);
                memoryAsserts.AddRange(v.Assertions.Memory);
            break;
            case EDeTesFrame.Complete v:
                korssaAsserts.AddRange(v.Assertions.Korssa);
                roggiAsserts.AddRange(v.Assertions.Roggi);
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
            PrintPostHeader(korssaAsserts, roggiAsserts, memoryAsserts, tree.Object.TimeTaken);
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
        if (Iter.Over<IEnumerable<IDeTesAssertionDataUntyped>>(korssaAsserts, roggiAsserts, memoryAsserts)
        .Flatten()
        .Any(x => !AssertPassed(x)))
        {
            Write("FAIL", CCol.Red);
            PrintPostHeader(korssaAsserts, roggiAsserts, memoryAsserts, tree.Object.TimeTaken);
            C.WriteLine();
            PrintFailedAssertionSummary(korssaAsserts, "k: ", depth);
            PrintFailedAssertionSummary(roggiAsserts, "r: ", depth);
            PrintFailedAssertionSummary(memoryAsserts, "m: ", depth);
            return;
        }
        var subTrees = tree.Branches.Expect("All cases where branches are not present should have been covered.");
        var nextInfo = subTrees[0].Object.IsA<IDeTesSelectionPath>();
        Write("IN ", CCol.DarkBlue);
        Write($"{FormatDescription(nextInfo.Domain.Description)}", CCol.DarkCyan);
        Write($": {nextInfo.RootSelectionKorssa}", CCol.DarkGray);
        C.WriteLine();
        foreach (var subTree in subTrees)
        {
            PrintEvalSummary(subTree, depth + 1);
        }
    }

    private static string FormatDescription(string? desc) => desc.NullToNone().RemapAs(desc => $"\"{desc}\" ").Or("");

    private static void PrintPostHeader(
        IEnumerable<IDeTesAssertionData<Kor>> korssaAsserts,
        IEnumerable<IDeTesAssertionData<RogOpt>> roggiAsserts,
        IEnumerable<IDeTesAssertionData<IMemoryFZO>> memoryAsserts,
        TimeSpan time)
    {
        const ConsoleColor blankColor = CCol.DarkGray;
        const ConsoleColor failedColor = CCol.DarkYellow;
        const ConsoleColor timerColor = CCol.DarkGray;
        foreach ((int total, int failed) in Iter
            .Over<IEnumerable<IDeTesAssertionDataUntyped>>(korssaAsserts, roggiAsserts, memoryAsserts)
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

    private static void PrintFailedAssertionSummary(
        IEnumerable<IDeTesAssertionDataUntyped> assertions,
        string starter,
        int depth)
    {
        foreach (var failed in assertions.Where(x => !AssertPassed(x)))
        {
            DepthPad(depth + 1);
            Write(starter, CCol.DarkYellow);
            Write($"{FormatDescription(failed.Description)}", CCol.DarkYellow);
            Write($"{FormatLinkedKorssa(failed.OnKorssa)}", CCol.DarkGray);
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