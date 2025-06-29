namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Rovetus;

using Roggis;
using Roveggi;
using Roveggi.Defined;

internal interface uBarRovetu : uFooRovetu
{
    public static readonly Varovu<uBarRovetu, Number, Roggi.IMulti<Number>> NUM_MAP = new(Axoi.Du, "num_map");
    public static readonly Varovu<uBarRovetu, IRoveggi<uFooRovetu>, Number> FOO_MAP = new(Axoi.Du, "foo_map");
}