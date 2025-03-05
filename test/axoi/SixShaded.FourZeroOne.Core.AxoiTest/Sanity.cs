namespace SixShaded.FourZeroOne.Core.AxoiTest;

using DeTes.Declaration;
using Internal.DummyAxoi.Roveggitus;
using Core = Syntax.Core;

[TestClass]
public class Sanity
{
    [TestMethod]
    public async Task Compose() =>
        await Run(
        c =>
            Core.kRoveggi<Stuff>()
                .kWithRovi(Stuff.NUM, 401.kFixed())
                .AssertRoggi(c, r => r.GetComponent(Stuff.NUM).Unwrap().Value == 401, "NUM check"));

    [TestMethod]
    public async Task AMap() =>
        await Run(
        c =>
            (1..5).kFixed()
            .kMap(x => x.kRef().kAdd(10.kFixed()))
            .AssertRoggi(c, r => r.Count == 5));

    [TestMethod]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new[] { 5, 2, 0, 1 }, 0)]
    public async Task Selection(int[] initialPool, int[] firstSelection, int secondSelection) =>
        await Run(
        c =>
            initialPool.kFixed()
                .kIOSelectMultiple(firstSelection.Length.kFixed())
                .WithDomain(c, [firstSelection], out var firstDomain, "first selection")
                .AssertRoggiUnstable(
                c, r =>
                    (firstSelection.Length > initialPool.Length)
                        ? !r.IsSome()
                        : r.Check(out var multi) &&
                          multi.Count == firstSelection.Length &&
                          firstSelection.Map(i => initialPool[i]).SequenceEqual(multi.Elements.Map(x => x.Value)))
                .ReferenceAs(c, out var reducedPool)
                .kIOSelectOne()
                .WithDomain(c, [secondSelection], out var secondDomain, "second selection")
                .AssertRoggiUnstable(c, r => true));
    private static Task Run(DeTesDeclaration declaration) => Assert.That.DeclarationHolds(declaration);
}