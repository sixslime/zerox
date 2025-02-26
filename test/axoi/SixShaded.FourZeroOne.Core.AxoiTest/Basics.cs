namespace SixShaded.FourZeroOne.Core.AxoiTest;

using DeTes.Declaration;
[TestClass]
public sealed class Basics
{
    [TestMethod]
    [DataRow(-400, 1, 10)]
    [DataRow(0, 1, 99)]
    [DataRow(777, 99, -1)]
    [DataRow(10, 20, 55)]
    [DataRow(-10, -20, -55)]
    public async Task Arithmetic(int xa, int xb, int xc) => await Run(c =>
        xa.tFixed()
            .tAdd(xb.tFixed())
            .tMultiply(xc.tFixed())
            .tAdd(xc.tFixed().tSubtract(xb.tFixed()))
            .tMultiply(xa.tFixed())
            .AssertRoggi(c, u => u.Value == (((xa + xb) * xc) + (xc - xb)) * xa));

    private static Task Run(DeTesDeclaration declaration) => Assert.That.DeclarationHolds(declaration);
}
