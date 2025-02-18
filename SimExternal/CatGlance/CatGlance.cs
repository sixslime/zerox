using System;
using System.Collections.Generic;
using DeTes.Declaration;
using DeTes.Analysis;
using DeTes.Realization;
using DeTes.Syntax;
using Perfection;
using FourZeroOne.FZOSpec;
namespace CatGlance
{
    using CCol = ConsoleColor;
    using C = Console;
    using Token = FourZeroOne.Token.IToken<FourZeroOne.Resolution.IResolution>;
    using Res = FourZeroOne.Resolution.IResolution;
    using ResOpt = IOption<FourZeroOne.Resolution.IResolution>;
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
        public required string Name { get; init; }
        public required IEnumerable<ICatGlanceable> Tests { get; init; }
        public required IDeTesFZOSupplier Supplier { get; init; }

        private IOption<IResult<RecursiveEvalTree<IDeTesResult, bool>, EDeTesInvalidTest>[]> _testEvals = new None<IResult<RecursiveEvalTree<IDeTesResult, bool>, EDeTesInvalidTest>[]>();
        
        public async Task Glance()
        {
            await EvalTests();
            var results = _testEvals.Unwrap();
            WriteLn($"==[ GLANCER '{Name}' ]==", CCol.Yellow);
            foreach (var (i, (test, result)) in Tests.ZipShort(results).Enumerate())
            {
                Write($"({i+1})", CCol.Blue);
                Write($" \"{test.Name}\"", CCol.Blue);
                C.WriteLine();
                if (result.Split(out var rootTree, out var invalid))
                {
                    PrintEvalSummary(rootTree, 1);
                }
                else
                {
                    C.WriteLine(invalid);
                }
            }
        }
        private async Task EvalTests()
        {
            if (_testEvals.IsSome()) return;
            var tests = Tests.ToArray();
            var count = tests.Length;
            var results = new IResult<RecursiveEvalTree<IDeTesResult, bool>, EDeTesInvalidTest>[count];

            for (int i = 0; i < count; i++)
                results[i] = (await new DeTesRealizer().Realize(tests[i], Supplier))
                    .RemapOk(x =>
                        x.RecursiveEvalTree(
                            deTesResult => deTesResult.CriticalPoint.RemapOk(stop =>
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
                                    _ => true
                                })),
                            others => others.All(x => x)));
            _testEvals = results.AsSome();
        }
        private static void PrintEvalSummary(RecursiveEvalTree<IDeTesResult, bool> tree, int depth)
        {
            List<IDeTesAssertionData<Token>> tokenAsserts = new();
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

            if (tree.Evaluation)
            {
                Write("PASS", CCol.Green);
                if (tree.Object.CriticalPoint.CheckErr(out var p)) Write($"[{p.Length}]", CCol.Gray);
                if (tokenAsserts.Count + resolutionAsserts.Count + memoryAsserts.Count > 0)
                {
                    Write(" | ", CCol.DarkGray);
                    Write(tokenAsserts.Count.ExprAs(x => x > 0 ? $"{x}" : "-") + " | ", CCol.DarkGray);
                    Write(resolutionAsserts.Count.ExprAs(x => x > 0 ? $"{x}" : "-") + " | ", CCol.DarkGray);
                    Write(memoryAsserts.Count.ExprAs(x => x > 0 ? $"{x}" : "-") + " | ", CCol.DarkGray);
                }
                C.WriteLine();
                return;
            }
            if (tree.Object.CriticalPoint.CheckOk(out var stopped))
            {
                if (stopped.Invert().Split(out var exception, out var halt))
                {
                    Write("EXCEPTION", CCol.Magenta);
                    Write(" : ");
                    Write(exception.ToString(), CCol.DarkRed);
                    C.WriteLine();
                    return;
                }
                if (halt is EProcessorHalt.InvalidState invalid)
                {
                    WriteLn("INVALID STATE", CCol.Magenta);
                    return;
                }
            }
            if (Iter.Over<IEnumerable<IDeTesAssertionDataUntyped>>(tokenAsserts, resolutionAsserts, memoryAsserts).Flatten().Any(x => !AssertPassed(x)))
            {
                WriteLn("FAILED", CCol.Red);
                PrintFailedAssertionSummary(tokenAsserts, "t: ", depth);
                PrintFailedAssertionSummary(resolutionAsserts, "r: ", depth);
                PrintFailedAssertionSummary(memoryAsserts, "m: ", depth);
                return;
            }
            var subTrees = tree.Branches.Expect("All cases where branches are not present should have been covered.");
            WriteLn($"{{{subTrees.Length}}}");
            foreach (var subTree in subTrees)
            {
                PrintEvalSummary(subTree, depth + 1);
            }
        }
        private static void PrintFailedAssertionSummary(IEnumerable<IDeTesAssertionDataUntyped> assertions, string starter, int depth)
        {
            foreach (var failed in assertions.Where(x => !AssertPassed(x)))
            {
                DepthPad(depth);
                Write(starter);
                Write($"[{failed.OnToken}] {failed.Description.NullToNone().RemapAs(desc => $"\"{desc}\"").Or("")}", CCol.DarkYellow);
                if (failed.Result.CheckErr(out var exception))
                {
                    Write($" threw: '{exception.Message}'", CCol.DarkMagenta);
                }
                C.WriteLine();
            }
        }
        private static bool AssertPassed(IDeTesAssertionDataUntyped assertion)
        {
            return assertion.Result.CheckOk(out var v) && v;
        }
        private static void DepthPad(int depth)
        {
            Write(string.Concat(" ".Yield(depth)));
        }
        private static void Write(string text, CCol color = CCol.Gray)
        {
            C.ForegroundColor = color;
            C.Write(text);
            C.ResetColor();
        }
        private static void WriteLn(string text, CCol color = CCol.Gray)
        {
            C.ForegroundColor = color;
            C.WriteLine(text);
            C.ResetColor();
        }
    }
}
