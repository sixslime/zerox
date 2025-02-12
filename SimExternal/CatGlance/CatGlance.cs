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

        public async Task Glance()
        {
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
                                    EDeTesFrame.PushOperation v
                                        => v.Assertions.Token.All(assert => assert.Result.CheckOk(out var pass) && pass),
                                    EDeTesFrame.Resolve v
                                        => v.Assertions.Resolution.All(assert => assert.Result.CheckOk(out var pass) && pass) &&
                                            v.Assertions.Memory.All(assert => assert.Result.CheckOk(out var pass) && pass),
                                    EDeTesFrame.Complete v
                                        => v.Assertions.Resolution.All(assert => assert.Result.CheckOk(out var pass) && pass) &&
                                            v.Assertions.Memory.All(assert => assert.Result.CheckOk(out var pass) && pass),
                                    _ => true
                                })),
                            others => others.All(x => x)));

            Console.WriteLine($"==[ DETES GLANCER '{Name}' ]==");
            foreach (var (i, (test, result)) in tests.ZipShort(results).Enumerate())
            {
                Console.Write($"({i+1}) '{test.Name}': ");
                if (result.Split(out var tree, out var invalid))
                {
                    Console.WriteLine($"{(tree.Evaluation ? "PASSED" : "FAILED !")}");
                }
                else
                {
                    Console.WriteLine(invalid);
                }
            }
        }
    }
}
