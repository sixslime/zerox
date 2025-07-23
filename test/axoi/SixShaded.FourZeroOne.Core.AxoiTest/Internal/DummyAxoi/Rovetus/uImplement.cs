namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Rovetus;

using Roveggi;
using Roveggi.Defined;
public interface uImplement : uAbstract, IConcreteRovetu
{
    public static readonly Rovu<uImplement, Roggis.NumRange> RANGE = new(TestAxoi.Du, "range");
    public static readonly ImplementationStatement<uImplement> __IMPLEMENTS =
        c =>
            c.ImplementGet(ABSTRACT_GET, self => self.kRef().kGetRovi(RANGE).kStart());
}