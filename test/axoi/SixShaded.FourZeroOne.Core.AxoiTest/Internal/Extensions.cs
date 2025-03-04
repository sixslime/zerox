namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal;

using CatGlance;
using DeTes.Analysis;
using MinimaFZO;
using NotRust;
using SixLib.ICEE;

internal static class Extensions
{
    public static async Task DeclarationHolds(this Assert assert, DeTes.Declaration.DeTesDeclaration declaration)
    {
        var glancer =
            new Glancer
            {
                Name = "Test",
                Supplier =
                    new DeTesFZOSupplier
                    {
                        Processor = new MinimaProcessorFZO(),
                        UnitializedState = new MinimaStateFZO(),
                    },
                Tests = [],
            };
        var test =
            new CatGlanceableTest("Test")
            {
                Declaration = declaration,
                InitialMemory = new MinimaMemoryFZO(),
            };
        var integrity = test.GenerateAssertIntegrityTests();
        var result =
            (await (glancer with
            {
                Tests = [test],
            }).Glance())[0];
        if (result.CheckOk(out var ok))
        {
            Console.WriteLine("=== roggido ===");
            PrintFinalRoggi(ok.Object, "");
        }
        Console.WriteLine("===============");
        var integrityResults =
            await (glancer with
            {
                Name = "Integrity",
                Tests = integrity.Enumerate().Map(x => x.value.ToGlancable($":{x.index + 1}")),
            }).Glance();
        DoAssert(result.CheckOk(out var r) && r.Evaluation);
        DoAssert(integrityResults.All(intResult => intResult.CheckOk(out var ri) && !ri.Evaluation));
    }

    private static void PrintFinalRoggi(IDeTesResult result, string prefix)
    {
        if (result.CriticalPoint.Split(out var hit, out var paths))
        {
            if (hit.Split(out var halt, out var ex))
            {
                switch (halt)
                {
                case FZOSpec.EProcessorHalt.Completed v:
                    Console.WriteLine($"({prefix}) {v.Roggi}");
                break;
                default:
                    Console.WriteLine($"! {halt}");
                break;
                }
                return;
            }
            Console.WriteLine(ex);
        }
        foreach (var path in paths)
            PrintFinalRoggi(path, $"{prefix}->{path.Selection.ICEE()}");
    }
    private static void DoAssert(bool condition)
    {
        if (!condition) throw new AssertFailedException();
    }
}