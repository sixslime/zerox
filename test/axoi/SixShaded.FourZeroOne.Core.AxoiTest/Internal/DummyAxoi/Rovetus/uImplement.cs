namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Rovetus;

using Roveggi;
using Roveggi.Defined;
public interface uImplement : uAbstract
{
    public static readonly Rovu<uImplement, Roggis.NumRange> RANGE = new(Axoi.Du, "range");
    private static readonly ImplementationStatement<uImplement> __IMPLEMENTS =
        c =>
            c.ImplementGet(ABSTRACT_GET, self => self.kRef().kGetRovi(RANGE).kStart());
}