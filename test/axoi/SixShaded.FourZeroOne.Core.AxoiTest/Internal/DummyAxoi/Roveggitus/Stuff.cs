namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Roveggitus;

using Roggi;
using Roggi.Defined;
using Roggis;

internal class Stuff : Roveggitu
{
    public static readonly Rovu<Stuff, Number> NUM = new(TestAxoi.Du, "num");
    public static readonly Rovu<Stuff, IMulti<IRoggi>> MULTI_ANY = new(TestAxoi.Du, "multi_any");
    public static readonly Rovu<Stuff, Roveggi<PowerExpr>> POWER_OBJ = new(TestAxoi.Du, "power_obj");
}