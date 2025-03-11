namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Roveggitus;

using Roggis;
using Roveggi.Defined;
internal class FooMemRovetu : MemoryRovetu<Number>
{
    public static readonly Rovu<FooMemRovetu, Number> ID = new(TestAxoi.Du, "id");
    public static readonly Rovu<FooMemRovetu, Bool> PART = new(TestAxoi.Du, "part");
}
