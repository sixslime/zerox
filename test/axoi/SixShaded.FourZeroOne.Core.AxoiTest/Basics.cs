namespace SixShaded.FourZeroOne.Core.AxoiTest;

using System.Reflection.Metadata;
using DeTes.Declaration;
using Internal.DummyAxoi.Korvessas;
using Internal.DummyAxoi.Roveggitus;
using Roggis;
using Core = Syntax.Core;
[TestClass]
public sealed class Basics
{
    [TestMethod]
    [DataRow(-400, 1, 10)]
    [DataRow(0, 1, 99)]
    [DataRow(777, 99, -1)]
    [DataRow(10, 20, 55)]
    [DataRow(-10, -20, -55)]
    public async Task Arithmetic(int xa, int xb, int xc) =>
        await Run(
        c =>
            xa.tFixed()
                .tAdd(xb.tFixed())
                .tMultiply(xc.tFixed())
                .tAdd(xc.tFixed().tSubtract(xb.tFixed()))
                .tMultiply(xa.tFixed())
                .AssertRoggi(c, r => r.Value == (((xa + xb) * xc) + (xc - xb)) * xa));

    [TestMethod]
    [DataRow(401, new[] { true, true, false }, 2, 6)]
    [DataRow(888, new[] { true, true, false }, 0, 10)]
    [DataRow(0, new[] { true, true, false }, 3, 4)]
    public async Task Roveggi(int num, bool[] bools, int basePower, int power) =>
        await Run(
        c =>
            Core.tCompose<Stuff>()
                .tWithComponent(Stuff.NUM, num.tFixed())
                .AssertRoggi(c, r => r.GetComponent(Stuff.NUM).Unwrap().Value == num, "NUM check")
                .AssertRoggi(c, r => !r.GetComponent(Stuff.MULTI_BOOL).IsSome(), "MULTI_BOOL check before set")
                .tWithComponent(Stuff.MULTI_BOOL, bools.tFixed())
                .tWithoutComponent(Stuff.NUM)
                .AssertRoggi(c, r => !r.GetComponent(Stuff.NUM).IsSome(), "NUM check after remove")
                .tWithComponent(
                Stuff.POWER_OBJ,
                Core.tCompose<PowerExpr>()
                    .tWithComponent(PowerExpr.POWER, power.tFixed())
                    .tWithComponent(PowerExpr.NUM, basePower.tFixed()))
                .AssertRoggi(c, r => r.GetComponent(Stuff.MULTI_BOOL).Unwrap().Elements.Map(x => x.IsTrue).SequenceEqual(bools), "MULTI_BOOL check")
                .tGetComponent(Stuff.POWER_OBJ)
                .AssertRoggi(
                c, r =>
                    r.GetComponent(PowerExpr.NUM).Unwrap().Value == basePower &&
                    r.GetComponent(PowerExpr.POWER).Unwrap().Value > 0, "POWER_OBJ check")
                .tTESTPower()
                .AssertRoggi(c, r => r.Value == basePower.Yield(power).Accumulate((a, b) => a * b).Unwrap(), "korvessa check"));

    [TestMethod]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new[] {5, 2, 0, 1}, 0)]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new[] {9, 0, 3}, 1)]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new[] {5}, 0)]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new[] { 999, 0, 777 }, 0)]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new[] { 999, 0, 777 }, 0)]
    [DataRow(new[] {999}, new[] {0}, 0)]
    [DataRow(new int[0], new[] {999}, 0)]
    public async Task Selection(int[] initialPool, int[] firstSelection, int secondSelection) =>
        await Run(
        c =>
            initialPool.tFixed()
                .tIOSelectMultiple(firstSelection.Length.tFixed())
                .DefineSelectionDomain(c, [firstSelection], out var firstDomain, "first selection")
                .AssertRoggiUnstable(
                c, r =>
                    (firstSelection.Length > initialPool.Length)
                        ? !r.IsSome()
                        : r.Check(out var multi) &&
                          multi.Count == firstSelection.Length &&
                          firstSelection.Map(i => initialPool[i]).SequenceEqual(multi.Elements.Map(x => x.Value)))
                .ReferenceAs(c, out var reducedPool)
                .tIOSelectOne()
                .DefineSelectionDomain(c, [secondSelection], out var secondDomain, "second selection")
                .AssertRoggiUnstable(
                c, r =>
                    (secondSelection >= firstSelection.Length)
                        ? !r.IsSome()
                        : r.Check(out var sel) && reducedPool.Roggi.Elements.GetAt(secondSelection).Unwrap() == sel));
    private static Task Run(DeTesDeclaration declaration) => Assert.That.DeclarationHolds(declaration);
}