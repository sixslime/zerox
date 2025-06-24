namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Roveggitus;

using Roggi;
using Roggi.Defined;
using Roggis;
using Roveggi;
using Roveggi.Defined;

internal interface uFooRovetu : IRovetu
{
    public static readonly Rovu<uFooRovetu, Number> NUM = new(TestAxoi.Du, "num");
    public static readonly Rovu<uFooRovetu, IMulti<Bool>> MULTI_BOOL = new(TestAxoi.Du, "multi_bool");
    public static readonly Rovu<uFooRovetu, IRoveggi<uPowerExpr>> POWER_OBJ = new(TestAxoi.Du, "power_obj");
}