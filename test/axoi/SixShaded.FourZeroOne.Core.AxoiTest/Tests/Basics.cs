namespace SixShaded.FourZeroOne.Core.AxoiTest.Tests;

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
            xa.kFixed()
                .kAdd(xb.kFixed())
                .kMultiply(xc.kFixed())
                .kAdd(xc.kFixed().kSubtract(xb.kFixed()))
                .kMultiply(xa.kFixed())
                .DeTesAssertRoggi(c, r => r.Value == ((xa + xb) * xc + (xc - xb)) * xa));

    [TestMethod]
    [DataRow(401, new[] { true, true, false }, 2, 6)]
    [DataRow(888, new[] { true, true, false }, 0, 10)]
    [DataRow(0, new[] { true, true, false }, 3, 4)]
    public async Task Roveggi(int num, bool[] bools, int basePower, int power) =>
        await Run(
        c =>
            Core.kRoveggi<FooRovetu>()
                .kWithRovi(FooRovetu.NUM, num.kFixed())
                .DeTesAssertRoggi(c, r => r.GetComponent(FooRovetu.NUM).Unwrap().Value == num, "NUM check")
                .DeTesAssertRoggi(c, r => !r.GetComponent(FooRovetu.MULTI_BOOL).IsSome(), "MULTI_BOOL check before set")
                .kWithRovi(FooRovetu.MULTI_BOOL, bools.kFixed())
                .kWithoutRovi(FooRovetu.NUM)
                .DeTesAssertRoggi(c, r => !r.GetComponent(FooRovetu.NUM).IsSome(), "NUM check after remove")
                .kWithRovi(
                FooRovetu.POWER_OBJ,
                Core.kRoveggi<PowerExpr>()
                    .kWithRovi(PowerExpr.POWER, power.kFixed())
                    .kWithRovi(PowerExpr.NUM, basePower.kFixed()))
                .DeTesAssertRoggi(c, r => r.GetComponent(FooRovetu.MULTI_BOOL).Unwrap().Elements.Map(x => x.IsTrue).SequenceEqual(bools), "MULTI_BOOL check")
                .kGetRovi(FooRovetu.POWER_OBJ)
                .DeTesAssertRoggi(
                c, r =>
                    r.GetComponent(PowerExpr.NUM).Unwrap().Value == basePower &&
                    r.GetComponent(PowerExpr.POWER).Unwrap().Value > 0, "POWER_OBJ check")
                .kTESTPower()
                .DeTesAssertRoggi(c, r => r.Value == basePower.Yield(power).Accumulate((a, b) => a * b).Unwrap(), "korvessa check"));

    [TestMethod]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new[] { 5, 2, 0, 1 }, 0)]
    [DataRow(new[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 }, new[] { 9, 0, 3 }, 1)]
    [DataRow(new[] { 0, 0, 0, 0, 0, 401, 0, 0 }, new[] { 5 }, 0)]
    [DataRow(new[] { 1 }, new[] { 0 }, 0)]
    [DataRow(new[] { -6, 66, 4444, 401, 0 }, new[] { 3, 2, 4, 0, 1 }, 0)]
    public async Task Selection(int[] initialPool, int[] firstSelection, int secondSelection) =>
        await Run(
        c =>
            initialPool.kFixed()
                .kIOSelectMultiple(firstSelection.Length.kFixed())
                .DeTesDomain(c, [firstSelection], out var firstDomain, "first selection")
                .DeTesAssertRoggiUnstable(
                c, r =>
                    firstSelection.Length > initialPool.Length
                        ? !r.IsSome()
                        : r.Check(out var multi) &&
                          multi.Count == firstSelection.Length &&
                          firstSelection.Map(i => initialPool[i]).SequenceEqual(multi.Elements.Map(x => x.Value)))
                .DeTesReference(c, out var reducedPool)
                .kIOSelectOne()
                .DeTesDomain(c, [secondSelection], out var secondDomain, "second selection")
                .DeTesAssertRoggiUnstable(
                c, r =>
                    secondSelection >= firstSelection.Length
                        ? !r.IsSome()
                        : r.Check(out var sel) && reducedPool.Roggi.Elements.GetAt(secondSelection).Unwrap() == sel));

    [TestMethod]
    public async Task IfElseBranching() =>
        await Run(
        c =>
            Iter.Over(0, 1)
                .kFixed()
                .kIOSelectOne()
                .DeTesDomain(c, [0, 1], out var firstIf, "firstIf")
                .kIsGreaterThan(0.kFixed())
                .kIfTrue<Number>(
                new()
                {
                    Then =
                        Iter.Over(0, 1)
                            .kFixed()
                            .kIOSelectOne()
                            .DeTesDomain(c, [0, 1], out var secondIf, "secondIf")
                            .kIsGreaterThan(0.kFixed())
                            .kIfTrue<Number>(
                            new()
                            {
                                Then = 11.kFixed(),
                                Else = 10.kFixed(),
                            }),
                    Else = 0.kFixed(),
                })
                .DeTesAssertRoggi(
                c, r =>
                    r.Value ==
                    (firstIf.SelectedIndex() == 0
                        ? 0
                        : 10 + secondIf.SelectedIndex())));

    [TestMethod]
    public async Task EnvironmentAndMemory() =>
        await Run(
        c =>
            Core.kSubEnvironment<Number>(
                new()
                {
                    Environment =
                    [
                        Core.kSubEnvironment<Multi<Number>>(
                            new()
                            {
                                Environment =
                                [
                                    400.kFixed()
                                        .kAdd(1.kFixed())
                                        .kAsVariable(out var theNumber),
                                ],
                                Value =
                                    Core.kMultiOf([theNumber.kRef(), theNumber.kRef().kMultiply(2.kFixed())])
                                        .DeTesAssertMemory(c, m => m.Objects.Count() == 1, "inner count check (1)")
                                        .DeTesAssertMemory(c, m => m.GetObject(theNumber).Check(out var v) && v.Value is 401, "reference check"),
                            })
                            .DeTesAssertMemory(c, m => !m.Objects.Any(), "outer pre-count check (0)")
                            .kAsVariable(out var theArray),
                        1.kFixed().kAsVariable(out var theIndex),
                        theArray.kRef()
                            .kGetIndex(theIndex.kRef())
                            .kAsVariable(out var theResult),
                        theNumber.kRef()
                            .DeTesAssertRoggiUnstable(c, r => !r.IsSome()),
                    ],
                    Value =
                        theResult.kRef()
                            .DeTesAssertMemory(c, m => m.Objects.Count() == 3, "outer post-count check (3)"),
                })
                .DeTesAssertRoggi(c, r => r.Value is 401));

    [TestMethod]
    public async Task MetaExecuteStressor() =>
        await Run(
        c =>
            (1..5).kFixed()
            .kMap(
            [],
            x =>
                Core.kMetaFunction<Number, MetaFunction<Number>>(
                [x],
                y =>
                    Core.kSubEnvironment<MetaFunction<Number>>(
                    new()
                    {
                        Environment =
                        [
                            x.kRef()
                                .DeTesAssertRoggiUnstable(c, r => r.IsSome(), "x capture check [x5]")
                                .kIsGreaterThan(2.kFixed())
                                .kIfTrue<Number>(
                                new()
                                {
                                    Then = x.kRef().kMultiply(2.kFixed()),
                                    Else = x.kRef(),
                                })
                                .kAsVariable(out var theValue),
                        ],
                        Value =
                            Core.kMetaFunction(
                            [theValue, y],
                            () => theValue.kRef().kMultiply(10.kFixed()).kAdd(y.kRef())),
                    })))
            .kMap(
            [],
            funcII =>
                funcII.kRef()
                    .kExecuteWith(
                    new()
                    {
                        A = 5.kFixed(),
                    })
                    .kExecute())
            .DeTesAssertRoggi(c, r => r.Count is 5, "count check (5)")
            .DeTesAssertRoggi(c, r => r.Elements.Map(x => x.Value).SequenceEqual([15, 25, 65, 85, 105]), "sequence check"));

    private static Task Run(DeTesDeclaration declaration) => Assert.That.DeclarationHolds(declaration);
}