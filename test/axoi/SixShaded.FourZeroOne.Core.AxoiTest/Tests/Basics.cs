namespace SixShaded.FourZeroOne.Core.AxoiTest.Tests;

using System.Reflection.Metadata;
using DeTes.Declaration;
using Internal.DummyAxoi.Korvessas;
using Internal.DummyAxoi.Rovetus;
using Roggis;
using Roggis.Instructions;
using Core = Syntax.Core;
using k = Korssas;

/*
 * TODO
 * - test AllValues & AllKeys
 */
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
                .DeTesAssertRoggi(c, r => r.Value == (((xa + xb) * xc) + (xc - xb)) * xa));

    [TestMethod]
    [DataRow(401, new[] { true, true, false }, 2, 6)]
    [DataRow(888, new[] { true, true, false }, 0, 10)]
    [DataRow(0, new[] { true, true, false }, 3, 4)]
    public async Task RoveggiRovi(int num, bool[] bools, int basePower, int power) =>
        await Run(
        c =>
            Core.kCompose<uFooRovetu>()
                .kWithRovi(uFooRovetu.NUM, num.kFixed())
                .DeTesAssertRoggi(c, r => r.GetComponent(uFooRovetu.NUM).Unwrap().Value == num, "NUM check")
                .DeTesAssertRoggi(c, r => !r.GetComponent(uFooRovetu.MULTI_BOOL).IsSome(), "MULTI_BOOL check before set")
                .kWithRovi(uFooRovetu.MULTI_BOOL, bools.kFixed())
                .kWithoutRovi(uFooRovetu.NUM)
                .DeTesAssertRoggi(c, r => !r.GetComponent(uFooRovetu.NUM).IsSome(), "NUM check after remove")
                .kWithRovi(
                uFooRovetu.POWER_OBJ,
                Core.kCompose<uPowerExpr>()
                    .kWithRovi(uPowerExpr.POWER, power.kFixed())
                    .kWithRovi(uPowerExpr.NUM, basePower.kFixed()))
                .DeTesAssertRoggi(c, r => r.GetComponent(uFooRovetu.MULTI_BOOL).Unwrap().Elements.Map(x => x.Check(out var v) && v.IsTrue).SequenceEqual(bools), "MULTI_BOOL check")
                .kGetRovi(uFooRovetu.POWER_OBJ)
                .DeTesAssertRoggi(
                c, r =>
                    r.GetComponent(uPowerExpr.NUM).Unwrap().Value == basePower &&
                    r.GetComponent(uPowerExpr.POWER).Unwrap().Value > 0, "POWER_OBJ check")
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
                          firstSelection.Map(i => initialPool[i]).SequenceEqual(multi.Elements.Map(x => x.Unwrap().Value)))
                .DeTesReference(c, out var reducedPool)
                .kIOSelectOne()
                .DeTesDomain(c, [secondSelection], out var secondDomain, "second selection")
                .DeTesAssertRoggiUnstable(
                c, r =>
                    secondSelection >= firstSelection.Length
                        ? !r.IsSome()
                        : r.Check(out var sel) && reducedPool.Roggi.Elements.GetAt(secondSelection).Press().Unwrap() == sel));

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
                                    Core.kMultiOld(theNumber.kRef(), theNumber.kRef().kMultiply(2.kFixed()))
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
            iX =>
                Core.kMetaFunction<Number, MetaFunction<Number>>(
                [iX],
                iY =>
                    Core.kSubEnvironment<MetaFunction<Number>>(
                    new()
                    {
                        Environment =
                        [
                            iX.kRef()
                                .DeTesAssertRoggiUnstable(c, r => r.IsSome(), "x capture check [x5]")
                                .kIsGreaterThan(2.kFixed())
                                .kIfTrue<Number>(
                                new()
                                {
                                    Then = iX.kRef().kMultiply(2.kFixed()),
                                    Else = iX.kRef(),
                                })
                                .kAsVariable(out var theValue),
                        ],
                        Value =
                            Core.kMetaFunction(
                            [theValue, iY],
                            () => theValue.kRef().kMultiply(10.kFixed()).kAdd(iY.kRef())),
                    })))
            .kMap(
            iFunc =>
                iFunc.kRef()
                    .kExecuteWith(
                    new()
                    {
                        A = 5.kFixed(),
                    })
                    .kExecute())
            .DeTesAssertRoggi(c, r => r.Count is 5, "count check (5)")
            .DeTesAssertRoggi(c, r => r.Elements.Map(x => x.Unwrap().Value).SequenceEqual([15, 25, 65, 85, 105]), "sequence check")
            .DeTesAssertMemory(c, m => !m.Objects.Any(), "memory empty check"));

    [TestMethod]
    public async Task MemoryRoveggi() =>
        await Run(
        c =>
            Core.kSubEnvironment<Number>(
                new()
                {
                    Environment =
                    [
                        (1..5).kFixed()
                        .kMap(
                        iNum =>
                            Core.kSubEnvironment<Multi<Roveggi.IRoveggi<uFooRovedantu>>>(
                            new()
                            {
                                Environment =
                                [
                                    Core.kCompose<uFooRovedantu>()
                                        .kWithRovi(uFooRovedantu.ID, iNum.kRef())
                                        .kAsVariable(out var iComp),
                                ],
                                Value =
                                    Core.kMultiOld(
                                    iComp.kRef()
                                        .kWithRovi(uFooRovedantu.PART, false.kFixed()),
                                    iComp.kRef()
                                        .kWithRovi(uFooRovedantu.PART, true.kFixed())),
                            }))
                        .kFlatten()
                        .DeTesAssertRoggi(c, r => r.Count is 10, "pre memassign count check (10)")
                        .kMap(
                        iComp =>
                            Core.kSubEnvironment<Rog>(
                            new()
                            {
                                Environment =
                                [
                                    iComp.kRef()
                                        .kGetRovi(uFooRovedantu.ID)
                                        .kAsVariable(out var iNum),
                                ],
                                Value =
                                    iComp.kRef()
                                        .kWrite(
                                        iComp.kRef()
                                            .kGetRovi(uFooRovedantu.PART)
                                            .kIfTrue<Number>(
                                            new()
                                            {
                                                Then = iNum.kRef().kMultiply(10.kFixed()),
                                                Else = iNum.kRef().kMultiply((-1).kFixed()),
                                            })),
                            }))
                        .DeTesAssertRoggi(c, r => r.Count is 10, "post memassign count check (10)"),
                    ],
                    Value =
                        Core.kSubEnvironment<Number>(
                            new()
                            {
                                Environment =
                                [
                                    (1..5)
                                    .kFixed()
                                    .kIOSelectOne()
                                    .DeTesDomain(c, Iter.Range(0, 4, true), out var idDomain, "id domain")
                                    .DeTesReference(c, out var dtId, "selected id")
                                    .kAsVariable(out var iSelectedId),
                                    Iter.Over(true, false)
                                        .Map(x => (Bool)x)
                                        .kFixed()
                                        .kIOSelectOne()
                                        .DeTesDomain(c, Iter.Over(0, 1), out var partDomain, "part domain")
                                        .DeTesReference(c, out var dtPart, "selected part")
                                        .kAsVariable(out var iSelectedPart),
                                ],
                                Value =
                                    Core.kCompose<uFooRovedantu>()
                                        .kWithRovi(uFooRovedantu.ID, iSelectedId.kRef())
                                        .kWithRovi(uFooRovedantu.PART, iSelectedPart.kRef())
                                        .kRead()
                                        .DeTesAssertRoggiUnstable(c, r => r.IsSome(), "data exists check"),
                            })
                            .DeTesAssertMemory(c, m => m.Objects.Count() > 1, "memory some data exists check (>1)")
                            .DeTesAssertMemory(c, m => m.Objects.Count() == 10, "memory exact count check (10)"),
                })
                .DeTesAssertRoggi(
                c,
                r =>
                    r.Value ==
                    (dtPart.Roggi.IsTrue
                        ? dtId.Roggi.Value * 10
                        : dtId.Roggi.Value * -1),
                "final result check"));

    // This is considered undefined behavior, do not do this.
    // specifically using 'kAsVariable(out <existing variable>)'.
    // this may seem to intuitively work at a simple level (as this example shows),
    // but 'out <existing>' only affects the C# variable and only *looks like* it works.
    //  it does not change <existing>, it stores a *new* roda in the C# variable.
    [TestMethod]
    public async Task Mutation() =>
        await Run(
        c =>
            Core.kSubEnvironment<Multi<Number>>(
            new()
            {
                Environment =
                [
                    0.kFixed().kAsVariable(out var iMutable),
                    iMutable.kRef()
                        .DeTesAssertRoggi(c, r => r.Value == 0, "init zero")
                        .kAsVariable(out var iZero),
                    // BAD!
                    iMutable.kRef().kAdd(1.kFixed()).kAsVariable(out iMutable),
                    iMutable.kRef()
                        .DeTesAssertRoggi(c, r => r.Value == 1, "init one")
                        .kAsVariable(out var iOne),
                    Core.kMultiOld<Rog>(
                    10.kFixed().kAsVariable(out var iTest),
                    iTest.kRef().kAdd(1.kFixed()).kAsVariable(out iTest),
                    Core.kMultiOld<Rog>(
                    iTest.kRef().kAdd(1.kFixed()).kAsVariable(out iTest),
                    iTest.kRef().kAdd(1.kFixed()).kAsVariable(out iTest)),
                    iTest.kRef().DeTesAssertRoggi(c, r => r.Value == 13)),
                ],
                Value =
                    Core.kMultiOld<Number>(
                    iMutable.kRef().DeTesAssertRoggi(c, r => r.Value == 1, "value mutable"),
                    iTest.kRef().DeTesAssertRoggi(c, r => r.Value == 13),
                    iZero.kRef().DeTesAssertRoggi(c, r => r.Value == 0, "value zero"),
                    iOne.kRef().DeTesAssertRoggi(c, r => r.Value == 1, "value one")),
            }));

    // DEV: maybe test set operations on roveggis, its probably fine though.
    [TestMethod]
    public async Task SetOperationNumbers() =>
        await Run(
        c =>
            Core.kSubEnvironment<Rog>(
            new()
            {
                Environment =
                [
                    (1..100).kFixed()
                    .kAsVariable(out var iToHundred),
                    (1..50).kFixed()
                    .kAsVariable(out var iToFifty),
                    5.Sequence(x => x + 5)
                        .TakeWhile(x => x <= 100)
                        .kFixed()
                        .kAsVariable(out var iFives),
                    3.Sequence(x => x + 3)
                        .TakeWhile(x => x <= 100)
                        .kFixed()
                        .kAsVariable(out var iThrees),
                ],
                Value =
                    Core.kMulti<Rog>(
                    new()
                    {
                        iToHundred.kRef()
                            .kExcept(iToFifty.kRef())
                            .DeTesAssertRoggi(c, r => r.Elements.Map(x => x.Unwrap().Value).SequenceEqual(Iter.Range(51, 100, true)), "except easy")
                            .kCount(),
                        iToHundred.kRef()
                            .kIntersect(iFives.kRef())
                            .kIntersect(iThrees.kRef())
                            .DeTesAssertRoggi(c, r => r.Elements.All(x => x.Unwrap().Value.ExprAs(v => v % 5 == 0 && v % 3 == 0)), "intersect")
                            .kCount(),
                        iFives.kRef()
                            .kUnion(iThrees.kRef())
                            .DeTesAssertRoggi(c, r => r.Elements.All(x => x.Unwrap().Value.ExprAs(v => v % 5 == 0 || v % 3 == 0)), "union")
                            .DeTesReference(c, out var tFizzBuzzSet)
                            .kCount(),
                        iToHundred.kRef()
                            .kExcept(iFives.kRef())
                            .kExcept(iThrees.kRef())
                            .DeTesAssertRoggi(c, r => r.Count == 100 - tFizzBuzzSet.Roggi.Count, "except")
                            .kCount(),

                    })
            }));
    /*
    [TestMethod]
    public async Task MellsanoStressor() =>
    await Run(c =>
    Core.kSubEnvironment<Multi<Number>>(new()
    {
        Environment =
            [
                0.kFixed().kAsVariable(out var iMellsanoCounter),
                Core.kAddMellsano<Rog>(new()
                {
                    Matches = m => m.mIsType<k.AddMellsano>(),
                    DefinitionCaptures = [iMellsanoCounter],
                    Definition = iOrig => Core.kMulti(
                    iOrig.kRef().kRealize(),
                    iMellsanoCounter.kRef().kAdd(1.kFixed()))
                })
            ]
    })
    */
    private static Task Run(DeTesDeclaration declaration) => Assert.That.DeclarationHolds(declaration);
}