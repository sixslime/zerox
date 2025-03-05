namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Roveggitus;

using Roggi;
using Roggi.Defined;
using Roggis;

internal class Stuff : Rovetu
{
    public static readonly Rovu<Stuff, Number> NUM = new(TestAxoi.Du, "num");
    public static readonly Rovu<Stuff, IMulti<Bool>> MULTI_BOOL = new(TestAxoi.Du, "multi_bool");
    public static readonly Rovu<Stuff, IRoveggi<PowerExpr>> POWER_OBJ = new(TestAxoi.Du, "power_obj");
}