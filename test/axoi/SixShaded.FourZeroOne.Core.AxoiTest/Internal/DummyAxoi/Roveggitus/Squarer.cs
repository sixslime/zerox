namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Roveggitus;

using Roggi;
using Roggi.Defined;
using Roggis;
using Core = Syntax.Core;

internal class Powertu : DecomposableRoveggitu<Powertu, Number>
{
    public static readonly Rovu<Powertu, Number> NUM = new(TestAxoi.Du, "num");
    public static readonly Rovu<Powertu, Number> POWER = new(TestAxoi.Du, "power");

    public override MetaFunction<IRoveggi<Powertu>, Number> DecomposeFunction =>
    Core.tMetaFunction<IRoveggi<Powertu>, Number>(
        self =>
        Core.tMetaRecursiveFunction<Number, Number, Number, Number>(
            (thisFunc, acc, i, num) =>
            i.tRef()
            .tIsGreaterThan(0.tFixed())
            .tIfTrue<Number>(
                new()
                {
                Then =
                thisFunc.tRef()
                .tExecuteWith(
                    new()
                    {
                    A = acc.tRef().tMultiply(num.tRef()),
                    B = i.tRef().tSubtract(1.tFixed()),
                    C = num.tRef(),
                    }),
                Else = acc.tRef(),
                }))
        .tExecuteWith(
            new()
            {
            A = self.tRef().tGetComponent(NUM),
            B = self.tRef().tGetComponent(POWER),
            C = self.tRef().tGetComponent(NUM),
            }))
    .Roggi;
}