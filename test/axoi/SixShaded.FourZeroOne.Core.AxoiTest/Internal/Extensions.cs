namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal;

using CatGlance;
using MinimaFZO;
using NotRust;

internal static class Extensions
{
    public static async Task DeclarationHolds(this Assert assert, DeTes.Declaration.DeTesDeclaration declaration)
    {
        var glancer = new Glancer { Name = "Test", Supplier = new DeTesFZOSupplier { Processor = new MinimaProcessorFZO(), UnitializedState = new MinimaStateFZO() }, Tests = [] };
        var test = new CatGlanceableTest("Test") { Declaration = declaration, InitialMemory = new MinimaMemoryFZO() };
        var integrity = test.GenerateAssertIntegrityTests();
        var result = (await (glancer with { Tests = [test] }).Glance())[0];
        var integrityResults = (await (glancer with { Name = "Integrity", Tests = integrity.Enumerate().Map(x => x.value.ToGlancable($":{x.index+1}")) }).Glance());
        DoAssert(result.CheckOk(out var r) && r.Evaluation);
        DoAssert(integrityResults.All(intResult => intResult.CheckOk(out var ri) && !ri.Evaluation));
    }

    private static void DoAssert(bool condition)
    {
        if (!condition) throw new AssertFailedException();
    }
}
