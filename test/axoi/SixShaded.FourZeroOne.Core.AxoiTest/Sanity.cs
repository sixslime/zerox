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
            Core.tCompose<Stuff>()
                .tWithComponent(Stuff.NUM, 401.tFixed())
                .AssertRoggi(c, r => r.GetComponent(Stuff.NUM).Unwrap().Value == 401, "NUM check"));

    [TestMethod]
    public async Task AMap() =>
        await Run(
        c =>
            (1..5).tFixed()
            .tMap(x => x.tRef().tAdd(10.tFixed()))
            .AssertRoggi(c, r => r.Count == 5));
    private static Task Run(DeTesDeclaration declaration) => Assert.That.DeclarationHolds(declaration);
}