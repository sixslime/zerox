namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class CatchNolla<R>
    where R : class, Rog
{
    public static Korvessa<R, MetaFunction<R>, R> Construct(IKorssa<R> value, IKorssa<MetaFunction<R>> fallback) =>
        new(value, fallback)
        {
            Du = Axoi.Korvedu("CatchNolla"),
            Definition =
                Core.kMetaFunction<R, MetaFunction<R>, R>(
                [],
                (iValue, iFallback) =>
                    iValue.kRef()
                        .kExists()
                        .kIfTrueExplicit<R>(
                        new()
                        {
                            Then = iValue.kRef().kMetaBoxed([]),
                            Else = iFallback.kRef(),
                        })
                        .kExecute()),
        };
}