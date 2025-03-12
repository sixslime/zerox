namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Roveggitus;

using Roggi;
using Roggi.Defined;
using Roggis;
using Roveggi;
using Roveggi.Defined;

internal class FooRovetu : Rovetu
{
    public static readonly Rovu<FooRovetu, Number> NUM = new(TestAxoi.Du, "num");
    public static readonly Rovu<FooRovetu, IMulti<Bool>> MULTI_BOOL = new(TestAxoi.Du, "multi_bool");
    public static readonly Rovu<FooRovetu, IRoveggi<PowerExpr>> POWER_OBJ = new(TestAxoi.Du, "power_obj");
}