namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Rovetus;

using Roveggi;
using Roveggi.Defined;
public interface uAbstract : IRovetu
{
    public static readonly AbstractGetRovu<uAbstract, Roggis.Number> ABSTRACT_GET = new("abstract_get");
    public static readonly AbstractSetRovu<uAbstract, Roggis.Number> ABSTRACT_SET = new("abstract_set");
}