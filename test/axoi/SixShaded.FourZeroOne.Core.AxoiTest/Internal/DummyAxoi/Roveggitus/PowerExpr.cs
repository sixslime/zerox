namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Roveggitus;

using Roggi;
using Roggi.Defined;
using Roggis;
using Core = Syntax.Core;

internal class PowerExpr : DecomposableRoveggitu<PowerExpr, Number>
{
    public static readonly Rovu<PowerExpr, Number> NUM = new(TestAxoi.Du, "num");
    public static readonly Rovu<PowerExpr, Number> POWER = new(TestAxoi.Du, "power");

    public override MetaFunction<IRoveggi<PowerExpr>, Number> DecomposeFunction =>
        Core.tMetaFunction<IRoveggi<PowerExpr>, Number>(
            self =>
                Core.tMetaRecursiveFunction<Number, Number, Number, Number>(
                    (recurse, acc, i, num) =>
                        i.tRef()
                            .tIsGreaterThan(1.tFixed())
                            .tIfTrue<Number>(
                            new()
                            {
                                Then =
                                    recurse.tRef()
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