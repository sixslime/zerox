namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Rovetus;

using Roggi;
using Roggi.Defined;
using Roggis;
using Roveggi;
using Roveggi.Defined;
using Core = Syntax.Core;

internal interface uPowerExpr : IRovetu
{
    public static readonly Rovu<uPowerExpr, Number> NUM = new(TestAxoi.Du, "num");
    public static readonly Rovu<uPowerExpr, Number> POWER = new(TestAxoi.Du, "power");
}