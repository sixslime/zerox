namespace SixShaded.FourZeroOne.Core.AxoiTest;

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
                .tWithComponent(Stuff.MULTI_BOOL, bools.Map(x => (Bool)x).tFixed())
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

    private static Task Run(DeTesDeclaration declaration) => Assert.That.DeclarationHolds(declaration);
}