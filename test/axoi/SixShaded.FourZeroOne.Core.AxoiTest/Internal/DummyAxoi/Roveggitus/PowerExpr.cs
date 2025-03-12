namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Roveggitus;

using Roggi;
using Roggi.Defined;
using Roggis;
using Roveggi.Defined;
using Core = Syntax.Core;

internal class PowerExpr : Rovetu
{
    public static readonly Rovu<PowerExpr, Number> NUM = new(TestAxoi.Du, "num");
    public static readonly Rovu<PowerExpr, Number> POWER = new(TestAxoi.Du, "power");
}