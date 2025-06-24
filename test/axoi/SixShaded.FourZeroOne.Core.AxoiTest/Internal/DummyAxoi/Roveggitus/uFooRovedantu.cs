namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Roveggitus;

using Roggis;
using Roveggi;
using Roveggi.Defined;

internal class uFooRovedantu : Rovedantu<Number>
{
    public static readonly Rovu<uFooRovedantu, Number> ID = new(TestAxoi.Du, "id");
    public static readonly Rovu<uFooRovedantu, Bool> PART = new(TestAxoi.Du, "part");
}