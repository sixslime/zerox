namespace SixShaded.FourZeroOne.Core.AxoiTest.Tests;

using DeTes.Declaration;
using Internal.DummyAxoi.Roveggitus;
using Core = Syntax.Core;
using Roggis;

[TestClass]
public class Sanity
{
    [TestMethod]
    public async Task Compose() =>
        await Run(
        c =>
            Core.kCompose<FooRovetu>()
                .kWithRovi(FooRovetu.NUM, 401.kFixed())
                .DeTesAssertRoggi(c, r => r.GetComponent(FooRovetu.NUM).Unwrap().Value == 401, "NUM check"));

    [TestMethod]
    public async Task AMap() =>
        await Run(
        c =>
            (1..5).kFixed()
            .kMap([], x => x.kRef().kAdd(10.kFixed()))
            .DeTesAssertRoggi(c, r => r.Count == 5));

    [TestMethod]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new[] { 5, 2, 0, 1 }, 0)]
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
                .DeTesAssertRoggiUnstable(c, r => true));

    [TestMethod]
    public async Task MetaExecuteCapture() =>
        await Run(
        c =>
            Core.kSubEnvironment<MetaFunction<Number, Number>>(
                new()
                {
                    Environment = [400.kFixed().kAsVariable(out var theNumber)],
                    Value =
                        (..1).kFixed()
                        .kIOSelectOne()
                        .DeTesDomain(c, [0, 1], out var domain)
                        .kIsGreaterThan(0.kFixed())
                        .kIfTrue<MetaFunction<Number, Number>>(
                        new()
                        {
                            Then =
                                Core.kMetaFunction<Number, Number>(
                                [theNumber],
                                x =>
                                    theNumber.kRef().kAdd(x.kRef())),
                            Else =
                                Core.kMetaFunction<Number, Number>(
                                [],
                                x =>
                                    theNumber.kRef().kAdd(x.kRef())),
                        }),
                })
                .kExecuteWith(
                new()
                {
                    A = 1.kFixed(),
                })
                .DeTesAssertRoggiUnstable(c, r => domain.SelectedIndex() == 1 ? r.Check(out var v) && v.Value is 401 : !r.IsSome()));

    [TestMethod]
    public async Task MemoryRoveggi() =>
        await Run(
        c =>
            Core.kSubEnvironment<Multi<Number>>(
            new()
            {
                Environment =
                [
                    Core.kCompose<FooMemRovetu>()
                        .kWithRovi(FooMemRovetu.ID, 8.kFixed())
                        .kWithRovi(FooMemRovetu.PART, true.kFixed())
                        .kAsVariable(out var iComp),
                    iComp.kRef().kWrite(401.kFixed()),
                ],
                Value =
                    Core.kMulti(
                    iComp.kRef()
                        .kGet()
                        .DeTesAssertRoggi(c, r => r.Value is 401, "direct reference"),
                    Core.kCompose<FooMemRovetu>()
                        .kWithRovi(FooMemRovetu.ID, 8.kFixed())
                        .kWithRovi(FooMemRovetu.PART, true.kFixed())
                        .kGet()
                        .DeTesAssertRoggi(c, r => r.Value is 401, "reconstructed")),
            }));

    private static Task Run(DeTesDeclaration declaration) => Assert.That.DeclarationHolds(declaration);
}