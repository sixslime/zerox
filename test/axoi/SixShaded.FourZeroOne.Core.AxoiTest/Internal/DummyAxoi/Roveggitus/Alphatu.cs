namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Roveggitus;

using Roggi;
using Roggi.Defined;
using Roggis;

internal class Alphatu : Roveggitu
{
    public static readonly Rovu<Alphatu, Number> NUM = new(TestAxoi.Du, "num");
    public static readonly Rovu<Alphatu, IMulti<IRoggi>> MULTI_ANY = new(AxoiTest.Du, "multi_any");

}